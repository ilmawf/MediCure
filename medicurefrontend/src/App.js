import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import './styles/styles.css';
import HomePage from './components/HomePage';
import LoginPage from './components/LoginPage';  // Import the LoginPage component
import AdminDashboard from './components/AdminDashboard';  // Import the Admin Dashboard component
import PrivateRoute from './components/PrivateRoute';  // Import the PrivateRoute component
import 'bootstrap/dist/css/bootstrap.min.css';

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
            <button className="login-btn">
              <Link to="/login">Login</Link>  {/* Link to login page */}
            </button>
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

        {/* Routes for different pages */}
        <Routes>
          {/* Public Routes */}
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />  {/* Login route */}

          {/* Protected Routes */}
          <PrivateRoute
            path="/admin-dashboard"
            component={AdminDashboard}  // Admin Dashboard route
            allowedRoles={['Admin']}  // Only Admin can access
          />
          
          {/* You can create similar routes for Doctor and Patient dashboards */}
        </Routes>
      </div>
    </Router>
  );
}

export default App;
