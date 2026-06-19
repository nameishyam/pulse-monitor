"use client"

import * as React from "react"

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
import { useState } from "react"
import AddMonitor from "./sidebar/AddMonitor"
import { PlusIcon } from "lucide-react"

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  const [isOpen, setIsOpen] = useState<boolean>(false)

  return (
    <Sidebar collapsible="icon" {...props}>
      <SidebarHeader className="pt-4">
        <Button
          onClick={() => {
            setIsOpen(!isOpen)
          }}
        >
          <PlusIcon className="mr-2" />
          Add Monitor
        </Button>
        {isOpen && <AddMonitor open={isOpen} onOpenChange={setIsOpen} />}
      </SidebarHeader>
      <SidebarContent>
        <UserMonitors />
      </SidebarContent>
      <SidebarFooter>
        <UserSidebar />
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  )
}
