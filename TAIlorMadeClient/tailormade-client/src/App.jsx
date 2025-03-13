import React, { useState } from "react";
import { UploadCloud } from "lucide-react";
import "bootstrap/dist/css/bootstrap.min.css";

export default function JobApplication() {
  const [jobDescription, setJobDescription] = useState("");
  const [selectedFile, setSelectedFile] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleFileChange = (event) => {
    const file = event.target.files[0];
    if (file && (file.type === "application/pdf" || file.name.endsWith(".docx"))) {
      setSelectedFile(file);
    } else {
      alert("Please upload a valid PDF or DOCX file.");
    }
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (!jobDescription || !selectedFile) {
      setError("Job description and resume file are required.");
      return;
    }

    setError(null);
    setLoading(true);

    const formData = new FormData();
    formData.append("jobDescription", jobDescription);
    formData.append("resumePdf", selectedFile);

    try {
      const response = await fetch("http://localhost:5000/api/CoverLetterRequests/generate-async", {
        method: "POST",
        body: formData,
      });

      const result = await response.json();
      if (!response.ok) {
        throw new Error(result.message || "Something went wrong");
      }

      alert(`Cover letter request submitted! Job ID: ${result.jobId}`);
      // todo: on timeout check for job id
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="d-flex justify-content-center align-items-center min-vh-100 min-vw-100 bg-light">
      <div className="card shadow-lg p-4 w-100 text-center" style={{ maxWidth: "500px" }}>
        <div className="card-body">
          <h2 className="card-title text-center mb-4">Generate Cover Letter</h2>

          {error && <p className="text-danger fw-semibold">{error}</p>}

          <form onSubmit={handleSubmit}>
            <div className="mb-3 text-start">
              <label className="form-label">Job Description</label>
              <textarea
                className="form-control"
                placeholder="Enter job description..."
                value={jobDescription}
                onChange={(e) => setJobDescription(e.target.value)}
                rows="4"
              />
            </div>

            <div className="mb-3 text-start">
              <label className="form-label">Attach Resume (PDF or DOCX)</label>
              <input type="file" className="form-control" accept=".pdf,.docx" onChange={handleFileChange} />
            </div>

            {selectedFile && <p className="text-success fw-semibold">Selected file: {selectedFile.name}</p>}

            <button type="submit" className="btn btn-primary w-100 d-flex align-items-center justify-content-center" disabled={loading}>
              {loading ? (
                <span className="spinner-border spinner-border-sm me-2"></span>
              ) : (
                <UploadCloud size={16} className="me-2" />
              )}
              {loading ? "Submitting..." : "Submit"}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
