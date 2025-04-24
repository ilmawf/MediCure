
import React, { useState, useEffect } from 'react';

const BookAppointment = () => {
  const [availableSlots, setAvailableSlots] = useState([]);
  const [selectedSlot, setSelectedSlot] = useState(null);
  const [patientName, setPatientName] = useState('');

  const token = localStorage.getItem('token');
  const doctorId = 1;  // Assuming the doctor ID is 1 for now

  useEffect(() => {
    // Fetch available slots for the selected doctor
    const fetchAvailableSlots = async () => {
      const response = await fetch(`http://localhost:5000/api/DoctorAvailability/${doctorId}`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (response.ok) {
        const data = await response.json();
        setAvailableSlots(data);
      }
    };

    fetchAvailableSlots();
  }, [doctorId]);

  const handleBooking = async () => {
    if (!selectedSlot || !patientName) {
      alert("Please select a slot and provide your name.");
      return;
    }

    const response = await fetch(`http://localhost:5000/api/DoctorAvailability/${selectedSlot.id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
    });

    if (response.ok) {
      alert("Appointment booked successfully!");
    } else {
      alert("Failed to book appointment.");
    }
  };

  return (
    <div>
      <h3>Book an Appointment</h3>
      <div>
        <label>Your Name: </label>
        <input 
          type="text" 
          value={patientName} 
          onChange={(e) => setPatientName(e.target.value)} 
          required 
        />
      </div>

      <div>
        <h4>Available Slots</h4>
        <ul>
          {availableSlots.map((slot) => (
            <li key={slot.DoctorAvailabilityID}>
              {new Date(slot.StartTime).toLocaleString()} - 
              {new Date(slot.EndTime).toLocaleString()}
              <button onClick={() => setSelectedSlot(slot)}>Select</button>
            </li>
          ))}
        </ul>
      </div>

      <button onClick={handleBooking}>Book Appointment</button>
    </div>
  );
};

export default BookAppointment;
