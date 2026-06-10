import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { documentService, summaryService } from '../services/api';
import '../styles/DocumentDetails.css';

export default function DocumentDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [document, setDocument] = useState(null);
  const [hasSummary, setHasSummary] = useState(false);
  const [loading, setLoading] = useState(true);
  const [summarizing, setSummarizing] = useState(false);
  const [message, setMessage] = useState({ type: '', text: '' });

  useEffect(() => {
    fetchDocumentAndSummary();
  }, [id]);

  const fetchDocumentAndSummary = async () => {
    try {
      setLoading(true);
      const docResponse = await documentService.getDocument(id);
      setDocument(docResponse.data);

      try {
        await summaryService.getSummary(id);
        setHasSummary(true);
      } catch {
        setHasSummary(false);
      }
    } catch (error) {
      setMessage({ type: 'error', text: 'Failed to load document' });
    } finally {
      setLoading(false);
    }
  };

  const handleGenerateSummary = async () => {
    setSummarizing(true);
    try {
      await summaryService.generateSummary(id, {
        summaryType: 'standard',
      });
      setHasSummary(true);
      setMessage({ type: 'success', text: 'Summary generated successfully!' });
    } catch (error) {
      setMessage({
        type: 'error',
        text: error.response?.data?.message || 'Failed to generate summary',
      });
    } finally {
      setSummarizing(false);
    }
  };

  const handleDownload = async () => {
    try {
      const response = await documentService.downloadDocument(id);
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `${document.title}.pdf`);
      document.body.appendChild(link);
      link.click();
      link.parentElement.removeChild(link);
    } catch (error) {
      setMessage({ type: 'error', text: 'Failed to download document' });
    }
  };

  if (loading) {
    return (
      <div className="details-container">
        <div className="loading">Loading document...</div>
      </div>
    );
  }

  if (!document) {
    return (
      <div className="details-container">
        <div className="error">Document not found</div>
      </div>
    );
  }

  return (
    <div className="details-container">
      <button onClick={() => navigate('/documents')} className="btn-back">
        ← Back to Documents
      </button>

      <div className="details-card">
        <div className="details-header">
          <h1>{document.title}</h1>
          <span className={`status status-${document.status?.toLowerCase() || 'pending'}`}>
            {document.status || 'Pending'}
          </span>
        </div>

        {message.text && (
          <div className={`message message-${message.type}`}>
            {message.text}
          </div>
        )}

        <div className="details-info">
          <div className="info-grid">
            <div className="info-item">
              <label>File Type</label>
              <p>{document.fileType?.toUpperCase() || 'Unknown'}</p>
            </div>
            <div className="info-item">
              <label>Uploaded</label>
              <p>{new Date(document.uploadedAt).toLocaleString()}</p>
            </div>
            <div className="info-item">
              <label>File Size</label>
              <p>{document.fileSize ? `${(document.fileSize / 1024).toFixed(2)} KB` : 'N/A'}</p>
            </div>
            <div className="info-item">
              <label>Status</label>
              <p>{document.status || 'Pending'}</p>
            </div>
          </div>
        </div>

        <div className="details-actions">
          <button onClick={handleDownload} className="btn btn-secondary">
            📥 Download Original
          </button>

          {hasSummary ? (
            <button
              onClick={() => navigate(`/documents/${id}/summary`)}
              className="btn btn-primary"
            >
              📄 View Summary
            </button>
          ) : (
            <button
              onClick={handleGenerateSummary}
              disabled={summarizing}
              className="btn btn-primary"
            >
              {summarizing ? 'Generating...' : '✨ Generate Summary'}
            </button>
          )}
        </div>
      </div>
    </div>
  );
}
