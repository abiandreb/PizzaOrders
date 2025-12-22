Pizza Ordering System — Architecture & Implementation Plan
Overview (Backend)

This system uses Redis for temporary cart storage and SQL for permanent orders.
Redis is the “scratchpad”, SQL is the “ledger”.

High-Level Flow
1. Cart Phase (temporary, Redis)

User lands on the website.

Backend generates sessionId (GUID).

Cart is stored in Redis under the key: cart:{sessionId}.

All cart operations include the sessionId.

2. Checkout Phase (finalizing)

Backend loads real product/topping prices from SQL.

Recalculates totals.

Creates an Order (SQL) with static snapshot of prices.

3. Payment Phase (stub for now)

Payment service receives: orderId + amount.

Always “succeeds”.

Backend marks the order as Paid.

Components
Redis Cart Service

Used for:

creating a cart,

adding/updating/removing items,

storing everything as one JSON object,

total calculation.

Key format:
cart:{sessionId}

SQL Entities

Order

OrderItem

OrderItemModifier

Product

Topping

ProductProperties (Owned type)

Order stores fixed historical prices, unaffected by future price changes.

API Endpoints
Cart (Redis)

POST /cart/create

GET /cart/{sessionId}

POST /cart/{sessionId}/add

PUT /cart/{sessionId}/update

DELETE /cart/{sessionId}/remove

DELETE /cart/{sessionId}

Checkout

POST /checkout/{sessionId} → validates cart → recalculates → creates SQL order

Payment (Stub)

POST /payment/pay → always returns success

Price Syncing Logic
In Cart (Redis)

Temporary.

Not trusted.

Values can be old or manipulated.

On Checkout

Reload product/topping prices from SQL.

Recalculate from scratch.

Save static values into Order and OrderItems.

Ensures historical accuracy even if prices change later.

Implementation Phases (Extended)
Phase 0 — Infrastructure

Redis installed and running

SQL database ready

Base project bootstrapped

Phase 1 — Cart (Redis)

Redis configuration ✔

ICacheService interface ✔

CartService implementation ✔

CartController (6 endpoints) ✔

Cart DTOs ✔

Phase 2 — Checkout

Validate cart contents ✔

Recalculate prices from SQL ✔

Create Order + OrderItems + modifiers snapshot ✔

Phase 3 — Payment Stub

Fake external call ✔

Write PaymentEntity ✔

Mark Order as Paid ✔

Phase 4 — Admin Panel (Optional)

Product CRUD ☐

Topping CRUD ☐

Price editing UI ☐

Frontend Steps (Angular)
Phase A — Core Setup

Create Angular project ✔

Add HttpClient module ✔

Create CartApiService for backend communication ✔

Store sessionId in localStorage or cookie ✔

Phase B — Product Listing Page

Fetch products by type ✔

Render cards ✔

Add “Add to Cart” button ✔

Show available sizes and toppings ☐

Phase C — Cart Page

Load cart using sessionId

Modify quantities

Add/remove toppings

Remove items

Display totals (calculated by backend)

Phase D — Checkout UI

Trigger POST /checkout/{sessionId}

Display confirmation page with final total

Redirect to payment

Phase E — Payment Stub Page

Call payment endpoint

Show “Order Paid” summary

Phase F — (Optional) Admin Panel

Product editor

Topping editor

Price updates

## Progress Tracker
- [x] Phase 0: Infrastructure setup
- [x] Phase 1: Cart (Redis) - COMPLETED
  - [x] Redis configuration
  - [x] ICacheService interface
  - [x] CartService class
  - [x] CartController with 6 endpoints
  - [x] Cart DTOs
- [x] Phase 2: Checkout - COMPLETED
- [x] Phase 3: Payment Stub - COMPLETED
- [ ] Phase 4: Admin Panel (Optional)

## Last Updated
2025-12-02