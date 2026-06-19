import { Link } from "react-router-dom"
import ModeToggle from "@/components/ModeToggle"

export default function Navbar() {
  return (
    <div className="fixed top-0 left-0 z-50 flex h-16 w-full items-center justify-between border-b bg-background px-4">
      <div className="flex items-center gap-6 py-2">
        <Link to="/">
          <div className="flex cursor-pointer flex-row items-center gap-1 text-xl font-bold transition-opacity hover:opacity-80">
            <span>Pulse</span>
            <span className="text-primary">Monitor</span>
          </div>
        </Link>
      </div>
      <ModeToggle />
    </div>
  )
}
