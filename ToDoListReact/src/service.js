import axios from 'axios';

const apiUrl = "http://localhost:5190"; 
axios.defaults.baseURL = apiUrl;


axios.interceptors.response.use(
  response => response,
  error => {
    console.error("API Error:", error.response ? error.response.data : error.message);
    return Promise.reject(error);
  }
);

export default {
  getTasks: async () => {
    const result = await axios.get("/items");
    return result.data;
  },

  addTask: async (name) => {
    try {
      const newTask = { name, isComplete: false };
      const result = await axios.post("/items", newTask);
      return result.data;
    } catch (error) {
      return null;
    }
  },

  setCompleted: async (id, isComplete, name) => {
    try {
      const updatedTask = { id, isComplete, name };
      await axios.put(`/items/${id}`, updatedTask);
      return true;
    } catch (error) {
      return false;
    }
  },

  deleteTask: async (id) => {
    try {
      await axios.delete(`/items/${id}`);
      return true;
    } catch (error) {
      return false;
    }
  }
};
