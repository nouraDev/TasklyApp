import { NavLink, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { Plus, Settings, LogOut, Home, MoreVertical } from "lucide-react";
import { Button } from "./ui/button";
import { taskCategoryApi } from "../api/TaskCategoryApi";
import { toast } from "sonner";
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "./ui/dropdown-menu";
import { DeleteConfirmationDialog } from "./delete-confirmation-dialog";
import { CategoryFormDialog } from "./categories/categoryFormDialog";
import type { TasksStasDto } from "../utils/types";
import { taskApi } from "../api/TaskApi";

interface ListItem {
  id: string;
  name: string;
  color: string;
  count: number;
}
interface SidebarProps {
  onListsLoaded: (lists: ListItem[]) => void;
}

export function Sidebar({ onListsLoaded  }: SidebarProps) {
  const navigate = useNavigate();
  const [lists, setLists] = useState<ListItem[]>([]);
  const [editList, setEditList] = useState<ListItem | null>(null);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [deleteConfirmationOpen, setDeleteConfirmationOpen] = useState(false);
  const [listToDelete, setListToDelete] = useState<string | null>(null);
  const [taskStats, setTaskStats] = useState<TasksStasDto | null>(null);

  const fetchStats = async () => {
    try {
      const user = JSON.parse(localStorage.getItem("user") || "{}");
      if (!user.id) return;

      const stats = await taskApi.getTaskStats(user.id); 
      console.log("Fetched task stats:", stats);
      setTaskStats(stats);
    } catch (err) {
      console.error("Failed to fetch task stats:", err);
    }
  };

  const fetchLists = async () => {
    try {
      const user = JSON.parse(localStorage.getItem("user") || "{}");
      const userId = user.id;
      if (!userId) {
        toast.error("User ID not found in localStorage");
        return;
      }

      const categories = await taskCategoryApi.getAllCategories(userId);
      const fetchedLists = categories.map((category: { id: string; name: string; color: string; tasksCount?: number }) => ({
        id: category.id,
        name: category.name,
        color: category.color,
        count: category.tasksCount || 0,
      }));
      setLists(fetchedLists);
      onListsLoaded(fetchedLists); 

      toast.success("Lists fetched successfully");
    } catch (error) {
      toast.error("Failed to fetch lists");
      console.error("Failed to fetch lists:", error);
    }
  };

  useEffect(() => {
    fetchLists();
    fetchStats();
  }, []);

  const handleCreateList = async (data: { name: string; color: string }) => {
    try {
      const user = JSON.parse(localStorage.getItem("user") || "{}");
      if (!user.id) {
        toast.error("User ID not found in localStorage");
        return;
      }

      const response = await taskCategoryApi.createCategory({
        name: data.name,
        userId: user.id,
        color: data.color,
      });
      console.log(response);
      if(response.isSuccess){   
        toast.success("Section Created successfully");  
        fetchLists();

      } else if (response.error) {
        toast.error(response.error.description || "Failed to create section");
      } else {
        toast.error(response.message || "Failed to create section");
      }
    } catch (error) {
      toast.error("Failed to create section");
      console.error("Failed to create section:", error);
    }
  };

  const handleEditList = async (data: { name: string; color: string }) => {
    try {
      if (!editList) {
        toast.error("No list selected for editing");
        return;
      }

      const response = await taskCategoryApi.updateCategory({
        id: editList.id,
        name: data.name,
        color: data.color,
      });
      if(response.isSuccess){   
        toast.success("Section Updated successfully");   
        setEditList(null);
        fetchLists();
      } else if (response.error) {
        toast.error(response.error.description || "Failed to update section");
      } else {
        toast.error(response.message || "Failed to update section");
      }

    } catch (error) {
      toast.error("Failed to update section");
      console.error("Failed to update section:", error);}
  };

  const handleDeleteList = async () => {
    try {
      if (!listToDelete) {
        toast.error("No list selected for deletion");
        return;
      }
      const response = await taskCategoryApi.deleteCategory(listToDelete);
      
      if(response.isSuccess){   
        toast.success("List deleted successfully");   
        fetchLists();
      } else if (response.error) {
        toast.error(response.error.description || "Failed to delete section");
      } else {
        toast.error(response.message || "Failed to delete section");
      }
    } catch (error) {
      toast.error("Failed to delete section");
      console.error("Failed to delete section :", error);
    } finally {
      setDeleteConfirmationOpen(false);
      setListToDelete(null);
    }
  };

  const handleSignOut = () => {
    localStorage.removeItem("user");
    navigate("/");
  };

  return (
    <aside className="w-64 h-screen bg-white dark:bg-gray-800 border-r p-4 flex flex-col">
      <div className="flex items-center justify-between mb-4">
        <h2 className="text-lg font-semibold">Menu</h2>
        <Button variant="ghost" size="icon">
          <Home className="h-5 w-5" />
        </Button>
      </div>
      {/* Search */}
      <input
        type="text"
        placeholder="Search"
        className="mb-4 p-2 rounded border w-full text-sm dark:bg-gray-700 dark:text-white"
      />
      {/* Tasks Section */}
      <div className="mb-4">
        <h3 className="text-xs font-semibold text-gray-400 uppercase mb-2">
          Tasks 
        </h3>
        <nav className="space-y-1">
          <NavLink
            to="/taskly/upcoming"
            className={({ isActive }) =>
              `flex justify-between p-2 rounded hover:bg-gray-200 dark:hover:bg-gray-700 ${
                isActive ? "bg-gray-200 dark:bg-gray-700" : ""
              }`
            }
          >
            <span>Upcoming</span>
            <span className="text-gray-500">{taskStats?.upComingTasks}</span>
          </NavLink>

          <NavLink
            to="/taskly/today"
            className={({ isActive }) =>
              `flex justify-between p-2 rounded hover:bg-gray-200 dark:hover:bg-gray-700 ${
                isActive ? "bg-gray-200 dark:bg-gray-700" : ""
              }`
            }
          >
            <span>Today</span>
            <span className="text-gray-500">{taskStats?.todayTasks}</span>
          </NavLink>

          <NavLink
            to="/taskly/eisenhower-matrix"
            className={({ isActive }) =>
              `flex justify-between p-2 rounded hover:bg-gray-200 dark:hover:bg-gray-700 ${
                isActive ? "bg-gray-200 dark:bg-gray-700" : ""
              }`
            }
          >
            <span>Eisenhower Matrix</span>
            <span className="text-gray-500">{taskStats?.totalTasks}</span>
          </NavLink>

          <NavLink
            to="/taskly/archive"
            className={({ isActive }) =>
              `flex items-center p-2 rounded hover:bg-gray-200 dark:hover:bg-gray-700 ${
                isActive ? "bg-gray-200 dark:bg-gray-700" : ""
              }`
            }
          >
            <span>Archive</span>
          </NavLink>
        </nav>
      </div>

      {/* Lists Section */}
      <div className="flex-1 overflow-y-auto">
        <h3 className="text-xs font-semibold text-gray-400 uppercase mb-2">Sections</h3>
        {lists.map((list) => (
          <div key={list.id} className="flex items-center justify-between">
            <span
              className="flex items-center gap-2 p-2 rounded hover:bg-gray-200 dark:hover:bg-gray-700 flex-1"
            >
              <span
                className="w-2 h-2 rounded-full"
                style={{ backgroundColor: list.color }}
              />
              {list.name}
            </span>
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button className="p-1 rounded">
                  <MoreVertical size={14} />
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent className="bg-white dark:bg-gray-800">
                <DropdownMenuItem
                  onClick={() => {
                    setEditList(list);
                    setDialogOpen(true);
                  }}
                >
                  Edit
                </DropdownMenuItem>
                <DropdownMenuItem
                  onClick={() => {
                    setListToDelete(list.id);
                    setDeleteConfirmationOpen(true);
                  }}
                  className="text-red-500"
                >
                  Delete
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>
        ))}

        <Button
          onClick={() => {
            setEditList(null);
            setDialogOpen(true);
          }}
          className="flex items-center gap-2 p-2 rounded text-gray-600 hover:bg-gray-200 dark:hover:bg-gray-700 text-sm w-full justify-start"
          variant="ghost"
        >
          <Plus size={16} /> 
          Add New List
        </Button>
      </div>

      {/* Dialog */}
      <CategoryFormDialog
        open={dialogOpen}
        onOpenChange={setDialogOpen}
        onSubmit={editList ? handleEditList : handleCreateList}
        initialData={
          editList
            ? {
                name: editList.name,
                color: editList.color,
              }
            : undefined
        }
        mode={editList ? "edit" : "create"}
      />

      {/* Delete Confirmation Dialog */}
      <DeleteConfirmationDialog
        open={deleteConfirmationOpen}
        onOpenChange={setDeleteConfirmationOpen}
        onConfirm={handleDeleteList}
      />

      {/* Footer */}
      <div className="mt-auto flex flex-col space-y-2">
        <NavLink to="/dashboard/settings" className="flex items-center gap-2 p-2 rounded hover:bg-gray-200 dark:hover:bg-gray-700">
          <Settings size={16} /> Settings
        </NavLink>
        <button onClick={handleSignOut} className="flex items-center gap-2 p-2 rounded hover:bg-gray-200 dark:hover:bg-gray-700">
          <LogOut size={16} /> Sign out
        </button>
      </div>
    </aside>
  );
}
