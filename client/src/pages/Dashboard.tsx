import { useEffect, useMemo, useState } from "react"
import { useOutletContext } from "react-router-dom"

import { api } from "@/lib/api"
import type { Log, Monitor } from "@/lib/types"

import { Separator } from "@/components/ui/separator"
import { SidebarInset, SidebarTrigger } from "@/components/ui/sidebar"
import { Badge } from "@/components/ui/badge"

import { Activity, Circle, Clock3, MonitorOff } from "lucide-react"
import { LatencyChart } from "@/components/monitor/LatencyChart"
import { RecentChecks } from "@/components/monitor/RecentChecks"

type DashboardContext = {
  monitor: Monitor | null
}

export default function Dashboard() {
  const { monitor } = useOutletContext<DashboardContext>()
  const [logs, setLogs] = useState<Log[]>([])

  useEffect(() => {
    let cancelled = false

    const fetchLogs = async () => {
      if (!monitor) {
        setLogs([])
        return
      }

      const res = await api.get(`/logs/${monitor.id}`)

      if (!cancelled) {
        setLogs(res.data)
      }
    }

    fetchLogs()

    return () => {
      cancelled = true
    }
  }, [monitor])

  const stats = useMemo(() => {
    if (!logs.length) return { uptime: "0.0", avgResponse: 0, latest: null }

    const successful = logs.filter(
      (log) =>
        log.statusCode !== undefined &&
        log.statusCode >= 200 &&
        log.statusCode < 400
    )

    const uptime = ((successful.length / logs.length) * 100).toFixed(1)

    const avgResponse = Math.round(
      logs.reduce((sum, log) => sum + (log.responseTime ?? 0), 0) / logs.length
    )

    return {
      uptime,
      avgResponse,
      latest: logs[0],
    }
  }, [logs])

  const isOnline =
    stats.latest?.statusCode !== undefined &&
    stats.latest.statusCode >= 200 &&
    stats.latest.statusCode < 400

  return (
    <SidebarInset className="h-screen overflow-hidden">
      <header className="flex h-12 shrink-0 items-center gap-2 border-b">
        <div className="flex items-center gap-2 px-4">
          <SidebarTrigger className="-ml-1" />

          <Separator
            orientation="vertical"
            className="mr-2 data-[orientation=vertical]:h-6"
          />

          <h1 className="text-lg font-semibold">
            {monitor?.name ?? "Dashboard"}
          </h1>
        </div>
      </header>

      {!monitor ? (
        <div className="flex h-[calc(100vh-3rem)] flex-col items-center justify-center p-4 text-muted-foreground">
          <MonitorOff className="mb-4 h-12 w-12 opacity-20" />
          <h2 className="text-xl font-semibold text-foreground">
            No Monitor Selected
          </h2>
          <p className="mt-2 text-sm">
            Select a monitor from the sidebar to view its performance data.
          </p>
        </div>
      ) : (
        <div className="flex h-[calc(100vh-3rem)] flex-col gap-4 p-4">
          <div className="flex flex-wrap items-center gap-6 text-sm">
            <div className="flex items-center gap-2">
              <Activity className="h-4 w-4 text-green-500" />
              <span className="text-muted-foreground">Uptime</span>
              <span className="font-semibold">{stats.uptime}%</span>
            </div>

            <div className="flex items-center gap-2">
              <Clock3 className="h-4 w-4 text-blue-500" />
              <span className="text-muted-foreground">Average</span>
              <span className="font-semibold">{stats.avgResponse} ms</span>
            </div>

            <div className="flex items-center gap-2">
              <Circle
                className={`h-3 w-3 fill-current ${
                  isOnline ? "text-green-500" : "text-red-500"
                }`}
              />

              <span className="font-semibold">
                {isOnline ? "Online" : "Offline"}
              </span>
            </div>

            <Badge variant="outline">{logs.length} checks</Badge>
          </div>

          <LatencyChart logs={logs} />

          <RecentChecks logs={logs} />
        </div>
      )}
    </SidebarInset>
  )
}
