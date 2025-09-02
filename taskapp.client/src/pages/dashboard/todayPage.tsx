import { useEffect, useState } from "react";
import { Card } from "../../components/ui/card";
import { Checkbox } from "../../components/ui/checkbox";
import { Button } from "../../components/ui/button";
import TaskForm from "../../components/task/task-form";
import { type Task, type ListItem } from "../../utils/types";
import { Clock, FileText, Tag, Trash } from "lucide-react";
import { DeleteConfirmationDialog } from "../../components/delete-confirmation-dialog";
import { isToday as dateFnsIsToday } from "date-fns";
import { useTaskActions } from "../../hooks/use-task-actions";

interface TodayPageProps {
  lists: ListItem[];
  onTaskCountChange?: (count: number) => void;
}

export default function TodayPage({ lists, onTaskCountChange }: Readonly<TodayPageProps>) {
  const [selectedTask, setSelectedTask] = useState<Task | null>(null);
  const [showForm, setShowForm] = useState(false);

  const {
    tasks,
    setDeleteTaskId,
    deleteDialogOpen,
    setDeleteDialogOpen,
    toggleComplete,
    handleDelete,
    fetchTasks, 
  } = useTaskActions({ onTaskCountChange });

  useEffect(() => {
    const load = async () => {
       fetchTasks(); 
    };
    load();
  }, [fetchTasks]);

  const updateTaskCount = (taskList: Task[]) => {
    if (onTaskCountChange) onTaskCountChange(taskList.length);
  };
  const todayTasks = tasks.filter(t => t.dueDate && dateFnsIsToday(new Date(t.dueDate)));

  useEffect(() => {
    updateTaskCount(todayTasks);
  }, [todayTasks, updateTaskCount]);
  
  const handleAddTask = () => {
    setSelectedTask(null);
    setShowForm(true);
  };

  const handleEditTask = (task: Task) => {
    const originalTask = tasks.find(t => t.id === task.id);
    setSelectedTask(originalTask || task);
    setShowForm(true);
  };

  const handleSaveTask = async () => {
    await fetchTasks();
    setShowForm(false);
    setSelectedTask(null);
  };

  return (
    <div className="flex h-screen bg-gray-50">
      {/* Left Panel */}
      <div className="p-6 flex flex-col w-2/3">
        <div className="flex items-center justify-between mb-4 w-full">
          <h1 className="text-2xl font-bold">Today</h1>
          <span className="text-lg font-semibold">{todayTasks.length}</span>
        </div>

        <Button className="w-full justify-start mb-4" onClick={handleAddTask}>
          + Add New Task
        </Button>

        <div className="space-y-3 overflow-y-auto scrollbar-thin w-full">
          {todayTasks.map((task) => (
            <Card
              key={task.id}
              className="flex justify-between p-4 cursor-pointer hover:shadow-lg transition-shadow duration-200 w-full"
              onClick={() => {
                handleEditTask(task);
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
                  {["Important and Urgent","Important and NotUrgent","Urgent and NotImportant","NotImportant and NotUrgent"][task.priorityCategory]}
                </span>
              </div>
            </Card>
          ))}
        </div>
      </div>

      {/* Right Panel */}
      {showForm && (
        <div className="w-1/3 p-6 bg-white border-l shadow-md flex flex-col">
          <TaskForm
            task={selectedTask}
            onSave={handleSaveTask}
            onCancel={() => setShowForm(false)}
            lists={lists}
            defaultDueDate={new Date()}
          />
        </div>
      )}

      <DeleteConfirmationDialog
        open={deleteDialogOpen}
        onOpenChange={setDeleteDialogOpen}
        onConfirm={handleDelete}
      />
    </div>
  );
}
