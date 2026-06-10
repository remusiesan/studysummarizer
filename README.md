# Study Summarizer - Backend API

A modern .NET 8 REST API for document management and AI-powered summarization. Built with JWT authentication, SQLite database, and comprehensive endpoints.

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK
- SQLite (included with .NET)

### Setup

```bash
# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update

# Run the application
dotnet run
```

**API runs on:** `http://localhost:5000`  
**Swagger UI:** `http://localhost:5000/api/v1`

---

## 📋 Project Structure

```
├── Controllers/           # API endpoints
│   ├── AuthController.cs       # Authentication (login, register)
│   ├── DocumentsController.cs  # Document operations
│   └── SummariesController.cs  # Summary endpoints
├── Services/             # Business logic
│   ├── AuthService.cs         # JWT token generation, user auth
│   ├── DocumentService.cs     # File upload, retrieval, download
│   └── SummaryService.cs      # Summary generation and updates
├── Models/               # Data models
│   ├── User.cs
│   ├── Document.cs
│   └── Summary.cs
├── DTOs/                 # Data transfer objects
│   ├── Auth/
│   └── Documents/
├── Data/                 # Database context
│   └── AppDbContext.cs
├── Middleware/           # Custom middleware
│   └── ExceptionHandlingMiddleware.cs
├── Exceptions/           # Custom exceptions
│   └── ApiException.cs
├── Program.cs            # Application configuration
├── Constants.cs          # Application constants
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

**Response:**
```json
{
  "message": "Document uploaded successfully",
  "id": "D1"
}
```

### List Documents
```http
GET /api/documents
Authorization: Bearer {token}
```

### Get Document Details
```http
GET /api/documents/{id}
Authorization: Bearer {token}
```

### Download Document
```http
GET /api/documents/{id}/file
Authorization: Bearer {token}
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

### Tables
- **Users** — User accounts and authentication
- **Documents** — Uploaded files and metadata
- **Summaries** — AI-generated summaries
- **AIModels** — Available AI models configuration

---

## 🔧 Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=studysummarizer.db"
  },
  "Jwt": {
    "Secret": "your-secret-key-here",
    "Issuer": "StudySummarizer",
    "Audience": "StudySummarizerAPI"
  }
}
```

### appsettings.Development.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

---

## 📦 Dependencies

- **ASP.NET Core 8** — Web framework
- **Entity Framework Core** — ORM
- **SQLite** — Database
- **BCrypt.Net** — Password hashing
- **JWT** — Token-based authentication
- **Serilog** — Logging

---

## 🔒 Security

✅ JWT token-based authentication  
✅ Password hashing with BCrypt  
✅ CORS enabled for frontend access  
✅ Exception handling middleware  
✅ Authorization checks on protected endpoints  

---

## 🧪 Testing

Run tests:
```bash
dotnet test
```

---

## 📝 API Specification

See [API_SPECIFICATION.md](API_SPECIFICATION.md) for detailed endpoint documentation.

---

## 🚢 Deployment

### Build Release
```bash
dotnet publish -c Release
```

### Docker (optional)
```bash
docker build -t study-summarizer .
docker run -p 5000:5000 study-summarizer
```

---

## 📧 Support

For issues or questions, check the [API Specification](API_SPECIFICATION.md).

---

## 📄 License

MIT License - See LICENSE file for details
