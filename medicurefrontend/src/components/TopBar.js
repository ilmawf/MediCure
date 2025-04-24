// components/TopBar.js
import React from 'react';

const TopBar = ({ adminName, onLogout }) => {
  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-primary">
      <div className="container-fluid">
        <a className="navbar-brand text-white" href="#">CityCare Admin Dashboard</a>
        <div className="d-flex">
          <span className="text-white me-3">Welcome, {adminName}</span>
          <button className="btn btn-danger" onClick={onLogout}>Logout</button>
        </div>
      </div>
    </nav>
  );
};

export default TopBar;
