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

Frontend Steps (React + TypeScript + Tailwind)
Phase A — Core Setup

Create React project ✔

Add API client with axios ✔

Create AuthContext and useAuth hook ✔

Store sessionId in localStorage ✔

Phase B — Product Listing Page

Fetch products by type ✔

Render cards ✔

Add "Add to Cart" button ✔

Phase C — Cart Page

Load cart using sessionId ✔

Modify quantities ✔

Remove items ✔

Display totals (calculated by backend) ✔

Phase D — Checkout UI

Trigger POST /checkout/{sessionId} ✔

Display confirmation page with final total ✔

Phase E — Payment Stub Page

Call payment endpoint ✔

Show "Order Paid" summary ✔

Phase F — Admin Panel

Product editor ✔

Topping editor ✔

Price updates ✔

## Product Image Storage (Azurite)

### Overview
Product images are stored in Azure Blob Storage (using Azurite for local development). Each product can have three image variants:
- **Thumbnail** (150x150): For product listings and small previews
- **Medium** (500x500): For product detail pages
- **Full** (original size): High-quality original image

### Architecture

**Application Layer:**
- `IImageStorageService`: Interface for image storage operations
- `ProductImageUrls`: Record type containing URLs for all three image sizes
- Image upload/download DTOs

**Domain Layer:**
- `ProductImage`: Value object representing image URLs (owned entity)
- Updated `ProductEntity` with `ProductImage` property

**Infrastructure Layer:**
- `AzuriteImageStorageService`: Implementation using Azure.Storage.Blobs and SixLabors.ImageSharp
- Automatic image resizing and optimization
- JPEG compression with quality settings (85 for resized, 90 for full)

### API Endpoints

**POST /api/ProductImage/upload/{productId}** (Admin only)
- Uploads an image and creates all three variants
- Validates file type (JPEG, PNG, WebP) and size (max 10MB)
- Returns URLs for all three sizes

**GET /api/ProductImage/{productId}**
- Retrieves image URLs for a product

**DELETE /api/ProductImage/{productId}** (Admin only)
- Deletes all image variants for a product from blob storage
- Clears image URLs from database

**GET /api/ProductImage/download?blobName={name}**
- Downloads a specific image by blob name (for debugging)

### Storage Structure

Images are organized in blob storage by product ID:
```
product-images/
  ├── {productId}/
  │   ├── thumbnail.jpg
  │   ├── medium.jpg
  │   └── full.jpg
```

### Configuration

**appsettings.Development.json:**
```json
{
  "ConnectionStrings": {
    "AzuriteStorage": "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;..."
  },
  "AzuriteStorage": {
    "ContainerName": "product-images"
  }
}
```

### Local Development

1. Start Azurite:
   ```bash
   azurite --silent --location c:\azurite --debug c:\azurite\debug.log
   ```
2. The service automatically creates the `product-images` container on startup
3. Container has public blob access for easy image retrieval

### Dependencies

- **Azure.Storage.Blobs** (12.24.0): Azure Blob Storage SDK
- **SixLabors.ImageSharp** (3.1.6): Image processing and resizing

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
- [x] Phase 4: Admin Panel (Backend) - COMPLETED
  - [x] Product CRUD
  - [x] Topping CRUD
  - [x] Price editing API
- [x] Phase 5: Frontend (React) - COMPLETED
  - [x] Auth (Login/Register)
  - [x] Product listing
  - [x] Shopping cart
  - [x] Checkout flow
  - [x] Admin panel UI
- [x] Phase 6: Product Image Storage - COMPLETED
  - [x] Azurite integration
  - [x] Multi-size image support (thumbnail, medium, full)
  - [x] Image upload/download/delete endpoints
  - [x] ProductImage value object
  - [x] Database schema updates