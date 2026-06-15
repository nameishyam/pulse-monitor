import type { User } from "@/lib/types"

export interface ProfileCardProps {
  user: User | null
  updateUser: (data: any) => void
}
