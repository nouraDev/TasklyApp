# TaskApp

A lightweight **Task Management application** built with **.NET 9, Clean Architecture, CQRS, React, and Docker**.  
It demonstrates modern practices such as Domain-Driven Design, MediatR for CQRS, EF Core for persistence, and containerized development.

---

##  Features
-  Create and list tasks  
-  Mark tasks as complete  
-  View archived tasks (overdue or completed)  
-  React frontend with TypeScript  

---

##  Tech Stack
- **Backend**: .NET 9, EF Core, Clean Architecture, Domain Events, CQRS (MediatR), FluentValidation  
- **Frontend**: React (TypeScript, Vite)  
- **Database**: SQL Server (via Docker)  
- **Testing**: xUnit  

---

##  Getting Started

### Prerequisites
- [Docker](https://docs.docker.com/get-docker/) installed and running  
- [Node.js](https://nodejs.org/) LTS version for frontend development  

---

### 1. Clone the repository
```bash
git clone https://github.com/your-org/taskapp.git
cd taskapp
```

### 2. Run backend (API + SQL Server via Docker)
```bash
docker-compose up -d --build
```
- API available at  [http://localhost:5200](http://localhost:5200)  

---

### 3. Run frontend
```bash
cd src/frontend
npm install
npm run dev
```
- Frontend available at  [http://localhost:9191](http://localhost:9191)  

---

##  Running Tests
From the root directory:
```bash
dotnet test
```

---

##  Development Notes
- EF Core migrations are applied automatically on startup.  
- If you want to manage migrations manually:  
  ```bash
  dotnet ef migrations add <MigrationName> --project src/TaskApp.Infrastructure --startup-project src/TaskApp.WepApi
  dotnet ef database update --project src/TaskApp.Infrastructure --startup-project src/TaskApp.WepApi
  ```

---

##  Roadmap / Improvements
- Add authentication & authorization  
- Add task categories/tags  
- Improve UI/UX with Tailwind or Material UI  
- Add CI/CD pipeline (GitHub Actions)  
