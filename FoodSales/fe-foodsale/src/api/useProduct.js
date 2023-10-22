import axios from "axios";

import { API_URL } from "src/constants";

const getCategory = async () => {
  try {
    const result = await axios.get(`${API_URL}/Category/GetCategories`);
    if (!result) throw new Error("Error");
    return result;
  } catch (err) {
    return null
  }
}

const getProduct = async (dataSend = {}) => {
  try {
    const result = await axios.get(`${API_URL}/Product/GetProducts`, { params: dataSend });
    if (!result) throw new Error("Error");
    return result;
  } catch (err) {
    return null
  }
}

export function useProduct() {
  return {
    getCategory,
    getProduct
  }
}
