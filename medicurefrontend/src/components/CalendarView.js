import React, { useState, useEffect } from 'react';
import { Calendar, momentLocalizer } from 'react-big-calendar';
import moment from 'moment';

const localizer = momentLocalizer(moment);

const CalendarView = () => {
  const [appointments, setAppointments] = useState([]);
  const token = localStorage.getItem('token');

  useEffect(() => {
    // Fetch appointments from the backend and populate the calendar
    fetchAppointments();
  }, []);

  const fetchAppointments = async () => {
    try {
      const response = await fetch('http://localhost:5000/api/appointments', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (response.ok) {
        const data = await response.json();
        setAppointments(
          data.map((appointment) => ({
            title: `${appointment.patientName} - ${appointment.status}`,
            start: new Date(appointment.appointmentDate),
            end: new Date(appointment.appointmentDate),
            allDay: false,
          }))
        );
      } else {
        throw new Error('Unable to fetch appointments');
      }
    } catch (err) {
      console.error(err.message);
    }
  };

  return (
    <div>
      <h2>Appointments Calendar</h2>
      <Calendar
        localizer={localizer}
        events={appointments}
        startAccessor="start"
        endAccessor="end"
        style={{ height: 500 }}
      />
    </div>
  );
};

export default CalendarView;
