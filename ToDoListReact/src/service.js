
import axios from "axios";

const instance = axios.create({
  baseURL: `${process.env.REACT_APP_API_URL}`,
});

console.log(process.env);

instance.interceptors.response.use(

    response => response,
  error => {
    console.error("API Error:", error.response ? error.response.data : error.message);
    return Promise.reject(error);
  }
);


export default {
  getTasks: async () => {
    const result = await instance.get(`/items`);
    return result.data;
  },

  addTask: async (name) => {
    console.log("addTask", name);
    const newTask = { name, isComplete: false };
    const result = await instance.post(`/items`, newTask);
    return result.data;
  },

  setCompleted: async (id, isComplete) => {
    const updatedTask = { id, isComplete };
    const result = await instance.put(`/items/${id}`, updatedTask);
    return result.data;
  },

  deleteTask: async (id) => {
    await instance.delete(`/items/${id}`);
    return { id };
  },

  
};