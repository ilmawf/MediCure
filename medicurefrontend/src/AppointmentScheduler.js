import React, { useState, useEffect } from 'react';
import axios from 'axios';

const AppointmentScheduler = () => {
  const [patients, setPatients] = useState([]);
  const [doctors, setDoctors] = useState([]);
  const [appointment, setAppointment] = useState({
    patientID: '',
    doctorID: '',
    date: '',
    time: '',
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Fetch patients and doctors data from the backend API
  useEffect(() => {
    const fetchData = async () => {
      try {
        const patientsResponse = await axios.get('http://localhost:5249/api/patients');
        const doctorsResponse = await axios.get('http://localhost:5249/api/doctors');
        setPatients(patientsResponse.data);
        setDoctors(doctorsResponse.data);
      } catch (err) {
        setError('Error fetching data');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  // Handle input change for appointment form
  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setAppointment((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  // Handle form submission to schedule appointment
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.post('http://localhost:5249/api/appointments', appointment);
      alert('Appointment scheduled successfully!');
      setAppointment({ patientID: '', doctorID: '', date: '', time: '' });
    } catch (err) {
      console.error('Error scheduling appointment', err);
    }
  };

  return (
    <div>
      <h2>Schedule Appointment</h2>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      {loading ? (
        <p>Loading...</p>
      ) : (
        <form onSubmit={handleSubmit}>
          {/* Select Patient */}
          <select
            name="patientID"
            value={appointment.patientID}
            onChange={handleInputChange}
            required
          >
            <option value="">Select Patient</option>
            {patients.map((patient) => (
              <option key={patient.patientID} value={patient.patientID}>
                {patient.name}
              </option>
            ))}
          </select>

          {/* Select Doctor */}
          <select
            name="doctorID"
            value={appointment.doctorID}
            onChange={handleInputChange}
            required
          >
            <option value="">Select Doctor</option>
            {doctors.map((doctor) => (
              <option key={doctor.doctorID} value={doctor.doctorID}>
                {doctor.name} - {doctor.specialty}
              </option>
            ))}
          </select>

          {/* Select Date */}
          <input
            type="date"
            name="date"
            value={appointment.date}
            onChange={handleInputChange}
            required
          />

          {/* Select Time */}
          <input
            type="time"
            name="time"
            value={appointment.time}
            onChange={handleInputChange}
            required
          />

          <button type="submit">Schedule Appointment</button>
        </form>
      )}
    </div>
  );
};

export default AppointmentScheduler;
