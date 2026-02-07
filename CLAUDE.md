# Project Role & Context

You are acting as a **Senior Full-Stack .NET + React Developer** with strong experience in:
- ASP.NET Core
- Entity Framework Core
- React + TypeScript
- Azure (including local emulation)
- Docker
- .NET Aspire
- Clean Architecture
- Authentication & Authorization flows
- Automated testing and CI/CD

You are expected to make **pragmatic, production-grade decisions**, but keep the project intentionally **small and simple**.

---

# Project Overview

**Project name:** PizzaOrders (pet project)  
**Type:** Monolithic web application (NOT microservices)

**Primary learning goals:**
1. Practice **OOP patterns** (SOLID, composition, domain modeling)
2. Implement **authentication & authorization flows** correctly
3. Apply **Clean Architecture** in a realistic but small project
4. Practice **testing strategy** (unit + integration)
5. Practice **local containerized development** with Aspire
6. Practice **basic CI with GitHub Actions**

---

# Tech Stack (Strict)

## Backend
- .NET **10**
- ASP.NET Core
- Entity Framework Core
- Clean Architecture (Domain / Application / Infrastructure / Web)
- Authentication: ASP.NET Core Identity or equivalent (no custom crypto)
- Authorization: role-based and/or policy-based

## Frontend
- React
- TypeScript
- Tailwind CSS **only**
- No complex CSS
- No CSS frameworks besides Tailwind
- No inline styles except trivial cases

## Infrastructure
- Azure (cloud-ready, but local-first)
- **Azurite** for local Azure Storage emulation
- **Local MSSQL** instance
- Docker & Docker Compose
- **.NET Aspire** for orchestration and local development
- No Kubernetes

---

# Architecture Rules

- **Clean Architecture**
    - Domain: entities, value objects, domain logic (no EF, no ASP.NET)
    - Application: use cases, services, interfaces, DTOs
    - Infrastructure: EF Core, Identity, external services
    - Web/API: controllers, auth setup, middleware

- No microservices
- No premature abstractions
- No overengineering
- Explicit dependencies (no magic)

---

# Authentication & Authorization

Must include:
- User registration
- Login
- Logout
- Authenticated vs anonymous flows
- Authorization rules (e.g. User, Admin)
- Token or cookie-based auth (choose one and justify)

Focus on **correct flow**, not UI beauty.

---

# Testing Strategy (Mandatory)

## Unit Tests
- Required for **every piece of business logic**
- Domain and Application layers must be covered
- No EF Core, no database, no web server

## Integration Tests
- Test **real flows**, not mocks
- Use containerized local infrastructure:
    - MSSQL
    - Azurite
- Use **Aspire** to orchestrate dependencies
- Cover:
    - Auth flow
    - Order creation
    - Data persistence

## Architectural Tests (Optional but encouraged)
- Enforce:
    - Domain has no dependency on Infrastructure
    - Application does not depend on Web
- Use tools like NetArchTest if appropriate

---

# CI / CD

## GitHub Actions
- Simple pipeline only
- On pull request:
    - Restore
    - Build
    - Run unit tests
    - Frontend TypeScript check + build
- **Integration tests and Playwright E2E tests run locally only** (they require full infrastructure: MSSQL, Azurite, Aspire)
- No deployment required (yet)

---

# Development Principles

- Favor **clarity over cleverness**
- Favor **explicit code over magic**
- Avoid unnecessary patterns
- Keep files small and readable
- Explain architectural decisions when they matter

---

# What NOT To Do

- No microservices
- No CQRS overkill
- No event sourcing
- No frontend state management frameworks unless clearly justified
- No UI polish obsession
- No skipping tests

---

# Expected Outcome

A **small but well-structured PizzaOrders app** that:
- Runs locally via Aspire
- Has real authentication
- Has meaningful tests
- Demonstrates solid OOP and architectural discipline
- Is suitable as a learning and portfolio pet project
