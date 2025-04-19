// PatientDetail.js
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams } from 'react-router-dom';

const PatientDetail = () => {
  const { patientID } = useParams();
  const [patient, setPatient] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchPatient = async () => {
      try {
        const response = await axios.get(`http://localhost:5249/api/patients/${patientID}`);
        setPatient(response.data);
      } catch (err) {
        setError('Error fetching patient details');
      } finally {
        setLoading(false);
      }
    };

    fetchPatient();
  }, [patientID]);

  if (loading) return <p>Loading...</p>;
  if (error) return <p style={{ color: 'red' }}>{error}</p>;

  return (
    <div>
      <h1>Patient Details</h1>
      {patient ? (
        <div>
          <p><strong>Name:</strong> {patient.name}</p>
          <p><strong>Age:</strong> {patient.age}</p>
          <p><strong>Contact:</strong> {patient.contact}</p>
          <p><strong>Medical History:</strong> {patient.medicalHistory}</p>
        </div>
      ) : (
        <p>Patient not found.</p>
      )}
    </div>
  );
};

export default PatientDetail;
