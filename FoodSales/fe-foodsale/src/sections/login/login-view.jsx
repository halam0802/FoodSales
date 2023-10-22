import { useState } from 'react';
import { useDispatch } from 'react-redux';

import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import Stack from '@mui/material/Stack';
import MuiAlert from '@mui/material/Alert';
import Snackbar from '@mui/material/Snackbar';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import LoadingButton from '@mui/lab/LoadingButton';
import { alpha, useTheme } from '@mui/material/styles';
import InputAdornment from '@mui/material/InputAdornment';

import { useRouter } from 'src/routes/hooks';

import { useAuth } from 'src/api/useAuth';
import { bgGradient } from 'src/theme/css';
import { storeLogin, storeLoading } from 'src/features/userSlice'

import Logo from 'src/components/logo';
import Iconify from 'src/components/iconify';

// ----------------------------------------------------------------------

export default function LoginView() {
  const theme = useTheme();

  const router = useRouter();

  const dispatch = useDispatch();

  const { login } = useAuth();

  const [showPassword, setShowPassword] = useState(false);

  const [username, setUsername] = useState('');

  const [password, setPassword] = useState('');

  const [snackbar, setSnackbar] = useState({
    open: false,
    vertical: 'bottom',
    horizontal: 'right',
    message: '',
    variant: 'error'
  });

  const handleLogin = () => {
    if (username === '' || password === '') {
      setSnackbar({
        ...snackbar,
        open: true,
        message: 'Username and/or password must not be empty.',
        variant: 'error'
      })
      return;
    }
    dispatch(storeLoading(true))
    login({
      username,
      password
    }).then(res => {
      dispatch(storeLoading(false))
      if (!res || !res?.data?.status) {
        setSnackbar({
          ...snackbar,
          open: true,
          message: 'Username and/or password incorrect.',
          variant: 'error'
        })
        return
      };

      setSnackbar({
        ...snackbar,
        open: true,
        message: 'Login successfully.',
        variant: 'success'
      })

      dispatch(storeLogin(res?.data?.data))
      setTimeout(() => router.push('/'), 500)
    })
  };

  const handleKeyPress = (event) => {
    if (event.key === 'Enter') {
      handleLogin()
    }
  }

  const handleCloseSnackbar = (event, reason) => {
    if (reason === 'clickaway') {
      return;
    }

    setSnackbar({ ...snackbar, open: false });
  };

  const renderForm = (
    <>
      <Stack spacing={3}>
        <TextField
          name="username"
          label="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          onKeyUp={handleKeyPress}
        />

        <TextField
          name="password"
          label="Password"
          type={showPassword ? 'text' : 'password'}
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          onKeyUp={handleKeyPress}
          InputProps={{
            endAdornment: (
              <InputAdornment position="end">
                <IconButton onClick={() => setShowPassword(!showPassword)} edge="end">
                  <Iconify icon={showPassword ? 'eva:eye-fill' : 'eva:eye-off-fill'} />
                </IconButton>
              </InputAdornment>
            ),
          }}
        />
      </Stack>

      <LoadingButton
        fullWidth
        size="large"
        type="submit"
        variant="contained"
        color="inherit"
        onClick={handleLogin}
        sx={{ my: 3 }}
      >
        Login
      </LoadingButton>
    </>
  );

  return (
    <Box
      sx={{
        ...bgGradient({
          color: alpha(theme.palette.background.default, 0.9),
          imgUrl: '/assets/background/overlay_4.jpg',
        }),
        height: 1,
      }}
    >
      <Logo
        sx={{
          position: 'fixed',
          top: { xs: 16, md: 24 },
          left: { xs: 16, md: 24 },
        }}
      />

      <Stack alignItems="center" justifyContent="center" sx={{ height: 1 }}>
        <Card
          sx={{
            p: 5,
            width: 1,
            maxWidth: 420,
          }}
        >
          <Typography variant="h4" sx={{ mb: 3 }}>Sign in to Food Sales</Typography>

          {renderForm}
        </Card>
      </Stack>

      <Snackbar
        open={snackbar.open}
        autoHideDuration={3000}
        message={snackbar.message}
        onClose={handleCloseSnackbar}
        anchorOrigin={{
          vertical: snackbar.vertical,
          horizontal: snackbar.horizontal
        }}
      >
        <MuiAlert
          elevation={6}
          severity={snackbar.variant}
          variant="filled"
          onClose={handleCloseSnackbar}
          sx={{ width: '100%' }}
        >
          {snackbar.message}
        </MuiAlert>
      </Snackbar>
    </Box>
  );
}
