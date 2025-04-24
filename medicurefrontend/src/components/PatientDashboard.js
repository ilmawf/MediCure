import React, { useState, useEffect } from 'react';

const PatientDashboard = () => {
  const [appointments, setAppointments] = useState([]);
  const [personalInfo, setPersonalInfo] = useState({});
  const [prescriptionDetails, setPrescriptionDetails] = useState('');
  const [doctorId, setDoctorId] = useState('');
  const [email, setEmail] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [medicalHistory, setMedicalHistory] = useState('');

  // Fetch appointments and personal info when the component mounts
  useEffect(() => {
    const token = localStorage.getItem('token');

    // Fetch appointments for the patient
    const fetchAppointments = async () => {
      const response = await fetch('http://localhost:5000/api/patient/appointments', {
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });
      const data = await response.json();
      setAppointments(data);
    };

    // Fetch personal info for the patient
    const fetchPersonalInfo = async () => {
      const response = await fetch('http://localhost:5000/api/patient/profile', {
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });
      const data = await response.json();
      setPersonalInfo(data);
      setEmail(data.email);
      setPhoneNumber(data.phoneNumber);
      setMedicalHistory(data.medicalHistory);
    };

    fetchAppointments();
    fetchPersonalInfo();
  }, []);

  // Handle prescription request form submission
  const handlePrescriptionRequest = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem('token');

    const response = await fetch('http://localhost:5000/api/patient/request-prescription', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`,
      },
      body: JSON.stringify({
        doctorId,
        prescriptionDetails,
      }),
    });

    const data = await response.json();
    if (response.ok) {
      alert('Prescription request sent successfully!');
    } else {
      alert(data.Message);
    }
  };

  // Handle personal info update form submission
  const handleUpdateInfo = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem('token');

    const response = await fetch('http://localhost:5000/api/patient/update-info', {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`,
      },
      body: JSON.stringify({
        email,
        phoneNumber,
        medicalHistory,
      }),
    });

    const data = await response.json();
    if (response.ok) {
      alert('Personal information updated successfully!');
    } else {
      alert(data.Message);
    }
  };

  return (
    <div>
      <h2>Patient Dashboard</h2>

      {/* Personal Information Section */}
      <h3>Personal Information</h3>
      <form onSubmit={handleUpdateInfo}>
        <label>
          Name: {personalInfo.name}
        </label>
        <br />
        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <br />
        <input
          type="text"
          placeholder="Phone Number"
          value={phoneNumber}
          onChange={(e) => setPhoneNumber(e.target.value)}
        />
        <br />
        <textarea
          placeholder="Medical History"
          value={medicalHistory}
          onChange={(e) => setMedicalHistory(e.target.value)}
        />
        <br />
        <button type="submit">Update Info</button>
      </form>

      {/* Prescription Request Section */}
      <h3>Request Prescription</h3>
      <form onSubmit={handlePrescriptionRequest}>
        <input
          type="text"
          placeholder="Doctor ID"
          value={doctorId}
          onChange={(e) => setDoctorId(e.target.value)}
        />
        <br />
        <textarea
          placeholder="Prescription Details"
          value={prescriptionDetails}
          onChange={(e) => setPrescriptionDetails(e.target.value)}
        />
        <br />
        <button type="submit">Request Prescription</button>
      </form>

      {/* Appointments Section */}
      <h3>Upcoming Appointments</h3>
      <ul>
        {appointments.map((appointment) => (
          <li key={appointment.appointmentID}>
            {appointment.doctorName} - {appointment.appointmentDate}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default PatientDashboard;
