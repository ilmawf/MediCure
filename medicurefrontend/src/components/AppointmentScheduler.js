// AppointmentScheduler.js
import React, { useState, useEffect } from 'react';
import axios from 'axios';

const AppointmentScheduler = () => {
  const [doctors, setDoctors] = useState([]);
  const [selectedDoctor, setSelectedDoctor] = useState('');
  const [availableSlots, setAvailableSlots] = useState([]);
  const [appointmentDate, setAppointmentDate] = useState('');
  const [selectedSlot, setSelectedSlot] = useState('');
  const [error, setError] = useState(null);

  // Fetch doctors when the component mounts
  useEffect(() => {
    const fetchDoctors = async () => {
      try {
        const response = await axios.get('http://localhost:5249/api/doctors');  // API to fetch doctors
        setDoctors(response.data);
      } catch (err) {
        setError('Error fetching doctors');
        console.error('Error fetching doctors:', err);
      }
    };

    fetchDoctors();
  }, []);

  // Fetch available time slots when a doctor is selected
  useEffect(() => {
    if (selectedDoctor) {
      const fetchAvailableSlots = async () => {
        try {
          const response = await axios.get(`http://localhost:5249/api/appointments/available-slots/${selectedDoctor}`);
          setAvailableSlots(response.data);
        } catch (err) {
          setError('Error fetching available slots');
          console.error('Error fetching available slots:', err);
        }
      };

      fetchAvailableSlots();
    }
  }, [selectedDoctor]);

  // Handle form submission to schedule appointment
  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!selectedSlot || !appointmentDate) {
      setError('Please select a time slot and date');
      return;
    }

    const appointment = {
      doctorID: selectedDoctor,
      appointmentDate,
      slot: selectedSlot,
    };

    try {
      await axios.post('http://localhost:5249/api/appointments', appointment);  // API to schedule appointment
      alert('Appointment scheduled successfully!');
    } catch (err) {
      setError('Error scheduling appointment');
      console.error('Error scheduling appointment:', err);
    }
  };

  return (
    <div>
      <h2>Schedule Appointment</h2>

      {/* Error handling */}
      {error && <p style={{ color: 'red' }}>{error}</p>}

      {/* Appointment Scheduling Form */}
      <form onSubmit={handleSubmit}>
        <label>
          Select Doctor:
          <select
            value={selectedDoctor}
            onChange={(e) => setSelectedDoctor(e.target.value)}
            required
          >
            <option value="">Select a Doctor</option>
            {doctors.map((doctor) => (
              <option key={doctor.doctorID} value={doctor.doctorID}>
                {doctor.name} - {doctor.specialty}
              </option>
            ))}
          </select>
        </label>

        {selectedDoctor && (
          <>
            <label>
              Select Date:
              <input
                type="date"
                value={appointmentDate}
                onChange={(e) => setAppointmentDate(e.target.value)}
                required
              />
            </label>

            <label>
              Select Time Slot:
              <select
                value={selectedSlot}
                onChange={(e) => setSelectedSlot(e.target.value)}
                required
              >
                <option value="">Select a Time Slot</option>
                {availableSlots.length === 0 ? (
                  <option value="">No available slots</option>
                ) : (
                  availableSlots.map((slot, index) => (
                    <option key={index} value={slot}>
                      {slot}
                    </option>
                  ))
                )}
              </select>
            </label>
          </>
        )}

        <button type="submit">Schedule Appointment</button>
      </form>
    </div>
  );
};

export default AppointmentScheduler;
