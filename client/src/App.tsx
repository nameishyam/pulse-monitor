import { BrowserRouter, Route, Routes } from "react-router-dom"
import { Toaster } from "@/components/ui/sonner"
import { ThemeProvider } from "@/context/ThemeContext"
import { AuthProvider } from "@/context/AuthContext"
import PublicRoute from "@/routes/PublicRoute"
import Signup from "@/pages/Signup"
import ProtectedRoute from "@/routes/ProtectedRoute"
import Dashboard from "@/pages/Dashboard"
import Login from "@/pages/Login"
import Landing from "@/pages/Landing"
import { TooltipProvider } from "./components/ui/tooltip"
import DashboardLayout from "./layout/DashboardLayout"
import PublicLayout from "./layout/PublicLayout"

export default function App() {
  return (
    <AuthProvider>
      <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
        <TooltipProvider>
          <BrowserRouter>
            <Routes>
              <Route element={<PublicLayout />}>
                <Route path="/" element={<Landing />} />
                <Route element={<PublicRoute />}>
                  <Route path="login" element={<Login />} />
                  <Route path="signup" element={<Signup />} />
                </Route>
              </Route>
              <Route element={<ProtectedRoute />}>
                <Route element={<DashboardLayout />}>
                  <Route path="dashboard" element={<Dashboard />} />
                </Route>
              </Route>
            </Routes>
          </BrowserRouter>
          <Toaster />
        </TooltipProvider>
      </ThemeProvider>
    </AuthProvider>
  )
}
