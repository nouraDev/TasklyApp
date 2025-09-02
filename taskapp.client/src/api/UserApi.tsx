import { api } from "./index";
import type { RegisterCredentials, ApiResponse, LoginCredentials, LoggedInUser } from "../utils/types";

export class UserApi {
  private readonly client;

  constructor(client = api) {
    this.client = client;
  }

async registerUser(request: RegisterCredentials): Promise<ApiResponse> {
  try {
    const res = await this.client.post("/users/register", request);
    return res.data;
  } catch (err: any) {
    console.error("Register request failed:", err.response?.data || err);
    return err.response?.data || { isSuccess: false, message: "Unknown error" };
  }
}
  async login(request: LoginCredentials): Promise<ApiResponse<LoggedInUser>> {
    try {
      const res = await this.client.post<ApiResponse<LoggedInUser>>("/users/login", request);
      return res.data; 
    } catch (err: any) {
      console.error("Login request failed:", err.response?.data || err);
      return err.response?.data || { isSuccess: false, message: "Unknown error" };
    }
  }
}

export const userApi = new UserApi();
