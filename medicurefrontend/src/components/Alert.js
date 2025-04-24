import React, { useState, useEffect } from 'react';

const Alert = () => {
  const [alerts, setAlerts] = useState([]);
  const [error, setError] = useState('');

  const token = localStorage.getItem('token'); // JWT Token for authorization

  useEffect(() => {
    const fetchAlerts = async () => {
      const response = await fetch('http://localhost:5000/api/alerts', {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (response.ok) {
        const data = await response.json();
        setAlerts(data);
      } else {
        setError('Unable to fetch alerts');
      }
    };

    fetchAlerts();
  }, [token]);

  const markAsRead = async (alertId) => {
    const response = await fetch(`http://localhost:5000/api/alerts/${alertId}`, {
      method: 'PUT',
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });

    if (response.ok) {
      setAlerts(alerts.map(alert => 
        alert.AlertID === alertId ? { ...alert, IsRead: true } : alert
      ));
    }
  };

  return (
    <div>
      <h3>System Alerts</h3>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      {alerts.length === 0 ? (
        <p>No alerts</p>
      ) : (
        <div className="alert-list">
          {alerts.map((alert) => (
            <div key={alert.AlertID} className={`alert alert-${alert.IsRead ? 'secondary' : 'danger'}`}>
              <strong>{alert.Title}</strong>: {alert.Message}
              {!alert.IsRead && (
                <button onClick={() => markAsRead(alert.AlertID)} className="btn btn-sm btn-primary">
                  Mark as Read
                </button>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Alert;
