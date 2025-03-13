import React, { useState } from "react";
import { UploadCloud } from "lucide-react";
import "bootstrap/dist/css/bootstrap.min.css";

export default function JobApplication() {
  const [jobDescription, setJobDescription] = useState("");
  const [selectedFile, setSelectedFile] = useState(null);

  const handleFileChange = (event) => {
    const file = event.target.files[0];
    if (file && (file.type === "application/pdf" || file.name.endsWith(".docx"))) {
      setSelectedFile(file);
    } else {
      alert("Please upload a valid PDF or DOCX file.");
    }
  };

  return (
    <div className="d-flex justify-content-center align-items-center min-vh-100 min-vw-100 bg-light">
      <div className="card shadow-lg p-4 w-100 text-center" style={{ maxWidth: "500px" }}>
        <div className="card-body">
          <h2 className="card-title text-center mb-4">Generate Cover Letter</h2>
          
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
            <input
              type="file"
              className="form-control"
              accept=".pdf,.docx"
              onChange={handleFileChange}
            />
          </div>

          {selectedFile && (
            <p className="text-success fw-semibold">Selected file: {selectedFile.name}</p>
          )}

          <button className="btn btn-primary w-100 d-flex align-items-center justify-content-center">
            <UploadCloud size={16} className="me-2" /> Submit
          </button>
        </div>
      </div>
    </div>
  );
}
