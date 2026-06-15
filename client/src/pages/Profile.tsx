import { useAuth } from "@/context/AuthContext"

import ProfileCard from "@/components/profile/ProfileCard"

export default function Profile() {
  const { user, updateUser } = useAuth()

  return (
    <div className="flex h-[90vh] overflow-hidden p-4">
      <div className="flex h-full w-1/2 flex-col overflow-hidden bg-background p-6">
        <ProfileCard user={user} updateUser={updateUser} />
      </div>
    </div>
  )
}
