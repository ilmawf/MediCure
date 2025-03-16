import React, { useEffect, useState } from 'react';
import axios from 'axios';

const PatientList = () => {
  const [patients, setPatients] = useState([]);

  // Fetching data from the backend API using Axios
  useEffect(() => {
    axios.get('http://localhost:5249/api/patient')  // URL of your API
      .then((response) => {
        setPatients(response.data);
      })
      .catch((error) => {
        console.error("There was an error fetching the patient data!", error);
      });
  }, []);

  return (
    <div>
      <h1>Patient List</h1>
      {patients.length === 0 ? (
        <p>No patients found.</p>
      ) : (
        <ul>
          {patients.map(patient => (
            <li key={patient.patientID}>
              {patient.name} - {patient.contact}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default PatientList;

