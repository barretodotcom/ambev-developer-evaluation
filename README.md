[Back to README](../README.md)

### Sales

#### POST /sales
- Description: Create a new sale with initial items
- Request Body:
  ```json
  {
    "customerId": "uuid",
    "customerName": "string",
    "saleNumber": "string",
    "saleDate": "2024-01-15T10:30:00Z",
    "branchId": "uuid",
    "branchName": "string",
    "items": [
      {
        "productId": "uuid",
        "productName": "string",
        "quantity": 5,
        "unitPrice": 100.50
      }
    ]
  }
  ```
- Response (201 Created):
  ```json
  {
    "success": true,
    "message": "Sale created successfully",
    "data": {
      "id": "uuid"
    }
  }
  ```
- Validation Rules:
  - `customerId`: Must not be empty
  - `customerName`: Must not be null or empty
  - `saleNumber`: Must not be null or empty
  - `branchId`: Must not be empty
  - `branchName`: Must not be null or empty
  - `saleDate`: Must not be in the future
  - `items`: Must not be empty
    - Each item's `productId`: Must not be empty
    - Each item's `productName`: Must not be empty
    - Each item's `quantity`: Must be greater than 0
    - Each item's `unitPrice`: Must be greater than 0
    - Combined quantity of same product: Cannot exceed 20 items
- Possible Errors:
  - `400 Bad Request`: Validation error
  - Item quantity exceeds 20 identical items per sale

#### GET /sales
- Description: Retrieve all sales with pagination
- Query Parameters:
  - `_page` (optional): Page number for pagination (default: 1)
  - `_size` (optional): Number of items per page (default: 10)
  - `_order` (optional): Ordering of results (e.g., "saleDate desc, saleNumber asc")
- Response:
  ```json
  {
    "data": [
      {
        "id": "uuid",
        "saleNumber": "string",
        "customerId": "uuid",
        "customerName": "string",
        "saleDate": "2024-01-15T10:30:00Z",
        "status": "Active",
        "itemsQuantity": "integer",
        "totalAmount": "number"
      }
    ],
    "totalItems": "integer",
    "currentPage": "integer",
    "totalPages": "integer"
  }
  ```

#### GET /sales/{id}
- Description: Retrieve a specific sale by ID
- Path Parameters:
  - `id`: Sale ID (UUID)
- Response:
  ```json
  {
    "success": true,
    "message": "Sale retrieved successfully",
    "data": {
      "id": "uuid",
      "saleNumber": "string",
      "customerId": "uuid",
      "customerName": "string",
      "saleDate": "2024-01-15T10:30:00Z",
      "status": "Active",
      "items": [
        {
          "id": "uuid",
          "productId": "uuid",
          "productName": "string",
          "quantity": 5,
          "unitPrice": 100.50,
          "discountPercentage": 0.10,
          "status": "Active"
        }
      ],
      "itemsQuantity": "integer"
    }
  }
  ```
- Possible Errors:
  - `404 Not Found`: Sale not found

#### PUT /sales/{id}
- Description: Update an existing sale and manage its items (add, update, or cancel)
- Path Parameters:
  - `id`: Sale ID (UUID)
- Request Body:
  ```json
  {
    "saleNumber": "string",
    "customerId": "uuid",
    "customerName": "string",
    "saleDate": "2024-01-15T10:30:00Z",
    "branchId": "uuid",
    "branchName": "string",
    "items": [
      {
        "id": "uuid (required for Update/Cancel operations)",
        "productId": "uuid",
        "productName": "string",
        "quantity": 5,
        "unitPrice": 100.50,
        "operation": 0
      }
    ]
  }
  ```
- Operation Types:
  - `0` - Cancel: Remove an existing item from the sale
  - `1` - Update: Modify an existing item's details
  - `2` - Create: Add a new item to the sale
- Response (200 OK):
  ```json
  {
    "success": true,
    "message": "Sale updated successfully",
    "data": {
      "id": "uuid",
      "saleNumber": "string",
      "customerId": "uuid",
      "customerName": "string",
      "saleDate": "2024-01-15T10:30:00Z",
      "branchId": "uuid",
      "branchName": "string",
      "status": "Active",
      "items": [
        {
          "id": "uuid",
          "productId": "uuid",
          "productName": "string",
          "quantity": 5,
          "unitPrice": 100.50,
          "discountPercentage": 0.10,
          "status": "Active"
        }
      ],
      "itemsQuantity": "integer",
      "totalAmount": "number"
    }
  }
  ```
- Possible Errors:
  - `400 Bad Request`: Validation error or business rule violation
  - Sale does not exist
  - Sale is already cancelled (cannot update a cancelled sale)
  - Item not found (when updating or cancelling a specific item)
  - Item is already cancelled (cannot update cancelled items)
  - Item quantity exceeds 20 identical items per sale

#### DELETE /sales/{id}
- Description: Cancel an entire sale and all its items
- Path Parameters:
  - `id`: Sale ID (UUID)
- Response (200 OK):
  ```json
  {
    "success": true,
    "message": "Sale cancelled successfully"
  }
  ```
- Possible Errors:
  - `400 Bad Request`: Sale not found or already cancelled

### Business Rules

#### Discount Calculation
Discounts are automatically calculated based on the total quantity of each product item:
- Quantity 1-3: No discount (0%)
- Quantity 4-9: 10% discount
- Quantity 10-20: 20% discount
- Quantity > 20: Not allowed (throws validation error)

#### Constraints
- Maximum 20 identical items per sale (cannot sell more than 20 of the same product)
- Sale date cannot be in the future
- Cancelled sales cannot be updated
- Cancelled items cannot be updated
- Sale must have at least one item

### Data Models

#### Sale
- `id`: Unique identifier (UUID)
- `saleNumber`: Sale reference number (max 50 characters)
- `customerId`: Reference to customer (UUID)
- `customerName`: Customer name (max 200 characters)
- `saleDate`: Date when sale occurred (must be ≤ current date)
- `branchId`: Reference to branch (UUID)
- `branchName`: Branch name (max 200 characters)
- `status`: Sale status (Active, Cancelled)
- `items`: Collection of sale items
- `itemsQuantity`: Total quantity of all active items
- `totalAmount`: Calculated total of all active items (sum of item.unitPrice × item.quantity × (1 - item.discountPercentage))
- `createdAt`: Creation timestamp
- `updatedAt`: Last update timestamp (nullable)

#### Sale Item
- `id`: Unique identifier (UUID)
- `productId`: Reference to product (UUID)
- `productName`: Product name (max 200 characters)
- `quantity`: Number of items
- `unitPrice`: Price per unit
- `discountPercentage`: Applied discount as decimal (0-1)
- `totalAmount`: Calculated total (unitPrice × quantity × (1 - discountPercentage))
- `status`: Item status (Active, Cancelled)
- `createdAt`: Creation timestamp
- `updatedAt`: Last update timestamp (nullable)

### Domain Events

#### SaleCreatedDomainEvent
Published when a new sale is created.
- `saleId`: The created sale ID
- `customerId`: Associated customer ID
- `saleDate`: Sale date

#### SaleUpdatedDomainEvent
Published when a sale is updated.
- `id`: Sale ID
- `customerId`: Associated customer ID

#### SaleCancelledDomainEvent
Published when a sale is cancelled.
- `saleId`: The cancelled sale ID

<br>