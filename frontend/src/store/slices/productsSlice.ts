import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import type { Product, Topping, ProductType } from '../../types';

interface ProductsState {
  products: Product[];
  toppings: Topping[];
  selectedType: ProductType;
  loading: boolean;
  error: string | null;
}

const initialState: ProductsState = {
  products: [],
  toppings: [],
  selectedType: 0, // ProductType.Pizza
  loading: false,
  error: null,
};

const productsSlice = createSlice({
  name: 'products',
  initialState,
  reducers: {
    setProducts: (state, action: PayloadAction<Product[]>) => {
      state.products = action.payload;
    },
    setToppings: (state, action: PayloadAction<Topping[]>) => {
      state.toppings = action.payload;
    },
    setSelectedType: (state, action: PayloadAction<ProductType>) => {
      state.selectedType = action.payload;
    },
    setLoading: (state, action: PayloadAction<boolean>) => {
      state.loading = action.payload;
    },
    setError: (state, action: PayloadAction<string | null>) => {
      state.error = action.payload;
    },
  },
});

export const { setProducts, setToppings, setSelectedType, setLoading, setError } = productsSlice.actions;
export default productsSlice.reducer;
