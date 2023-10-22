import { createSlice } from "@reduxjs/toolkit";

const localStoreUser = localStorage.getItem('user');

export const userSlice = createSlice({
  name: "User",
  initialState: {
    user: localStoreUser ? JSON.parse(localStoreUser) : null,
    loading: false
  },
  reducers: {
    storeLogin: (state, action) => {
      state.user = action.payload;
      localStorage.setItem('user', JSON.stringify(action.payload))
    },
    storeLogout: (state) => {
      state.user = null;
      localStorage.removeItem('user')
    },
    storeLoading: (state, action) => {
      state.loading = action.payload
    }
  },
})

export const { storeLogin, storeLogout, storeLoading } = userSlice.actions;

export const selectUser = (state) => state.user.user;

export const selectLoading = (state) => state.user.loading;

export default userSlice.reducer;