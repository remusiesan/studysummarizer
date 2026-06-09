# Study Summarizer API

A modern .NET 8 microservice API for document management and AI-powered summarization. Built with JWT authentication, SQLite database, and comprehensive REST endpoints.

## 🎯 Overview

Study Summarizer provides a complete backend solution for:
- **Document Management**: Upload, retrieve, list, and delete documents
- **Summarization**: Generate, retrieve, and update document summaries
- **User Management**: User registration, authentication, and profile management
- **Secure Access**: JWT Bearer token-based authentication

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 LTS SDK
- SQLite (included with .NET)

### Installation

```bash
# Clone the repository
git clone <repository-url>
cd studysummarizer

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run --urls="http://localhost:5000"
```

The API will be available at `http://localhost:5000`

## 📋 API Endpoints

### Base URL
```
http://localhost:5000/api
```

### 👤 User Management

#### Register New User
```http
POST /users/register
Content-Type: application/json

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

#### User Login
```http
POST /users/login
Content-Type: application/json

{
  "email": "student01@example.com",
  "password": "securePassword123"
}

Response (200 OK):
{
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### Get User Profile
```http
GET /users/profile
Authorization: Bearer <token>

Response (200 OK):
{
  "id": "U1",
  "username": "student01",
  "email": "student01@example.com",
  "registeredAt": "2025-10-15T12:00:00Z"
}
```

#### List User's Documents
```http
GET /users/{userId}/documents
Authorization: Bearer <token>

Response (200 OK):
[
  {
    "documentId": "D1",
    "title": "Introduction to AI",
    "status": "summarized"
  }
]
```

### 📄 Document Management

#### Upload Document
```http
POST /documents
Authorization: Bearer <token>
Content-Type: multipart/form-data

Form Fields:
- title: Document title (string)
- file: Actual file to upload (binary)

Response (200 OK):
{
  "message": "Document uploaded successfully",
  "id": "D1"
}
```

**Note**: FileType is automatically extracted from the file extension. FilePath is generated as `uploads/{documentId}_{filename}`

#### List All Documents
```http
GET /documents

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

#### Get Document Details
```http
GET /documents/{id}

Response (200 OK):
{
  "id": "D1",
  "title": "Introduction to Artificial Intelligence",
  "fileType": "pdf",
  "status": "pending",
  "uploadDate": "2025-10-15T14:35:00Z"
}
```

#### Download Document File
```http
GET /documents/{id}/file

Response (200 OK):
[Binary file content]
Headers:
- Content-Type: application/octet-stream
- Content-Disposition: attachment; filename="filename.pdf"
```

#### Delete Document
```http
DELETE /documents/{id}
Authorization: Bearer <token>

Response (200 OK):
{
  "message": "Document deleted successfully"
}
```

### 📝 Summarization Management

#### Generate Summary
```http
POST /documents/{documentId}/summarize
Authorization: Bearer <token>
Content-Type: application/json

{
  "summaryType": "concise"
}

Response (200 OK):
{
  "message": "Summarization started",
  "documentId": "D1"
}
```

#### Retrieve Summary
```http
GET /documents/{documentId}/summary

Response (200 OK):
{
  "documentId": "D1",
  "title": "Introduction to Artificial Intelligence",
  "summary": "Artificial Intelligence (AI) focuses on creating systems capable of performing tasks that normally require human intelligence...",
  "generatedAt": "2025-10-15T14:40:00Z"
}
```

#### Update/Regenerate Summary
```http
PATCH /documents/{documentId}/summary
Authorization: Bearer <token>
Content-Type: application/json

{
  "summaryType": "detailed"
}

Response (200 OK):
{
  "message": "Summary regenerated successfully"
}
```

## 🔐 Authentication

The API uses **JWT Bearer Token** authentication for protected endpoints.

### How to Use Authentication

1. **Register** a new user or **login** to get a token:
```bash
curl -X POST http://localhost:5000/api/users/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"password"}'
```

2. **Include the token** in the `Authorization` header for protected endpoints:
```bash
curl -H "Authorization: Bearer YOUR_TOKEN" \
  http://localhost:5000/api/users/profile
```

### Token Details
- **Type**: JWT (JSON Web Token)
- **Format**: Bearer token in `Authorization` header
- **Expiration**: 24 hours
- **Algorithm**: HS256 (HMAC with SHA-256)

### Protected Endpoints
- POST `/documents` - Upload document
- DELETE `/documents/{id}` - Delete document
- POST `/documents/{documentId}/summarize` - Generate summary
- PATCH `/documents/{documentId}/summary` - Update summary
- GET `/users/profile` - Get user profile
- GET `/users/{userId}/documents` - List user documents

## 🏗️ Project Structure

```
studysummarizer/
├── Controllers/              # API endpoint handlers
│   ├── AuthController.cs
│   ├── DocumentsController.cs
│   └── SummariesController.cs
├── Services/                 # Business logic
│   ├── AuthService.cs
│   ├── DocumentService.cs
│   ├── SummaryService.cs
│   └── IdGeneratorService.cs
├── Models/                   # Data models
│   ├── User.cs
│   ├── Document.cs
│   ├── Summary.cs
│   └── AIModel.cs
├── DTOs/                     # Data Transfer Objects
│   ├── Auth/
│   ├── Documents/
│   └── Summaries/
├── Data/                     # Database context
│   └── AppDbContext.cs
├── Middleware/               # Request/response handling
│   └── ExceptionHandlingMiddleware.cs
├── Exceptions/               # Custom exceptions
│   └── ApiException.cs
├── Constants.cs              # Application constants
├── Program.cs                # Application configuration
├── StudySummarizer.csproj    # Project file
└── README.md                 # This file
```

## 🗄️ Database

### Technology
- **Database**: SQLite
- **ORM**: Entity Framework Core
- **Location**: `studysummarizer.db`

### Tables
- **Users**: User accounts and authentication
- **Documents**: Uploaded documents metadata
- **Summaries**: Generated summaries
- **AIModels**: AI model configurations (optional)

### ID Format
- **User IDs**: `U1`, `U2`, `U3`... (String format)
- **Document IDs**: `D1`, `D2`, `D3`... (String format)
- **Summary IDs**: `S1`, `S2`, `S3`... (String format)

## 📦 Technology Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| **Framework** | .NET | 8.0 LTS |
| **Language** | C# | Latest |
| **Database** | SQLite | - |
| **ORM** | Entity Framework Core | 8.0.8 |
| **Authentication** | JWT | - |
| **API Documentation** | Swagger/OpenAPI | - |
| **Logging** | Serilog | 4.3.1 |
| **Validation** | FluentValidation | 11.9.2 |
| **Password Hashing** | BCrypt.Net-Next | 4.2.0 |

## 🔧 Configuration

### Environment Variables

```bash
# JWT Configuration (from appsettings.json)
Jwt:Secret=your-secret-key-change-this-in-production
Jwt:Issuer=StudySummarizer
Jwt:Audience=StudySummarizerAPI
```

### Logging

Logs are stored in the `logs/` directory:
```bash
logs/app-YYYY-MM-DD.txt
```

### File Uploads

Uploaded files are stored in:
```
uploads/
└── D1_filename.ext
└── D2_filename.ext
```

## 🧪 Testing

### Test User Registration & Login
```bash
# Register
curl -X POST http://localhost:5000/api/users/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","email":"test@example.com","password":"Test@123"}'

# Login
curl -X POST http://localhost:5000/api/users/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"Test@123"}'
```

### Test Document Upload
```bash
# Create a test file
echo "Test content" > test.txt

# Upload (use token from login)
curl -X POST http://localhost:5000/api/documents \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -F "title=Test Document" \
  -F "file=@test.txt"
```

### Test Protected Endpoints
```bash
# Get user profile
curl -H "Authorization: Bearer YOUR_TOKEN" \
  http://localhost:5000/api/users/profile

# Get user documents
curl -H "Authorization: Bearer YOUR_TOKEN" \
  http://localhost:5000/api/users/U1/documents
```

## 📊 API Response Format

All API responses follow a consistent format:

### Success Response
```json
{
  "message": "Operation successful",
  "data": { /* response data */ }
}
```

### Error Response
```json
{
  "error": "Error message",
  "code": "ERROR_CODE",
  "timestamp": "2025-10-15T14:35:00Z"
}
```

### HTTP Status Codes
- **200 OK**: Request successful
- **201 Created**: Resource created
- **400 Bad Request**: Invalid input
- **401 Unauthorized**: Authentication required
- **404 Not Found**: Resource not found
- **500 Internal Server Error**: Server error

## 🔒 Security Features

✅ **JWT Authentication**: Secure token-based authentication  
✅ **BCrypt Password Hashing**: Industry-standard password hashing  
✅ **CORS Support**: Configurable cross-origin requests  
✅ **Input Validation**: FluentValidation framework  
✅ **Exception Handling**: Centralized error management  
✅ **Authorization**: Role-based endpoint protection  

## 📝 Constants

Key application constants are defined in `Constants.cs`:

```csharp
Constants.Jwt.SchemeId = "Bearer"
Constants.ApiRoutes.Documents = "api/documents"
Constants.ApiRoutes.Users = "api/users"
Constants.DocumentStatus.Pending = "pending"
Constants.DocumentStatus.Summarizing = "summarizing"
Constants.DocumentStatus.Summarized = "summarized"
```

See `Constants.cs` for the complete list of constants.

## 🛠️ Development

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run
```

### Clean
```bash
dotnet clean
```

### Restore Dependencies
```bash
dotnet restore
```

## 📄 API Documentation

Interactive API documentation is available via Swagger UI:
```
http://localhost:5000/api/v1
```

## 🐛 Troubleshooting

### Database Issues
If you encounter database errors, delete the database file and rebuild:
```bash
rm studysummarizer.db
dotnet build
dotnet run
```

### Port Already in Use
If port 5000 is in use, specify a different port:
```bash
dotnet run --urls="http://localhost:5001"
```

### Authentication Issues
- Ensure the token is included in the `Authorization` header
- Check that the token has not expired (24-hour expiration)
- Verify the JWT secret in `appsettings.json`

## 📚 Additional Resources

- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [JWT Authentication](https://tools.ietf.org/html/rfc7519)
- [Swagger/OpenAPI](https://swagger.io/)

## 📄 License

This project is part of the Learning .NET course.

## 👨‍💻 Author

Claude Haiku 4.5

## 🤝 Contributing

This is an educational project. Contributions are welcome!

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📞 Support

For issues and questions:
- Check existing documentation
- Review API_SPECIFICATION.md for detailed endpoint info
- Check logs in the `logs/` directory

---

**Last Updated**: June 9, 2026  
**API Version**: 1.0.0  
**Status**: ✅ Production Ready
