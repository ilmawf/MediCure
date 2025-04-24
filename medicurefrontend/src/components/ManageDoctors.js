import React, { useState, useEffect } from 'react';

const ManageDoctors = () => {
  const [doctors, setDoctors] = useState([]);
  const [newDoctor, setNewDoctor] = useState({ name: '', specialty: '' });
  const [error, setError] = useState('');

  const token = localStorage.getItem('token');  // JWT Token for authorization

  useEffect(() => {
    fetchDoctors();
  }, []);

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

  const handleAddDoctor = async () => {
    try {
      const response = await fetch('http://localhost:5000/api/doctors', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(newDoctor),
      });

      if (response.ok) {
        const data = await response.json();
        setDoctors((prevDoctors) => [...prevDoctors, data]);
        setNewDoctor({ name: '', specialty: '' });
      } else {
        throw new Error('Failed to add doctor');
      }
    } catch (err) {
      setError(err.message);
    }
  };

  const handleDeleteDoctor = async (id) => {
    try {
      const response = await fetch(`http://localhost:5000/api/doctors/${id}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (response.ok) {
        setDoctors((prevDoctors) => prevDoctors.filter((doctor) => doctor.id !== id));
      } else {
        throw new Error('Failed to delete doctor');
      }
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div>
      <h2>Manage Doctors</h2>
      {error && <p style={{ color: 'red' }}>{error}</p>}

      {/* Add new doctor form */}
      <div>
        <h3>Add New Doctor</h3>
        <input
          type="text"
          placeholder="Doctor Name"
          value={newDoctor.name}
          onChange={(e) => setNewDoctor({ ...newDoctor, name: e.target.value })}
        />
        <input
          type="text"
          placeholder="Specialty"
          value={newDoctor.specialty}
          onChange={(e) => setNewDoctor({ ...newDoctor, specialty: e.target.value })}
        />
        <button onClick={handleAddDoctor}>Add Doctor</button>
      </div>

      {/* Display list of doctors */}
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Specialty</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {doctors.map((doctor) => (
            <tr key={doctor.id}>
              <td>{doctor.id}</td>
              <td>{doctor.name}</td>
              <td>{doctor.specialty}</td>
              <td>
                <button onClick={() => handleDeleteDoctor(doctor.id)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ManageDoctors;
