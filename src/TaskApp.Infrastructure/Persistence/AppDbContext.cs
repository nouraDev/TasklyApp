using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using TaskApp.Domain.Entities;
using TaskApp.Domain.Shared;

namespace TaskApp.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;
        public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<TaskCategory> TaskCategories => Set<TaskCategory>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(CreateIsDeletedFilter(entityType.ClrType));
                }
            }

        }
        private static LambdaExpression CreateIsDeletedFilter(Type entityType)
        {
            var param = Expression.Parameter(entityType, "entity");
            var prop = Expression.Property(param, "IsDeleted");
            var condition = Expression.Equal(prop, Expression.Constant(false));
            return Expression.Lambda(condition, param);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        SetCreationAuditFields(entry.Entity);
                        break;
                    case EntityState.Modified:
                        SetModificationAuditFields(entry.Entity);
                        break;
                    case EntityState.Deleted:
                        SoftDeleteEntity(entry);
                        break;
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {

                switch (entry.State)
                {
                    case EntityState.Added:
                        SetCreationAuditFields(entry.Entity);
                        break;

                    case EntityState.Modified:
                        SetModificationAuditFields(entry.Entity);
                        break;

                    case EntityState.Deleted:
                        SoftDeleteEntity(entry);
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);
            await DispatchDomainEvents();

            return result;
        }
        private async Task DispatchDomainEvents()
        {
            var entities = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var events = entities.SelectMany(e => e.DomainEvents).ToList();

            entities.ForEach(e => e.ClearDomainEvents());

            await _dispatcher.Dispatch(events);
        }

        private void SetCreationAuditFields(BaseEntity entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }

        private void SetModificationAuditFields(BaseEntity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
        }
        private void SoftDeleteEntity(EntityEntry entry)
        {
            if (entry.Entity is BaseEntity entity && entity is BaseEntity softDeletableEntity && !softDeletableEntity.IsDeleted)
            {
                softDeletableEntity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
                entry.State = EntityState.Modified;
            }

            foreach (var navigationEntry in entry.Navigations)
            {
                if (!navigationEntry.IsLoaded)
                {
                    navigationEntry.Load();
                }

                if (navigationEntry is CollectionEntry collectionEntry && collectionEntry.CurrentValue != null)
                {
                    foreach (var dependentEntity in collectionEntry.CurrentValue)
                    {
                        if (dependentEntity is BaseEntity softDeletable && !softDeletable.IsDeleted)
                        {
                            var dependentEntry = entry.Context.Entry(dependentEntity);
                            SoftDeleteEntity(dependentEntry);
                        }
                    }
                }
            }
        }

    }
}
