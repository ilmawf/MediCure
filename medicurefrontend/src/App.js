// App.js
import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import PatientList from './PatientList';
import DoctorList from './DoctorList';
import AppointmentScheduler from './AppointmentScheduler';
import AddDoctor from './AddDoctor';
import EditDoctor from './EditDoctor';

function App() {
  return (
    <Router>
      <div className="App">
        {/* Header and Navigation Links */}
        <header>
          <h1>Hospital Management System</h1>
          <nav>
            <ul>
              <li>
                <Link to="/">Patients</Link>
              </li>
              <li>
                <Link to="/doctors">Doctors</Link>
              </li>
              <li>
                <Link to="/appointments">Appointments</Link>
              </li>
            </ul>
          </nav>
        </header>

        {/* Routes */}
        <Routes>
          <Route path="/" element={<PatientList />} />
          <Route path="/doctors" element={<DoctorList />} />
          <Route path="/appointments" element={<AppointmentScheduler />} />
          <Route path="/add-doctor" element={<AddDoctor />} />
          <Route path="/edit-doctor/:doctorID" element={<EditDoctor />} />

          {/* 404 Page for undefined routes */}
          <Route
            path="*"
            element={
              <div>
                <h2>404 - Page Not Found</h2>
              </div>
            }
          />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
