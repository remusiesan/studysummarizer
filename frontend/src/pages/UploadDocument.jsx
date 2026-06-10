import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { documentService } from '../services/api';
import '../styles/UploadDocument.css';

export default function UploadDocument() {
  const navigate = useNavigate();
  const [title, setTitle] = useState('');
  const [file, setFile] = useState(null);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState({ type: '', text: '' });

  const handleFileChange = (e) => {
    setFile(e.target.files[0]);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!title.trim()) {
      setMessage({ type: 'error', text: 'Please enter a document title' });
      return;
    }

    if (!file) {
      setMessage({ type: 'error', text: 'Please select a file to upload' });
      return;
    }

    setLoading(true);
    try {
      const response = await documentService.uploadDocument(title, file);
      setMessage({ type: 'success', text: 'Document uploaded successfully!' });
      setTimeout(() => navigate('/documents'), 2000);
    } catch (error) {
      setMessage({
        type: 'error',
        text: error.response?.data?.message || 'Failed to upload document',
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="upload-container">
      <div className="upload-card">
        <h1>Upload Document</h1>
        <p className="subtitle">Upload a PDF, DOCX, or TXT file to get started</p>

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="title">Document Title</label>
            <input
              id="title"
              type="text"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              placeholder="e.g., Introduction to Artificial Intelligence"
              disabled={loading}
            />
          </div>

          <div className="form-group">
            <label htmlFor="file">Select File</label>
            <div className="file-input-wrapper">
              <input
                id="file"
                type="file"
                onChange={handleFileChange}
                accept=".pdf,.docx,.txt"
                disabled={loading}
              />
              <span className="file-name">{file?.name || 'No file selected'}</span>
            </div>
            <small>Supported formats: PDF, DOCX, TXT</small>
          </div>

          {message.text && (
            <div className={`message message-${message.type}`}>
              {message.text}
            </div>
          )}

          <button type="submit" disabled={loading} className="btn btn-primary">
            {loading ? 'Uploading...' : 'Upload Document'}
          </button>
        </form>
      </div>
    </div>
  );
}
