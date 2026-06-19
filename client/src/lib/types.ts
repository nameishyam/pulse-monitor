import type { Dispatch, SetStateAction } from "react"

export interface User {
  firstName?: string
  lastName?: string
  email?: string
}

export interface Monitor {
  id: string
  name: string
  url: string
  intervalSeconds: number
  requestBody: string
  httpMethod: string
  status: "up" | "down" | "unknown"
  logs: Log[]
  lastCheckedAt: string
}

export interface Log {
  responseTime?: number
  statusCode?: number
  errorMessage?: string
  createdAt?: string
}

export interface AuthContextType {
  user: User | null
  monitors: Monitor[]
  setUser: Dispatch<SetStateAction<User | null>>
  setMonitors: Dispatch<SetStateAction<Monitor[]>>
  updateUser: (updates: Partial<User>) => void
  login: () => Promise<void>
  logout: () => Promise<void>
  isAuthenticated: () => boolean
  loading: boolean
}
