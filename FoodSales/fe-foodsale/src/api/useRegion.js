import axios from "axios";

import { API_URL } from "src/constants";

const getRegion = async () => {
  try {
    const result = await axios.get(`${API_URL}/Region/GetRegions`);
    if (!result) throw new Error("Error");
    return result;
  } catch (err) {
    return null
  }
}

const getCity = async (dataSend = {}) => {
  try {
    const result = await axios.get(`${API_URL}/City/GetCities`, { params: dataSend });
    if (!result) throw new Error("Error");
    return result;
  } catch (err) {
    return null
  }
}

export function useRegion() {
  return {
    getRegion,
    getCity
  }
}
