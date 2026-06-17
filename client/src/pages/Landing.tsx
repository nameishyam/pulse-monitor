import { Button } from "@/components/ui/button"
import { useAuth } from "@/context/AuthContext"
import { useNavigate } from "react-router-dom"

export default function Landing() {
  const { isAuthenticated } = useAuth()
  const navigate = useNavigate()

  return (
    <div className="relative h-[90vh] bg-background">
      <div className="container mx-auto px-4 py-16">
        <div className="mb-16 text-center">
          <h1 className="mb-6 text-5xl font-bold text-foreground">
            <div className="flex flex-row items-center justify-center gap-3">
              <div>Pulse</div> <div className="text-primary">Monitor</div>
            </div>
          </h1>
          <p className="mx-auto max-w-2xl text-xl text-muted-foreground">
            High-Performance, Real-Time API & Website Uptime Monitoring
          </p>
        </div>

        <div className="mx-auto mb-16 grid max-w-6xl gap-6 md:grid-cols-3">
          <div className="rounded-lg border border-border bg-card p-6 transition-colors hover:border-foreground/20">
            <h3 className="mb-3 text-lg font-semibold text-card-foreground">
              Monitor Any Endpoint
            </h3>
            <p className="text-muted-foreground">
              Configure HTTP targets supporting GET, POST, PUT, and DELETE
              methods with customizable check intervals.
            </p>
          </div>

          <div className="rounded-lg border border-border bg-card p-6 transition-colors hover:border-foreground/20">
            <h3 className="mb-3 text-lg font-semibold text-card-foreground">
              Real-Time Performance Metrics
            </h3>
            <p className="text-muted-foreground">
              Track millisecond-accurate response times, status codes, and
              network latency variations through a live interface.
            </p>
          </div>

          <div className="rounded-lg border border-border bg-card p-6 transition-colors hover:border-foreground/20">
            <h3 className="mb-3 text-lg font-semibold text-card-foreground">
              Immediate Downtime Logs
            </h3>
            <p className="text-muted-foreground">
              Capture instant network connection errors, timeouts, or unexpected
              server failures before they affect your users.
            </p>
          </div>
        </div>

        <div className="text-center">
          {isAuthenticated() ? (
            <>
              <p className="mb-8 text-lg text-foreground">
                Review your current operational targets or register a new
                monitor.
              </p>
              <div className="flex justify-center gap-4">
                <Button
                  className="hover:cursor-pointer"
                  variant="outline"
                  onClick={() => {
                    navigate("/dashboard")
                  }}
                >
                  View Dashboard
                </Button>
              </div>
            </>
          ) : (
            <>
              <p className="mb-8 text-lg text-foreground">
                Start tracking your backend systems and endpoint reliability.
              </p>
              <div className="flex justify-center gap-4">
                <Button
                  className="hover:cursor-pointer"
                  variant="outline"
                  onClick={() => {
                    navigate("/login")
                  }}
                >
                  Login
                </Button>
                <Button
                  className="hover:cursor-pointer"
                  onClick={() => {
                    navigate("/signup")
                  }}
                >
                  Get Started
                </Button>
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  )
}
