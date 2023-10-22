import PropTypes from 'prop-types';
import { useState, useEffect } from 'react';

import Button from '@mui/material/Button';
import MuiAlert from '@mui/material/Alert';
import Toolbar from '@mui/material/Toolbar';
import Snackbar from '@mui/material/Snackbar';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';

// ----------------------------------------------------------------------

export default function UserTableToolbar({ searchByDate }) {
  const [cleared, setCleared] = useState({
    from: false,
    to: false
  });

  const [value, setValue] = useState({
    from: null,
    to: null
  });

  const [snackbar, setSnackbar] = useState({
    open: false,
    vertical: 'bottom',
    horizontal: 'right',
    message: '',
    variant: 'error'
  });

  useEffect(() => {
    if (cleared) {
      const timeout = setTimeout(() => {
        setCleared({ from: false, to: false });
      }, 1500);

      return () => clearTimeout(timeout);
    }
    return () => { };
  }, [cleared]);

  const handleSearch = () => {
    if (value.from && value.to) {
      if (new Date(value.from) > new Date(value.to)) {
        setSnackbar({
          ...snackbar,
          open: true,
          message: 'From date must not be larger than To date.',
        });
        return
      }
    }
    searchByDate(value)
  }

  const handleCloseSnackbar = (event, reason) => {
    if (reason === 'clickaway') {
      return;
    }

    setSnackbar({ ...snackbar, open: false });
  };

  return (
    <>
      <Toolbar
        sx={{
          height: 96,
          display: 'flex',
          justifyContent: 'flex-end',
          p: (theme) => theme.spacing(0, 1, 0, 3)
        }}
      >

        <DatePicker
          label="From Date"
          format="DD MMM YYYY"
          value={value.from}
          onChange={(val) => setValue({ ...value, from: val?.$d?.toISOString() || null })}
          slotProps={{
            field: {
              clearable: true,
              onClear: () => {
                setCleared({ ...cleared, from: true });
                setValue({ ...value, from: null })
              }
            },
          }}
        />

        <span style={{ margin: '0 20px' }}>-</span>

        <DatePicker
          label="To Date"
          format="DD MMM YYYY"
          value={value.to}
          onChange={(val) => setValue({ ...value, to: val?.$d?.toISOString() || null })}
          slotProps={{
            field: {
              clearable: true,
              onClear: () => {
                setCleared({ ...cleared, to: true }); setValue({ ...value, to: null })
              }
            },
          }}
        />

        <Button
          size='large'
          sx={{ ml: 2 }}
          onClick={handleSearch}
        >
          Search
        </Button>

      </Toolbar>
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
    </>
  );
}

UserTableToolbar.propTypes = {
  searchByDate: PropTypes.func,
};
