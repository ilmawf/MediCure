import React from 'react';
import { Navigate } from 'react-router-dom';

const PrivateRoute = ({ children, allowedRoles }) => {
  const token = localStorage.getItem('token');
  let userRole = null;

  if (token) {
    try {
      const decoded = JSON.parse(atob(token.split('.')[1]));
      userRole = decoded?.Role;
    } catch (error) {
      console.error('Invalid token', error);
    }
  }

  if (!token || !allowedRoles.includes(userRole)) {
    return <Navigate to="/login" replace />;
  }

  return children;
};

export default PrivateRoute;
