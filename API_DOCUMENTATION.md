# API Endpoints Reference

## Base URL
```
http://localhost:5189/api
```

## Income Endpoints

### Get All Income
```
GET /api/income
```

**Query Parameters:**
- `userId` (required): The user's ID

**Response:**
```json
{
  "success": true,
  "message": "Operation successful",
  "data": [
    {
      "id": 1,
      "amount": 2500.00,
      "source": "Salary",
      "date": "2026-01-01",
      "description": "Monthly salary"
    }
  ]
}
```

### Get Income by ID
```
GET /api/income/{id}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "amount": 2500.00,
    "source": "Salary",
    "date": "2026-01-01",
    "description": "Monthly salary"
  }
}
```

### Create Income
```
POST /api/income
```

**Request Body:**
```json
{
  "amount": 2500.00,
  "source": "Salary",
  "date": "2026-01-01",
  "description": "Monthly salary"
}
```

**Response:** 201 Created
```json
{
  "success": true,
  "message": "Item created successfully",
  "data": {
    "id": 1,
    "amount": 2500.00,
    "source": "Salary",
    "date": "2026-01-01",
    "description": "Monthly salary"
  }
}
```

### Update Income
```
PUT /api/income/{id}
```

**Request Body:**
```json
{
  "amount": 2600.00,
  "source": "Salary",
  "date": "2026-02-01",
  "description": "Monthly salary"
}
```

**Response:** 200 OK

### Delete Income
```
DELETE /api/income/{id}
```

**Response:** 204 No Content or 200 OK with message

---

## Expense Endpoints

### Get All Expenses
```
GET /api/expenses
```

**Query Parameters:**
- `userId` (required): The user's ID
- `categoryId` (optional): Filter by category
- `startDate` (optional): Filter from date
- `endDate` (optional): Filter to date

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "categoryId": 1,
      "amount": 150.00,
      "date": "2026-01-05",
      "description": "Groceries"
    }
  ]
}
```

### Create Expense
```
POST /api/expenses
```

**Request Body:**
```json
{
  "categoryId": 1,
  "amount": 150.00,
  "date": "2026-01-05",
  "description": "Groceries"
}
```

### Update Expense
```
PUT /api/expenses/{id}
```

### Delete Expense
```
DELETE /api/expenses/{id}
```

---

## Category Endpoints

### Get All Categories
```
GET /api/categories
```

**Query Parameters:**
- `userId` (required): The user's ID

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "Groceries",
      "description": "Food and groceries"
    }
  ]
}
```

### Create Category
```
POST /api/categories
```

**Request Body:**
```json
{
  "name": "Groceries",
  "description": "Food and groceries"
}
```

### Update Category
```
PUT /api/categories/{id}
```

### Delete Category
```
DELETE /api/categories/{id}
```

---

## Budget Endpoints

### Get All Budgets
```
GET /api/budgets
```

**Query Parameters:**
- `userId` (required): The user's ID

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "categoryId": 1,
      "limit": 500.00,
      "month": 1,
      "year": 2026
    }
  ]
}
```

### Get Budget by ID
```
GET /api/budgets/{id}
```

### Create Budget
```
POST /api/budgets
```

**Request Body:**
```json
{
  "categoryId": 1,
  "limit": 500.00,
  "month": 1,
  "year": 2026
}
```

### Get Remaining Budget
```
GET /api/budgets/{id}/remaining
```

**Response:**
```json
{
  "success": true,
  "data": {
    "budgetId": 1,
    "limit": 500.00,
    "spent": 150.00,
    "remaining": 350.00,
    "percentUsed": 30
  }
}
```

### Update Budget
```
PUT /api/budgets/{id}
```

### Delete Budget
```
DELETE /api/budgets/{id}
```

---

## Analytics Endpoints

### Get Expenses by Category
```
GET /api/analytics/expenses-by-category
```

**Query Parameters:**
- `userId` (required): The user's ID
- `startDate` (optional): Filter from date
- `endDate` (optional): Filter to date

**Response:**
```json
{
  "success": true,
  "data": {
    "Groceries": 450.00,
    "Utilities": 200.00,
    "Entertainment": 150.00
  }
}
```

### Get Total Expenses
```
GET /api/analytics/total-expenses
```

**Response:**
```json
{
  "success": true,
  "data": 800.00
}
```

### Get Total Income
```
GET /api/analytics/total-income
```

**Response:**
```json
{
  "success": true,
  "data": 2500.00
}
```

### Get Net Balance
```
GET /api/analytics/net-balance
```

**Response:**
```json
{
  "success": true,
  "data": 1700.00
}
```

### Get Monthly Trends
```
GET /api/analytics/monthly-trends
```

**Query Parameters:**
- `userId` (required): The user's ID
- `months` (optional): Number of months to include (default: 12)

**Response:**
```json
{
  "success": true,
  "data": {
    "2025-12": {
      "income": 2500.00,
      "expenses": 1200.00,
      "balance": 1300.00
    },
    "2026-01": {
      "income": 2500.00,
      "expenses": 800.00,
      "balance": 1700.00
    }
  }
}
```

### Get Budget vs Actual
```
GET /api/analytics/budget-vs-actual
```

**Response:**
```json
{
  "success": true,
  "data": {
    "Budget_1": 350.00,
    "Budget_2": 50.00
  }
}
```

---

## Error Responses

### 400 Bad Request
```json
{
  "success": false,
  "message": "The provided input is invalid",
  "errors": [
    "Amount must be greater than zero",
    "Date cannot be in the future"
  ]
}
```

### 401 Unauthorized
```json
{
  "success": false,
  "message": "Authentication required"
}
```

### 403 Forbidden
```json
{
  "success": false,
  "message": "You don't have permission to access this resource"
}
```

### 404 Not Found
```json
{
  "success": false,
  "message": "Item not found"
}
```

### 500 Internal Server Error
```json
{
  "success": false,
  "message": "An unexpected server error occurred"
}
```

---

## Authentication

All endpoints (except login/register) require authentication via:
- JWT Bearer token in Authorization header
- Or Blazor server-side authentication context

**Example:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## Rate Limiting

Currently no rate limiting is enforced. Plan to implement if needed for production.

---

## Pagination

For future implementation:
```
GET /api/income?pageNumber=1&pageSize=10
```

Response will include pagination metadata:
```json
{
  "success": true,
  "data": {
    "items": [],
    "totalCount": 100,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 10,
    "hasNextPage": true,
    "hasPreviousPage": false
  }
}
```
