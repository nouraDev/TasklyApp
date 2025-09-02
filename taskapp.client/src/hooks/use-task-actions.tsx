import { useState, useCallback } from "react";
import { toast } from "sonner";
import { taskApi } from "../api/TaskApi";
import type { Task } from "../utils/types";

interface UseTaskActionsProps {
  initialTasks?: Task[];
  onTaskCountChange?: (count: number) => void;
}

export function useTaskActions({ initialTasks = [], onTaskCountChange }: UseTaskActionsProps) {
  const [tasks, setTasks] = useState<Task[]>(initialTasks);
  const [deleteTaskId, setDeleteTaskId] = useState<string | null>(null);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);

  const updateTaskCount = useCallback((taskList: Task[]) => {
    if (onTaskCountChange) onTaskCountChange(taskList.length);
  }, [onTaskCountChange]);

  const toggleComplete = useCallback(async (task: Task, checked: boolean) => {
    const user = JSON.parse(localStorage.getItem("user") || "{}");
    try {
      const response = await taskApi.completeTask(task.id, {
        taskId: task.id,
        userId: user.id,
      });

      if (response.isSuccess) {
        setTasks((prev) =>
          prev.map((t) =>
            t.id === task.id
              ? { ...t, isCompleted: checked, completedAt: checked ? new Date() : null }
              : t
          )
        );
        toast.success(checked ? "Task completed" : "Task marked incomplete");
      } else {
        toast.error(response.error?.description || response.message || "Failed to update task");
      }
    } catch (err) {
      console.error("Failed to complete task:", err);
      toast.error("Something went wrong while updating the task");
    }
  }, []);

  const handleDelete = useCallback(async () => {
    if (!deleteTaskId) return;
    try {
      await taskApi.deleteTask(deleteTaskId);
      setTasks((prev) => prev.filter((t) => t.id !== deleteTaskId));
      setDeleteDialogOpen(false);
      setDeleteTaskId(null);
      updateTaskCount(tasks.filter((t) => t.id !== deleteTaskId));
    } catch (err) {
      console.error("Failed to delete task:", err);
    }
  }, [deleteTaskId, tasks, updateTaskCount]);

  const fetchTasks = useCallback(async () => {
    const user = JSON.parse(localStorage.getItem("user") || "{}");
    const userId = user.id;
    if (!userId) return;

    const data = await taskApi.getTasksForUser(userId);
    if (data && Array.isArray(data)) {
      const tasksWithDate: Task[] = data.map((t) => ({
        ...t,
        dueDate: t.dueDate ? new Date(t.dueDate) : null,
        createdAt: t.createdAt ? new Date(t.createdAt) : new Date(),
        completedAt: t.completedAt ? new Date(t.completedAt) : null,
      }));
      setTasks(tasksWithDate);
      updateTaskCount(tasksWithDate);
    }
  }, [updateTaskCount]);

  return {
    tasks,
    setTasks,
    deleteTaskId,
    setDeleteTaskId,
    deleteDialogOpen,
    setDeleteDialogOpen,
    toggleComplete,
    handleDelete,
    fetchTasks,
  };
}
