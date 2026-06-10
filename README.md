# Study Summarizer - Backend API

A .NET 8 REST API for document management and AI-powered summarization. Built with JWT authentication, SQLite, onion architecture, and the Unit of Work pattern.

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK

### Setup

```bash
# Restore dependencies
dotnet restore

# Run the application (database is created automatically on first run)
dotnet run
```

**API runs on:** `http://localhost:5000`  
**Swagger UI:** `http://localhost:5000/api/v1`

---

## 📋 Project Structure

```
├── Application/                  # Interfaces and contracts (no infrastructure deps)
│   ├── Interfaces/
│   │   └── ITokenService.cs
│   ├── Repositories/
│   │   ├── IRepository.cs
│   │   └── IUnitOfWork.cs
│   └── Settings/
│       └── JwtSettings.cs
├── Infrastructure/               # Implementations with external dependencies
│   └── Services/
│       └── JwtTokenService.cs    # JWT token generation
├── Controllers/                  # API endpoints
│   ├── AuthController.cs
│   ├── DocumentsController.cs
│   └── SummariesController.cs
├── Services/                     # Application services (business logic)
│   ├── AuthService.cs
│   ├── DocumentService.cs
│   ├── SummaryService.cs
│   └── IdGeneratorService.cs
├── Repository/                   # Repository and Unit of Work implementations
│   ├── Repository.cs
│   ├── UnitOfWork.cs
│   └── RepositoryExtensions.cs
├── Models/                       # Domain entities
│   ├── User.cs
│   ├── Document.cs
│   ├── Summary.cs
│   └── AIModel.cs
├── DTOs/                         # Data transfer objects
│   ├── Auth/
│   ├── Documents/
│   └── Summaries/
├── Data/
│   └── AppDbContext.cs
├── Middleware/
│   └── ExceptionHandlingMiddleware.cs
├── Exceptions/
│   └── ApiException.cs
├── Properties/
│   └── launchSettings.json
├── Program.cs
├── Constants.cs
└── StudySummarizer.csproj
```

---

## 🔐 Authentication

### Register User
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "message": "User registered successfully",
  "userId": "U1",
  "token": "eyJhbGc..."
}
```

### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "message": "Login successful",
  "token": "eyJhbGc...",
  "userId": "U1"
}
```

### Get Profile
```http
GET /api/auth/profile
Authorization: Bearer {token}
```

### Get User Documents
```http
GET /api/auth/{userId}/documents
Authorization: Bearer {token}
```

---

## 📄 Document Endpoints

### Upload Document
```http
POST /api/documents
Authorization: Bearer {token}
Content-Type: multipart/form-data

form-data:
  title: "My Document"
  file: (binary file)
```

**Supported formats:** PDF, DOC, DOCX, TXT, XLS, XLSX, PPT, PPTX, PNG, JPG, JPEG, GIF  
**Max file size:** 20 MB

**Response:**
```json
{
  "message": "Document uploaded successfully",
  "id": "D1"
}
```

### List Documents
```http
GET /api/documents?pageNumber=1&pageSize=10
```

### Get Document Details
```http
GET /api/documents/{id}
```

### Download Document
```http
GET /api/documents/{id}/file
```

### Delete Document
```http
DELETE /api/documents/{id}
Authorization: Bearer {token}
```

---

## ✨ Summary Endpoints

### Generate Summary
```http
POST /api/documents/{documentId}/summarize
Authorization: Bearer {token}
Content-Type: application/json

{
  "summaryType": "standard"
}
```

### Get Summary
```http
GET /api/documents/{documentId}/summary
```

### Regenerate Summary
```http
PATCH /api/documents/{documentId}/summary
Authorization: Bearer {token}
Content-Type: application/json

{
  "summaryType": "concise"
}
```

---

## 🗄️ Database

**Type:** SQLite  
**File:** `studysummarizer.db`  
**Initialisation:** created automatically via `EnsureCreated()` on startup

### Tables
- **Users** — user accounts and authentication
- **Documents** — uploaded files and metadata (file content stored as blob)
- **Summaries** — generated summaries
- **AIModels** — AI model configuration

---

## 🔧 Configuration

### appsettings.json
```json
{
  "Jwt": {
    "Secret": "your-secret-key-here",
    "Issuer": "StudySummarizer",
    "Audience": "StudySummarizerAPI"
  }
}
```

URL and environment are configured in `Properties/launchSettings.json` for development, and via the `ASPNETCORE_URLS` environment variable in production.

---

## 📦 Dependencies

- **ASP.NET Core 8** — web framework
- **Entity Framework Core + SQLite** — ORM and database
- **BCrypt.Net** — password hashing
- **Microsoft.IdentityModel.Tokens** — JWT token generation
- **FluentValidation** — request validation
- **Serilog** — structured logging

---

## 🔒 Security

✅ JWT token-based authentication  
✅ Password hashing with BCrypt  
✅ CORS enabled for frontend access  
✅ Global exception handling middleware  
✅ FluentValidation on all request DTOs  
✅ Authorization checks on protected endpoints  

---

## 📝 API Specification

See [API_SPECIFICATION.md](API_SPECIFICATION.md) for detailed endpoint documentation.
