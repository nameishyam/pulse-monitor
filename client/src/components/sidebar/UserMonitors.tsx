import {
  SidebarGroup,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuItem,
} from "@/components/ui/sidebar"
import { useAuth } from "@/context/AuthContext"

export function UserMonitors() {
  const { monitors } = useAuth()

  return (
    <SidebarGroup>
      <SidebarGroupLabel>Monitors</SidebarGroupLabel>
      <SidebarMenu>
        {monitors.map((monitor) => (
          <SidebarMenuItem key={monitor.id}>{monitor.name}</SidebarMenuItem>
        ))}
      </SidebarMenu>
    </SidebarGroup>
  )
}
