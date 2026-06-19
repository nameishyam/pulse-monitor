import { useMemo } from "react"
import type { Log } from "@/lib/types"
import {
  type ChartConfig,
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
} from "@/components/ui/chart"
import {
  CartesianGrid,
  Area,
  AreaChart,
  ResponsiveContainer,
  XAxis,
  YAxis,
} from "recharts"

type LatencyChartProps = {
  logs: Log[]
}

const chartConfig = {
  latency: {
    label: "Latency",
    color: "var(--chart-2)",
  },
} satisfies ChartConfig

export function LatencyChart({ logs }: LatencyChartProps) {
  const chartData = useMemo(
    () =>
      [...logs].reverse().map((log, index) => ({
        check: index + 1,
        latency: log.responseTime ?? 0,
      })),
    [logs]
  )

  return (
    <div className="shrink-0 space-y-2">
      <div>
        <h2 className="text-md font-semibold text-foreground">Latency</h2>
        <p className="text-xs text-muted-foreground">Response time history</p>
      </div>

      <div className="pt-2">
        <ChartContainer config={chartConfig} className="h-48 w-full">
          <ResponsiveContainer width="100%" height="100%">
            <AreaChart data={chartData}>
              <CartesianGrid vertical={false} />

              <XAxis
                dataKey="check"
                type="number"
                domain={["dataMin", "dataMax"]}
                tickCount={10}
                tickLine={false}
                axisLine={false}
              />

              <YAxis
                tickFormatter={(value) => `${value}ms`}
                tickLine={false}
                axisLine={false}
              />

              <ChartTooltip content={<ChartTooltipContent />} />

              <Area
                type="monotone"
                dataKey="latency"
                stroke="var(--color-latency)"
                fill="var(--color-latency)"
                fillOpacity={1}
                strokeWidth={2}
              />
            </AreaChart>
          </ResponsiveContainer>
        </ChartContainer>
      </div>
    </div>
  )
}
