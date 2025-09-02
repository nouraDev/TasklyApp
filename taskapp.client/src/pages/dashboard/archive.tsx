import { useEffect, useState } from "react"
import type { Task } from "../../utils/types"
import { Card, CardContent, CardHeader, CardTitle } from "../../components/ui/card"
import { motion } from "framer-motion"
import { useTaskActions } from "../../hooks/use-task-actions"
import { Checkbox } from "../../components/ui/checkbox"
import { format } from "date-fns"
import { Trophy, Flag } from "lucide-react"
import { taskApi } from "../../api/TaskApi"

export default function ArchivePage() {
  const {tasks, toggleComplete } = useTaskActions({})

  const [overdueTasks, setOverdueTasks] = useState<Task[]>([])
  const [completedTasks, setCompletedTasks] = useState<Task[]>([])

  const fetchArchivedTasks = async () => {
    const user = JSON.parse(localStorage.getItem("user") || "null");
    
    try {
      const data = await taskApi.getArchivedTasks(user.id)
      const now = new Date()

      setOverdueTasks(
        data.filter((t) => !t.isCompleted && t.dueDate && t.dueDate < now)
      )
      setCompletedTasks(data.filter((t) => t.isCompleted))
    } catch (err) {
      console.error("Failed to fetch archived tasks:", err)
    }
  }

  useEffect(() => {
    fetchArchivedTasks()
  }, [])

  useEffect(() => {
    const now = new Date()
    setOverdueTasks(
      tasks.filter(
        (t) => !t.isCompleted && t.dueDate && t.dueDate < now
      )
    )
    setCompletedTasks(tasks.filter((t) => t.isCompleted))
  }, [tasks])

  const handleComplete = async (task: Task) => {
    await toggleComplete(task, !task.isCompleted)
  }

  const renderOverdue = () => {
    if (overdueTasks.length === 0) {
      return <p className="text-sm text-gray-500 italic">No overdue tasks 🎉</p>
    }

    return overdueTasks.map((task) => (
      <motion.div
        key={task.id}
        initial={{ opacity: 0, y: 5 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.2 }}
        className="p-4 rounded-xl bg-red-50 dark:bg-red-900/30 shadow mb-3 flex justify-between items-center"
      >
        <div>
          <p className="font-medium text-gray-900 dark:text-gray-100">{task.title}</p>
          {task.dueDate && (
            <p className="text-sm text-red-600 flex items-center gap-1">
              <Flag size={14} className="text-red-500" /> Due:{" "}
              {format(task.dueDate, "PPpp")}
            </p>
          )}
        </div>
        <Checkbox
          onCheckedChange={() => handleComplete(task)}
          aria-label="Mark as completed"
        />
      </motion.div>
    ))
  }

  const renderCompleted = () => {
    if (completedTasks.length === 0) {
      return <p className="text-sm text-gray-500 italic">No completed tasks yet</p>
    }

    return completedTasks.map((task) => (
      <motion.div
        key={task.id}
        initial={{ opacity: 0, y: 5 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.3 }}
        className="p-4 rounded-xl bg-green-50 dark:bg-green-900/30 shadow mb-3 flex flex-col"
      >
        <div className="flex items-center justify-between">
          <p className="font-semibold text-green-800 dark:text-green-200 flex items-center gap-2">
            <Trophy className="text-yellow-500" size={18} />
            {task.title}
          </p>
        </div>
        {task.completedAt && (
          <p className="text-xs text-gray-600 dark:text-gray-400 mt-1">
            Completed on: {format(task.completedAt, "PPpp")}
          </p>
        )}
      </motion.div>
    ))
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-6 p-6 min-h-[calc(100vh-4rem)]">
      {/* Overdue Tasks */}
      <Card className="bg-white dark:bg-gray-900 shadow-lg rounded-2xl flex flex-col">
        <CardHeader className="bg-red-100 dark:bg-red-800 rounded-t-2xl mb-3">
          <CardTitle className="text-red-800 dark:text-red-100 font-bold">
            ⏰ Overdue Tasks
          </CardTitle>
        </CardHeader>
        <CardContent className="flex-1 max-h-full overflow-y-auto scrollbar-thin">
          {renderOverdue()}
        </CardContent>
      </Card>

      {/* Completed Tasks */}
      <Card className="bg-white dark:bg-gray-900 shadow-lg rounded-2xl flex flex-col">
        <CardHeader className="bg-green-100 dark:bg-green-800 rounded-t-2xl mb-3">
          <CardTitle className="text-green-800 dark:text-green-100 font-bold">
            🏆 Completed Tasks
          </CardTitle>
        </CardHeader>
        <CardContent className="flex-1 max-h-full overflow-y-auto scrollbar-thin">
          {renderCompleted()}
        </CardContent>
      </Card>
    </div>
  )
}
