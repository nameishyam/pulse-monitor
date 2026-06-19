import { UserSidebar } from "@/components/sidebar/UserSidebar"
import { UserMonitors } from "@/components/sidebar/UserMonitors"
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarRail,
} from "@/components/ui/sidebar"
import { Button } from "./ui/button"
import {
  useState,
  type ComponentProps,
  type Dispatch,
  type SetStateAction,
} from "react"
import AddMonitor from "./sidebar/AddMonitor"
import { PlusIcon } from "lucide-react"
import type { Monitor, User } from "@/lib/types"

interface AppSidebarProps extends ComponentProps<typeof Sidebar> {
  user: User | null
  monitors: Monitor[]
  logout: () => Promise<void>
  setMonitor: Dispatch<SetStateAction<Monitor | null>>
}

export function AppSidebar({
  user,
  monitors,
  logout,
  setMonitor,
  ...props
}: AppSidebarProps) {
  const [isOpen, setIsOpen] = useState<boolean>(false)

  return (
    <Sidebar collapsible="icon" {...props}>
      <SidebarHeader className="pt-4">
        <Button
          onClick={() => {
            setIsOpen(!isOpen)
          }}
          className="justify-start group-data-[collapsible=icon]:justify-center group-data-[collapsible=icon]:px-0"
        >
          <PlusIcon className="mr-2 group-data-[collapsible=icon]:mr-0" />
          <span className="group-data-[collapsible=icon]:hidden">
            Add Monitor
          </span>
        </Button>
        {isOpen && <AddMonitor open={isOpen} onOpenChange={setIsOpen} />}
      </SidebarHeader>

      <SidebarContent>
        <UserMonitors monitors={monitors} setMonitor={setMonitor} />
      </SidebarContent>

      <SidebarFooter>
        <UserSidebar user={user} logout={logout} />
      </SidebarFooter>

      <SidebarRail />
    </Sidebar>
  )
}
