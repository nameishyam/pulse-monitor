# Pulse Monitor

A full-stack uptime monitoring platform that continuously monitors HTTP endpoints, records availability, stores execution history, and provides a dashboard for tracking application health.

The application allows users to create monitors for APIs or websites, configure intervals, automatically perform health checks in the background, and review execution logs from a modern web interface.

---

# Features

## Authentication

- User registration
- User login
- JWT authentication
- HttpOnly authentication cookies
- Protected API endpoints
- Secure logout

---

## Monitor Management

- Create monitors
- Update monitors
- Delete monitors
- Retrieve all monitors
- Retrieve monitor details
- User-specific monitor isolation

Each monitor contains:

- Monitor name
- Endpoint URL
- Monitoring interval
- Latest status
- Latest HTTP status code
- Last checked timestamp

---

## Background Monitoring

A background worker continuously executes configured monitors.

Features include:

- Automatic execution
- Configurable intervals
- Stores response status
- Updates monitor state
- Generates execution logs
- Runs independently from API requests

---

## Logging

Every monitor execution generates a log containing:

- Execution timestamp
- Response status
- HTTP status code
- Response time
- Success/Failure
- Error information (when applicable)

This allows complete monitoring history.

---

## Dashboard

The React dashboard provides:

- Authentication
- Sidebar navigation
- Monitor list
- Selected monitor details
- Live status display
- Recent execution logs
- Profile management

---

# Tech Stack

## Frontend

- React
- TypeScript
- Vite
- React Router
- Axios
- Tailwind CSS
- shadcn/ui
- Lucide React
- React Hook Form
- Zod
- Sonner

---

## Backend

- ASP.NET Core (.NET 10)
- Entity Framework Core
- PostgreSQL
- Supabase
- JWT Authentication
- BackgroundService
- Dependency Injection
- Repository Pattern
- Service Layer Architecture

---

## Database

- PostgreSQL
- Supabase

---

# Architecture

```
Client
│
├── React
├── Axios
└── Dashboard
        │
        ▼
ASP.NET Web API
│
├── Controllers
├── Services
├── Repositories
├── Entity Framework Core
├── Authentication
└── Background Worker
        │
        ▼
PostgreSQL (Supabase)
```

---

# Solution Structure

```
PulseMonitor
│
├── Client
│   ├── components
│   ├── pages
│   ├── context
│   ├── hooks
│   ├── lib
│   └── routes
│
└── Server
    ├── Server.Api
    ├── Server.Service
    ├── Server.Repository
    ├── Server.Domain
    └── Server.Infrastructure
```

---

# Backend Architecture

```
HTTP Request
      │
      ▼
Controller
      │
      ▼
Service Layer
      │
      ▼
Repository Layer
      │
      ▼
Entity Framework Core
      │
      ▼
PostgreSQL
```

The project follows a layered architecture separating:

- Presentation
- Business Logic
- Data Access
- Domain Models
- Infrastructure

This improves maintainability, scalability, and testability.

---

# Monitoring Flow

```
Create Monitor
       │
       ▼
Saved in Database
       │
       ▼
Background Worker
       │
       ▼
HTTP Request
       │
       ▼
Receive Response
       │
       ▼
Update Monitor
       │
       ▼
Create Log
       │
       ▼
Dashboard Updates
```

---

# Security

- JWT Authentication
- HttpOnly Cookies
- Secure Cookies
- SameSite Cookie Policy
- Password Hashing
- Authorization Policies
- User Isolation
- Repository Validation
- DTO Validation

---

# Validation

Input validation is implemented using:

- Data Annotations
- Service Layer Validation
- Strongly Typed DTOs

---

# Background Worker

The monitoring engine is implemented using:

```
BackgroundService
```

Responsibilities:

- Poll monitors
- Execute HTTP requests
- Update monitor status
- Store execution logs
- Handle failures
- Retry on next interval

---

# Error Handling

The API returns consistent HTTP responses.

Examples include:

```
200 OK
201 Created
204 No Content
400 Bad Request
401 Unauthorized
403 Forbidden
404 Not Found
409 Conflict
500 Internal Server Error
```

---

# Running the Project

## Clone

```bash
git clone https://github.com/<username>/pulse-monitor.git
```

---

## Backend

```bash
cd Server
```

Restore packages

```bash
dotnet restore
```

Run migrations

```bash
dotnet ef database update
```

Run API

```bash
dotnet run --project Server.Api
```

---

## Frontend

```bash
cd Client
```

Install dependencies

```bash
npm install
```

Start development server

```bash
npm run dev
```

---

# Environment Variables

Backend

```env
ConnectionStrings__DefaultConnection=

Jwt__Issuer=
Jwt__Audience=
Jwt__Key=
Jwt__ExpiryMinutes=
```

Frontend

```env
VITE_API_URL=http://localhost:5000
```

---

# Design Principles

- Repository Pattern
- Dependency Injection
- Service Layer
- Separation of Concerns
- SOLID Principles
- Clean Architecture Concepts
- Strongly Typed DTOs
- Layered Project Structure

---

# Future Improvements

- Real-time updates using SignalR
- Email notifications
- SMS notifications
- Webhook alerts
- SSL certificate monitoring
- Custom request headers
- Custom HTTP Methods and Request Bodies
- Authentication headers
- Response content validation
- Status analytics
- Historical uptime charts
- Incident management
- Docker deployment
- Kubernetes support
- Multi-region workers

---

# License

This project is licensed under the MIT License.
