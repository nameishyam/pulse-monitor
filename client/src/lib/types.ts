export interface User {
  firstName?: string
  lastName?: string
  email?: string
  bio?: string
  profile_url?: string
  createdAt?: string
}

export interface AuthContextType {
  user: User | null
  updateUser: (updates: Partial<User>) => void
  login: () => Promise<void>
  logout: () => Promise<void>
  isAuthenticated: () => boolean
  loading: boolean
}
