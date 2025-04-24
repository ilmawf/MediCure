import React, { useState } from 'react';

const SymptomChecker = () => {
  const [symptoms, setSymptoms] = useState('');
  const [diagnosis, setDiagnosis] = useState('');

  // Handle the form submission
  const handleSubmit = async (e) => {
    e.preventDefault();

    const response = await fetch('http://localhost:5000/api/symptom-checker', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ symptoms }),
    });

    const data = await response.json();
    setDiagnosis(data.diagnosis);  // Assuming backend returns the diagnosis result
  };

  return (
    <div>
      <h2>Symptom Checker</h2>
      <form onSubmit={handleSubmit}>
        <textarea
          placeholder="Enter your symptoms (e.g., fever, cough, headache)"
          value={symptoms}
          onChange={(e) => setSymptoms(e.target.value)}
        />
        <button type="submit">Check Symptoms</button>
      </form>

      {/* Display the diagnosis result */}
      {diagnosis && (
        <div>
          <h3>Diagnosis:</h3>
          <p>{diagnosis}</p>
        </div>
      )}
    </div>
  );
};

export default SymptomChecker;
