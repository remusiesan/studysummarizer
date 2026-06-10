# Study Summarizer — API Specification

**Base URL:** `http://localhost:5000/api`  
**Auth:** JWT Bearer Token — include as `Authorization: Bearer <token>` on protected endpoints  
**Token expiry:** 24 hours

---

## Authentication Endpoints

### Register
**POST** `/auth/register`

Request:
```json
{
  "email": "student@example.com",
  "password": "securePassword123"
}
```

Response `200 OK`:
```json
{
  "message": "User registered successfully",
  "userId": "U1",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
}
```

---

### Login
**POST** `/auth/login`

Request:
```json
{
  "email": "student@example.com",
  "password": "securePassword123"
}
```

Response `200 OK`:
```json
{
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6...",
  "userId": "U1"
}
```

---

### Get Profile
**GET** `/auth/profile`  
🔒 Requires auth

Response `200 OK`:
```json
{
  "id": "U1",
  "username": "student",
  "email": "student@example.com",
  "registeredAt": "2025-10-15T12:00:00Z"
}
```

---

### Get User Documents
**GET** `/auth/{userId}/documents`  
🔒 Requires auth

Response `200 OK`:
```json
[
  {
    "documentId": "D1",
    "title": "Introduction to AI",
    "status": "summarized"
  }
]
```

---

## Document Endpoints

### Upload Document
**POST** `/documents`  
🔒 Requires auth  
Content-Type: `multipart/form-data`

Form fields:
- `title` (string) — document title, max 255 characters
- `file` (binary) — file to upload, max 20 MB

Supported formats: PDF, DOC, DOCX, TXT, XLS, XLSX, PPT, PPTX, PNG, JPG, JPEG, GIF

Response `201 Created`:
```json
{
  "message": "Document uploaded successfully",
  "id": "D1"
}
```

---

### List All Documents
**GET** `/documents?pageNumber=1&pageSize=10`

Response `200 OK`:
```json
{
  "items": [
    {
      "id": "D1",
      "title": "Introduction to AI",
      "fileType": "pdf",
      "status": "pending",
      "uploadedAt": "2025-10-15T14:35:00Z",
      "fileSize": 204800
    }
  ],
  "totalCount": 1,
  "pageNumber": 1,
  "pageSize": 10
}
```

---

### Get Document by ID
**GET** `/documents/{id}`

Response `200 OK`:
```json
{
  "id": "D1",
  "title": "Introduction to AI",
  "fileType": "pdf",
  "status": "pending",
  "uploadedAt": "2025-10-15T14:35:00Z",
  "fileSize": 204800
}
```

---

### Download Document File
**GET** `/documents/{id}/file`

Response: binary file stream  
`Content-Type: application/octet-stream`

---

### Delete Document
**DELETE** `/documents/{id}`  
🔒 Requires auth

Response `200 OK`:
```json
{
  "message": "Document deleted successfully"
}
```

---

## Summary Endpoints

### Generate Summary
**POST** `/documents/{documentId}/summarize`  
🔒 Requires auth

Request:
```json
{
  "summaryType": "concise"
}
```

Response `201 Created`:
```json
{
  "message": "Summarization started",
  "documentId": "D1"
}
```

---

### Get Summary
**GET** `/documents/{documentId}/summary`

Response `200 OK`:
```json
{
  "documentId": "D1",
  "title": "Introduction to AI",
  "summary": "Artificial Intelligence (AI) focuses on creating systems...",
  "generatedAt": "2025-10-15T14:40:00Z"
}
```

---

### Update / Regenerate Summary
**PATCH** `/documents/{documentId}/summary`  
🔒 Requires auth

Request:
```json
{
  "summaryType": "detailed"
}
```

Response `200 OK`:
```json
{
  "message": "Summary regenerated successfully"
}
```

---

## ID Format

| Entity   | Format       | Example |
|----------|--------------|---------|
| User     | `U{n}`       | U1, U2  |
| Document | `D{n}`       | D1, D2  |
| Summary  | `S{n}`       | S1, S2  |

---

## Document Status Values

| Value        | Meaning                              |
|--------------|--------------------------------------|
| `pending`    | Uploaded, not yet summarised         |
| `summarizing`| Summary generation in progress       |
| `summarized` | Summary available                    |

---

## Error Responses

All errors follow the same envelope:

```json
{
  "success": false,
  "message": "Error description",
  "errorCode": "VALIDATION_ERROR"
}
```

| HTTP Status | Error Code         | When                              |
|-------------|--------------------|-----------------------------------|
| 400         | `VALIDATION_ERROR` | Invalid request data              |
| 401         | `UNAUTHORIZED`     | Missing or invalid token          |
| 404         | `NOT_FOUND`        | Resource does not exist           |
| 500         | `INTERNAL_ERROR`   | Unhandled server error            |

---

## Technology Stack

- **Framework:** .NET 8
- **Auth:** JWT Bearer Tokens
- **Database:** SQLite via Entity Framework Core
- **API Docs:** Swagger / OpenAPI at `/api/v1`
