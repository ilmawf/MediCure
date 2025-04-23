// DoctorList.js
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';

const DoctorList = () => {
  const [doctors, setDoctors] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Fetching data from the backend API using Axios
  useEffect(() => {
    const fetchDoctors = async () => {
      try {
        const response = await axios.get('http://localhost:5249/api/doctors');
        setDoctors(response.data);
      } catch (err) {
        setError('Error fetching doctors');
      } finally {
        setLoading(false);
      }
    };

    fetchDoctors();
  }, []);

  // Handle delete doctor
  const handleDelete = async (doctorID) => {
    try {
      await axios.delete(`http://localhost:5249/api/doctors/${doctorID}`);
      setDoctors((prev) => prev.filter((doctor) => doctor.doctorID !== doctorID));
    } catch (err) {
      console.error('Error deleting doctor:', err);
    }
  };

  return (
    <div>
      <h2>Doctor List</h2>

      {/* Show error message if there's an error */}
      {error && <p style={{ color: 'red' }}>{error}</p>}

      {/* Loading state */}
      {loading ? (
        <p>Loading...</p>
      ) : (
        <>
          <Link to="/add-doctor">
            <button>Add New Doctor</button>
          </Link>
          {/* Doctor List */}
          {doctors.length === 0 ? (
            <p>No doctors found.</p>
          ) : (
            <ul>
              {doctors.map((doctor) => (
                <li key={doctor.doctorID}>
                  {doctor.name} - {doctor.specialty}
                  <Link to={`/edit-doctor/${doctor.doctorID}`}>
                    <button>Edit</button>
                  </Link>
                  <button onClick={() => handleDelete(doctor.doctorID)}>Delete</button>
                </li>
              ))}
            </ul>
          )}
        </>
      )}
    </div>
  );
};

export default DoctorList;
