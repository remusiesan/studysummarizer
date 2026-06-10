# Frontend Setup Guide

## Quick Start

### 1. Install Dependencies
```bash
cd frontend
npm install
```

### 2. Configure Environment
Create a `.env` file in the `frontend/` directory:
```bash
cp .env.example .env
```

Update the API URL if your backend runs on a different port:
```env
VITE_API_URL=http://localhost:5000/api
```

### 3. Start Development Server
```bash
npm run dev
```

Frontend will be available at: **http://localhost:5173**

---

## Authentication Flow

### Public Routes (No Login Required)
- `/` — Home page
- `/login` — Login page
- `/register` — Registration page

### Protected Routes (Login Required)
- `/documents` — Document list
- `/documents/:id` — Document details
- `/documents/:id/summary` — Summary viewer
- `/upload` — Upload document

### How It Works

1. **User Registration/Login**
   - User fills in email and password
   - Credentials sent to `/api/auth/register` or `/api/auth/login`
   - Backend returns JWT token and userId
   - Token stored in `localStorage` as `authToken`

2. **Protected Routes**
   - `ProtectedRoute` component checks for valid token
   - If no token → redirects to `/login`
   - If token exists → allows access

3. **API Requests**
   - All API calls automatically include `Authorization: Bearer <token>` header
   - Token extracted from `localStorage` in `api.js` interceptor

4. **Logout**
   - Click "Logout" button in navbar
   - Token removed from `localStorage`
   - User redirected to home page

---

## Project Structure

```
frontend/
├── src/
│   ├── pages/
│   │   ├── Home.jsx              # Landing page
│   │   ├── Login.jsx             # Login form
│   │   ├── Register.jsx          # Registration form
│   │   ├── UploadDocument.jsx    # Document upload
│   │   ├── DocumentList.jsx      # All documents
│   │   ├── DocumentDetails.jsx   # Document info
│   │   └── SummaryViewer.jsx     # Summary display
│   │
│   ├── components/
│   │   └── ProtectedRoute.jsx    # Auth guard component
│   │
│   ├── services/
│   │   └── api.js                # API client with axios
│   │
│   ├── styles/
│   │   ├── Home.css
│   │   ├── Auth.css              # Login/Register styles
│   │   ├── UploadDocument.css
│   │   ├── DocumentList.css
│   │   ├── DocumentDetails.css
│   │   └── SummaryViewer.css
│   │
│   ├── App.jsx                   # Main app with routing
│   ├── App.css                   # Global styles
│   └── main.jsx                  # Entry point
│
├── package.json
├── vite.config.js
├── .env.example
└── .gitignore
```

---

## Key Features Implemented

### 1. Authentication Pages
- **Login Page** — Email + password authentication
- **Register Page** — Create new account with password confirmation

### 2. Navigation Bar
- **Unauthenticated Users** — See Login/Register buttons
- **Authenticated Users** — See Documents/Upload links + Logout button

### 3. Protected Routes
- Automatically redirects unauthenticated users to login
- Seamless user experience with automatic redirects

### 4. API Integration
- JWT token management
- Automatic token injection in headers
- Error handling for auth failures

### 5. Document Management
- **Upload** — Create new documents (requires login)
- **List** — View all documents (requires login)
- **Details** — View document info (requires login)
- **Summary** — Read and regenerate summaries (requires login)
- **View Public Summaries** — Anyone can view summaries without login

---

## Environment Variables

### VITE_API_URL
- **Default**: `http://localhost:5000/api`
- **Purpose**: Backend API base URL
- **Usage**: Configure if backend runs on different port/host

---

## Building for Production

### Development Build
```bash
npm run dev
```

### Production Build
```bash
npm run build
```

Creates optimized `dist/` folder ready for deployment.

### Preview Production Build
```bash
npm run preview
```

---

## Troubleshooting

### Issue: "Failed to login" 
**Check:**
- Backend is running on `http://localhost:5000`
- `VITE_API_URL` in `.env` is correct
- Email and password are correct

### Issue: "Token not found" after login
**Check:**
- Browser localStorage is enabled
- No errors in browser console
- Backend returned token in response

### Issue: Redirected to login after refresh
**This is normal** — tokens are stored in localStorage and persist across refreshes.

### Issue: CORS errors
**Check:**
- Backend has CORS enabled
- `VITE_API_URL` matches backend URL exactly
- No trailing slashes in URL

---

## Development Tips

### Debug Auth State
Open browser DevTools → Application → Local Storage:
```
authToken: (your JWT token)
userId: (your user ID)
```

### Network Debugging
Open Network tab and check:
- Request headers include `Authorization: Bearer ...`
- Backend responses have `200 OK` status

### Hot Module Replacement (HMR)
Changes to `.jsx` and `.css` files automatically reload in browser.

---

## API Endpoints Used

### Authentication
- `POST /api/auth/login` — Login user
- `POST /api/auth/register` — Register user

### Documents
- `POST /api/documents` — Upload document
- `GET /api/documents` — Get all documents
- `GET /api/documents/{id}` — Get document details
- `GET /api/documents/{id}/file` — Download document
- `DELETE /api/documents/{id}` — Delete document

### Summaries
- `POST /api/documents/{documentId}/summarize` — Generate summary
- `GET /api/documents/{documentId}/summary` — Get summary
- `PATCH /api/documents/{documentId}/summary` — Regenerate summary

---

## Next Steps

1. ✅ Install dependencies: `npm install`
2. ✅ Configure `.env` with API URL
3. ✅ Start frontend: `npm run dev`
4. ✅ Start backend: `cd backend && dotnet run`
5. ✅ Open http://localhost:5173 in browser
6. ✅ Register new account
7. ✅ Upload documents
8. ✅ Generate summaries

---

## Support

For issues or questions:
1. Check browser console for errors
2. Check backend logs
3. Verify `.env` configuration
4. Check network tab for failed requests
