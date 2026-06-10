import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { documentService } from '../services/api';
import '../styles/DocumentList.css';

export default function DocumentList() {
  const navigate = useNavigate();
  const [documents, setDocuments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [message, setMessage] = useState({ type: '', text: '' });

  useEffect(() => {
    fetchDocuments();
  }, []);

  const fetchDocuments = async () => {
    try {
      setLoading(true);
      const response = await documentService.getAllDocuments();
      setDocuments(response.data);
    } catch (error) {
      setMessage({
        type: 'error',
        text: 'Failed to load documents',
      });
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this document?')) return;

    try {
      await documentService.deleteDocument(id);
      setDocuments(documents.filter((doc) => doc.id !== id));
      setMessage({ type: 'success', text: 'Document deleted successfully' });
    } catch (error) {
      setMessage({
        type: 'error',
        text: 'Failed to delete document',
      });
    }
  };

  const handleDownload = async (id, title) => {
    try {
      const response = await documentService.downloadDocument(id);
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `${title}.pdf`);
      document.body.appendChild(link);
      link.click();
      link.parentElement.removeChild(link);
    } catch (error) {
      setMessage({ type: 'error', text: 'Failed to download document' });
    }
  };

  if (loading) {
    return (
      <div className="documents-container">
        <div className="loading">Loading documents...</div>
      </div>
    );
  }

  return (
    <div className="documents-container">
      <div className="documents-header">
        <h1>My Documents</h1>
        <button
          onClick={() => navigate('/upload')}
          className="btn btn-primary"
        >
          + Upload New Document
        </button>
      </div>

      {message.text && (
        <div className={`message message-${message.type}`}>
          {message.text}
        </div>
      )}

      {documents.length === 0 ? (
        <div className="empty-state">
          <h2>No documents yet</h2>
          <p>Upload your first document to get started</p>
          <button
            onClick={() => navigate('/upload')}
            className="btn btn-primary"
          >
            Upload Document
          </button>
        </div>
      ) : (
        <div className="documents-grid">
          {documents.map((doc) => (
            <div key={doc.id} className="document-card">
              <div className="doc-header">
                <h3>{doc.title}</h3>
                <span className={`status status-${doc.status?.toLowerCase() || 'pending'}`}>
                  {doc.status || 'Pending'}
                </span>
              </div>

              <div className="doc-info">
                <p>
                  <strong>Type:</strong> {doc.fileType?.toUpperCase() || 'Unknown'}
                </p>
                <p>
                  <strong>Uploaded:</strong>{' '}
                  {new Date(doc.uploadedAt).toLocaleDateString()}
                </p>
              </div>

              <div className="doc-actions">
                <button
                  onClick={() => navigate(`/documents/${doc.id}`)}
                  className="btn btn-secondary"
                >
                  View Details
                </button>
                <button
                  onClick={() => handleDownload(doc.id, doc.title)}
                  className="btn btn-secondary"
                >
                  Download
                </button>
                <button
                  onClick={() => handleDelete(doc.id)}
                  className="btn btn-danger"
                >
                  Delete
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
