import React, { useState } from "react";
import { UploadCloud } from "lucide-react";

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
    <div className="flex justify-center items-center min-h-screen bg-gray-100 p-4">
      <div className="w-full max-w-lg p-6 bg-white shadow-md rounded-xl">
        <h2 className="text-xl font-semibold mb-4">Generate Cover Letter</h2>

        <textarea
          placeholder="Enter job description..."
          value={jobDescription}
          onChange={(e) => setJobDescription(e.target.value)}
          className="w-full h-32 p-2 border rounded-md"
        />

        <div className="mt-4">
          <label className="block text-sm font-medium">Attach Resume (PDF or DOCX)</label>
          <input
            type="file"
            accept=".pdf,.docx"
            onChange={handleFileChange}
            className="w-full border p-2 rounded-md"
          />
        </div>

        {selectedFile && (
          <p className="text-sm text-green-600 mt-2">Selected file: {selectedFile.name}</p>
        )}

        <button className="w-full bg-blue-600 text-white py-2 mt-4 rounded-lg flex items-center justify-center gap-2">
          <UploadCloud size={16} /> Submit
        </button>
      </div>
    </div>
  );
}
