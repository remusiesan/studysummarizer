# Study Summarizer - Monorepo

A full-stack application for uploading course documents and generating AI-powered summaries.

## 📁 Project Structure

```
studysummarizer/
├── backend/               # .NET API Backend
│   ├── Controllers/       # API endpoints
│   ├── Models/           # Data models
│   ├── Services/         # Business logic
│   ├── Data/             # Database context
│   ├── DTOs/             # Data transfer objects
│   ├── Middleware/       # Custom middleware
│   ├── Exceptions/       # Custom exceptions
│   ├── Program.cs        # Application entry point
│   ├── StudySummarizer.csproj
│   └── appsettings.json
│
└── frontend/              # React UI
    ├── src/
    │   ├── pages/        # Page components
    │   ├── services/     # API client
    │   ├── styles/       # CSS styles
    │   ├── components/   # Reusable components
    │   ├── hooks/        # Custom React hooks
    │   └── App.jsx       # Main app component
    ├── package.json
    ├── vite.config.js
    └── .env.example      # Environment variables template
```

## 🚀 Getting Started

### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd backend
   ```

2. Install dependencies:
   ```bash
   dotnet restore
   ```

3. Update the database:
   ```bash
   dotnet ef database update
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

   The API will be available at `http://localhost:5000`

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Create a `.env` file from the template:
   ```bash
   cp .env.example .env
   ```

4. Start the development server:
   ```bash
   npm run dev
   ```

   The frontend will be available at `http://localhost:5173`

## 🏗️ Architecture

### Backend (.NET)
- **Framework**: ASP.NET Core
- **Database**: SQLite (configurable)
- **ORM**: Entity Framework Core
- **Authentication**: JWT tokens

### Frontend (React)
- **Framework**: React 18 with Vite
- **Routing**: React Router v6
- **HTTP Client**: Axios
- **Styling**: CSS3

## 📋 API Endpoints

### Documents
- `POST /api/documents` - Upload a new document
- `GET /api/documents` - Get all documents
- `GET /api/documents/{id}` - Get document details
- `GET /api/documents/{id}/file` - Download document
- `DELETE /api/documents/{id}` - Delete document

### Summaries
- `POST /api/documents/{documentId}/summarize` - Generate summary
- `GET /api/documents/{documentId}/summary` - Get summary
- `PATCH /api/documents/{documentId}/summary` - Regenerate summary with different type

### Authentication
- `POST /api/auth/login` - Login user
- `POST /api/auth/register` - Register new user

## 🎯 Features

### Upload Document Page (`/upload`)
- Select file from device (PDF, DOCX, TXT)
- Enter document title
- Upload with progress indication
- Success/error feedback

### Document List Page (`/documents`)
- View all uploaded documents
- See document status (pending, summarized)
- Download original document
- Delete document
- Quick actions for each document

### Document Details Page (`/documents/:id`)
- View full document information
- File type and upload date
- Generate summary
- View existing summary
- Download original file

### Summary Viewer Page (`/documents/:id/summary`)
- Read AI-generated summary
- Choose summary type (concise, detailed, standard)
- Regenerate summary
- Copy to clipboard
- Export as file

## 🔧 Configuration

### Backend Configuration
Edit `backend/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=studysummarizer.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Frontend Configuration
Create `frontend/.env`:
```
VITE_API_URL=http://localhost:5000/api
```

## 📦 Dependencies

### Backend
- Microsoft.AspNetCore.App
- Entity.Framework.Core
- (See StudySummarizer.csproj for complete list)

### Frontend
- react@18
- react-router-dom@6
- axios
- vite

## 🧪 Testing

### Backend
```bash
cd backend
dotnet test
```

### Frontend
```bash
cd frontend
npm test
```

## 🚢 Deployment

### Backend
1. Build the project:
   ```bash
   cd backend
   dotnet publish -c Release
   ```

2. Deploy to your hosting platform (Azure, AWS, etc.)

### Frontend
1. Build for production:
   ```bash
   cd frontend
   npm run build
   ```

2. Deploy the `dist/` folder to your static hosting (Vercel, Netlify, etc.)

## 📝 Git Workflow

```bash
# Create feature branch
git checkout -b feature/your-feature

# Make changes in frontend or backend
# Commit changes
git add .
git commit -m "feat: description of changes"

# Push to remote
git push origin feature/your-feature

# Create pull request
```

## 🐛 Troubleshooting

### API Connection Issues
- Ensure backend is running on `http://localhost:5000`
- Check `VITE_API_URL` in frontend `.env`
- Verify CORS is enabled in backend

### Database Issues
- Delete `studysummarizer.db` and run migrations again
- Check database connection string in `appsettings.json`

### Frontend Build Issues
- Clear `node_modules` and reinstall: `rm -rf node_modules && npm install`
- Clear Vite cache: `rm -rf .vite`

## 📧 Support

For issues or questions, please create an issue in the repository.

## 📄 License

This project is licensed under the MIT License.
