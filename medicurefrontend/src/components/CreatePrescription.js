
import React, { useState } from 'react';

const CreatePrescription = () => {
  const [prescription, setPrescription] = useState({
    patientId: '',
    medication: '',
    dosage: '',
    instructions: ''
  });
  const [error, setError] = useState('');

  const token = localStorage.getItem('token');

  const handleSubmit = async (e) => {
    e.preventDefault();

    const response = await fetch('http://localhost:5000/api/prescription', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`,
      },
      body: JSON.stringify(prescription),
    });

    if (response.ok) {
      alert('Prescription created successfully');
    } else {
      setError('Failed to create prescription');
    }
  };

  return (
    <div>
      <h3>Create Prescription</h3>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Patient ID"
          value={prescription.patientId}
          onChange={(e) => setPrescription({ ...prescription, patientId: e.target.value })}
          required
        />
        <input
          type="text"
          placeholder="Medication"
          value={prescription.medication}
          onChange={(e) => setPrescription({ ...prescription, medication: e.target.value })}
          required
        />
        <input
          type="text"
          placeholder="Dosage"
          value={prescription.dosage}
          onChange={(e) => setPrescription({ ...prescription, dosage: e.target.value })}
          required
        />
        <textarea
          placeholder="Instructions"
          value={prescription.instructions}
          onChange={(e) => setPrescription({ ...prescription, instructions: e.target.value })}
          required
        />
        <button type="submit">Create Prescription</button>
      </form>
    </div>
  );
};

export default CreatePrescription;
