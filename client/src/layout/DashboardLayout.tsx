import { Outlet } from "react-router-dom"
import { SidebarProvider } from "@/components/ui/sidebar"
import { AppSidebar } from "@/components/AppSidebar"
import { useAuth } from "@/context/AuthContext"
import { useState } from "react"
import { type Monitor } from "@/lib/types"

export default function DashboardLayout() {
  const { user, monitors, logout } = useAuth()
  const [monitor, setMonitor] = useState<Monitor | null>(null)

  return (
    <SidebarProvider>
      <AppSidebar
        user={user}
        monitors={monitors}
        logout={logout}
        setMonitor={setMonitor}
      />
      <main className="flex-1">
        <Outlet context={{ monitor }} />
      </main>
    </SidebarProvider>
  )
}
