import axios from 'axios';

const API_URL = 'http://localhost:5249/api';

export const getPatients = async () => {
  try {
    const response = await axios.get(`${API_URL}/patient`);
    return response.data;
  } catch (error) {
    console.error("Error fetching patients:", error);
  }
};

export const getDoctors = async () => {
  try {
    const response = await axios.get(`${API_URL}/doctor`);
    return response.data;
  } catch (error) {
    console.error("Error fetching doctors:", error);
  }
};

export const createAppointment = async (appointment) => {
  try {
    const response = await axios.post(`${API_URL}/appointment`, appointment);
    return response.data;
  } catch (error) {
    console.error("Error creating appointment:", error);
  }
};
