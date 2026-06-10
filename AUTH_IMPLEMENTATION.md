# Authentication Implementation Guide

## Complete Authentication Flow

### 1️⃣ User Visits App (Not Logged In)

```
http://localhost:5173/
    ↓
Home page displays
    ↓
Navbar shows: "Login" & "Register" buttons
```

### 2️⃣ User Clicks Register or Login

**Register Flow:**
```
User → /register page
     ↓
Fills email & password
     ↓
Submits form → POST /api/auth/register
     ↓
Backend returns JWT token + userId
     ↓
Token stored in localStorage
     ↓
Redirected to /documents
```

**Login Flow:**
```
User → /login page
     ↓
Fills email & password
     ↓
Submits form → POST /api/auth/login
     ↓
Backend returns JWT token + userId
     ↓
Token stored in localStorage
     ↓
Redirected to /documents
```

### 3️⃣ User Accesses Protected Routes

```
User tries to access /documents
     ↓
ProtectedRoute component checks localStorage for token
     ↓
Token exists? → Allow access ✅
Token missing? → Redirect to /login 🔒
```

### 4️⃣ API Requests Include Token

```
Every API request includes:
Authorization: Bearer eyJhbGc...
```

Configured in `frontend/src/services/api.js`:
```javascript
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('authToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
```

### 5️⃣ User Clicks Logout

```
User clicks "Logout" button in navbar
     ↓
authService.logout() called
     ↓
Token removed from localStorage
     ↓
Auth state updated
     ↓
Redirected to home page /
     ↓
Navbar shows Login/Register again
```

---

## Key Files

### Authentication Pages
- **[Login.jsx](frontend/src/pages/Login.jsx)** — Email + password form
- **[Register.jsx](frontend/src/pages/Register.jsx)** — Create account with validation

### Protected Route
- **[ProtectedRoute.jsx](frontend/src/components/ProtectedRoute.jsx)** — Guards authenticated routes

### Custom Hook
- **[useAuth.js](frontend/src/hooks/useAuth.js)** — Manages authentication state globally

### API Service
- **[api.js](frontend/src/services/api.js)** — API client with token injection

### Main App
- **[App.jsx](frontend/src/App.jsx)** — Routing with auth-aware navbar

---

## Smart Redirects

### Already Logged In?
- Visiting `/login` → Redirects to `/documents`
- Visiting `/register` → Redirects to `/documents`

### Not Logged In?
- Visiting `/upload` → Redirects to `/login`
- Visiting `/documents` → Redirects to `/login`
- Visiting `/documents/:id` → Redirects to `/login`
- Visiting `/documents/:id/summary` → Redirects to `/login`

### Always Accessible
- `/` (home) — Unauthenticated users see login buttons
- `/login` — Public
- `/register` — Public

---

## User Experience Timeline

| Step | Page | User Sees | Action |
|------|------|-----------|--------|
| 1 | Home | Login/Register buttons | Click Register |
| 2 | Register | Email, password fields | Fill & submit |
| 3 | Redirected | Documents page loaded | Logged in! ✅ |
| 4 | Navbar | Upload, Documents links + Logout | Click Upload |
| 5 | Upload | File upload form | Upload a document |
| 6 | Redirected | Documents page | Document listed |
| 7 | Navbar | Click Logout | Logout button |
| 8 | Home | Login/Register buttons | Session ended |

---

## Security Features

✅ **JWT Token Storage** — Secure token in localStorage
✅ **Protected Routes** — Guards sensitive pages
✅ **Token Injection** — Auto-included in API requests
✅ **Redirect Guards** — Prevents accessing auth pages when logged in
✅ **Logout Cleanup** — Token removed from storage
✅ **Password Validation** — Min 6 chars, confirmation match
✅ **CORS-Ready** — Token sent with Authorization header

---

## Token Management

### Where Token Stored
```
Browser → Application → Local Storage
Key: "authToken"
Value: "eyJhbGc..." (JWT)
```

### How Token Persists
```
localStorage persists across browser refresh
User closes and reopens app → Still logged in ✅
```

### How Token Cleared
```
Click Logout → authService.logout()
              → localStorage.removeItem('authToken')
              → User fully logged out
```

---

## Testing the Auth Flow

### 1. Test Registration
```
1. Open http://localhost:5173
2. Click "Register"
3. Enter email: test@example.com
4. Enter password: password123
5. Confirm password: password123
6. Click Register
7. Should redirect to /documents
8. Navbar shows "Logout"
```

### 2. Test Protected Routes
```
1. Logout (click "Logout" in navbar)
2. Try to visit http://localhost:5173/documents
3. Should redirect to /login
```

### 3. Test Already Logged In Redirect
```
1. Register/Login (now authenticated)
2. Try to visit http://localhost:5173/login
3. Should redirect to /documents
```

### 4. Test Logout
```
1. Click "Logout" button
2. Should redirect to home
3. Navbar shows "Login/Register"
4. localStorage.getItem('authToken') should be null
```

---

## Troubleshooting

### Issue: "Stuck on login page"
**Solution:**
- Check backend is running: `cd backend && dotnet run`
- Check API URL in `.env`: `VITE_API_URL=http://localhost:5000/api`
- Check browser console for errors

### Issue: "Can't access protected pages after login"
**Solution:**
- Check localStorage: DevTools → Application → Local Storage
- Token should exist with key `authToken`
- Try refreshing the page

### Issue: "Logout doesn't work"
**Solution:**
- Check browser console for errors
- Verify token was removed from localStorage
- Try hard refresh: `Ctrl+Shift+R` (Windows) or `Cmd+Shift+R` (Mac)

---

## Environment Setup

### Backend (.NET)
```bash
cd backend
dotnet run
# Listens on http://localhost:5000
```

### Frontend (React)
```bash
cd frontend
npm install
npm run dev
# Available at http://localhost:5173
```

### Test Credentials
After registration, use the same email/password to login.

---

## Next Steps

✅ Auth implemented
✅ Protected routes working
✅ Smart redirects in place
⏭️ Ready to test with backend
⏭️ Ready to deploy

### To Continue
1. Start backend: `cd backend && dotnet run`
2. Start frontend: `cd frontend && npm run dev`
3. Open http://localhost:5173
4. Register a new account
5. Upload and summarize documents!
