// components/ViewPrescriptions.js
import React, { useState, useEffect } from 'react';

const ViewPrescriptions = () => {
  const [prescriptions, setPrescriptions] = useState([]);
  const [error, setError] = useState('');

  const token = localStorage.getItem('token');
  const patientId = 1;  // Assuming the patient ID is 1 for now (replace with real data)

  useEffect(() => {
    // Fetch prescriptions for the logged-in patient
    const fetchPrescriptions = async () => {
      const response = await fetch(`http://localhost:5000/api/prescription/${patientId}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
        }
      });

      if (response.ok) {
        const data = await response.json();
        setPrescriptions(data);
      } else {
        setError('Unable to fetch prescriptions');
      }
    };

    fetchPrescriptions();
  }, [patientId]);

  return (
    <div>
      <h3>My Prescriptions</h3>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      <ul>
        {prescriptions.map((prescription) => (
          <li key={prescription.PrescriptionID}>
            <h5>{prescription.Medication}</h5>
            <p>Dosage: {prescription.Dosage}</p>
            <p>Instructions: {prescription.Instructions}</p>
            <p>Prescribed by: Doctor {prescription.DoctorID}</p>
            <p>Date: {new Date(prescription.Date).toLocaleDateString()}</p>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ViewPrescriptions;
