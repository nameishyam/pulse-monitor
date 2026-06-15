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
              <div>Arena</div> <div className="text-primary">AI</div>
            </div>
          </h1>
          <p className="mx-auto max-w-2xl text-xl text-muted-foreground">
            Practice Group Discussions With AI Participants
          </p>
        </div>

        <div className="mx-auto mb-16 grid max-w-5xl gap-6 md:grid-cols-3">
          <div className="rounded-lg border border-border bg-card p-6 transition-colors hover:border-foreground/20">
            <h3 className="mb-3 text-lg font-semibold text-card-foreground">
              Create Discussion Rooms
            </h3>
            <p className="text-muted-foreground">
              Start discussions on any topic and configure your AI panel
            </p>
          </div>

          <div className="rounded-lg border border-border bg-card p-6 transition-colors hover:border-foreground/20">
            <h3 className="mb-3 text-lg font-semibold text-card-foreground">
              Debate With AI Experts
            </h3>
            <p className="text-muted-foreground">
              Engage with AI participants having distinct personalities and
              viewpoints
            </p>
          </div>

          <div className="rounded-lg border border-border bg-card p-6 transition-colors hover:border-foreground/20">
            <h3 className="mb-3 text-lg font-semibold text-card-foreground">
              Receive Feedback
            </h3>
            <p className="text-muted-foreground">
              Get discussion summaries, strengths, weaknesses, and improvement
              suggestions
            </p>
          </div>
        </div>

        <div className="text-center">
          {isAuthenticated() ? (
            <>
              <p className="mb-8 text-lg text-foreground">
                Continue your discussion sessions or start a new one.
              </p>
              <div className="flex justify-center gap-4">
                <Button
                  className="hover:cursor-pointer"
                  variant="outline"
                  onClick={() => {
                    navigate("/dashboard")
                  }}
                >
                  Dashboard
                </Button>
                <Button
                  className="hover:cursor-pointer"
                  onClick={() => {
                    navigate("/sessions/create")
                  }}
                >
                  New Session
                </Button>
              </div>
            </>
          ) : (
            <>
              <p className="mb-8 text-lg text-foreground">
                Start practicing group discussions with AI participants.
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
