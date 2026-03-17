import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  CreateUserRequest,
  DeleteUserRequest,
  LoginUserRequest,
  UpdateUserRequest,
  UserResponse,
  UsersResponse,
} from '../models/user.models';

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly baseUrl = `${environment.apiUrl}/users`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<UsersResponse> {
    return this.http.get<UsersResponse>(this.baseUrl);
  }

  getByName(name: string): Observable<UserResponse> {
    return this.http.get<UserResponse>(`${this.baseUrl}/${name}`);
  }

  create(request: CreateUserRequest): Observable<UserResponse> {
    return this.http.post<UserResponse>(this.baseUrl, request);
  }

  login(request: LoginUserRequest): Observable<UserResponse> {
    return this.http.post<UserResponse>(`${this.baseUrl}/login`, request);
  }

  update(name: string, request: UpdateUserRequest): Observable<UserResponse> {
    return this.http.put<UserResponse>(`${this.baseUrl}/${name}`, request);
  }

  delete(request: DeleteUserRequest): Observable<void> {
    return this.http.delete<void>(this.baseUrl, { body: request });
  }
}
