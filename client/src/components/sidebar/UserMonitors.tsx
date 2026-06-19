import {
  SidebarGroup,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarMenuSub,
  SidebarMenuSubButton,
  SidebarMenuSubItem,
} from "@/components/ui/sidebar"
import { ChevronRight, MonitorIcon } from "lucide-react"
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from "@/components/ui/collapsible"
import type { Monitor } from "@/lib/types"
import type { Dispatch, SetStateAction } from "react"

interface UserMonitorsProps {
  monitors: Monitor[]
  setMonitor: Dispatch<SetStateAction<Monitor | null>>
}

export function UserMonitors({ monitors, setMonitor }: UserMonitorsProps) {
  return (
    <SidebarGroup>
      <SidebarMenu>
        <Collapsible defaultOpen className="group/collapsible">
          <SidebarMenuItem>
            <CollapsibleTrigger asChild>
              <SidebarMenuButton>
                <MonitorIcon />
                <span>Monitors</span>
                <ChevronRight className="ml-auto transition-transform duration-200 group-data-[state=open]/collapsible:rotate-90" />
              </SidebarMenuButton>
            </CollapsibleTrigger>

            <CollapsibleContent>
              <SidebarMenuSub>
                {monitors.map((monitor) => (
                  <SidebarMenuSubItem key={monitor.id}>
                    <SidebarMenuSubButton
                      className="hover:cursor-pointer"
                      onClick={() => setMonitor(monitor)}
                    >
                      {monitor.name}
                    </SidebarMenuSubButton>
                  </SidebarMenuSubItem>
                ))}
              </SidebarMenuSub>
            </CollapsibleContent>
          </SidebarMenuItem>
        </Collapsible>
      </SidebarMenu>
    </SidebarGroup>
  )
}
