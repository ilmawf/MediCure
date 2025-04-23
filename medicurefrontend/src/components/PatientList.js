// PatientList.js
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';

const PatientList = () => {
  const [patients, setPatients] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [newPatient, setNewPatient] = useState({
    name: '',
    age: '',
    contact: '',
  });

  // Fetching data from the backend API using Axios
  useEffect(() => {
    const fetchPatients = async () => {
      try {
        const response = await axios.get('http://localhost:5249/api/patients');
        setPatients(response.data);
      } catch (err) {
        setError('Error fetching patients');
      } finally {
        setLoading(false);
      }
    };

    fetchPatients();
  }, []);

  // Handle input change for adding new patient
  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setNewPatient((prev) => ({ ...prev, [name]: value }));
  };

  // Handle form submission to add new patient
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post('http://localhost:5249/api/patients', newPatient);
      setPatients((prev) => [...prev, response.data]); // Update patients list with new patient
      setNewPatient({ name: '', age: '', contact: '' }); // Reset form fields
    } catch (err) {
      console.error('Error adding patient:', err);
    }
  };

  // Handle delete patient
  const handleDelete = async (patientID) => {
    try {
      await axios.delete(`http://localhost:5249/api/patients/${patientID}`);
      setPatients((prev) => prev.filter((patient) => patient.patientID !== patientID));
    } catch (err) {
      console.error('Error deleting patient:', err);
    }
  };

  return (
    <div>
      <h2>Patient List</h2>

      {/* Show error message if there's an error */}
      {error && <p style={{ color: 'red' }}>{error}</p>}

      {/* Loading state */}
      {loading ? (
        <p>Loading...</p>
      ) : (
        <>
          {/* Add Patient Form */}
          <form onSubmit={handleSubmit}>
            <h3>Add New Patient</h3>
            <input
              type="text"
              name="name"
              placeholder="Patient Name"
              value={newPatient.name}
              onChange={handleInputChange}
              required
            />
            <input
              type="number"
              name="age"
              placeholder="Patient Age"
              value={newPatient.age}
              onChange={handleInputChange}
              required
            />
            <input
              type="text"
              name="contact"
              placeholder="Patient Contact"
              value={newPatient.contact}
              onChange={handleInputChange}
              required
            />
            <button type="submit">Add Patient</button>
          </form>

          {/* Patient List */}
          {patients.length === 0 ? (
            <p>No patients found.</p>
          ) : (
            <ul>
              {patients.map((patient) => (
                <li key={patient.patientID}>
                  {patient.name} - {patient.contact}
                  {/* View Button */}
                  <Link to={`/view-patient/${patient.patientID}`}>
                    <button>View</button>
                  </Link>
                  {/* Edit Button */}
                  <Link to={`/edit-patient/${patient.patientID}`}>
                    <button>Edit</button>
                  </Link>
                  {/* Delete Button */}
                  <button onClick={() => handleDelete(patient.patientID)}>Delete</button>
                </li>
              ))}
            </ul>
          )}
        </>
      )}
    </div>
  );
};

export default PatientList;
