import PropTypes from 'prop-types';
import { useDispatch } from 'react-redux';
import { useState, useEffect } from 'react';

import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import MuiAlert from '@mui/material/Alert';
import Snackbar from '@mui/material/Snackbar';
import MenuItem from '@mui/material/MenuItem';
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


export default function EditSale({ handleGetDetail, detailSale, refreshPage }) {
    const dispatch = useDispatch();

    const { updateSale } = useSale();
    const { getRegion, getCity } = useRegion();
    const { getCategory, getProduct } = useProduct();

    const [open, setOpen] = useState(false);

    const [regions, setRegions] = useState([]);
    const [regionValue, setRegionValue] = useState('');
    const [regionDefault, setRegionDefault] = useState(null);

    const [cities, setCities] = useState([]);
    const [cityValue, setCityValue] = useState('');
    const [cityDefault, setCityDefault] = useState(null);

    const [categories, setCategories] = useState([]);
    const [categoryValue, setCategoryValue] = useState('');
    const [categoryDefault, setCategoryDefault] = useState(null);

    const [products, setProducts] = useState([]);
    const [productValue, setProductValue] = useState('');
    const [productDefault, setProductDefault] = useState(null);
    const [productPrice, setProductPrice] = useState(null)

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

    useEffect(() => {
        if (!Object.keys(detailSale).length) return

        const currentRegion = regions.find(region => region.itemValue.toLowerCase() === detailSale.regionId.toLowerCase())
        if (!currentRegion) return
        setRegionDefault({ ...currentRegion })
        setRegionValue(currentRegion.itemText)


        getCity({ regionId: currentRegion.itemValue }).then(res => {
            if (!res?.data?.status) return
            setCities(res?.data?.data)
            const currentCity = res?.data?.data.find(city => city.itemValue.toLowerCase() === detailSale.cityId.toLowerCase())
            if (!currentCity) return
            setCityDefault({ ...currentCity })
            setCityValue(currentCity.itemText)
        })

        const currentCategory = categories.find(category => category.itemValue.toLowerCase() === detailSale.categoryId.toLowerCase())
        if (!currentCategory) return
        setCategoryDefault({ ...currentCategory })
        setCategoryValue(currentCategory.itemText)

        getProduct({ categoryId: currentCategory.itemValue }).then(res => {
            if (!res?.data?.status) return
            setProducts(res?.data?.data)
            const currentProduct = res?.data?.data.find(product => product.itemValue.toLowerCase() === detailSale.productId.toLowerCase())
            if (!currentProduct) return
            setProductDefault({ ...currentProduct })
            setProductValue(currentProduct.itemText)
            setProductPrice(currentProduct.itemPrice)
        })

        setDataModel({ ...detailSale })
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [detailSale])

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
            setProductPrice(null)
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

        setDataModel({
            productId: '',
            categoryId: '',
            regionId: '',
            cityId: '',
            quantity: '',
            unitPrice: ''
        })
    };

    const handleEditSale = () => {
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
        updateSale(dataModel).then(res => {
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
                message: 'Updated successfully.',
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
        <>
            <MenuItem onClick={() => { handleClickOpen(); handleGetDetail() }}>
                <Iconify icon="eva:edit-fill" sx={{ mr: 2 }} />
                Edit
            </MenuItem>
            <Dialog
                open={open}
                onClose={handleClose}
                fullWidth
                maxWidth="lg"
            >
                <DialogTitle>Edit food sale</DialogTitle>
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
                                value={regionDefault}
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
                                value={cityDefault}
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
                                value={categoryDefault}
                            />
                        </Grid>
                        <Grid item xs={12} md={6} lg={4}>
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
                                value={productDefault}
                            />
                        </Grid>
                        <Grid item xs={12} md={6} lg={2}>
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
                    <Button onClick={handleEditSale}>Save</Button>
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
        </>
    );
}

EditSale.propTypes = {
    handleGetDetail: PropTypes.any,
    detailSale: PropTypes.any,
    refreshPage: PropTypes.func
}