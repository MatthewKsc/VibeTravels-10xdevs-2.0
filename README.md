# VibeTravels 2.0

> **10xDevs 2.0 Course Certification Project**

VibeTravels is an AI-powered travel planning application that converts simple trip notes into actionable itineraries. Built with .NET 9, Angular 20, and PostgreSQL, it leverages AI to transform rough travel ideas into structured day-by-day plans or activity lists.

---

## 📋 Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Project Structure & Architecture](#project-structure--architecture)
- [Getting Started](#getting-started)
  - [1. Database Setup](#1-database-setup)
  - [2. API Setup](#2-api-setup)
  - [3. Client Setup](#3-client-setup)
- [Configuration](#configuration)
- [Testing the API](#testing-the-api)
- [Features](#features)
- [Development](#development)

---

## Overview

VibeTravels addresses the challenge of planning engaging trips by providing:

- 📝 **Notes Management**: Create and manage travel notes with CRUD operations
- 👤 **User Profiles**: Store travel preferences (style, accommodation, climate)
- 🤖 **AI Plan Generation**: Convert notes into detailed itineraries using GPT-4o-mini
- 📅 **Smart Plans**: Day-by-day itineraries or activity lists based on trip parameters
- 🔐 **JWT Authentication**: Secure email/password authentication

---

## Tech Stack

### Backend

- **.NET 9** - Web API with Minimal APIs
- **PostgreSQL** - Database
- **Entity Framework Core** - ORM with migrations
- **OpenAI API** - AI-powered plan generation (via GPT-4o-mini)
- **Serilog** - Structured logging

### Frontend

- **Angular 20** - Frontend framework
- **Angular Material** - UI component library
- **TypeScript 5.9** - Type-safe development
- **SCSS** - Styling
- **RxJS** - Reactive programming

### Infrastructure

- **Docker & Docker Compose** - PostgreSQL containerization
- **GitHub Actions** - CI/CD (planned)

---

## Prerequisites

Ensure you have the following installed:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 18+](https://nodejs.org/) and npm
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Entity Framework Core Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet) (install via `dotnet tool install --global dotnet-ef`)

**Recommended IDEs:**

- Visual Studio 2022
- JetBrains Rider
- Visual Studio Code / Cursor

---

## Project Structure & Architecture

### Folder Structure

```
VibeTravels-10xdevs-2.0/
├── API/
│   └── VibeTravelsApi/
│       ├── VibeTravels.Api/              # Web API layer (endpoints, middleware)
│       ├── VibeTravels.Application/      # Application logic (commands, queries, DTOs)
│       ├── VibeTravels.Core/             # Domain entities, repositories, value objects
│       ├── VibeTravels.Infrastructure/   # Data access, EF Core, external services
│       └── VibeTravels.Shared/           # Shared utilities (CQRS, exceptions)
├── Client/
│   └── vibe-travels/                     # Angular 20 application
│       ├── src/app/
│       │   ├── core/                     # Auth, guards, interceptors
│       │   ├── modules/                  # Feature modules (auth, notes, plans, user)
│       │   └── shared/                   # Shared components, services
│       └── src/environments/             # Environment configurations
├── AI_conversation_history/              # Project planning documents
├── Examples/                             # Usage examples
└── docker-compose.yml                    # PostgreSQL container setup
```

### Backend Architecture (Clean Architecture)

The API follows **Clean Architecture** principles with clear separation of concerns across four layers:

```
┌─────────────────────────────────────────────┐
│         VibeTravels.Api (HTTP)              │
│  - Minimal API Endpoints                    │
│  - Middleware (Auth, Logging, CORS)         │
│  - HTTP Context Extensions                  │
└────────────────┬────────────────────────────┘
                 │
┌────────────────▼────────────────────────────┐
│      VibeTravels.Application                │
│  - Commands & Queries (CQRS)                │
│  - DTOs & Mappings                          │
│  - Business Logic                           │
│  - AI Service Abstractions                  │
└────────────────┬────────────────────────────┘
                 │
┌────────────────▼────────────────────────────┐
│         VibeTravels.Core                    │
│  - Domain Entities                          │
│  - Repository Interfaces                    │
│  - Value Objects                            │
│  - Domain Exceptions                        │
└────────────────┬────────────────────────────┘
                 │
┌────────────────▼────────────────────────────┐
│      VibeTravels.Infrastructure             │
│  - EF Core DbContext & Migrations           │
│  - Repository Implementations               │
│  - OpenAI Integration                       │
│  - Serilog Configuration                    │
└─────────────────────────────────────────────┘
```

**Layer Responsibilities:**

- **VibeTravels.Api**: HTTP endpoints, middleware, program setup, request/response handling
- **VibeTravels.Application**: Business logic, CQRS commands/queries, DTOs, validation
- **VibeTravels.Core**: Domain entities, repository interfaces, business rules, value objects
- **VibeTravels.Infrastructure**: Database context, repository implementations, external service integrations
- **VibeTravels.Shared**: Cross-cutting concerns (CQRS base classes, shared exceptions)

**Key Design Patterns:**

- **CQRS (Command Query Responsibility Segregation)**: Separate commands (writes) from queries (reads)
- **Repository Pattern**: Abstraction over data access layer
- **Dependency Injection**: All layers are loosely coupled via interfaces
- **Specification Pattern**: Complex query logic encapsulation

### Frontend Architecture (Angular)

The Angular application follows **feature-based modular architecture**:

```
src/app/
├── core/                    # Singleton services, app-wide logic
│   └── auth/                # Authentication (services, guards, interceptors)
├── modules/                 # Feature modules (lazy-loaded)
│   ├── auth/                # Sign in/up components
│   ├── notes/               # Notes CRUD operations
│   ├── plans/               # Plans generation & management
│   ├── user/                # User profile management
│   └── navigation/          # App navigation & theme switching
└── shared/                  # Reusable components & services
    ├── components/          # Confirmation dialogs, common UI
    └── services/            # Notification service, utilities
```

**Key Angular Patterns:**

- **Smart/Presentational Components**: Container components handle logic, presentational components handle UI
- **Reactive Forms**: Type-safe, reactive form handling with validation
- **RxJS Observables**: Reactive data streams for async operations
- **Material Design**: Consistent theming and component library
- **Route Guards**: Authentication and authorization protection

---

## Getting Started

### 1. Database Setup

Start the PostgreSQL database using Docker Compose:

```powershell
# From the project root directory
docker-compose up -d
```

This will:

- Start PostgreSQL on `localhost:5432`
- Create database: `VibeTravels`
- Credentials: `postgres` / `postgres`

**Verify the database is running:**

```powershell
docker ps | Select-String "vibetravels-postgres"
```

### 2. API Setup

#### a) Configure Environment Variables

Set up required environment variables for sensitive configuration or update `appsettings.Development.json`:

```powershell
# Set JWT Secret (required)
$env:JWT_SECRET = "your-secure-secret-key-here-minimum-32-characters"

# Set OpenAI API Key (optional - see AI Features section)
$env:OPENAI_API_KEY = "your-openai-api-key-here"
```

**Important**: The `appsettings.Development.json` uses these environment variables via placeholders:

- `${JWT_SECRET}` - Required for authentication
- `${OPENAI_API_KEY}` - Required only if AI features are enabled

#### b) Run Database Migrations

Navigate to the API project and apply migrations:

```powershell
cd API\VibeTravelsApi\VibeTravels.Api
dotnet ef database update
```

This will create all necessary tables in the PostgreSQL database based on migrations in `VibeTravels.Infrastructure/DAL/Migrations/`.

#### c) Restore Dependencies and Run

```powershell
# Restore NuGet packages
dotnet restore

# Run the API
dotnet run
```

The API will start on **http://localhost:5131** (default port).

**Verify API is running:**
Navigate to http://localhost:5131/scalar or http://localhost:5131/api/system/alive

### 3. Client Setup

#### a) Install Dependencies

```powershell
cd Client\vibe-travels
npm install
```

#### b) Configure Environment (Optional)

The client is pre-configured to connect to `http://localhost:5131/api` (see `src/environments/environments.ts`).

If you need to change the API URL, edit:

- `src/environments/environments.ts` (development)
- `src/environments/environments.production.ts` (production)

#### c) Run Client (you need to have Angular Cli installed)

```powershell
ng serve
```

The Angular app will start on **http://localhost:4200**

---

## Configuration

### API Configuration (`appsettings.Development.json`)

Located at: `API/VibeTravelsApi/VibeTravels.Api/appsettings.Development.json`

#### AI Features Configuration

VibeTravels supports two modes for plan generation:

**1. AI-Powered (Production Mode)**

- Set `"OpenAi.Enabled": true`
- Provide valid OpenAI API key via `OPENAI_API_KEY` environment variable
- Plans are generated using GPT-4o-mini model (use other models as you wish)

**2. Mock Mode (Development/Testing)**

- Set `"OpenAi.Enabled": false`
- Uses a mock service that returns simple string
- No API key required
- Useful for testing without incurring OpenAI costs

### Client Configuration (`environments.ts`)

Located at: `Client/vibe-travels/src/environments/environments.ts`

```typescript
export const environment = {
  production: false,
  apiUrl: "http://localhost:5131/api",
};
```

---

## Testing the API

The API includes `.http` test files for all endpoints in `API/VibeTravelsApi/VibeTravels.Api/Http/`.

### Using HTTP Files (Recommended)

These files work with:

- Visual Studio 2022 (built-in)
- JetBrains Rider (built-in)
- VS Code with [REST Client extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)

### Quick Start Test Flow

**1. Sign Up a New User** (`UsersApi.http`)

```http
POST http://localhost:5131/api/users/signup
Content-Type: application/json

{
  "email": "user1@example.com",
  "password": "Passw0rd!123"
}
```

**2. Sign In** (`UsersApi.http`)

```http
POST http://localhost:5131/api/users/signin
Content-Type: application/json

{
  "email": "user1@example.com",
  "password": "Passw0rd!123"
}
```

The response includes an `accessToken` which is automatically stored for subsequent requests.

**3. Create a Profile** (`ProfileApi.http`)

```http
PUT http://localhost:5131/api/profiles/me
Authorization: Bearer {{accessToken}}
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "travelStyle": "Adventure",
  "accommodationType": "Hotel",
  "preferredClimate": "Tropical"
}
```

**4. Create a Note** (`NoteApi.http`)

```http
POST http://localhost:5131/api/notes
Authorization: Bearer {{accessToken}}
Content-Type: application/json

{
  "title": "Tokyo Adventure",
  "location": "Tokyo, Japan",
  "bodyMd": "I want to explore traditional temples, try authentic ramen, visit teamLab Borderless, and experience cherry blossom season. Interested in both modern tech districts like Akihabara and historical areas like Asakusa..."
}
```

**5. Generate a Plan** (`PlanApi.http`)

```http
POST http://localhost:5131/api/plans
Authorization: Bearer {{accessToken}}
Content-Type: application/json

{
  "noteId": "{{noteId}}",
  "params": {
    "days": 7,
    "travelers": 2,
    "startDay": "2025-04-01"
  }
}
```

Available test files:

- `UsersApi.http` - Authentication (sign up, sign in)
- `ProfileApi.http` - User profile management
- `NoteApi.http` - Notes CRUD operations
- `PlanApi.http` - Plan generation and management
- `SystemApi.http` - Api endpoints like health check

---

## Features

### Core Functionality

#### 🔐 Authentication

- Email/password registration and login
- JWT-based authentication
- Token expiration: 60 minutes
- Protected routes and API endpoints

#### 📝 Notes Management

- Create, read, update, delete (CRUD) travel notes
- Required fields:
  - **Title**: 1-200 characters
  - **Location**: 1-255 characters
  - **Body**: 100-10,000 characters (Markdown supported)
- Soft delete functionality
- Search and filter capabilities

#### 👤 User Profile

- Store travel preferences:
  - Travel style (Adventure, Relaxation, Cultural, etc.)
  - Accommodation type (Hotel, Hostel, Airbnb, etc.)
  - Preferred climate (Tropical, Temperate, Cold, etc.)
- Used to personalize AI-generated plans

#### 🤖 AI Plan Generation

- Convert notes into structured travel plans
- Input parameters:
  - Number of days (optional)
  - Number of travelers (optional)
  - Start date (optional)
- Output types:
  - **Day-by-day itinerary** (if days specified)
  - **Activity list** (if days not specified)
- Generation status tracking: `queued` → `running` → `succeeded` / `failed`

#### 📅 Plans Management

- View all generated plans
- Decision workflow: `not generated` -> `generated` → `accepted` / `rejected`
- Edit plan content after generation
- Accept/reject with optional reasons
- Retry failed generations
- Filter by status, note, structure type

#### 🎨 UI/UX

- Light/dark theme toggle
- Material Design components
- Responsive layout
- Loading states and error handling

---

## Development

### Logging

API logs are written to:

- Console (structured JSON in production)
- File: `API/VibeTravelsApi/VibeTravels.Api/Logs/logs_YYYY_MM_DD.txt`

Configured via Serilog in `appsettings.Development.json`.

### CORS Configuration

The API is configured to accept requests from `http://localhost:4200` (Angular dev server).

To add more origins, edit `appsettings.Development.json`:

```json
"Cors": {
  "AllowedOrigins": [
    "http://localhost:4200",
    "https://your-production-domain.com"
  ]
}
```

---

## Project Status

This is an MVP (Minimum Viable Product) built for the 10xDevs 2.0 course certification.

## License

This is a student project for educational purposes.

---
