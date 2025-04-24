import React, { useState, useEffect } from 'react';

const ManageStaff = () => {
  const [staff, setStaff] = useState([]);
  const [newStaff, setNewStaff] = useState({ name: '', role: '', department: '', dutyTime: '' });
  const [error, setError] = useState('');
  const [isEditing, setIsEditing] = useState(false);
  const [currentStaffId, setCurrentStaffId] = useState(null);  // Track the ID of the staff member being edited

  const token = localStorage.getItem('token');  // JWT Token for authorization

  useEffect(() => {
    fetchStaff();
  }, []); // Fetch staff list on component load

  // Fetch staff from the backend
  const fetchStaff = async () => {
    try {
      const response = await fetch('http://localhost:5000/api/staff', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (response.ok) {
        const data = await response.json();
        setStaff(data);
      } else {
        throw new Error('Unable to fetch staff');
      }
    } catch (err) {
      setError(err.message);
    }
  };

  // Add new staff
  const handleAddStaff = async () => {
    try {
      const response = await fetch('http://localhost:5000/api/staff', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(newStaff),
      });

      if (response.ok) {
        const data = await response.json();
        setStaff((prevStaff) => [...prevStaff, data]);
        setNewStaff({ name: '', role: '', department: '', dutyTime: '' });  // Clear the form
      } else {
        throw new Error('Failed to add staff');
      }
    } catch (err) {
      setError(err.message);
    }
  };

  // Edit existing staff member
  const handleEditStaff = async () => {
    try {
      const response = await fetch(`http://localhost:5000/api/staff/${currentStaffId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(newStaff),
      });

      if (response.ok) {
        const data = await response.json();
        setStaff((prevStaff) =>
          prevStaff.map((staffMember) =>
            staffMember.StaffID === data.StaffID ? data : staffMember
          )
        );
        setNewStaff({ name: '', role: '', department: '', dutyTime: '' });  // Clear the form
        setIsEditing(false);  // Stop editing mode
      } else {
        throw new Error('Failed to update staff');
      }
    } catch (err) {
      setError(err.message);
    }
  };

  // Delete a staff member
  const handleDeleteStaff = async (id) => {
    try {
      const response = await fetch(`http://localhost:5000/api/staff/${id}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (response.ok) {
        setStaff((prevStaff) => prevStaff.filter((staffMember) => staffMember.StaffID !== id));
      } else {
        throw new Error('Failed to delete staff');
      }
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div>
      <h2>Manage Staff</h2>
      {error && <p style={{ color: 'red' }}>{error}</p>}

      {/* Add/Edit Staff Form */}
      <div>
        <h3>{isEditing ? 'Edit Staff' : 'Add New Staff'}</h3>
        <input
          type="text"
          placeholder="Name"
          value={newStaff.name}
          onChange={(e) => setNewStaff({ ...newStaff, name: e.target.value })}
        />
        <input
          type="text"
          placeholder="Role"
          value={newStaff.role}
          onChange={(e) => setNewStaff({ ...newStaff, role: e.target.value })}
        />
        <input
          type="text"
          placeholder="Department"
          value={newStaff.department}
          onChange={(e) => setNewStaff({ ...newStaff, department: e.target.value })}
        />
        <input
          type="text"
          placeholder="Duty Time"
          value={newStaff.dutyTime}
          onChange={(e) => setNewStaff({ ...newStaff, dutyTime: e.target.value })}
        />
        {isEditing ? (
          <button onClick={handleEditStaff}>Update Staff</button>
        ) : (
          <button onClick={handleAddStaff}>Add Staff</button>
        )}
      </div>

      {/* Display Staff */}
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Role</th>
            <th>Department</th>
            <th>Duty Time</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {staff.map((staffMember) => (
            <tr key={staffMember.StaffID}>
              <td>{staffMember.Name}</td>
              <td>{staffMember.Role}</td>
              <td>{staffMember.Department}</td>
              <td>{staffMember.DutyTime}</td>
              <td>
                <button onClick={() => { 
                  setIsEditing(true); 
                  setCurrentStaffId(staffMember.StaffID); 
                  setNewStaff(staffMember); 
                }}>
                  Edit
                </button>
                <button onClick={() => handleDeleteStaff(staffMember.StaffID)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ManageStaff;
