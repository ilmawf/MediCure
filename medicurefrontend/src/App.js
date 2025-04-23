// App.js
import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import './styles/styles.css';  // Import global CSS
import HomePage from './components/HomePage'; 

function App() {
  return (
    <Router>
      <div className="App">
        {/* Top Bar Section */}
        <div className="top-bar">
          <div className="top-left">
            <p>Need Help? Call Us: 123-456-789</p>
          </div>

          <div className="top-right">
            <button className="login-btn">Login</button>
          </div>
        </div>

        {/* Main Header Section */}
        <header className="header">
          {/* Logo Section */}
          <div className="logo">
            <img src={require('./assets/logo.png')} alt="Hospital Logo" />
          </div>

          {/* Navigation Links Section */}
          <nav className="nav-links">
            <ul>
              <li><Link to="/find-a-doctor">Find a Doctor</Link></li>
              <li><Link to="/locations">Locations & Directions</Link></li>
              <li><Link to="/departments">Departments</Link></li>
              <li><Link to="/patients">Patients & Visitors</Link></li>
              <li><Link to="/health-blog">Health Blog</Link></li>
            </ul>
          </nav>
        </header>

        <Routes>
           <Route path="/" element={<HomePage />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
