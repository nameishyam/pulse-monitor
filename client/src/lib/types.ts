export interface User {
  firstName?: string
  lastName?: string
  email?: string
  bio?: string
  profileUrl?: string
  createdAt?: string
}

export interface Monitor {
  id: string
  name: string
  url: string
  intervalSeconds: number
  requestBody: string
  httpMethod: string
  status: "up" | "down" | "unknown"
  lastCheckedAt: string
}

export interface AuthContextType {
  user: User | null
  monitors: Monitor[]
  setUser: React.Dispatch<React.SetStateAction<User | null>>
  setMonitors: React.Dispatch<React.SetStateAction<Monitor[]>>
  updateUser: (updates: Partial<User>) => void
  login: () => Promise<void>
  logout: () => Promise<void>
  isAuthenticated: () => boolean
  loading: boolean
}
