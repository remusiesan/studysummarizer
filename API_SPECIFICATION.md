# Study Summarizer API Specification

## ✅ Completion Status: ALL REQUIREMENTS MET

### Base URL
```
http://localhost:5000/api
```

---

## 📋 Document Management Endpoints

### 1. Upload a Document
**POST** `/documents`
```json
Request:
{
  "title": "Introduction to Artificial Intelligence",
  "fileType": "pdf",
  "filePath": "/uploads/ai_intro.pdf"
}

Response (200 OK):
{
  "message": "Document uploaded successfully",
  "id": "D1"
}
```
✅ **Status**: Implemented & Tested

---

### 2. List All Documents
**GET** `/documents`
```json
Response (200 OK):
[
  {
    "id": "D1",
    "title": "Introduction to Artificial Intelligence",
    "fileType": "pdf",
    "status": "pending"
  }
]
```
✅ **Status**: Implemented & Tested

---

### 3. Get Document by ID
**GET** `/documents/{id}`
```json
Response (200 OK):
{
  "id": "D1",
  "title": "Introduction to Artificial Intelligence",
  "fileType": "pdf",
  "status": "pending",
  "uploadDate": "2025-10-15T14:35:00Z"
}
```
✅ **Status**: Implemented & Tested

---

### 4. Download Document File
**GET** `/documents/{id}/file`
```
Headers: Content-Type: application/pdf
Response: Binary file content
```
✅ **Status**: Implemented

---

### 5. Delete Document
**DELETE** `/documents/{id}`
```json
Response (200 OK):
{
  "message": "Document deleted successfully"
}
```
✅ **Status**: Implemented & Tested

---

## 📝 Summarization Management Endpoints

### 1. Generate Summary
**POST** `/documents/{documentId}/summarize`
```json
Request:
{
  "summaryType": "concise"
}

Response (200 OK):
{
  "message": "Summarization started",
  "documentId": "D1"
}
```
✅ **Status**: Implemented & Tested

---

### 2. Retrieve Summary
**GET** `/documents/{documentId}/summary`
```json
Response (200 OK):
{
  "documentId": "D1",
  "title": "Introduction to Artificial Intelligence",
  "summary": "Artificial Intelligence (AI) focuses on creating systems...",
  "generatedAt": "2025-10-15T14:40:00Z"
}
```
✅ **Status**: Implemented & Tested

---

### 3. Update/Regenerate Summary
**PATCH** `/documents/{documentId}/summary`
```json
Request:
{
  "summaryType": "detailed"
}

Response (200 OK):
{
  "message": "Summary regenerated successfully"
}
```
✅ **Status**: Implemented & Tested

---

## 👤 User Management Endpoints (Required)

### 1. Register New User
**POST** `/users/register`
```json
Request:
{
  "username": "student01",
  "email": "student01@example.com",
  "password": "securePassword123"
}

Response (200 OK):
{
  "message": "User registered successfully",
  "userId": "U1"
}
```
✅ **Status**: Implemented & Tested

---

### 2. User Login
**POST** `/users/login`
```json
Request:
{
  "email": "student01@example.com",
  "password": "securePassword123"
}

Response (200 OK):
{
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
}
```
✅ **Status**: Implemented & Tested

---

### 3. Get Current User Profile
**GET** `/users/profile`
```
Headers: Authorization: Bearer <token>

Response (200 OK):
{
  "id": "U1",
  "username": "student01",
  "email": "student01@example.com",
  "registeredAt": "2025-10-15T12:00:00Z"
}
```
✅ **Status**: Implemented & Tested

---

### 4. List User's Documents
**GET** `/users/{userId}/documents`
```
Headers: Authorization: Bearer <token>

Response (200 OK):
[
  {
    "documentId": "D1",
    "title": "Introduction to AI",
    "status": "summarized"
  }
]
```
✅ **Status**: Implemented & Tested

---

## 🔐 Authentication
- **Type**: JWT Bearer Token
- **Method**: Include token in `Authorization: Bearer <token>` header
- **Expiration**: 24 hours
- **Protected Endpoints**: Document upload, delete, summarization updates, user profile access

---

## 📊 ID Generation Format
- **User IDs**: U1, U2, U3... (String format)
- **Document IDs**: D1, D2, D3... (String format)
- **Summary IDs**: S1, S2, S3... (String format)

---

## 📦 Technology Stack
- **Framework**: .NET 8.0
- **Authentication**: JWT Bearer Tokens
- **Database**: SQLite (EF Core)
- **API Documentation**: Swagger/OpenAPI

---

## ✨ Test Results
- ✅ User Registration - PASS
- ✅ User Login - PASS
- ✅ Get User Profile - PASS
- ✅ Document Upload - PASS
- ✅ List Documents - PASS
- ✅ Get Document Details - PASS
- ✅ Generate Summary - PASS
- ✅ Get Summary - PASS
- ✅ Update Summary - PASS
- ✅ Get User Documents - PASS
- ✅ Delete Document - PASS

**Overall Status**: ✅ ALL REQUIREMENTS MET & TESTED
