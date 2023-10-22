import { useState } from 'react';
import { useDispatch } from 'react-redux';

import Card from '@mui/material/Card';
import Stack from '@mui/material/Stack';
import Table from '@mui/material/Table';
import TableRow from '@mui/material/TableRow';
import TextField from '@mui/material/TextField';
import Container from '@mui/material/Container';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import Pagination from '@mui/material/Pagination';
import Typography from '@mui/material/Typography';
import TableFooter from '@mui/material/TableFooter';
import TableContainer from '@mui/material/TableContainer';

import { useRouter } from 'src/routes/hooks';

import { useDebouncedEffect } from 'src/hooks/use-debounce-effect';

import { users } from 'src/_mock/user';
import { useSale } from 'src/api/useSale';
// import { mockData, mockDetail } from 'src/_mock/data';
import { storeLogout, storeLoading } from 'src/features/userSlice';

import Scrollbar from 'src/components/scrollbar';
import AddSale from 'src/components/modal/add-sale';

import TableNoData from '../table-no-data';
import UserTableRow from '../user-table-row';
import UserTableHead from '../user-table-head';
import UserTableToolbar from '../user-table-toolbar';


// ----------------------------------------------------------------------

export default function UserPage() {
  const { getSaleList, deleteSale, getSaleDetail } = useSale();

  const dispatch = useDispatch();

  const router = useRouter();

  const [selected, setSelected] = useState([]);

  const [filter, setFilter] = useState({
    productName: "",
    categoryName: "",
    regionName: "",
    cityName: "",
    quantity: 0,
    unitPrice: 0,
    totalPrice: 0,
    fromDate: "",
    toDate: "",
    sortDate: "desc",
    sortRegion: "",
    sortCity: "",
    sortCategory: "",
    sortProduct: "",
    sortQuantity: "",
    sortUnitPrice: "",
    sortTotalPrice: "",
    pageIndex: 1,
    pageSize: 10
  })

  const [totalPage, setTotalPage] = useState(10);

  const [data, setDatata] = useState([]);

  const [detail, setDetail] = useState({});

  useDebouncedEffect(() => {
    handleGetPageData()
  }, [filter], 500)

  const handleModifyComplete = () => {
    if (filter.pageIndex === 1) {
      handleGetPageData()
    } else {
      setFilter({ ...filter, pageIndex: 1 })
    }
  }

  const handleGetPageData = () => {
    dispatch(storeLoading(true))
    getSaleList(filter).then(res => {
      dispatch(storeLoading(false))
      if (!res?.data?.status) {
        dispatch(storeLogout())
        router.push('/login')
      }
      setTotalPage(res?.data?.data?.totalPages)
      setDatata(res?.data?.data?.data)
    })
  }

  const handleSort = (event, id) => {
    const shallowObj = filter;
    if (!Object.getOwnPropertyDescriptor(shallowObj, id) || id === '' || !id) return
    const order = shallowObj[id] === '' ? 'desc' : '';
    shallowObj[id] = order;
    setFilter({ ...shallowObj });
  };

  const handleSelectAllClick = (event) => {
    if (event.target.checked) {
      const newSelecteds = users.map((n) => n.name);
      setSelected(newSelecteds);
      return;
    }
    setSelected([]);
  };

  const handleClick = (event, name) => {
    const selectedIndex = selected.indexOf(name);
    let newSelected = [];
    if (selectedIndex === -1) {
      newSelected = newSelected.concat(selected, name);
    } else if (selectedIndex === 0) {
      newSelected = newSelected.concat(selected.slice(1));
    } else if (selectedIndex === selected.length - 1) {
      newSelected = newSelected.concat(selected.slice(0, -1));
    } else if (selectedIndex > 0) {
      newSelected = newSelected.concat(
        selected.slice(0, selectedIndex),
        selected.slice(selectedIndex + 1)
      );
    }
    setSelected(newSelected);
  };

  const handleChangePage = (event, newPage) => {
    setFilter({ ...filter, pageIndex: newPage });
  };

  const handleDeleteSale = (id) => {
    dispatch(storeLoading(true))
    deleteSale(id).then(res => {
      dispatch(storeLoading(false))
      if (!res?.data?.status) return
      handleModifyComplete()
    })
  };

  const handleGetDetail = (id) => {
    dispatch(storeLoading(true))
    getSaleDetail({ id }).then(res => {
      dispatch(storeLoading(false))
      if (!res?.data) return
      setDetail({ ...res?.data })
    })
  };

  const handleSearchByDate = ({ from, to }) => {
    setFilter({
      ...filter,
      fromDate: from || "",
      toDate: to || ""
    })
  }

  return (
    <Container sx={{ maxWidth: '1500px !important' }}>

      <Stack direction="row" alignItems="center" justifyContent="space-between" mb={5}>
        <Typography variant="h4">Food Sales</Typography>

        <AddSale refreshPage={handleModifyComplete} />
      </Stack>

      <Card>
        <UserTableToolbar searchByDate={handleSearchByDate} />

        <Scrollbar>
          <TableContainer sx={{ overflow: 'unset' }}>
            <Table sx={{ minWidth: 800 }}>
              <UserTableHead
                numSelected={selected.length}
                onRequestSort={handleSort}
                onSelectAllClick={handleSelectAllClick}
                filter={filter}
                headLabel={[
                  { id: 'sortDate', label: 'Order Date' },
                  { id: 'sortRegion', label: 'Region' },
                  { id: 'sortCity', label: 'City' },
                  { id: 'sortCategory', label: 'Category' },
                  { id: 'sortProduct', label: 'Product' },
                  { id: 'sortQuantity', label: 'Quantity', align: 'center' },
                  { id: 'sortUnitPrice', label: 'Unit Price', align: 'center' },
                  { id: 'sortTotalPrice', label: 'Total Price', align: 'center' },
                  { id: '' },
                ]}
              />
              <TableBody>
                <TableRow tabIndex={-1}>

                  <TableCell />

                  <TableCell>
                    <TextField
                      fullWidth
                      size="small"
                      value={filter.regionName}
                      onChange={(e) => setFilter({ ...filter, regionName: e.target.value })}
                    />
                  </TableCell>

                  <TableCell>
                    <TextField
                      fullWidth
                      size="small"
                      value={filter.cityName}
                      onChange={(e) => setFilter({ ...filter, cityName: e.target.value })}
                    />
                  </TableCell>

                  <TableCell>
                    <TextField
                      fullWidth
                      size="small"
                      value={filter.categoryName}
                      onChange={(e) => setFilter({ ...filter, categoryName: e.target.value })}
                    />
                  </TableCell>

                  <TableCell>
                    <TextField
                      fullWidth
                      size="small"
                      value={filter.productName}
                      onChange={(e) => setFilter({ ...filter, productName: e.target.value })}
                    />
                  </TableCell>

                  <TableCell>
                    <TextField
                      fullWidth
                      size="small"
                      value={filter.quantity || ''}
                      onChange={(e) => setFilter({ ...filter, quantity: +e.target.value || 0 })}
                    />
                  </TableCell>

                  <TableCell>
                    <TextField
                      fullWidth
                      size="small"
                      value={filter.unitPrice || ''}
                      onChange={(e) => setFilter({ ...filter, unitPrice: +e.target.value || 0 })}
                    />
                  </TableCell>

                  <TableCell>
                    <TextField
                      fullWidth
                      size="small"
                      value={filter.totalPrice || ''}
                      onChange={(e) => setFilter({ ...filter, totalPrice: +e.target.value || 0 })}
                    />
                  </TableCell>

                  <TableCell />
                </TableRow>
                {data.map((row) => (
                  <UserTableRow
                    key={row.id}
                    saleId={row.id}
                    date={row.createAt}
                    region={row.regionName}
                    city={row.cityName}
                    category={row.categoryName}
                    product={row.productName}
                    quantity={row.quantity}
                    unitPrice={row.unitPrice}
                    totalPrice={row.totalPrice}
                    handleClick={(event) => handleClick(event, row.name)}
                    deleteSale={handleDeleteSale}
                    getDetailSale={handleGetDetail}
                    detailSale={detail}
                    refreshPage={handleModifyComplete}
                  />
                ))}

                {/* <TableEmptyRows
                  height={77}
                  emptyRows={emptyRows(page, rowsPerPage, users.length)}
                /> */}

                {!data.length && <TableNoData />}
              </TableBody>
              {
                !!data.length &&
                <TableFooter>
                  <TableRow>
                    <TableCell colSpan={10}>
                      <Stack alignItems="end">
                        <Pagination page={filter.pageIndex} count={totalPage} onChange={handleChangePage} />
                      </Stack>
                    </TableCell>
                  </TableRow>
                </TableFooter>
              }
            </Table>
          </TableContainer>
        </Scrollbar>

      </Card>
    </Container>
  );
}
