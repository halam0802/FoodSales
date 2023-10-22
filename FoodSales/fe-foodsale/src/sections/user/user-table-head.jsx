import PropTypes from 'prop-types';

import TableRow from '@mui/material/TableRow';
import TableHead from '@mui/material/TableHead';
import TableCell from '@mui/material/TableCell';
import TableSortLabel from '@mui/material/TableSortLabel';

import Iconify from 'src/components/iconify';

// ----------------------------------------------------------------------

export default function UserTableHead({
  headLabel,
  numSelected,
  onRequestSort,
  onSelectAllClick,
  filter
}) {
  const onSort = (property) => (event) => {
    onRequestSort(event, property);
  };

  return (
    <TableHead>
      <TableRow>

        {headLabel.map((headCell) => (
          <TableCell
            key={headCell.id}
            align={headCell.align || 'left'}
            sx={{ width: headCell.width, minWidth: headCell.minWidth }}
          >
            <TableSortLabel
              hideSortIcon
              onClick={onSort(headCell.id)}
            >
              {headCell.label}
              {headCell.label &&
                <Iconify
                  width={12}
                  icon={`eva:arrow-${filter[headCell.id] === '' ? 'up' : 'down'}-fill`}
                />}
            </TableSortLabel>
          </TableCell>
        ))}
      </TableRow>
    </TableHead>
  );
}

UserTableHead.propTypes = {
  headLabel: PropTypes.array,
  numSelected: PropTypes.number,
  onRequestSort: PropTypes.func,
  onSelectAllClick: PropTypes.func,
  filter: PropTypes.any
};
