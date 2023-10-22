import axios from "axios";

import { API_URL } from "src/constants";

const getSaleList = async (dataSend = {}) => {
  try {
    const result = await axios.post(`${API_URL}/FoodSale/FoodSalePaging`, dataSend);
    if (!result) throw new Error("Error");
    return result;
  } catch (err) {
    return null
  }
}

const addSale = async (dataSend = {}) => {
  try {
    const result = await axios.post(`${API_URL}/FoodSale/FoodSaleAdd`, dataSend);
    if (!result) throw new Error("Error");
    return result;
  } catch (err) {
    return null
  }
}

const deleteSale = async (id = '') => {
  try {
    const result = await axios.delete(`${API_URL}/FoodSale/${id}`);
    if (!result) throw new Error("Error");
    return result;
  } catch (err) {
    return null
  }
}

const getSaleDetail = async (dataSend = {}) => {
  try {
    const result = await axios.get(`${API_URL}/FoodSale/FoodSaleDetail`, { params: dataSend });
    if (!result) throw new Error("Error");
    return result;
  } catch (err) {
    return null
  }
}

const updateSale = async (dataSend = {}) => {
  try {
    const result = await axios.post(`${API_URL}/FoodSale/FoodSaleUpdate`, dataSend);
    if (!result) throw new Error("Error");
    return result;
  } catch (err) {
    return null
  }
}

export function useSale() {
  return {
    getSaleList,
    addSale,
    deleteSale,
    getSaleDetail,
    updateSale
  }
}
