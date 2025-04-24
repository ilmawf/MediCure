
import React, { useEffect, useState } from 'react';
import { Line } from 'react-chartjs-2';  // Line chart from react-chartjs-2
import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend } from 'chart.js';

// Register chart.js components
ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

const AppointmentChart = () => {
  const [chartData, setChartData] = useState({});

  useEffect(() => {
    // Fetch data for appointments over time (weekly/monthly)
    const fetchChartData = async () => {
      const response = await fetch('http://localhost:5000/api/admin/appointments-stats', {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      });

      if (response.ok) {
        const data = await response.json();

        // Prepare the data for Chart.js
        setChartData({
          labels: data.dates,  // Dates (e.g., last 7 days)
          datasets: [
            {
              label: 'Appointments',
              data: data.appointments,  // Number of appointments
              borderColor: '#0b8fac',
              backgroundColor: 'rgba(11, 143, 172, 0.2)',
              tension: 0.4,
            },
          ],
        });
      }
    };

    fetchChartData();
  }, []);

  return (
    <div>
      <h3>Appointments Over Time</h3>
      <Line data={chartData} options={{ responsive: true, maintainAspectRatio: false }} />
    </div>
  );
};

export default AppointmentChart;
