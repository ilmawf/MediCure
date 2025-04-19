// EditPatient.js
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';

const EditPatient = () => {
  const [patient, setPatient] = useState({});
  const [name, setName] = useState('');
  const [age, setAge] = useState('');
  const [contact, setContact] = useState('');
  const { patientID } = useParams();  // Get the patient ID from URL
  const navigate = useNavigate();  // For navigation after update

  useEffect(() => {
    axios.get(`http://localhost:5249/api/patients/${patientID}`)
      .then((response) => {
        setPatient(response.data);
        setName(response.data.name);
        setAge(response.data.age);
        setContact(response.data.contact);
      })
      .catch((error) => {
        console.error('There was an error fetching the patient data!', error);
      });
  }, [patientID]);

  const handleSubmit = (e) => {
    e.preventDefault();

    const updatedPatient = { name, age, contact };

    axios.put(`http://localhost:5249/api/patients/${patientID}`, updatedPatient)
      .then((response) => {
        console.log('Patient updated successfully', response.data);
        navigate('/'); // Redirect to patient list
      })
      .catch((error) => {
        console.error('There was an error updating the patient!', error);
      });
  };

  return (
    <div>
      <h1>Edit Patient</h1>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
        <input
          type="number"
          placeholder="Age"
          value={age}
          onChange={(e) => setAge(e.target.value)}
        />
        <input
          type="text"
          placeholder="Contact"
          value={contact}
          onChange={(e) => setContact(e.target.value)}
        />
        <button type="submit">Update Patient</button>
      </form>
    </div>
  );
};

export default EditPatient;
