
import React, { useState, useEffect } from 'react';

const ViewBills = () => {
  const [bills, setBills] = useState([]);
  const [error, setError] = useState('');

  const token = localStorage.getItem('token');
  const patientId = 1;  // Use actual patient ID here

  useEffect(() => {
    const fetchBills = async () => {
      const response = await fetch(`http://localhost:5000/api/billing/${patientId}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
        }
      });

      if (response.ok) {
        const data = await response.json();
        setBills(data);
      } else {
        setError('Unable to fetch billing records');
      }
    };

    fetchBills();
  }, [patientId]);

  return (
    <div>
      <h3>My Bills</h3>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      <ul>
        {bills.map((bill) => (
          <li key={bill.BillingID}>
            <p>Amount: ${bill.Amount}</p>
            <p>Status: {bill.Status}</p>
            <p>Billing Date: {new Date(bill.BillingDate).toLocaleDateString()}</p>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ViewBills;
