# PizzaOrders Frontend

React + TypeScript + Tailwind CSS frontend for the PizzaOrders application.

## Getting Started

### Prerequisites
- Node.js (v18+)
- Backend API running on `http://localhost:5062`

### Installation
```bash
npm install
```

### Development
```bash
npm run dev
```

Frontend will be available at `http://localhost:3000`

### Build
```bash
npm run build
```

## Features

- **Authentication**: JWT-based auth with login/register
- **Product Catalog**: Browse pizzas, drinks, and desserts
- **Shopping Cart**: Add/remove items, update quantities
- **Checkout**: Complete order with stub payment
- **Admin Panel**: CRUD operations for products and toppings (Admin role required)

## Architecture

- **Components**: Reusable UI components
- **Pages**: Route-level components
- **Services**: API client with axios
- **Context**: Auth state management
- **Hooks**: Custom hooks for auth and cart
- **Types**: TypeScript type definitions

## API Integration

The frontend proxies API requests through Vite to `http://localhost:5062`.
Configure backend URL in `vite.config.ts` if needed.

## CORS Configuration

The backend is configured to allow requests from:
- `http://localhost:3000` (React frontend)
- `http://localhost:4200` (Angular frontend)
