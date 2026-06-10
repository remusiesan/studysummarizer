import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import '../styles/Home.css';

export default function Home() {
  const navigate = useNavigate();
  const { isAuthenticated } = useAuth();

  return (
    <div className="home-container">
      <div className="hero">
        <h1>📚 Study Summarizer</h1>
        <p className="tagline">Upload your course documents and get AI-powered summaries</p>

        <div className="features">
          <div className="feature">
            <span className="feature-icon">📤</span>
            <h3>Upload Documents</h3>
            <p>Upload PDF, DOCX, and TXT files easily</p>
          </div>
          <div className="feature">
            <span className="feature-icon">✨</span>
            <h3>AI Summarization</h3>
            <p>Get intelligent summaries of your documents</p>
          </div>
          <div className="feature">
            <span className="feature-icon">📊</span>
            <h3>Manage All Documents</h3>
            <p>Organize and access all your uploads in one place</p>
          </div>
        </div>

        <div className="cta-buttons">
          {isAuthenticated ? (
            <>
              <button onClick={() => navigate('/upload')} className="btn btn-primary btn-large">
                Start Uploading
              </button>
              <button onClick={() => navigate('/documents')} className="btn btn-secondary btn-large">
                View Documents
              </button>
            </>
          ) : (
            <>
              <button onClick={() => navigate('/login')} className="btn btn-primary btn-large">
                Login
              </button>
              <button onClick={() => navigate('/register')} className="btn btn-secondary btn-large">
                Register
              </button>
            </>
          )}
        </div>
      </div>
    </div>
  );
}
