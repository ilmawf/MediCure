import React, { useState, useEffect } from 'react';
import TopBar from './TopBar';
import Sidebar from './Sidebar';
import AppointmentChart from './AppointmentChart';
import RecentAppointments from './RecentAppointments';
import Alert from './Alert';

const Dashboard = () => {
  const [metrics, setMetrics] = useState({
    totalPatients: 0,
    appointmentsToday: 0,
    totalStaff: 0,
    labReportsCount: 0
  });
  const [adminName, setAdminName] = useState('');

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) {
      window.location.href = '/login';
    }

    const fetchMetrics = async () => {
      const response = await fetch('http://localhost:5000/api/admin/metrics', {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });

      if (response.ok) {
        const data = await response.json();
        setMetrics(data);
        const decodedToken = JSON.parse(atob(token.split('.')[1]));
        setAdminName(decodedToken.email);
      }
    };

    fetchMetrics();
  }, []);

  const handleLogout = () => {
    localStorage.removeItem('token');
    window.location.href = '/login';
  };

  // Function to download reports
  const handleDownloadReport = async (reportType) => {
    const response = await fetch(`http://localhost:5000/api/reports/${reportType}`, {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
      },
    });

    if (response.ok) {
      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `${reportType}.pdf`;
      link.click();
    } else {
      alert('Error downloading report');
    }
  };

  return (
    <div className="dashboard">
      <TopBar adminName={adminName} onLogout={handleLogout} />
      <div className="d-flex">
        <Sidebar />
        <div className="container p-4">
          <h2>Dashboard Home</h2>
          <div className="row">
            <div className="col-md-3">
              <div className="card">
                <div className="card-body text-center">
                  <h5 className="card-title">Total Patients</h5>
                  <p className="card-text">{metrics.totalPatients}</p>
                </div>
              </div>
            </div>
            <div className="col-md-3">
              <div className="card">
                <div className="card-body text-center">
                  <h5 className="card-title">Appointments Today</h5>
                  <p className="card-text">{metrics.appointmentsToday}</p>
                </div>
              </div>
            </div>
            <div className="col-md-3">
              <div className="card">
                <div className="card-body text-center">
                  <h5 className="card-title">Total Staff</h5>
                  <p className="card-text">{metrics.totalStaff}</p>
                </div>
              </div>
            </div>
            <div className="col-md-3">
              <div className="card">
                <div className="card-body text-center">
                  <h5 className="card-title">Lab Reports</h5>
                  <p className="card-text">{metrics.labReportsCount}</p>
                </div>
              </div>
            </div>
          </div>

          {/* Report Download Section */}
          <div className="row">
            <div className="col-md-12">
              <h3>Download Reports</h3>
              <div className="btn-group">
                <button className="btn btn-primary" onClick={() => handleDownloadReport('patient-report')}>Download Patient Report</button>
                <button className="btn btn-primary" onClick={() => handleDownloadReport('appointments-today')}>Download Today's Appointments Report</button>
                <button className="btn btn-primary" onClick={() => handleDownloadReport('appointments-week')}>Download This Week's Appointments Report</button>
                <button className="btn btn-primary" onClick={() => handleDownloadReport('billing-report')}>Download Billing Report</button>
                <button className="btn btn-primary" onClick={() => handleDownloadReport('patient-statistics')}>Download Patient Statistics Report</button>
              </div>
            </div>
          </div>

          {/* Other Components */}
          <AppointmentChart />
          <RecentAppointments />
          <Alert />
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
