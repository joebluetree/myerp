export interface AuthUser {
  id: string;
  userName: string;
  displayName: string;
  roles: string[];
}

export interface AuthSession {
  token: string;
  expiresAtUtc: string;
  user: AuthUser;
}
