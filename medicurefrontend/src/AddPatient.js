// AddPatient.js
import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const AddPatient = () => {
  const [name, setName] = useState('');
  const [age, setAge] = useState('');
  const [contact, setContact] = useState('');
  const navigate = useNavigate(); // For navigation after patient is added

  const handleSubmit = async (e) => {
    e.preventDefault();

    const newPatient = { name, age, contact };

    try {
      // Sending POST request to add a new patient
      const response = await axios.post('http://localhost:5249/api/patients', newPatient);
      console.log('Patient added successfully', response.data);
      
      // Redirect to the patient list after adding
      navigate('/');
    } catch (err) {
      console.error('Error adding patient:', err);
    }
  };

  return (
    <div>
      <h1>Add New Patient</h1>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />
        <input
          type="number"
          placeholder="Age"
          value={age}
          onChange={(e) => setAge(e.target.value)}
          required
        />
        <input
          type="text"
          placeholder="Contact"
          value={contact}
          onChange={(e) => setContact(e.target.value)}
          required
        />
        <button type="submit">Add Patient</button>
      </form>
    </div>
  );
};

export default AddPatient;
