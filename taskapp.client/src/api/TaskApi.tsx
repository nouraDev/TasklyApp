import { api } from "."
import type { ApiResponse, CreateTaskDto, Task, TasksStasDto } from "../utils/types"

export class TaskApi {
  private readonly client

  constructor(client = api) {
    this.client = client
  }

  async getTasksForUser(userId: string): Promise<Task[]> {
    try {
      const res = await this.client.get<Task[]>(`/tasks/user`, {
        params: { userId },
      });
      return res.data;
    } catch (err: any) {
      console.error("Fetching tasks failed:", err.response?.data || err);
      return err.response?.data || { isSuccess: false, message: "Unknown error", data: [] };
    }
    }
    async getArchivedTasks(userId: string): Promise<Task[]> {
      try {
        const res = await this.client.get<Task[]>("/tasks/archived", {
          params: { userId },
        });
        return res.data;
      } catch (err: any) {
        console.error("Fetching archived tasks failed:", err.response?.data || err);
        return err.response?.data || { isSuccess: false, message: "Unknown error", data: [] };
      }
    }

    async getTaskStats(userId: string): Promise<TasksStasDto | null> {
    try {
      const res = await this.client.get<TasksStasDto>("/tasks/stats", {
        params: { userId },
      });
      return res.data || null;
    } catch (err: any) {
      console.error("Fetching task stats failed:", err.response?.data || err);
      return null;
    }
  }

  async createTask(payload: CreateTaskDto): Promise<ApiResponse> {
    try {
      const res = await this.client.post<ApiResponse>("/tasks/create", payload);
      return res.data;
    } catch (err: any) {
      console.error("Task create request failed:", err.response?.data || err);
      return (
        err.response?.data || { isSuccess: false, message: "Unknown error" }
      );
    }
  }
    async updateTask(taskId: string, payload: Partial<CreateTaskDto>): Promise<ApiResponse<Task>> {
    try {
      const res = await this.client.put<ApiResponse<Task>>(`/tasks/update/${taskId}`, payload);
      return res.data;
    } catch (err: any) {
      console.error("Task update request failed:", err.response?.data || err);
      return err.response?.data || { isSuccess: false, message: "Unknown error" };
    }
  }
  async deleteTask(taskId: string, ): Promise<ApiResponse> {
    try {
      const res = await this.client.delete<ApiResponse>(`/tasks/delete/${taskId}`);
      return res.data;
    } catch (err: any) {
      console.error("Task delete request failed:", err.response?.data || err);
      return err.response?.data || { isSuccess: false, message: "Unknown error" };
    }
  }
    async completeTask(taskId: string, payload: { taskId: string; userId: string }): Promise<ApiResponse<void>> {
    try {
      const res = await this.client.put<ApiResponse<void>>(`/tasks/${taskId}/complete`, payload);
      return res.data;
    } catch (err: any) {
      console.error("Complete task request failed:", err.response?.data || err);
      return err.response?.data || { isSuccess: false, message: "Unknown error" };
    }
  }
}

export const taskApi = new TaskApi()
