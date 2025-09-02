import { useState, useEffect, type JSX } from "react";
import { Card } from "../../components/ui/card";
import { Checkbox } from "../../components/ui/checkbox";
import { Button } from "../../components/ui/button";
import TaskForm from "../../components/task/task-form";
import { Trash, Clock, FileText, Tag } from "lucide-react";
import { DeleteConfirmationDialog } from "../../components/delete-confirmation-dialog";
import { type Task, type ListItem } from "../../utils/types";
import { isToday, isTomorrow, isThisWeek } from "date-fns";
import { useTaskActions } from "../../hooks/use-task-actions";
export function UpcomingPage({ lists, onTaskCountChange }: Readonly<{ lists: ListItem[], onTaskCountChange?: (count: number) => void }>) {
  const [selectedTask, setSelectedTask] = useState<Task | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [, setDeleteTaskId] = useState<string | null>(null);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [defaultDueDate, setDefaultDueDate] = useState<Date | null>(null);

  // Use reusable task actions
  const { tasks, fetchTasks, toggleComplete, handleDelete } = useTaskActions({ onTaskCountChange });

  useEffect(() => {
    fetchTasks();
  }, [fetchTasks]);

  // Filter tasks by time categories
  const todayTasks = tasks.filter((t) => t.dueDate && isToday(new Date(t.dueDate)));
  const tomorrowTasks = tasks.filter((t) => t.dueDate && isTomorrow(new Date(t.dueDate)));
  const weekTasks = tasks.filter(
    (t) =>
      t.dueDate &&
      isThisWeek(new Date(t.dueDate), { weekStartsOn: 1 }) &&
      !isToday(new Date(t.dueDate)) &&
      !isTomorrow(new Date(t.dueDate))
  );

  const upcomingCount  = todayTasks.length + tomorrowTasks.length + weekTasks.length;

  const handleAddTask = (defaultDate: Date | null) => {
    setSelectedTask(null);
    setShowForm(true);
    setDefaultDueDate(defaultDate);
  };

  const handleSave = async () => {
    fetchTasks();
    setShowForm(false);
    setSelectedTask(null);
    setDefaultDueDate(null);
  };

  const renderTasks = (tasks: Task[]) => (
    <div className="space-y-3">
      <div className="flex flex-wrap gap-4">
        {tasks.map((task) => (
          <Card
            key={task.id}
            className="flex justify-between p-4 cursor-pointer hover:shadow-lg transition-shadow duration-200 w-[90%]"
            onClick={() => {
              setSelectedTask(task);
              setShowForm(true);
              setDefaultDueDate(task.dueDate ? new Date(task.dueDate) : null);
            }}
          >
            <div className="flex flex-col gap-2">
              <div className="flex items-start gap-3">
                <Checkbox
                  checked={task.isCompleted}
                  className="mt-1"
                  onClick={(e) => e.stopPropagation()}
                  onCheckedChange={(checked) => toggleComplete(task, !!checked)}
                />
                <div className="flex flex-col gap-1">
                  <h3 className="font-semibold text-lg">{task.title}</h3>
                  <div className="flex items-center gap-2 text-sm text-gray-500 line-clamp-1">
                    <Button
                      onClick={(e) => {
                        e.stopPropagation();
                        setDeleteTaskId(task.id);
                        setDeleteDialogOpen(true);
                      }}
                      className="text-red-500 hover:text-red-700 p-1 focus:outline-none focus:ring-0"
                    >
                      <Trash className="w-4 h-4" />
                    </Button>

                    {task.dueDate && (
                      <>
                        <Clock className="w-4 h-4 text-gray-400" />
                        <span className="whitespace-nowrap">
                          {new Date(task.dueDate).toLocaleTimeString([], {
                            hour: "2-digit",
                            minute: "2-digit",
                          })}
                        </span>
                      </>
                    )}

                    {task.description && (
                      <>
                        <FileText className="w-4 h-4 text-gray-400" />
                        <span className="truncate">{task.description}</span>
                      </>
                    )}
                  </div>
                </div>
              </div>
            </div>

            <div className="flex flex-col items-end gap-1 text-sm text-gray-400">
              <div className="flex items-center gap-1">
                <Tag className="w-4 h-4" />
                <span>{task.listName}</span>
              </div>
              <span
                className={`text-xs font-medium px-2 py-0.5 rounded ${
                  task.priorityCategory === 0
                    ? "bg-red-100 text-red-800"
                    : task.priorityCategory === 1
                    ? "bg-yellow-100 text-yellow-800"
                    : task.priorityCategory === 2
                    ? "bg-orange-100 text-orange-800"
                    : "bg-gray-100 text-gray-800"
                }`}
              >
                {[
                  "Important and Urgent",
                  "Important and NotUrgent",
                  "Urgent and NotImportant",
                  "NotImportant and NotUrgent",
                ][task.priorityCategory]}
              </span>
            </div>
          </Card>
        ))}
      </div>
    </div>
  );

  return (
    <div className="min-h-screen p-6 bg-gray-50">
      <h1 className="text-2xl font-bold mb-4 flex items-center gap-2">
        Upcoming <span className="text-gray-500">{upcomingCount}</span>
      </h1>

      {showForm ? (
        <div className="flex min-h-screen">
          <div className="w-1/2 border-r border-gray-200 p-6 overflow-y-auto">
            <TaskGroup
              title="Today"
              tasks={todayTasks}
              onAdd={() => handleAddTask(new Date())}
              renderTasks={renderTasks}
            />
            <TaskGroup
              title="Tomorrow"
              tasks={tomorrowTasks}
              onAdd={() => {
                const tomorrow = new Date();
                tomorrow.setDate(tomorrow.getDate() + 1);
                handleAddTask(tomorrow);
              }}
              renderTasks={renderTasks}
            />
            <TaskGroup
              title="This Week"
              tasks={weekTasks}
              onAdd={() => handleAddTask(null)}
              renderTasks={renderTasks}
            />
          </div>

          <div className="w-1/2 p-6 overflow-y-auto bg-white mb-6">
            <TaskForm
              task={selectedTask}
              onSave={handleSave}
              onCancel={() => setShowForm(false)}
              lists={lists}
              defaultDueDate={defaultDueDate}
            />
          </div>
        </div>
      ) : (
        <div className="grid grid-rows-2 gap-6">
          <div>
            <TaskGroup
              title="Today"
              tasks={todayTasks}
              onAdd={() => handleAddTask(new Date())}
              renderTasks={renderTasks}
            />
          </div>

          <div className="grid grid-cols-2 gap-6">
            <div>
              <TaskGroup
                title="Tomorrow"
                tasks={tomorrowTasks}
                onAdd={() => {
                  const tomorrow = new Date();
                  tomorrow.setDate(tomorrow.getDate() + 1);
                  handleAddTask(tomorrow);
                }}
                renderTasks={renderTasks}
              />
            </div>
            <div>
              <TaskGroup
                title="This Week"
                tasks={weekTasks}
                onAdd={() => handleAddTask(null)}
                renderTasks={renderTasks}
              />
            </div>
          </div>
        </div>
      )}

      <DeleteConfirmationDialog
        open={deleteDialogOpen}
        onOpenChange={setDeleteDialogOpen}
        onConfirm={() => handleDelete()}
      />
    </div>
  );
}

function TaskGroup({
  title,
  tasks,
  onAdd,
  renderTasks,
}: Readonly<{
  title: string;
  tasks: Task[];
  onAdd: () => void;
  renderTasks: (tasks: Task[]) => JSX.Element;
}>) {
  return (
    <div className="mb-6">
      <h2 className="text-lg font-semibold mb-2">{title}</h2>
      <div className="flex justify-start mb-2">
        <Button className="w-[90%] justify-center text-center" onClick={onAdd}>
          + Add New Task
        </Button>
      </div>

      <div className="max-h-[300px] overflow-y-auto scrollbar-thin scrollbar-thumb-gray-300 scrollbar-track-gray-100">
        {renderTasks(tasks)}
      </div>
    </div>
  );
}

