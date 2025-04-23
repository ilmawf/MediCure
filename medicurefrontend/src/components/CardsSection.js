// components/CardsSection.js
import React from 'react';
import { Link } from 'react-router-dom';

const CardsSection = () => {
  return (
    <div className="cards-section">
      <div className="card">
        <div className="card-icon">
          <img src={require('../assets/doctor-icon.jpg')} alt="Doctor Icon" />
        </div>
        <div className="card-content">
          <h3>Our Doctors</h3>
          <p>Search by name, specialty, location, and more.</p>
          <Link to="/doctors">
            <button className="card-btn">Find a Doctor</button>
          </Link>
        </div>
      </div>

      <div className="card">
        <div className="card-icon">
          <img src={require('../assets/location-icon.jpg')} alt="Location Icon" />
        </div>
        <div className="card-content">
          <h3>Locations & Directions</h3>
          <p>Find any of our 300+ locations.</p>
          <Link to="/locations">
            <button className="card-btn">Get Directions</button>
          </Link>
        </div>
      </div>

      <div className="card">
        <div className="card-icon">
          <img src={require('../assets/calender-icon.jpg')} alt="Appointments Icon" />
        </div>
        <div className="card-content">
          <h3>Appointments</h3>
          <p>Get the in-person or virtual care you need.</p>
          <Link to="/appointments">
            <button className="card-btn">Schedule Now</button>
          </Link>
        </div>
      </div>
    </div>
  );
};

export default CardsSection;
