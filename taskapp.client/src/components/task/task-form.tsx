import React, { useEffect, useState } from "react";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../ui/select";
import { Button } from "../ui/button";
import { Textarea } from "../ui/textarea";
import {
  type PriorityCategory,
  type CreateTaskDto,
  type ListItem,
  type Task,
  PriorityCategoryValues
} from "../../utils/types";
import { Input } from "../ui/input";
import { toast } from "sonner";
import { taskApi } from "../../api/TaskApi";
import { SelectViewport } from "@radix-ui/react-select";

interface Props {
  task: Task | null;
  onSave: () => void;
  onCancel: () => void;
  lists: ListItem[];
  defaultDueDate?: Date | null; 
}


const TaskForm: React.FC<Props> = ({ task, onSave, onCancel, lists,defaultDueDate }) => {
  const [title, setTitle] = useState(task?.title || "");
  const [description, setDescription] = useState(task?.description || "");
  const [dueDate, setDueDate] = useState(
    task?.dueDate
      ? new Date(task.dueDate).toISOString().slice(0, 16)
      : defaultDueDate
      ? defaultDueDate.toISOString().slice(0, 16)
      : ""
  );

  const [priority, setPriority] = useState<PriorityCategory>(
    task?.priorityCategory ?? PriorityCategoryValues.ImportantUrgent
  );

  const [listId, setListId] = useState(task?.listId || "");

  const priorityOptions = [
    { label: "ImportantUrgent", value: "0" },
    { label: "ImportantNotUrgent", value: "1" },
    { label: "UrgentNotImportant", value: "2" },
    { label: "NotImportantNotUrgent", value: "3" },
  ];

useEffect(() => {
  setTitle(task?.title || "");
  setDescription(task?.description || "");
  setDueDate(
    task?.dueDate
      ? new Date(task.dueDate).toISOString().slice(0, 16)
      : defaultDueDate
      ? defaultDueDate.toISOString().slice(0, 16)
      : ""
  );
  
  setPriority(task?.priorityCategory || PriorityCategoryValues.ImportantUrgent);
  setListId(task?.listId || "");
}, [task?.id, defaultDueDate]);


  const handleSubmit = async () => {
    const user = JSON.parse(localStorage.getItem("user") || "{}");

    const payload: CreateTaskDto = {
      TaskId: task?.id || undefined,
      title,
      description,
      dueDate: dueDate ? new Date(dueDate) : null,
      priorityCategory: priority,
      listId,
      userId: user.id || "",
    };
    let response;

    if (task) {
      response = await taskApi.updateTask(task.id, payload);
    } else {
      response = await taskApi.createTask(payload);
    }

    if (response.isSuccess) {
      onSave();
      toast.success(task ? "Task updated successfully" : "Task created successfully");
    } else if (response.error) {
      toast.error(response.error.description || "Failed to save task");
    } else {
      toast.error(response.message || "Failed to save task");
    }
  };

  return (
    <div className="space-y-4 ">
      <h2 className="text-xl font-semibold">
        {task ? "Edit Task" : "New Task"}
      </h2>

      {/* Title */}
      <Input
        placeholder="Title"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
      />

      {/* Description */}
      <Textarea
        placeholder="Description"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        className="min-h-[150px]"
      />

      {/* Section (List) Select */}
      <Select value={listId} onValueChange={(val) => setListId(val)}>
        <SelectTrigger className="bg-white dark:bg-gray-800 border rounded-md px-3 py-2">
          <SelectValue placeholder="Select a list (section)" />
        </SelectTrigger>
        <SelectContent className="bg-white dark:bg-gray-800">
          {lists.map((list) => (
            <SelectItem key={list.id} value={list.id}>
              <div className="flex items-center gap-2">
                <span
                  className="w-3 h-3 rounded-full"
                  style={{ backgroundColor: list.color }}
                />
                {list.name}
              </div>
            </SelectItem>
          ))}
        </SelectContent>
      </Select>

      {/* Priority Select */}
      <Select
        value={priority.toString()}
        onValueChange={(val: string) =>
          setPriority(parseInt(val) as PriorityCategory) 
        }
      >
        <SelectTrigger className="bg-white border border-gray-300 rounded-md">
          <SelectValue placeholder="Select priority" />
        </SelectTrigger>
        <SelectContent className="bg-white">
          <SelectViewport>
            {priorityOptions.map((option) => (
              <SelectItem
                key={option.value}
                value={option.value}
                className="px-3 py-2 rounded-md radix-highlighted:bg-gray-200"
              >
                {option.label.replace(/([A-Z])/g, " $1").trim()}
              </SelectItem>
            ))}
          </SelectViewport>
        </SelectContent>
      </Select>

      {/* Due Date & Time */}
      <Input
        type="datetime-local"
        value={dueDate}
        onChange={(e) => setDueDate(e.target.value)}
      />

      <div className="flex justify-end gap-3 pt-2">
        <Button onClick={onCancel}>Cancel</Button>
        <Button
          onClick={handleSubmit}
          className="bg-yellow-400 text-black hover:bg-yellow-500"
        >
          {task ? "Update Task" : "Save Task"}
        </Button>
      </div>
    </div>
  );
};

export default TaskForm;
