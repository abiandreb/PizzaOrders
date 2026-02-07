import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import type { Cart, CartItem } from '../../types';

interface CartState {
  cart: Cart | null;
  isCartOpen: boolean;
  loading: boolean;
}

const initialState: CartState = {
  cart: null,
  isCartOpen: false,
  loading: false,
};

const cartSlice = createSlice({
  name: 'cart',
  initialState,
  reducers: {
    setCart: (state, action: PayloadAction<Cart>) => {
      state.cart = action.payload;
    },
    clearCart: (state) => {
      state.cart = null;
    },
    setCartOpen: (state, action: PayloadAction<boolean>) => {
      state.isCartOpen = action.payload;
    },
    setLoading: (state, action: PayloadAction<boolean>) => {
      state.loading = action.payload;
    },
    addItemToCart: (state, action: PayloadAction<CartItem>) => {
      if (!state.cart) {
        state.cart = {
          sessionId: '',
          items: [action.payload],
          totalPrice: action.payload.totalPrice,
        };
      } else {
        state.cart.items.push(action.payload);
        state.cart.totalPrice += action.payload.totalPrice;
      }
    },
    removeItemFromCart: (state, action: PayloadAction<{ productId: number; toppingIds: number[] }>) => {
      if (state.cart) {
        const index = state.cart.items.findIndex(
          (item) =>
            item.productId === action.payload.productId &&
            JSON.stringify(item.toppingIds.sort()) === JSON.stringify(action.payload.toppingIds.sort())
        );
        if (index !== -1) {
          const removedItem = state.cart.items[index];
          state.cart.items.splice(index, 1);
          state.cart.totalPrice -= removedItem.totalPrice;
        }
      }
    },
    updateCartItemQuantity: (
      state,
      action: PayloadAction<{ productId: number; toppingIds: number[]; quantity: number; totalPrice: number }>
    ) => {
      if (state.cart) {
        const item = state.cart.items.find(
          (item) =>
            item.productId === action.payload.productId &&
            JSON.stringify(item.toppingIds.sort()) === JSON.stringify(action.payload.toppingIds.sort())
        );
        if (item) {
          const priceDiff = action.payload.totalPrice - item.totalPrice;
          item.quantity = action.payload.quantity;
          item.totalPrice = action.payload.totalPrice;
          state.cart.totalPrice += priceDiff;
        }
      }
    },
  },
});

export const {
  setCart,
  clearCart,
  setCartOpen,
  setLoading,
  addItemToCart,
  removeItemFromCart,
  updateCartItemQuantity,
} = cartSlice.actions;
export default cartSlice.reducer;
