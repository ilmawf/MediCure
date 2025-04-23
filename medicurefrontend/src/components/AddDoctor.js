// AddDoctor.js
import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const AddDoctor = () => {
  const [name, setName] = useState('');
  const [specialty, setSpecialty] = useState('');
  const [contact, setContact] = useState('');
  const navigate = useNavigate(); // For navigation after adding a doctor

  const handleSubmit = async (e) => {
    e.preventDefault();

    const newDoctor = { name, specialty, contact };

    try {
      await axios.post('http://localhost:5249/api/doctors', newDoctor);
      navigate('/doctors'); // Redirect to doctor list after adding
    } catch (err) {
      console.error('Error adding doctor:', err);
    }
  };

  return (
    <div>
      <h1>Add New Doctor</h1>
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
        <button type="submit">Add Doctor</button>
      </form>
    </div>
  );
};

export default AddDoctor;
