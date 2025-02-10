import axios from 'axios';

const apiUrl =  `https://${process.env.REACT_APP_API_DOMAIN}`

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
 
      const newTask = { name, isComplete: false };
      const result = await axios.post("/items", newTask);
      return result.data;
  
  },

  setCompleted: async (id, isComplete, name) => {
  
      const updatedTask = { id, isComplete, name };
      await axios.put(`/items/${id}`, updatedTask);
      return true;
   
  },

  deleteTask: async (id) => {

      await axios.delete(`/items/${id}`);
      return true;
    
  }
};
