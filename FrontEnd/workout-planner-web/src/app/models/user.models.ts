import { UserRole } from './enums';

export interface CreateUserRequest {
  password: string;
  name: string;
  surname: string;
  email: string;
  role: UserRole;
}

export interface UpdateUserRequest {
  userId: string;
  password?: string;
  name?: string;
  surname?: string;
  email?: string;
  role?: UserRole;
}

export interface DeleteUserRequest {
  name: string;
}

export interface UserExistsRequest {
  name: string;
}

export interface GetUserByNameRequest {
  name: string;
}

export interface UserResponse {
  id: string;
  name: string;
  email: string;
  role: UserRole;
}

export interface UsersResponse {
  users: UserResponse[];
}
