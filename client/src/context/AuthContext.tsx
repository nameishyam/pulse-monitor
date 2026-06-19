import {
  createContext,
  useContext,
  useState,
  useCallback,
  useMemo,
  type ReactNode,
  useEffect,
} from "react"
import { api } from "@/lib/api"
import type { User, AuthContextType, Monitor } from "@/lib/types"

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(null)
  const [monitors, setMonitors] = useState<Monitor[]>([])
  const [loading, setLoading] = useState<boolean>(true)

  const fetchMe = useCallback(async () => {
    try {
      const res = await api.get("/auth/me")
      setUser(res.data.user || null)
      setMonitors(res.data.monitors || [])
    } catch (err: any) {
      if (err.response?.status === 401) {
        setUser(null)
        setMonitors([])
      } else {
        console.error("Unexpected /me error:", err)
      }
    }
  }, [])

  useEffect(() => {
    fetchMe().finally(() => setLoading(false))
  }, [fetchMe])

  const login = useCallback(async () => {
    await fetchMe()
  }, [fetchMe])

  const logout = useCallback(async () => {
    try {
      await api.post("/auth/logout")
    } catch (err) {
      console.log(err)
    }
    setUser(null)
    setMonitors([])
  }, [])

  const updateUser = useCallback((updates: Partial<User>) => {
    setUser((prev) => (prev ? { ...prev, ...updates } : null))
  }, [])

  const isAuthenticated = useCallback(() => !!user, [user])

  const value = useMemo(
    () => ({
      user,
      updateUser,
      login,
      logout,
      isAuthenticated,
      setUser,
      setMonitors,
      monitors,
      loading,
    }),
    [user, monitors, login, logout, isAuthenticated, updateUser, loading]
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export const useAuth = () => {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error("useAuth must be used inside AuthProvider")
  return ctx
}
