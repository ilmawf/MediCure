// HomePage.js
import React from 'react';
import { Link } from 'react-router-dom';
import Chatbot from './Chatbot';
import CardsSection from './CardsSection';


const HomePage = () => {
  return (
    <div className="home-page">
     <div className="home-container">
      <div className="hero-section">
        {/* Hero Image */}
        <img src={require('../assets/hero-img.jpg')} alt="Healthcare" className="hero-image" />
        
        {/* Overlay Text */}
        <div className="hero-text">
          <h1>We’re here when you need us</h1>
          <p>Your health is our priority. Get the best care now.</p>
        </div>
      </div>

      {/* Cards Section */}
      <CardsSection />  {/* Display the cards section */}

      {/* Services Section - Image Overlay */}
      <section className="services-section">
        <div className="services-container">
          <div className="services-image">
            <img src={require('../assets/service-img.jpg')} alt="Healthcare" className="services-image-content" />
          </div>
          <div className="services-list">
            <h2>Our Services</h2>
            <ul>
              <li><Link to="/services/cancer">Cancer</Link></li>
              <li><Link to="/services/ear-nose-throat">Ear, Nose, and Throat</Link></li>
              <li><Link to="/services/geriatrics">Geriatrics</Link></li>
              <li><Link to="/services/heart">Heart - Cardiology and Cardiovascular Surgery</Link></li>
              <li><Link to="/services/neurology">Neurology</Link></li>
              <li><Link to="/services/neurosurgery">Neurosurgery</Link></li>
              <li><Link to="/services/orthopedics">Orthopedics</Link></li>
              <li><Link to="/services/spine">Spine</Link></li>
              <li><Link to="/services/surgery">Surgery</Link></li>
              <li><Link to="/services/urology">Urology</Link></li>
            </ul>
            <Link to="/services">
              <button className="view-all-btn">View All Our Services</button>
            </Link>
          </div>
        </div>
      </section>

      
        <Chatbot /> {/* Chatbot component */}
      

      <footer>
        <p>&copy; 2025 MediCure Health System. All rights reserved.</p>
      </footer>
    </div>
    </div>

    
    
  );
};

export default HomePage;
