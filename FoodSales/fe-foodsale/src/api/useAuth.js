import axios from "axios";

import { API_URL } from "src/constants";

const login = async (dataSend = {}) => {
  try {
    const result = await axios.post(`${API_URL}/User/Login`, dataSend);
    if (!result) throw new Error("Error");
    return result;
  } catch (err) {
    return null
  }
};

const setToken = (token) => {
  if (!token) {
    delete axios.defaults.headers.common.Authorization;
    return;
  };

  axios.defaults.headers.common = {'Authorization': `Bearer ${token}`}
}

export function useAuth() {
  return {
    login,
    setToken
  }
}
