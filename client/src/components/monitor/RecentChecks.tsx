import { useMemo, useState } from "react"
import type { Log } from "@/lib/types"
import { Badge } from "@/components/ui/badge"
import { ScrollArea } from "@/components/ui/scroll-area"

type RecentChecksProps = {
  logs: Log[]
}

export function RecentChecks({ logs }: RecentChecksProps) {
  const [statusFilter, setStatusFilter] = useState<"all" | "passed" | "failed">(
    "all"
  )

  const passedLogs = useMemo(
    () =>
      logs.filter(
        (log) =>
          log.statusCode !== undefined &&
          log.statusCode >= 200 &&
          log.statusCode < 400
      ),
    [logs]
  )

  const failedLogs = useMemo(
    () =>
      logs.filter(
        (log) =>
          !(
            log.statusCode !== undefined &&
            log.statusCode >= 200 &&
            log.statusCode < 400
          )
      ),
    [logs]
  )

  const displayedLogs = useMemo(() => {
    if (statusFilter === "passed") return passedLogs
    if (statusFilter === "failed") return failedLogs
    return logs
  }, [logs, statusFilter, passedLogs, failedLogs])

  return (
    <div className="flex min-h-0 flex-1 flex-col pt-2">
      <div className="flex items-center gap-2 px-1">
        <button
          onClick={() => setStatusFilter("all")}
          className={`rounded-full px-4 py-1.5 text-sm font-medium transition-colors ${
            statusFilter === "all"
              ? "bg-muted text-foreground"
              : "text-muted-foreground hover:bg-muted/50 hover:text-foreground"
          }`}
        >
          All
        </button>
        <button
          onClick={() => setStatusFilter("passed")}
          className={`flex items-center gap-2 rounded-full px-4 py-1.5 text-sm font-medium transition-colors ${
            statusFilter === "passed"
              ? "bg-muted text-foreground"
              : "text-muted-foreground hover:bg-muted/50 hover:text-foreground"
          }`}
        >
          Passed
          <span className="flex h-5 w-5 items-center justify-center rounded-full bg-background/60 text-[10px] font-semibold text-foreground">
            {passedLogs.length}
          </span>
        </button>
        <button
          onClick={() => setStatusFilter("failed")}
          className={`flex items-center gap-2 rounded-full px-4 py-1.5 text-sm font-medium transition-colors ${
            statusFilter === "failed"
              ? "bg-muted text-foreground"
              : "text-muted-foreground hover:bg-muted/50 hover:text-foreground"
          }`}
        >
          Failed
          <span className="flex h-5 w-5 items-center justify-center rounded-full bg-background/60 text-[10px] font-semibold text-foreground">
            {failedLogs.length}
          </span>
        </button>
      </div>

      <div className="mt-4 grid grid-cols-[1fr_120px_100px] gap-4 rounded-t-md border-y bg-muted/20 px-4 py-3 text-sm font-medium text-muted-foreground">
        <div>Check Details</div>
        <div>Status</div>
        <div className="text-right">Latency</div>
      </div>

      <ScrollArea className="min-h-0 flex-1">
        {displayedLogs.length === 0 ? (
          <div className="p-8 text-center text-sm text-muted-foreground">
            No logs found for this filter.
          </div>
        ) : (
          displayedLogs.map((log, index) => {
            const success =
              log.statusCode !== undefined &&
              log.statusCode >= 200 &&
              log.statusCode < 400

            return (
              <div
                key={index}
                className="grid grid-cols-[1fr_120px_100px] items-center gap-4 border-b px-4 py-3 transition-colors hover:bg-muted/30"
              >
                <div>
                  <p className="text-sm font-medium">
                    {success
                      ? `HTTP ${log.statusCode}`
                      : (log.errorMessage ??
                        `HTTP ${log.statusCode ?? "Unknown"}`)}
                  </p>
                  <p className="text-xs text-muted-foreground">
                    {log.createdAt
                      ? new Date(log.createdAt).toLocaleString()
                      : "Unknown time"}
                  </p>
                </div>

                <div>
                  <Badge
                    variant="outline"
                    className={`text-xs ${
                      success
                        ? "border-green-500/30 text-green-500"
                        : "border-red-500/30 text-red-500"
                    }`}
                  >
                    {success ? "Healthy" : "Failed"}
                  </Badge>
                </div>

                <div className="text-right font-mono text-sm">
                  {log.responseTime !== undefined
                    ? `${log.responseTime} ms`
                    : "--"}
                </div>
              </div>
            )
          })
        )}
      </ScrollArea>
    </div>
  )
}
