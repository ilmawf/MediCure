import React, { useEffect, useState } from 'react';
import axios from 'axios';

const PatientList = () => {
  const [patients, setPatients] = useState([]);

  // Fetching data from the backend API using Axios
  useEffect(() => {
    axios.get('http://localhost:5000/api/patient')  // URL of your API
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
      <ul>
        {patients.length > 0 ? (
          patients.map((patient) => (
            <li key={patient.patientID}>{patient.name}</li>
          ))
        ) : (
          <p>No patients found.</p>
        )}
      </ul>
    </div>
  );
};

export default PatientList;
