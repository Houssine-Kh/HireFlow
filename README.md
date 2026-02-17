
```markdown
# 📘 HireFlow

> **A full-stack enterprise recruitment platform built with .NET 10, Angular 21, and Clean Architecture.**

**HireFlow** is an internal recruitment management system designed to streamline the hiring process. It features a unified platform where Recruiters can manage openings and Candidates can apply directly, bridging the gap between talent acquisition and applicant tracking.

## 🚀 Project Status

> **Current Phase:** 🚧 MVP Development
> **Completed:** Authentication, JWT Security, Candidate Profile Wizard, Admin Dashboard & Recruiter Approval Workflow, Job Management (Recruiter).
> **Next Up:** **Job Discovery & Applications (Candidate Side)**: Candidates need to browse published jobs, view details, and submit applications.

---

## 🏗️ Architecture & Tech Stack

HireFlow follows **Clean Architecture** principles with **CQRS** (Command Query Responsibility Segregation) to ensure scalability and maintainability.

### **Backend (.NET 10)**

* **Core:** ASP.NET Core Web API
* **Architecture:** Clean Architecture + Vertical Slices
* **Patterns:** CQRS (MediatR), Repository Pattern, Unit of Work, REPR Pattern
* **Validation:** FluentValidation
* **Database:** SQL Server (EF Core)
* **Logging:** Serilog

### **Frontend (Angular 21)**

* **Framework:** Standalone Components
* **State Management:** NGRX SignalStore (Signals & Reactive State)
* **UI Library:** PrimeNG (Aura Theme) + Tailwind CSS
* **Architecture:** Modular Feature-Based Structure

---

## ✨ Key Features

### 🔐 1. Authentication & Security

* **Unified Registration:** Single entry point for Candidates and Recruiters.
* **Role-Based Access Control (RBAC):**
    * **Candidates:** Auto-activated upon registration.
    * **Recruiters:** Created as `Pending`. Must be approved by an Admin to log in.
* **Secure Access:** JWT implementation with Refresh Tokens.

### 🧙‍♂️ 2. Candidate Experience

* **Lean Registration:** Quick sign-up with just Name/Email.
* **Profile Wizard:** A multi-step workflow triggered post-login to collect CVs, phone numbers, and LinkedIn URLs.
* **Just-In-Time Application:** Candidates can browse jobs freely and are only prompted to complete their profile when attempting to apply.

### 💼 3. Recruitment Management (New!)

* **Job Dashboard:** Dedicated workspace for recruiters to view and filter their job postings.
* **Lifecycle Management:** Full control over job status workflow:
    * **Draft:** Create listings privately before going live.
    * **Published:** Make jobs visible to candidates instantly.
    * **Closed:** Stop receiving applications while keeping historical data.
* **Reactive UI:** Instant feedback on actions (Create/Edit/Publish) using optimistic UI updates and Toast notifications.
* **Rich Editing:** Detailed job descriptions with validation.

### 🛡️ 4. Admin Governance

* **Recruiter Approval:** Centralized dashboard for reviewing and approving pending recruiter accounts to maintain platform integrity.

---

## 🛠️ Getting Started

### Prerequisites

* Node.js (Latest LTS)
* .NET SDK 10.0
* SQL Server (LocalDB or Express)

### 1. Backend Setup

```bash
# Navigate to API (Adjust 'backend' to your actual folder name if different)
cd backend/HireFlow.Api

# Configure Secrets (Avoids putting keys in appsettings.json)
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=HireFlowDb;Trusted_Connection=True;TrustServerCertificate=True;"
dotnet user-secrets set "JwtSettings:Secret" "YOUR_SUPER_SECURE_KEY_HERE"

# Run Migrations
dotnet ef database update --project ../HireFlow.Infrastructure

# Start API
dotnet run

```

### 2. Frontend Setup

```bash
# Navigate to Client (Adjust path if your client is in a different folder)
cd backend/HireFlow.Client

# Install Dependencies
npm install

# Run Application
ng serve

```

---

## 📂 Project Structure

```text
HireFlow
├──hireflow-client
│   ├──public
│   ├──src
│   │   ├──app
│   │   │   ├──core                 # Interceptors, Guards, Utilities
│   │   │   ├──domain               # DTOs, Models
│   │   │   ├──features             # Admin, Auth, Candidate (Pages)
│   │   │   ├──infrastructure       # Services & Stores (SignalStore)
│   │   │   └──app.routes.ts
│   ├──angular.json
│   └──package.json
│
├──HireFlow.Backend
│   ├──HireFlow.Api                 # Controllers, Middleware, Exception Handling
│   ├──HireFlow.Application         # CQRS Commands/Queries, Validators
│   ├──HireFlow.Domain              # Entities, Value Objects, Enums
│   └──HireFlow.Infrastructure      # EF Core, Repositories, Identity
└──README.md

```

---

## 👤 Author

**Houssine Khafif**

* **LinkedIn:** [linkedin.com/in/houssine-khafif](https://www.linkedin.com/in/houssine-khafif/)
* **GitHub:** [@houssinekhafif](https://github.com/Houssine-Kh)
* **Architecture:** Clean Architecture, Domain-Driven Design
* **Focus:** Enterprise SaaS Development

---

*Built with ❤️ and Clean Code.*

```

```
