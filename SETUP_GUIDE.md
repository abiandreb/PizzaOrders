# PizzaOrders Setup & Troubleshooting Guide

## Quick Start

### 1. Start Backend (API)
```bash
cd PizzaOrders.API
dotnet run
```
Backend will run on: `http://localhost:5062` and `https://localhost:7258`

### 2. Start Frontend
```bash
cd frontend
npm install  # First time only
npm run dev
```
Frontend will run on: `http://localhost:3000`

### 3. Verify Backend is Running
Open browser to: `http://localhost:5062/scalar/v1`
You should see the Scalar API documentation.

## Testing the Connection

### Test 1: Check Backend API Directly
```bash
curl http://localhost:5062/api/product?productType=0
```
Should return a list of pizzas (or empty array if no products exist).

### Test 2: Check CORS Headers
```bash
curl -H "Origin: http://localhost:3000" -H "Access-Control-Request-Method: GET" -H "Access-Control-Request-Headers: Content-Type" -X OPTIONS http://localhost:5062/api/product -v
```
Look for these headers in the response:
- `Access-Control-Allow-Origin: http://localhost:3000`
- `Access-Control-Allow-Methods: GET, POST, PUT, DELETE, etc.`

### Test 3: Frontend API Call
Open browser console at `http://localhost:3000` and run:
```javascript
fetch('/api/product?productType=0')
  .then(r => r.json())
  .then(console.log)
  .catch(console.error)
```

## Common Issues

### Issue 1: CORS Error
**Error**: "Access to fetch at 'http://localhost:5062/api/...' from origin 'http://localhost:3000' has been blocked by CORS policy"

**Solution**:
- Verify backend `Program.cs` has CORS configured for `http://localhost:3000`
- Restart the backend after CORS changes

### Issue 2: Connection Refused
**Error**: "Failed to fetch" or "ERR_CONNECTION_REFUSED"

**Solution**:
- Check if backend is running: `netstat -ano | findstr :5062`
- Verify backend port in `frontend/vite.config.ts` matches `launchSettings.json`

### Issue 3: 404 Not Found
**Error**: API returns 404

**Solution**:
- Verify controller route: `[Route("api/[controller]")]`
- Check endpoint URL matches controller name (e.g., `/api/product` → `ProductController`)

### Issue 4: Redis Connection Error
**Error**: "Redis connection string not found" or connection timeout

**Solution**:
- Install Redis: `winget install Redis.Redis` (Windows)
- Or use Docker: `docker run -d -p 6379:6379 redis`
- Verify Redis is running: `redis-cli ping` (should return "PONG")

### Issue 5: SQL Connection Error
**Error**: "Cannot open database" or SQL connection timeout

**Solution**:
- Check connection string in `appsettings.Development.json`
- Verify SQL Server is running
- Run migrations: `dotnet ef database update`

### Issue 6: Azurite Connection Error
**Error**: "No connection could be made to Azurite" or blob storage errors

**Solution**:
- Install Azurite: `npm install -g azurite`
- Start Azurite: `azurite --silent --location c:\azurite --debug c:\azurite\debug.log`
- Or start blob service only: `azurite-blob --location c:\azurite --blobPort 10000`
- Verify Azurite is running: `curl http://127.0.0.1:10000/devstoreaccount1?comp=list`
- Check if port 10000 is available: `netstat -ano | findstr :10000`

### Issue 7: Image Upload Fails
**Error**: "Container not found" or image upload errors

**Solution**:
- Ensure Azurite is running before starting the API
- The `product-images` container is created automatically on first API startup
- Restart the API if the container wasn't created
- Verify container exists: Open Azure Storage Explorer and connect to local Azurite

## API Endpoints Reference

### Public Endpoints (No Auth Required)
- `GET /api/product?productType=0` - Get products by type (0=Pizza, 1=Drink, 2=Dessert)
- `GET /api/product/{id}` - Get product by ID
- `POST /api/cart/create` - Create new cart
- `GET /api/cart/{sessionId}` - Get cart
- `POST /api/cart/{sessionId}/add` - Add item to cart
- `POST /api/checkout/{sessionId}` - Checkout cart
- `POST /api/auth/register-user` - Register new user
- `POST /api/auth/login-user` - Login

### Admin Endpoints (Requires Admin Role)
- `GET /api/management/products` - Get all products (admin)
- `POST /api/management/products` - Create product
- `PUT /api/management/products` - Update product
- `DELETE /api/management/products/{id}` - Delete product
- `GET /api/management/toppings` - Get all toppings
- `POST /api/management/toppings` - Create topping
- `PUT /api/management/toppings` - Update topping
- `DELETE /api/management/toppings/{id}` - Delete topping
- `POST /api/ProductImage/upload/{productId}` - Upload product image (creates thumbnail, medium, full)
- `GET /api/ProductImage/{productId}` - Get product image URLs
- `DELETE /api/ProductImage/{productId}` - Delete all product images
- `GET /api/ProductImage/download?blobName={name}` - Download specific image blob

## Default Admin Credentials
Check `PizzaOrders.Infrastructure/Users_Seed.sql` for seeded admin user credentials.

## Browser DevTools Debugging

### Check Network Tab
1. Open DevTools (F12)
2. Go to Network tab
3. Filter by "Fetch/XHR"
4. Refresh page
5. Look for failed requests (red)
6. Click on failed request to see:
   - Request URL
   - Request Headers
   - Response Status
   - Response Headers
   - Response Body

### Check Console
Look for error messages like:
- CORS errors
- Network errors
- JavaScript errors

## Environment Verification Checklist

- [ ] Backend running on port 5062
- [ ] Frontend running on port 3000
- [ ] Redis running on port 6379
- [ ] SQL Server accessible
- [ ] Database migrations applied
- [ ] Azurite running on port 10000
- [ ] CORS allows localhost:3000
- [ ] No firewall blocking ports
- [ ] Browser console shows no errors

## Azurite Setup for Product Images

### Installation
```bash
npm install -g azurite
```

### Starting Azurite
```bash
# Full service
azurite --silent --location c:\azurite --debug c:\azurite\debug.log

# Blob service only (recommended)
azurite-blob --location c:\azurite --blobPort 10000
```

### Verifying Azurite
```bash
# Check if Azurite is running
curl http://127.0.0.1:10000/devstoreaccount1?comp=list

# Should return XML with container list
```

### Using Azure Storage Explorer (Optional)
1. Download: https://azure.microsoft.com/en-us/products/storage/storage-explorer/
2. Connect to Local Azurite:
   - Click "Connect" button
   - Select "Local Storage Emulator"
   - Use default settings
3. Browse `product-images` container to view uploaded images

### Image Upload Testing
```bash
# Using cURL (replace {productId} and {token})
curl -X POST https://localhost:7258/api/ProductImage/upload/{productId} \
  -H "Authorization: Bearer {token}" \
  -F "file=@path/to/image.jpg"

# Expected response:
{
  "thumbnailUrl": "http://127.0.0.1:10000/devstoreaccount1/product-images/{productId}/thumbnail.jpg",
  "mediumUrl": "http://127.0.0.1:10000/devstoreaccount1/product-images/{productId}/medium.jpg",
  "fullUrl": "http://127.0.0.1:10000/devstoreaccount1/product-images/{productId}/full.jpg"
}
```

### Image Storage Structure
```
product-images/
  ├── {productId}/
  │   ├── thumbnail.jpg  (150x150)
  │   ├── medium.jpg     (500x500)
  │   └── full.jpg       (original size)
```

### Configuration
All Azurite settings are in `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "AzuriteStorage": "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;"
  },
  "AzuriteStorage": {
    "ContainerName": "product-images"
  }
}
```
