import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private readonly SESSION_ID_KEY = 'sessionId';

  constructor() { }

  setSessionId(sessionId: string): void {
    localStorage.setItem(this.SESSION_ID_KEY, sessionId);
  }

  getSessionId(): string | null {
    return localStorage.getItem(this.SESSION_ID_KEY);
  }

  clearSessionId(): void {
    localStorage.removeItem(this.SESSION_ID_KEY);
  }
}
