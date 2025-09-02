import { useEffect } from "react"
import type { Task } from "../../utils/types"
import { Badge } from "../../components/ui/badge"
import { Card, CardContent, CardHeader, CardTitle } from "../../components/ui/card"
import { motion } from "framer-motion"
import { useTaskActions } from "../../hooks/use-task-actions"

export default function EisenhowerMatrixPage({
  onTaskCountChange,
}: Readonly<{ onTaskCountChange?: (count: number) => void }>) {
  const { tasks, fetchTasks } = useTaskActions({ onTaskCountChange })

  useEffect(() => {
    fetchTasks()
  }, [fetchTasks])

  // Classification logic
  const doTasks = tasks.filter((t: Task) => t.priorityCategory === 0)
  const scheduleTasks = tasks.filter((t: Task) => t.priorityCategory === 1)
  const delegateTasks = tasks.filter((t: Task) => t.priorityCategory === 2)
  const deleteTasks = tasks.filter((t: Task) => t.priorityCategory === 3)

  const renderTasks = (taskList: Task[], color: string) => {
    if (taskList.length === 0) {
      return <p className="text-sm text-gray-500 italic">No tasks</p>
    }
    return taskList.map((task) => (
      <motion.div
        key={task.id}
        initial={{ opacity: 0, y: 5 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.2 }}
        className="p-3 rounded-xl bg-gray-50 dark:bg-gray-800 shadow-sm mb-2"
      >
        <p className="font-medium text-gray-900 dark:text-gray-100">{task.title}</p>
        <Badge className={`mt-1 ${color}`}>
          {task.priorityCategory === 0 && "Important + Urgent"}
          {task.priorityCategory === 1 && "Important + Not Urgent"}
          {task.priorityCategory === 2 && "Urgent + Not Important"}
          {task.priorityCategory === 3 && "Not Urgent + Not Important"}
        </Badge>
      </motion.div>
    ))
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 p-6 min-h-[calc(100vh-4rem)]">
      {/* Do */}
      <Card className="bg-white dark:bg-gray-900 shadow-lg rounded-2xl flex flex-col">
        <CardHeader className="bg-red-100 rounded-t-2xl mb-3">
          <CardTitle className="text-red-800 font-bold">
            Do (Important + Urgent)
          </CardTitle>
        </CardHeader>
        <CardContent className="flex-1 max-h-full overflow-y-auto scrollbar-thin">
          {renderTasks(doTasks, "bg-red-100 text-red-800")}
        </CardContent>
      </Card>

      {/* Schedule */}
      <Card className="bg-white dark:bg-gray-900 shadow-lg rounded-2xl flex flex-col">
        <CardHeader className="bg-yellow-100 rounded-t-2xl mb-3">
          <CardTitle className="text-yellow-800 font-bold">
            Schedule (Important + Not Urgent)
          </CardTitle>
        </CardHeader>
        <CardContent className="flex-1 max-h-full overflow-y-auto scrollbar-thin">
          {renderTasks(scheduleTasks, "bg-yellow-100 text-yellow-800")}
        </CardContent>
      </Card>

      {/* Delegate */}
      <Card className="bg-white dark:bg-gray-900 shadow-lg rounded-2xl flex flex-col">
        <CardHeader className="bg-orange-100 rounded-t-2xl mb-3">
          <CardTitle className="text-orange-800 font-bold">
            Delegate (Urgent + Not Important)
          </CardTitle>
        </CardHeader>
        <CardContent className="flex-1 max-h-full overflow-y-auto scrollbar-thin">
          {renderTasks(delegateTasks, "bg-orange-100 text-orange-800")}
        </CardContent>
      </Card>

      {/* Delete */}
      <Card className="bg-white dark:bg-gray-900 shadow-lg rounded-2xl flex flex-col">
        <CardHeader className="bg-gray-100 rounded-t-2xl mb-3">
          <CardTitle className="text-gray-800 font-bold">
            Delete (Not Urgent + Not Important)
          </CardTitle>
        </CardHeader>
        <CardContent className="flex-1 max-h-full overflow-y-auto scrollbar-thin">
          {renderTasks(deleteTasks, "bg-gray-100 text-gray-800")}
        </CardContent>
      </Card>
    </div>
  )
}
