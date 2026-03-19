import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface SessionData {
  userId: string;
  userName: string;
  userRole: string;
}

@Injectable({ providedIn: 'root' })
export class SessionService {
  private readonly storageKey = 'app.session';
  private readonly sessionSubject = new BehaviorSubject<SessionData | null>(this.readFromStorage());

  get sessionChanges$(): Observable<SessionData | null> {
    return this.sessionSubject.asObservable();
  }

  getSession(): SessionData | null {
    return this.sessionSubject.value;
  }

  hasSession(): boolean {
    return this.sessionSubject.value !== null;
  }

  setSession(session: SessionData): void {
    this.sessionSubject.next(session);
    localStorage.setItem(this.storageKey, JSON.stringify(session));
  }

  updateRole(userRole: string): void {
    const current = this.sessionSubject.value;
    if (!current) {
      return;
    }

    const updated: SessionData = {
      ...current,
      userRole,
    };

    this.sessionSubject.next(updated);
    localStorage.setItem(this.storageKey, JSON.stringify(updated));
  }

  clearSession(): void {
    this.sessionSubject.next(null);
    localStorage.removeItem(this.storageKey);
  }

  private readFromStorage(): SessionData | null {
    const raw = localStorage.getItem(this.storageKey);
    if (!raw) {
      return null;
    }

    try {
      const parsed = JSON.parse(raw) as Partial<SessionData>;
      if (!parsed.userId || !parsed.userName || !parsed.userRole) {
        return null;
      }

      return {
        userId: parsed.userId,
        userName: parsed.userName,
        userRole: parsed.userRole,
      };
    } catch {
      localStorage.removeItem(this.storageKey);
      return null;
    }
  }
}
