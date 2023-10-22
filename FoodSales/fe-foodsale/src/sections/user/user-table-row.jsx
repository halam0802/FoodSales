import { useState } from 'react';
import PropTypes from 'prop-types';

import Popover from '@mui/material/Popover';
import TableRow from '@mui/material/TableRow';
import MenuItem from '@mui/material/MenuItem';
import TableCell from '@mui/material/TableCell';
import IconButton from '@mui/material/IconButton';

import { fDateTime } from 'src/utils/format-time';

import Iconify from 'src/components/iconify';
import EditSale from 'src/components/modal/edit-sale/edit-sale';

// ----------------------------------------------------------------------

export default function UserTableRow({
  selected,
  saleId,
  date,
  region,
  city,
  category,
  product,
  quantity,
  unitPrice,
  totalPrice,
  handleClick,
  deleteSale,
  detailSale,
  getDetailSale,
  refreshPage
}) {
  const [open, setOpen] = useState(null);

  const handleOpenMenu = (event) => {
    setOpen(event.currentTarget);
  };

  const handleCloseMenu = () => {
    setOpen(null);
  };

  const handleDeleteSale = () => {
    deleteSale(saleId);
    handleCloseMenu()
  }

  const handleGetDetail = () => {
    getDetailSale(saleId);
  }

  const handleRefreshPage = () => {
    handleCloseMenu()
    refreshPage()
  }

  return (
    <>
      <TableRow hover tabIndex={-1} role="checkbox" selected={selected}>
        <TableCell>{fDateTime(date)}</TableCell>

        <TableCell>{region}</TableCell>

        <TableCell>{city}</TableCell>

        <TableCell>{category}</TableCell>

        <TableCell>{product}</TableCell>

        <TableCell align='center'>{quantity}</TableCell>

        <TableCell align='center'>{unitPrice}</TableCell>

        <TableCell align='center'>{totalPrice}</TableCell>

        <TableCell align="right">
          <IconButton onClick={handleOpenMenu}>
            <Iconify icon="eva:more-vertical-fill" />
          </IconButton>
        </TableCell>
      </TableRow>

      <Popover
        open={!!open}
        anchorEl={open}
        onClose={handleCloseMenu}
        anchorOrigin={{ vertical: 'top', horizontal: 'left' }}
        transformOrigin={{ vertical: 'top', horizontal: 'right' }}
        PaperProps={{
          sx: { width: 140 },
        }}
      >
        <EditSale
          handleGetDetail={handleGetDetail}
          detailSale={detailSale}
          refreshPage={handleRefreshPage}
        />

        <MenuItem onClick={handleDeleteSale} sx={{ color: 'error.main' }}>
          <Iconify icon="eva:trash-2-outline" sx={{ mr: 2 }} />
          Delete
        </MenuItem>
      </Popover>
    </>
  );
}

UserTableRow.propTypes = {
  handleClick: PropTypes.func,
  saleId: PropTypes.any,
  selected: PropTypes.any,
  date: PropTypes.any,
  region: PropTypes.any,
  city: PropTypes.any,
  category: PropTypes.any,
  product: PropTypes.any,
  quantity: PropTypes.any,
  unitPrice: PropTypes.any,
  totalPrice: PropTypes.any,
  deleteSale: PropTypes.func,
  detailSale: PropTypes.any,
  getDetailSale: PropTypes.func,
  refreshPage: PropTypes.func
};
