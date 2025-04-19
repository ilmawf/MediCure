// EditDoctor.js
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';

const EditDoctor = () => {
  const { doctorID } = useParams();
  const [doctor, setDoctor] = useState({});
  const [name, setName] = useState('');
  const [specialty, setSpecialty] = useState('');
  const [contact, setContact] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchDoctor = async () => {
      try {
        const response = await axios.get(`http://localhost:5249/api/doctors/${doctorID}`);
        setDoctor(response.data);
        setName(response.data.name);
        setSpecialty(response.data.specialty);
        setContact(response.data.contact);
      } catch (err) {
        console.error('Error fetching doctor data:', err);
      }
    };

    fetchDoctor();
  }, [doctorID]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const updatedDoctor = { name, specialty, contact };

    try {
      await axios.put(`http://localhost:5249/api/doctors/${doctorID}`, updatedDoctor);
      navigate('/doctors'); // Redirect to doctor list after updating
    } catch (err) {
      console.error('Error updating doctor:', err);
    }
  };

  return (
    <div>
      <h1>Edit Doctor</h1>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Doctor Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />
        <input
          type="text"
          placeholder="Specialty"
          value={specialty}
          onChange={(e) => setSpecialty(e.target.value)}
          required
        />
        <input
          type="text"
          placeholder="Contact"
          value={contact}
          onChange={(e) => setContact(e.target.value)}
          required
        />
        <button type="submit">Update Doctor</button>
      </form>
    </div>
  );
};

export default EditDoctor;
