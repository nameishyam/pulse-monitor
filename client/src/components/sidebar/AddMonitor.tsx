import { Button } from "@/components/ui/button"
import {
  AlertDialog,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog"
import { Controller, useForm } from "react-hook-form"

import { Field, FieldError, FieldLabel } from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import { SpinnerCustom } from "@/components/ui/spinner"
import { api } from "@/lib/api"
import { zodResolver } from "@hookform/resolvers/zod"
import { useState } from "react"
import { toast } from "sonner"
import { z } from "zod"
import { useAuth } from "@/context/AuthContext"
import CodeMirror from "@uiw/react-codemirror"
import { json } from "@codemirror/lang-json"
import { EditorView } from "@codemirror/view"
import { closeBrackets, autocompletion } from "@codemirror/autocomplete"

const monitorSchema = z.object({
  name: z.string().min(1, "Name is required"),
  url: z.string().min(1, "URL is required"),
  intervalSeconds: z.number().optional(),
  httpMethod: z.string().optional(),
  requestBody: z.string().optional(),
})

export default function AddMonitor({
  open,
  onOpenChange,
}: {
  open: boolean
  onOpenChange: (open: boolean) => void
}) {
  const { setMonitors } = useAuth()
  const [isLoading, setIsLoading] = useState<boolean>(false)

  const monitorForm = useForm<z.infer<typeof monitorSchema>>({
    resolver: zodResolver(monitorSchema),
    defaultValues: {
      name: "",
      url: "",
      intervalSeconds: 60,
      httpMethod: "GET",
      requestBody: "",
    },
  })

  const handleSubmit = async (values: z.infer<typeof monitorSchema>) => {
    setIsLoading(true)
    try {
      const dataToSend = {
        name: values.name,
        url: values.url,
        intervalSeconds: values.intervalSeconds,
        httpMethod: values.httpMethod,
        requestBody: values.requestBody,
      }

      const response = await api.post("/monitors", dataToSend)
      toast.success("Monitor created successfully!")
      monitorForm.reset()
      setMonitors((prevMonitors) => [...prevMonitors, response.data])
      onOpenChange(false)
    } catch (err: any) {
      if (err.response?.status === 409) {
        toast.error("Monitor already exists")
      } else if (err.response?.status === 400) {
        toast.error("Invalid data provided")
      } else if (err.response?.status === 500) {
        toast.error("Server error, please try again later")
      } else {
        toast.error("An error occurred while creating the monitor")
      }
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <AlertDialog open={open} onOpenChange={onOpenChange}>
      <AlertDialogContent className="scrollbar-hide max-h-[90vh] max-w-3xl overflow-y-auto">
        <AlertDialogTitle className="mb-2 text-2xl font-bold">
          Create Monitor
        </AlertDialogTitle>

        <AlertDialogDescription className="mb-6 text-sm text-muted-foreground">
          Configure an endpoint to continuously monitor its availability and
          response.
        </AlertDialogDescription>

        <form
          onSubmit={monitorForm.handleSubmit(handleSubmit)}
          className="space-y-6"
        >
          <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
            <Controller
              name="name"
              control={monitorForm.control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor={field.name}>Monitor Name</FieldLabel>

                  <Input
                    {...field}
                    id={field.name}
                    placeholder="Production API Health Check"
                    aria-invalid={fieldState.invalid}
                  />

                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />

            <Controller
              name="url"
              control={monitorForm.control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor={field.name}>Endpoint URL</FieldLabel>

                  <Input
                    {...field}
                    id={field.name}
                    placeholder="https://api.example.com/health"
                    aria-invalid={fieldState.invalid}
                  />

                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
          </div>

          <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
            <Controller
              name="intervalSeconds"
              control={monitorForm.control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor={field.name}>
                    Check Interval (seconds)
                  </FieldLabel>

                  <Input
                    id={field.name}
                    type="number"
                    min={1}
                    placeholder="60"
                    value={field.value ?? ""}
                    onChange={(e) =>
                      field.onChange(
                        e.target.value === ""
                          ? undefined
                          : Number(e.target.value)
                      )
                    }
                    aria-invalid={fieldState.invalid}
                  />

                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />

            <Controller
              name="httpMethod"
              control={monitorForm.control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor={field.name}>HTTP Method</FieldLabel>

                  <select
                    {...field}
                    id={field.name}
                    className="flex h-9 w-full rounded-md border bg-transparent px-3 py-1 text-sm shadow-xs"
                  >
                    <option value="GET">GET</option>
                    <option value="POST">POST</option>
                    <option value="PUT">PUT</option>
                    <option value="PATCH">PATCH</option>
                    <option value="DELETE">DELETE</option>
                    <option value="HEAD">HEAD</option>
                  </select>

                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
          </div>

          <Controller
            name="requestBody"
            control={monitorForm.control}
            render={({ field, fieldState }) => (
              <Field data-invalid={fieldState.invalid}>
                <FieldLabel htmlFor={field.name}>
                  Request Body (JSON)
                </FieldLabel>

                <div className="overflow-hidden rounded-md border">
                  <CodeMirror
                    value={field.value ?? ""}
                    height="220px"
                    extensions={[
                      json(),
                      closeBrackets(),
                      autocompletion(),
                      EditorView.lineWrapping,
                    ]}
                    basicSetup={{
                      lineNumbers: true,
                      foldGutter: true,
                      highlightActiveLine: true,
                      highlightActiveLineGutter: true,
                      bracketMatching: true,
                      closeBrackets: true,
                      autocompletion: true,
                    }}
                    placeholder={`{
  "name": "EF Core migrations allow schema versioning.",
  "url": "https://learn.microsoft.com",
  "intervalSeconds": 2,
  "httpMethod": "POST"
}`}
                    onChange={(value) => field.onChange(value)}
                  />
                </div>

                {fieldState.invalid && (
                  <FieldError errors={[fieldState.error]} />
                )}
              </Field>
            )}
          />

          <div className="grid grid-cols-2 gap-3 pt-2">
            <Button type="submit" className="w-full" disabled={isLoading}>
              {isLoading ? <SpinnerCustom /> : "Create Monitor"}
            </Button>

            <AlertDialogCancel asChild>
              <Button type="button" variant="outline" className="w-full">
                Cancel
              </Button>
            </AlertDialogCancel>
          </div>
        </form>
      </AlertDialogContent>
    </AlertDialog>
  )
}
