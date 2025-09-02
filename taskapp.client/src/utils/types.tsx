export interface RegisterCredentials {
  name: string;
  email: string;
  password: string;
}
export interface ApiError {
  code: string;
  description: string;
  type: number;
}

export interface ApiResponse<T = any> {
  isSuccess: boolean;
  isFailure?: boolean;
  message?: string;
  data?: T;
  error?: ApiError;
}
export interface LoginCredentials {
  email: string;
  password: string;
}
export interface LoggedInUser {
  id: string; 
  name: string;
  email: string;
}
export interface ListItem {
  id: string
  name: string
  color: string
  tasksCount?: number
}

export const PriorityCategoryValues = {
  ImportantUrgent: 0,
  ImportantNotUrgent: 1,
  UrgentNotImportant: 2,
  NotImportantNotUrgent: 3,
} as const;
export type PriorityCategory = typeof PriorityCategoryValues[keyof typeof PriorityCategoryValues];


export interface Task {
  id: string;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: Date;
  completedAt: Date | null;
  dueDate: Date | null;
  priorityCategory: PriorityCategory;
  listId: string;
  listName: string;
}
export interface TasksStasDto {
  totalTasks: number;
  completedTasks: number;
  overdueTasks: number;
  upComingTasks: number;
  todayTasks: number;
}


export interface CreateTaskDto {
  TaskId? : string;
  title: string;
  description: string;
  priorityCategory: PriorityCategory;
  listId: string;
  userId: string;
  dueDate: Date | null;
}
