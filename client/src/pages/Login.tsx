import { useState } from "react"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { z } from "zod"
import { toast } from "sonner"
import { Eye, EyeOff } from "lucide-react"

import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardContent,
  CardFooter,
  CardAction,
} from "@/components/ui/card"
import {
  InputOTP,
  InputOTPGroup,
  InputOTPSeparator,
  InputOTPSlot,
} from "@/components/ui/input-otp"
import { useAuth } from "@/context/AuthContext"
import { useNavigate } from "react-router-dom"
import { api } from "@/lib/api"
import { SpinnerCustom } from "@/components/ui/spinner"
import { Field, FieldError, FieldLabel } from "@/components/ui/field"

const loginSchema = z.object({
  email: z.string().refine((val) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(val), {
    message: "Invalid email address",
  }),
  password: z.string().min(6, "Password must be at least 6 characters"),
})

const resetEmailSchema = z.object({
  email: z.string().refine((val) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(val), {
    message: "Invalid email address",
  }),
})

const resetPasswordSchema = z
  .object({
    newPassword: z.string().min(6, "Password must be at least 6 characters"),
    confirmNewPassword: z
      .string()
      .min(6, "Password must be at least 6 characters"),
  })
  .refine((data) => data.newPassword === data.confirmNewPassword, {
    message: "Passwords don't match",
    path: ["confirmNewPassword"],
  })

type Step = "login" | "email" | "otp" | "newPassword"

export default function Login() {
  const navigate = useNavigate()
  const { login } = useAuth()
  const [step, setStep] = useState<Step>("login")
  const [isLoading, setIsLoading] = useState(false)
  const [resetEmail, setResetEmail] = useState("")
  const [resetEmailError, setResetEmailError] = useState("")
  const [otp, setOtp] = useState("")
  const [otpError, setOtpError] = useState("")
  const [showPassword, setShowPassword] = useState(false)
  const [showNewPassword, setShowNewPassword] = useState(false)
  const [showConfirmPassword, setShowConfirmPassword] = useState(false)

  const loginForm = useForm<z.infer<typeof loginSchema>>({
    resolver: zodResolver(loginSchema),
    defaultValues: { email: "", password: "" },
  })

  const handleLogin = async (values: z.infer<typeof loginSchema>) => {
    setIsLoading(true)
    try {
      await api.post(`/auth/login`, values)
      await login()
      toast.success("Login successful!")
      navigate("/dashboard")
    } catch (err: any) {
      if (err.response?.status === 404) {
        toast.error("No user found with this email")
      } else if (err.response?.status === 400) {
        toast.error("Invalid email or password")
      } else if (err.response?.status === 500) {
        toast.error("Server error, please try again later")
      } else {
        toast.error("An error occurred while logging in")
      }
    } finally {
      setIsLoading(false)
    }
  }

  const emailForm = useForm<z.infer<typeof resetEmailSchema>>({
    resolver: zodResolver(resetEmailSchema),
    defaultValues: { email: "" },
  })

  const handleSendEmail = async () => {
    if (!resetEmail || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(resetEmail)) {
      setResetEmailError("Invalid email address")
      return
    }
    setResetEmailError("")
    setIsLoading(true)
    try {
      const res = await api.post(`/auth/forgot`, { email: resetEmail })
      if (res.status !== 200) {
        toast.error("Failed to send OTP")
        return
      }
      toast.success("OTP sent to your email")
      setStep("otp")
    } catch (err: any) {
      if (err.response?.status === 404) {
        toast.error("No user found with this email")
      } else if (err.response?.status === 500) {
        toast.error("Server error, please try again later")
      } else {
        toast.error("An error occurred while sending OTP")
      }
    } finally {
      setIsLoading(false)
    }
  }

  const handleVerifyOtp = async () => {
    if (otp.length !== 6) {
      setOtpError("OTP must be 6 digits")
      return
    }
    setOtpError("")
    setIsLoading(true)
    try {
      const res = await api.post(`/auth/verify`, { email: resetEmail, otp })
      if (res.status === 200) {
        toast.success("OTP verified")
        setStep("newPassword")
      }
    } catch (err: any) {
      if (err.response?.status === 400) {
        toast.error("Invalid OTP")
      } else if (err.response?.status === 404) {
        toast.error("No user found with this email")
      } else if (err.response?.status === 500) {
        toast.error("Server error, please try again later")
      } else {
        toast.error("An error occurred while verifying OTP")
      }
    } finally {
      setIsLoading(false)
    }
  }

  const passwordForm = useForm<z.infer<typeof resetPasswordSchema>>({
    resolver: zodResolver(resetPasswordSchema),
    defaultValues: { newPassword: "", confirmNewPassword: "" },
  })

  const handleResetPassword = async (
    values: z.infer<typeof resetPasswordSchema>
  ) => {
    setIsLoading(true)
    try {
      await api.patch(`/auth/reset`, {
        email: resetEmail,
        password: values.newPassword,
      })
      toast.success("Password reset successfully!")
      handleBackToLogin()
    } catch (err: any) {
      if (err.response?.status === 404) {
        toast.error("No user found with this email")
      } else if (err.response?.status === 500) {
        toast.error("Server error, please try again later")
      } else {
        toast.error("An error occurred while resetting password")
      }
    } finally {
      setIsLoading(false)
    }
  }

  const handleForgotPassword = () => {
    const loginEmail = loginForm.getValues("email")
    setResetEmail(loginEmail ?? "")
    setResetEmailError("")
    setOtp("")
    setOtpError("")
    passwordForm.reset()
    setStep("email")
  }

  const handleBackToLogin = () => {
    setResetEmail("")
    setResetEmailError("")
    setOtp("")
    setOtpError("")
    passwordForm.reset()
    emailForm.reset()
    setStep("login")
  }

  const resetDescription: Record<Exclude<Step, "login">, string> = {
    email: "Enter your email to receive an OTP",
    otp: "Enter the OTP sent to your email",
    newPassword: "Create your new password",
  }

  return (
    <div className="flex h-[90vh] items-center justify-center p-4">
      {step === "login" && (
        <Card className="w-full max-w-sm">
          <CardHeader>
            <CardTitle>Login</CardTitle>
            <CardDescription>Enter your Email and password</CardDescription>
            <CardAction>
              <Button variant="link" onClick={() => navigate("/signup")}>
                Sign Up
              </Button>
            </CardAction>
          </CardHeader>

          <form
            onSubmit={loginForm.handleSubmit(handleLogin)}
            className="space-y-4"
          >
            <CardContent>
              <div className="flex flex-col gap-6">
                <Field data-invalid={!!loginForm.formState.errors.email}>
                  <FieldLabel htmlFor="login-email">Email</FieldLabel>
                  <Input
                    id="login-email"
                    type="email"
                    placeholder="xyz@example.com"
                    disabled={isLoading}
                    aria-invalid={!!loginForm.formState.errors.email}
                    {...loginForm.register("email")}
                  />
                  {loginForm.formState.errors.email && (
                    <FieldError errors={[loginForm.formState.errors.email]} />
                  )}
                </Field>

                <Field data-invalid={!!loginForm.formState.errors.password}>
                  <div className="flex items-center justify-between">
                    <FieldLabel htmlFor="login-password">Password</FieldLabel>
                    <Button
                      type="button"
                      variant="link"
                      onClick={handleForgotPassword}
                    >
                      Forgot Password?
                    </Button>
                  </div>

                  <div className="relative">
                    <Input
                      id="login-password"
                      type={showPassword ? "text" : "password"}
                      disabled={isLoading}
                      aria-invalid={!!loginForm.formState.errors.password}
                      {...loginForm.register("password")}
                    />
                    <Button
                      type="button"
                      variant="ghost"
                      size="sm"
                      className="absolute top-0 right-0 h-full px-3 py-2"
                      onClick={() => setShowPassword((p) => !p)}
                    >
                      {showPassword ? (
                        <EyeOff className="h-4 w-4" />
                      ) : (
                        <Eye className="h-4 w-4" />
                      )}
                    </Button>
                  </div>

                  {loginForm.formState.errors.password && (
                    <FieldError
                      errors={[loginForm.formState.errors.password]}
                    />
                  )}
                </Field>
              </div>
            </CardContent>

            <CardFooter>
              <Button type="submit" className="w-full" disabled={isLoading}>
                {isLoading ? <SpinnerCustom /> : "Login"}
              </Button>
            </CardFooter>
          </form>
        </Card>
      )}

      {step !== "login" && (
        <Card className="w-full max-w-sm">
          <CardHeader>
            <CardTitle>Reset Password</CardTitle>
            <CardDescription>{resetDescription[step]}</CardDescription>
          </CardHeader>

          <CardContent>
            <div className="flex flex-col gap-6">
              {step === "email" && (
                <Field data-invalid={!!resetEmailError}>
                  <FieldLabel htmlFor="reset-email">Email</FieldLabel>
                  <Input
                    id="reset-email"
                    type="email"
                    placeholder="xyz@example.com"
                    disabled={isLoading}
                    aria-invalid={!!resetEmailError}
                    value={resetEmail}
                    onChange={(e) => {
                      setResetEmail(e.target.value)
                      if (resetEmailError) setResetEmailError("")
                    }}
                  />
                  {resetEmailError && (
                    <FieldError errors={[{ message: resetEmailError }]} />
                  )}
                </Field>
              )}

              {step === "otp" && (
                <>
                  <div className="text-sm text-muted-foreground">
                    OTP sent to: {resetEmail}
                  </div>

                  <Field data-invalid={!!otpError}>
                    <FieldLabel>OTP</FieldLabel>
                    <InputOTP
                      maxLength={6}
                      value={otp}
                      onChange={(val) => {
                        setOtp(val)
                        if (otpError) setOtpError("")
                      }}
                    >
                      <InputOTPGroup>
                        <InputOTPSlot index={0} />
                        <InputOTPSlot index={1} />
                        <InputOTPSlot index={2} />
                      </InputOTPGroup>
                      <InputOTPSeparator />
                      <InputOTPGroup>
                        <InputOTPSlot index={3} />
                        <InputOTPSlot index={4} />
                        <InputOTPSlot index={5} />
                      </InputOTPGroup>
                    </InputOTP>
                    {otpError && (
                      <FieldError errors={[{ message: otpError }]} />
                    )}
                  </Field>
                </>
              )}

              {step === "newPassword" && (
                <form
                  id="reset-password-form"
                  onSubmit={passwordForm.handleSubmit(handleResetPassword)}
                  className="flex flex-col gap-6"
                >
                  {/* new password */}
                  <Field
                    data-invalid={!!passwordForm.formState.errors.newPassword}
                  >
                    <FieldLabel htmlFor="new-password">New Password</FieldLabel>
                    <div className="relative">
                      <Input
                        id="new-password"
                        type={showNewPassword ? "text" : "password"}
                        placeholder="Enter new password"
                        disabled={isLoading}
                        aria-invalid={
                          !!passwordForm.formState.errors.newPassword
                        }
                        {...passwordForm.register("newPassword")}
                      />
                      <Button
                        type="button"
                        variant="ghost"
                        size="sm"
                        className="absolute top-0 right-0 h-full px-3 py-2"
                        onClick={() => setShowNewPassword((p) => !p)}
                      >
                        {showNewPassword ? (
                          <EyeOff className="h-4 w-4" />
                        ) : (
                          <Eye className="h-4 w-4" />
                        )}
                      </Button>
                    </div>
                    {passwordForm.formState.errors.newPassword && (
                      <FieldError
                        errors={[passwordForm.formState.errors.newPassword]}
                      />
                    )}
                  </Field>

                  <Field
                    data-invalid={
                      !!passwordForm.formState.errors.confirmNewPassword
                    }
                  >
                    <FieldLabel htmlFor="confirm-password">
                      Confirm New Password
                    </FieldLabel>
                    <div className="relative">
                      <Input
                        id="confirm-password"
                        type={showConfirmPassword ? "text" : "password"}
                        placeholder="Confirm new password"
                        disabled={isLoading}
                        aria-invalid={
                          !!passwordForm.formState.errors.confirmNewPassword
                        }
                        {...passwordForm.register("confirmNewPassword")}
                      />
                      <Button
                        type="button"
                        variant="ghost"
                        size="sm"
                        className="absolute top-0 right-0 h-full px-3 py-2"
                        onClick={() => setShowConfirmPassword((p) => !p)}
                      >
                        {showConfirmPassword ? (
                          <EyeOff className="h-4 w-4" />
                        ) : (
                          <Eye className="h-4 w-4" />
                        )}
                      </Button>
                    </div>
                    {passwordForm.formState.errors.confirmNewPassword && (
                      <FieldError
                        errors={[
                          passwordForm.formState.errors.confirmNewPassword,
                        ]}
                      />
                    )}
                  </Field>
                </form>
              )}
            </div>
          </CardContent>

          <CardFooter className="flex flex-col gap-2">
            {step === "email" && (
              <Button
                type="button"
                onClick={handleSendEmail}
                disabled={isLoading}
                className="w-full"
              >
                {isLoading ? <SpinnerCustom /> : "Send OTP"}
              </Button>
            )}

            {step === "otp" && (
              <Button
                type="button"
                onClick={handleVerifyOtp}
                disabled={isLoading}
                className="w-full"
              >
                {isLoading ? <SpinnerCustom /> : "Verify OTP"}
              </Button>
            )}

            {step === "newPassword" && (
              <Button
                type="submit"
                form="reset-password-form"
                disabled={isLoading}
                className="w-full"
              >
                {isLoading ? <SpinnerCustom /> : "Reset Password"}
              </Button>
            )}

            <Button
              type="button"
              variant="link"
              onClick={handleBackToLogin}
              disabled={isLoading}
            >
              Back to Login
            </Button>
          </CardFooter>
        </Card>
      )}
    </div>
  )
}
