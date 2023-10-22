import { useState } from 'react';
import PropTypes from 'prop-types';
import { useDispatch } from 'react-redux';

import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import MuiAlert from '@mui/material/Alert';
import Snackbar from '@mui/material/Snackbar';
import TextField from '@mui/material/TextField';
import DialogTitle from '@mui/material/DialogTitle';
import Autocomplete from '@mui/material/Autocomplete';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';

import { handleNumberChange } from 'src/utils/format-number';

import { useSale } from 'src/api/useSale';
import { useRegion } from 'src/api/useRegion';
import { useProduct } from 'src/api/useProduct';
import { storeLoading } from 'src/features/userSlice';

import Iconify from 'src/components/iconify';

export default function AddSale({ refreshPage }) {
    const dispatch = useDispatch();

    const { addSale } = useSale();
    const { getRegion, getCity } = useRegion();
    const { getCategory, getProduct } = useProduct();

    const [open, setOpen] = useState(false);

    const [regions, setRegions] = useState([]);
    const [regionValue, setRegionValue] = useState('');

    const [cities, setCities] = useState([]);
    const [cityValue, setCityValue] = useState('');

    const [categories, setCategories] = useState([]);
    const [categoryValue, setCategoryValue] = useState('');

    const [products, setProducts] = useState([]);
    const [productValue, setProductValue] = useState('');
    const [productPrice, setProductPrice] = useState();

    const [dataModel, setDataModel] = useState({
        productId: "",
        categoryId: "",
        regionId: "",
        cityId: "",
        quantity: "",
        unitPrice: ""
    });

    const [snackbar, setSnackbar] = useState({
        open: false,
        vertical: 'bottom',
        horizontal: 'right',
        message: '',
        variant: 'error'
    });

    const handleClickOpen = () => {
        setOpen(true);
        getRegion().then(res => {
            if (!res?.data?.status) return
            setRegions(res?.data?.data)
        })
        getCategory().then(res => {
            if (!res?.data?.status) return
            setCategories(res?.data?.data)
        })
    };

    const handleClose = () => {
        setOpen(false);
        handleResetValue();
    };

    const handleSetRegion = (e, val) => {
        if (!val) {
            setRegionValue('');
            setCities([]);
            setCityValue('');
            setDataModel({ ...dataModel, regionId: '' })
            return
        }

        getCity({ regionId: val.itemValue }).then(res => {
            if (!res?.data?.status) return
            setRegionValue(val.itemText)
            setCityValue('')
            setCities(res?.data?.data)
            setDataModel({ ...dataModel, regionId: val.itemValue })
        })
    };

    const handleSetCity = (e, val) => {
        if (!val) {
            setCityValue('');
            setDataModel({ ...dataModel, cityId: '' })
            return
        }
        setCityValue(val.itemText);
        setDataModel({ ...dataModel, cityId: val.itemValue })
    };

    const handleSetCategory = (e, val) => {
        if (!val) {
            setCategoryValue('');
            setProducts([]);
            setProductValue('');
            setProductPrice(null);
            setDataModel({ ...dataModel, categoryId: '' })
            return
        }

        getProduct({ categoryId: val.itemValue }).then(res => {
            if (!res?.data?.status) return
            setCategoryValue(val.itemText)
            setProductValue('')
            setProductPrice(null)
            setProducts(res?.data?.data)
            setDataModel({ ...dataModel, categoryId: val.itemValue })
        })
    };

    const handleSetProduct = (e, val) => {
        if (!val) {
            setProductValue('');
            setDataModel({ ...dataModel, productId: '' })
            return
        }
        setProductValue(val.itemText);
        setProductPrice(val.itemPrice)
        setDataModel({ ...dataModel, productId: val.itemValue })
    };

    const handleResetValue = () => {
        setRegions([]);
        setRegionValue('');

        setCities([]);
        setCityValue('');

        setCategories([]);
        setCategoryValue('');

        setProducts([]);
        setProductValue('');
        setProductPrice(null);

        setDataModel({
            productId: '',
            categoryId: '',
            regionId: '',
            cityId: '',
            quantity: '',
            unitPrice: ''
        })
    };

    const handleAddSale = () => {
        let flag = false;
        Object.keys(dataModel).forEach(key => {
            if (dataModel[key] === '' || !dataModel[key]) {
                setSnackbar({
                    ...snackbar,
                    open: true,
                    message: 'All fields must not be empty.',
                    variant: 'error'
                });
                flag = true
            }
        })
        if (flag) return
        dispatch(storeLoading(true))
        addSale(dataModel).then(res => {
            dispatch(storeLoading(false))
            if (!res?.data?.status) {
                setSnackbar({
                    ...snackbar,
                    open: true,
                    message: 'Something went wrong.',
                    variant: 'error'
                })
                return
            }
            setSnackbar({
                ...snackbar,
                open: true,
                message: 'Added successfully.',
                variant: 'success'
            })
            refreshPage();
            handleClose();
        })
    };

    const handleCloseSnackbar = (event, reason) => {
        if (reason === 'clickaway') {
            return;
        }

        setSnackbar({ ...snackbar, open: false });
    };

    return (
        <div>
            <Button variant="contained" color="inherit" onClick={handleClickOpen} startIcon={<Iconify icon="eva:plus-fill" />}>
                New Food Sale
            </Button>
            <Dialog
                open={open}
                onClose={handleClose}
                fullWidth
                maxWidth="lg"
            >
                <DialogTitle>Add new food sale</DialogTitle>
                <DialogContent sx={{ height: '400px', maxHeight: '80vh' }}>
                    <DialogContentText>
                        Fill out all the fields below.
                    </DialogContentText>
                    <Grid container spacing={2} sx={{ mt: 1, alignItems: 'center' }}>
                        <Grid item xs={12} md={6} lg={6}>
                            <Autocomplete
                                getOptionLabel={(option) => option.itemText}
                                disablePortal
                                id="region-box"
                                options={regions}
                                sx={{ width: '100%' }}
                                onChange={handleSetRegion}
                                renderInput={(params) => <TextField
                                    {...params}
                                    label="Region"
                                    onChange={(e) => setRegionValue(e.target.value)}
                                />}
                                inputValue={regionValue}
                            />
                        </Grid>
                        <Grid item xs={12} md={6} lg={6}>
                            <Autocomplete
                                getOptionLabel={(option) => option.itemText}
                                disablePortal
                                id="city-box"
                                options={cities}
                                sx={{ width: '100%' }}
                                onChange={handleSetCity}
                                renderInput={(params) => <TextField
                                    {...params}
                                    label="City"
                                    onChange={(e) => setCityValue(e.target.value)}
                                />}
                                inputValue={cityValue}
                            />
                        </Grid>
                        <Grid item xs={12} md={6} lg={6}>
                            <Autocomplete
                                getOptionLabel={(option) => option.itemText}
                                disablePortal
                                id="category-box"
                                options={categories}
                                sx={{ width: '100%' }}
                                onChange={handleSetCategory}
                                renderInput={(params) => <TextField
                                    {...params}
                                    label="Category"
                                    onChange={(e) => setCategoryValue(e.target.value)}
                                />}
                                inputValue={categoryValue}
                            />
                        </Grid>
                        <Grid item xs={6} md={3} lg={4}>
                            <Autocomplete
                                getOptionLabel={(option) => option.itemText}
                                disablePortal
                                id="product-box"
                                options={products}
                                sx={{ width: '100%' }}
                                onChange={handleSetProduct}
                                renderInput={(params) => <TextField
                                    {...params}
                                    label="Product"
                                    onChange={(e) => setProductValue(e.target.value)}
                                />}
                                inputValue={productValue}
                            />
                        </Grid>
                        <Grid item xs={6} md={3} lg={2}>
                            Price: {productPrice}
                        </Grid>
                        <Grid item xs={12} md={6} lg={6}>
                            <TextField
                                fullWidth
                                label="Quantity"
                                value={dataModel.quantity}
                                onChange={(e) => {
                                    const input = e.target.value;
                                    if (!input || (input[input.length - 1].match('[0-9]') && input[0].match('[1-9]')))
                                        setDataModel({ ...dataModel, quantity: e.target.value })
                                }}
                            />
                        </Grid>
                        <Grid item xs={12} md={6} lg={6}>
                            <TextField
                                fullWidth
                                label="Unit Price"
                                value={dataModel.unitPrice}
                                onChange={(e) => {
                                    const input = e.target.value;
                                    if (!input || handleNumberChange(input))
                                        setDataModel({ ...dataModel, unitPrice: e.target.value })
                                }}
                            />
                        </Grid>
                    </Grid>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Cancel</Button>
                    <Button onClick={handleAddSale}>Add</Button>
                </DialogActions>
            </Dialog>
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
        </div>
    );
}

AddSale.propTypes = {
    refreshPage: PropTypes.any
}