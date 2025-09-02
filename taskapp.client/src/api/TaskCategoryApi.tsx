import { api } from "."
import type { ApiResponse, ListItem } from "../utils/types"

export class TaskCategoryApi {
  private readonly client

  constructor(client = api) {
    this.client = client
  }

  async getAllCategories(userId: string): Promise<ListItem[]> {
    try {
      const response = await this.client.get(`/categories/user/${userId}`)
      return response.data 
    } catch (error) {
      console.error("Failed to fetch categories:", error)
      throw error
    }
  }
    async createCategory(payload: { name: string; userId: string; color: string }) : Promise<ApiResponse>  {
  try {
      const res = await this.client.post<ApiResponse>("/categories/create", payload);
    return res.data;
      } catch (err: any) {
    console.error("create request failed:", err.response?.data || err);
    return err.response?.data || { isSuccess: false, message: "Unknown error" };
  }
  }
  async updateCategory( payload: {id: string, name: string; color: string }): Promise<ApiResponse>  {
  try {
    const res = await this.client.patch<ApiResponse>(`/categories/update/${payload.id}`, payload);
    return res.data;
  } catch (err: any) {
    console.error("Update request failed:", err.response?.data || err);
    return err.response?.data || { isSuccess: false, message: "Unknown error" };
  }
}
  async deleteCategory(categoryId: string) :  Promise<ApiResponse>  {
    const res = await this.client.delete<ApiResponse>(`/categories/delete/${categoryId}`);
    return res.data;}

}

export const taskCategoryApi = new TaskCategoryApi()
