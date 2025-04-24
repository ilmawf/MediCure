// components/Sidebar.js
import React from 'react';
import { Link } from 'react-router-dom';

const Sidebar = () => {
  return (
    <div className="bg-light vh-100 p-3">
      <ul className="nav flex-column">
        <li className="nav-item">
          <Link to="/admin/dashboard" className="nav-link active">Dashboard</Link>
        </li>
        <li className="nav-item">
          <Link to="/admin/manage-patients" className="nav-link">Manage Patients</Link>
        </li>
        <li className="nav-item">
          <Link to="/admin/manage-appointments" className="nav-link">Manage Appointments</Link>
        </li>
        <li className="nav-item">
          <Link to="/admin/manage-doctors" className="nav-link">Manage Doctors</Link>
        </li>
      </ul>
    </div>
  );
};

export default Sidebar;
