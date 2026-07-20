# ECommerce API

A .NET Web API for an e-commerce platform, built with Onion Architecture and a rich domain model. Built as a learning project covering catalog browsing, response caching, authentication, order processing, and Stripe payment integration.

## Architecture

The solution follows Onion Architecture, with dependencies pointing inward:

```
ECommerce.Domain          — entities, interfaces, no external dependencies
ECommerce.Application     — services, DTOs, business logic (depends on Domain)
ECommerce.Infrastructure  — EF Core, Redis, Stripe, Identity (depends on Application + Domain)
ECommerce.API             — controllers, startup (depends on all of the above)
```

## Features

- **Product catalog** — browsing, filtering, and specification-based querying
- **Response caching** — Redis-backed caching on read-heavy catalog endpoints via a custom `[Cached]` action filter
- **Identity & JWT authentication** — user registration/login, role-based authorization (Admin role for product management)
- **Basket** — Redis-backed customer basket
- **Orders** — order creation with server-side price recalculation (never trusts client-supplied prices)
- **Payments** — Stripe PaymentIntent integration with webhook-driven order status updates
- **Admin dashboard** — minimal static HTML/JS page for adding products (see `AdminDashboard/`)

## Tech Stack

- ASP.NET Core Web API (.NET 10)
- Entity Framework Core + SQL Server
- ASP.NET Core Identity + JWT Bearer authentication
- StackExchange.Redis
- Stripe.net
- AutoMapper

## Prerequisites

- .NET 10 SDK
- SQL Server (local or containerized)
- Redis (see below)
- A [Stripe](https://dashboard.stripe.com/register) account with test-mode API keys
- [Stripe CLI](https://stripe.com/docs/stripe-cli) (for local webhook testing)

## Getting Started

### 1. Clone and restore

```bash
git clone <your-repo-url>
cd ECommerce
dotnet restore
```

### 2. Start Redis

```bash
docker run -d --name redis -p 6379:6379 redis
```

### 3. Configure secrets

This project uses `dotnet user-secrets` for local development — **no real secrets are committed to this repo**. Set your own:

```bash
cd ECommerce.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your SQL Server connection string>"
dotnet user-secrets set "ConnectionStrings:IdentityConnection" "<your SQL Server connection string, separate DB>"
dotnet user-secrets set "JwtSettings:Key" "<a long random secret, 32+ bytes>"
dotnet user-secrets set "StripeSettings:SecretKey" "sk_test_..."
dotnet user-secrets set "StripeSettings:PublishableKey" "pk_test_..."
dotnet user-secrets set "StripeSettings:WebhookSecret" "whsec_..."
```

Webhook secret comes from running `stripe listen --forward-to https://localhost:<port>/api/payments/webhook` locally.

### 4. Apply migrations

```bash
dotnet ef database update --context AppDbContext --project ECommerce.Infrastructure --startup-project ECommerce.API
dotnet ef database update --context AppIdentityDbContext --project ECommerce.Infrastructure --startup-project ECommerce.API
```

### 5. Run

```bash
dotnet run --project ECommerce.API
```

On first run, an admin user is seeded automatically; the generated password is printed to the console — check your terminal output.

### 6. Admin dashboard (optional)

Open `AdminDashboard/index.html` directly in a browser. Log in with the seeded admin credentials to add products. Note: this dashboard is a local dev tool only — it relies on a permissive CORS policy that should not be used in production.

## API Overview

| Area | Endpoints |
|---|---|
| Products | `GET /api/products`, `GET /api/products/{id}`, `GET /api/products/types`, `GET /api/products/Brand`, `POST /api/products` (Admin) |
| Account | `POST /api/account/register`, `POST /api/account/login`, `GET /api/account`, `GET/PUT /api/account/address` |
| Basket | `GET/POST/DELETE /api/basket` |
| Orders | `POST /api/orders`, `GET /api/orders`, `GET /api/orders/{id}`, `GET /api/orders/deliveryMethods` |
| Payments | `POST /api/payments/{basketId}`, `POST /api/payments/webhook` |

Swagger UI is available at the API root when running in development.

## Known Limitations

- `basketId` is not currently verified to belong to the authenticated user on Order/Payment endpoints — acceptable for a learning project, would need fixing before any production use
- Cached product responses are not invalidated on product create/update/delete beyond their TTL
- Admin dashboard uses a permissive dev-only CORS policy

## License

Personal learning project — no license specified.
