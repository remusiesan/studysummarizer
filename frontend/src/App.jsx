import { BrowserRouter as Router, Routes, Route, Link, useNavigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import UploadDocument from './pages/UploadDocument';
import DocumentList from './pages/DocumentList';
import DocumentDetails from './pages/DocumentDetails';
import SummaryViewer from './pages/SummaryViewer';
import ProtectedRoute from './components/ProtectedRoute';
import './App.css';

function Navbar() {
  const navigate = useNavigate();
  const { isAuthenticated, logout } = useAuth();

  const handleLogout = () => {
    logout();
    navigate('/', { replace: true });
  };

  return (
    <nav className="navbar">
      <Link to="/" className="navbar-brand">
        📚 StudySummarizer
      </Link>
      <div className="nav-links">
        <Link to="/">Home</Link>
        {isAuthenticated && (
          <>
            <Link to="/documents">Documents</Link>
            <Link to="/upload">Upload</Link>
          </>
        )}
      </div>
      <div className="nav-auth">
        {isAuthenticated ? (
          <button onClick={handleLogout} className="btn-logout">
            Logout
          </button>
        ) : (
          <>
            <Link to="/login" className="btn-nav-link">
              Login
            </Link>
            <Link to="/register" className="btn-nav-link btn-primary-link">
              Register
            </Link>
          </>
        )}
      </div>
    </nav>
  );
}

function AppContent() {
  return (
    <div className="app">
      <Navbar />

      <main className="main-content">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route
            path="/upload"
            element={
              <ProtectedRoute>
                <UploadDocument />
              </ProtectedRoute>
            }
          />
          <Route
            path="/documents"
            element={
              <ProtectedRoute>
                <DocumentList />
              </ProtectedRoute>
            }
          />
          <Route
            path="/documents/:id"
            element={
              <ProtectedRoute>
                <DocumentDetails />
              </ProtectedRoute>
            }
          />
          <Route
            path="/documents/:id/summary"
            element={
              <ProtectedRoute>
                <SummaryViewer />
              </ProtectedRoute>
            }
          />
        </Routes>
      </main>

      <footer className="footer">
        <p>&copy; 2024 Study Summarizer. All rights reserved.</p>
      </footer>
    </div>
  );
}

function App() {
  return (
    <Router>
      <AuthProvider>
        <AppContent />
      </AuthProvider>
    </Router>
  );
}

export default App;
