import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { summaryService } from '../services/api';
import '../styles/SummaryViewer.css';

export default function SummaryViewer() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [summary, setSummary] = useState(null);
  const [summaryType, setSummaryType] = useState('standard');
  const [loading, setLoading] = useState(true);
  const [regenerating, setRegenerating] = useState(false);
  const [message, setMessage] = useState({ type: '', text: '' });

  useEffect(() => {
    fetchSummary();
  }, [id]);

  const fetchSummary = async () => {
    try {
      setLoading(true);
      const response = await summaryService.getSummary(id);
      setSummary(response.data);
    } catch (error) {
      setMessage({ type: 'error', text: 'Failed to load summary' });
    } finally {
      setLoading(false);
    }
  };

  const handleRegenerate = async () => {
    setRegenerating(true);
    try {
      const response = await summaryService.updateSummary(id, {
        summaryType,
      });
      setSummary(response.data);
      setMessage({ type: 'success', text: 'Summary regenerated successfully!' });
    } catch (error) {
      setMessage({
        type: 'error',
        text: error.response?.data?.message || 'Failed to regenerate summary',
      });
    } finally {
      setRegenerating(false);
    }
  };

  const handleCopy = () => {
    if (summary?.content) {
      navigator.clipboard.writeText(summary.content);
      setMessage({ type: 'success', text: 'Summary copied to clipboard!' });
      setTimeout(() => setMessage({ type: '', text: '' }), 3000);
    }
  };

  const handleExport = () => {
    if (summary?.content) {
      const element = document.createElement('a');
      const file = new Blob([summary.content], { type: 'text/plain' });
      element.href = URL.createObjectURL(file);
      element.download = `summary-${id}.txt`;
      document.body.appendChild(element);
      element.click();
      document.body.removeChild(element);
    }
  };

  if (loading) {
    return (
      <div className="summary-container">
        <div className="loading">Loading summary...</div>
      </div>
    );
  }

  if (!summary) {
    return (
      <div className="summary-container">
        <div className="error">
          <p>Summary not found</p>
          <button onClick={() => navigate(`/documents/${id}`)} className="btn btn-primary">
            Back to Document
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="summary-container">
      <button onClick={() => navigate(`/documents/${id}`)} className="btn-back">
        ← Back to Document
      </button>

      <div className="summary-card">
        <h1>Document Summary</h1>

        {message.text && (
          <div className={`message message-${message.type}`}>
            {message.text}
          </div>
        )}

        <div className="summary-controls">
          <div className="control-group">
            <label htmlFor="summary-type">Summary Type:</label>
            <select
              id="summary-type"
              value={summaryType}
              onChange={(e) => setSummaryType(e.target.value)}
              disabled={regenerating}
            >
              <option value="standard">Standard</option>
              <option value="concise">Concise</option>
              <option value="detailed">Detailed</option>
            </select>
          </div>

          <button
            onClick={handleRegenerate}
            disabled={regenerating}
            className="btn btn-secondary"
          >
            {regenerating ? 'Regenerating...' : '🔄 Regenerate'}
          </button>
        </div>

        <div className="summary-content">
          <p>{summary.content}</p>
        </div>

        <div className="summary-actions">
          <button onClick={handleCopy} className="btn btn-secondary">
            📋 Copy
          </button>
          <button onClick={handleExport} className="btn btn-secondary">
            💾 Export
          </button>
        </div>

        <div className="summary-meta">
          <p>
            <strong>Generated:</strong>{' '}
            {new Date(summary.generatedAt).toLocaleString()}
          </p>
        </div>
      </div>
    </div>
  );
}
