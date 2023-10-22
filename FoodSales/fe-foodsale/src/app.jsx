/* eslint-disable perfectionist/sort-imports */
import 'src/global.css';
import { useSelector } from 'react-redux';
import { selectUser, selectLoading } from 'src/features/userSlice';

import { useScrollToTop } from 'src/hooks/use-scroll-to-top';

import Router from 'src/routes/sections';
import ThemeProvider from 'src/theme';

import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';

import CircularProgress from '@mui/material/CircularProgress';

import { useAuth } from './api/useAuth';
// ----------------------------------------------------------------------

export default function App() {
  useScrollToTop();

  const user = useSelector(selectUser);
  const loading = useSelector(selectLoading);

  const { setToken } = useAuth();

  setToken(user?.token ?? null)

  return (
    <LocalizationProvider dateAdapter={AdapterDayjs}>
      <ThemeProvider>
        {
          loading &&
          <div style={{
            height: '100vh',
            width: '100vw',
            zIndex: 9999999,
            position: 'fixed',
            display: 'grid',
            placeItems: 'center',
            backgroundColor: 'rgba(0, 0, 0, .5)',
          }}
          >
            <CircularProgress />
          </div>
        }
        <Router />
      </ThemeProvider>
    </LocalizationProvider>
  );
}
