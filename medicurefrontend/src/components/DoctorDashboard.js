import React, { useState, useEffect } from 'react';

const DoctorDashboard = () => {
  const [appointments, setAppointments] = useState([]);

  useEffect(() => {
    const token = localStorage.getItem('token');
    const fetchAppointments = async () => {
      const response = await fetch('http://localhost:5000/api/doctor/appointments', {
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });
      const data = await response.json();
      setAppointments(data);
    };

    fetchAppointments();
  }, []);

  return (
    <div>
      <h2>Doctor Dashboard</h2>
      <h3>Upcoming Appointments</h3>
      <ul>
        {appointments.map((appointment) => (
          <li key={appointment.appointmentID}>
            {appointment.patientName} - {appointment.appointmentDate}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default DoctorDashboard;
