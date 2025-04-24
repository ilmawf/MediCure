import React, { useState, useEffect } from 'react';

const ManagePatients = () => {
  const [patients, setPatients] = useState([]);
  const [newPatient, setNewPatient] = useState({ name: '', email: '', doctorId: '', ward: '' });
  const [isAdmin, setIsAdmin] = useState(false);  // Check if the user is an admin
  const [error, setError] = useState('');
  const [searchTerm, setSearchTerm] = useState("");  // For search functionality
  const [doctorId, setDoctorId] = useState("");  // For doctor filtering
  const [doctors, setDoctors] = useState([]);  // List of doctors for dropdown

  const token = localStorage.getItem('token');  // JWT Token for authorization
  const role = localStorage.getItem('role');  // User role (admin, patient)

  useEffect(() => {
    // Check if the user is an admin
    if (role === 'admin') {
      setIsAdmin(true);
    }

    // Fetch patients, doctors, and handle search/filtering
    fetchPatients();
    fetchDoctors();
  }, [role, searchTerm, doctorId]);  // Re-fetch when searchTerm or doctorId changes

  // Fetch patients from the backend (Admin only)
  const fetchPatients = async () => {
    try {
      let url = `http://localhost:5000/api/patients?search=${searchTerm}`;
      if (doctorId) {
        url += `&doctorId=${doctorId}`;
      }

      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (response.ok) {
        const data = await response.json();
        setPatients(data);
      } else {
        throw new Error('Unable to fetch patients');
      }
    } catch (err) {
      setError(err.message);
    }
  };

  // Fetch doctors list for assigning to patients
  const fetchDoctors = async () => {
    try {
      const response = await fetch('http://localhost:5000/api/doctors', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (response.ok) {
        const data = await response.json();
        setDoctors(data);
      } else {
        throw new Error('Unable to fetch doctors');
      }
    } catch (err) {
      setError(err.message);
    }
  };

  // Add a new patient (Admin only)
  const handleAddPatient = async () => {
    try {
      const response = await fetch('http://localhost:5000/api/patients', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(newPatient),
      });

      if (response.ok) {
        const data = await response.json();
        setPatients((prevPatients) => [...prevPatients, data]);
        setNewPatient({ name: '', email: '', doctorId: '', ward: '' });  // Clear the form
      } else {
        throw new Error('Failed to add patient');
      }
    } catch (err) {
      setError(err.message);
    }
  };

  // Delete a patient (Admin only)
  const handleDeletePatient = async (id) => {
    try {
      const response = await fetch(`http://localhost:5000/api/patients/${id}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (response.ok) {
        setPatients((prevPatients) => prevPatients.filter((patient) => patient.patientID !== id));
      } else {
        throw new Error('Failed to delete patient');
      }
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div>
      <h2>Manage Patients</h2>
      {error && <p style={{ color: 'red' }}>{error}</p>}

      {/* Search and Filter */}
      <div>
        <input
          type="text"
          placeholder="Search by name or email"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}  // Update search term
        />
        <select onChange={(e) => setDoctorId(e.target.value)} value={doctorId}>
          <option value="">All Doctors</option>
          {doctors.map((doctor) => (
            <option key={doctor.DoctorID} value={doctor.DoctorID}>
              {doctor.Name}
            </option>
          ))}
        </select>
      </div>

      {/* Add new patient form (only visible to Admins) */}
      {isAdmin && (
        <div>
          <h3>Add New Patient</h3>
          <input
            type="text"
            placeholder="Name"
            value={newPatient.name}
            onChange={(e) => setNewPatient({ ...newPatient, name: e.target.value })}
          />
          <input
            type="email"
            placeholder="Email"
            value={newPatient.email}
            onChange={(e) => setNewPatient({ ...newPatient, email: e.target.value })}
          />
          <select
            value={newPatient.doctorId}
            onChange={(e) => setNewPatient({ ...newPatient, doctorId: e.target.value })}
          >
            <option value="">Select Doctor</option>
            {doctors.map((doctor) => (
              <option key={doctor.DoctorID} value={doctor.DoctorID}>
                {doctor.Name}
              </option>
            ))}
          </select>
          <input
            type="text"
            placeholder="Ward"
            value={newPatient.ward}
            onChange={(e) => setNewPatient({ ...newPatient, ward: e.target.value })}
          />
          <button onClick={handleAddPatient}>Add Patient</button>
        </div>
      )}

      {/* Display patients */}
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Email</th>
            <th>Assigned Doctor</th>
            <th>Ward</th>
            {isAdmin && <th>Actions</th>} {/* Admins can perform actions */}
          </tr>
        </thead>
        <tbody>
          {patients.map((patient) => (
            <tr key={patient.patientID}>
              <td>{patient.patientID}</td>
              <td>{patient.name}</td>
              <td>{patient.email}</td>
              <td>{patient.doctorId}</td>
              <td>{patient.ward}</td>
              {isAdmin && (
                <td>
                  <button onClick={() => handleDeletePatient(patient.patientID)}>Delete</button>
                </td>
              )}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ManagePatients;
