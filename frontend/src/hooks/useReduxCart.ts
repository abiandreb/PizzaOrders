import { useCallback, useEffect, useState } from 'react';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import {
  setCart,
  clearCart as clearCartAction,
  setCartOpen,
  setLoading,
} from '../store/slices/cartSlice';
import { api } from '../services/api';
import type { AddToCartRequest, UpdateCartRequest } from '../types';

export const useReduxCart = () => {
  const dispatch = useAppDispatch();
  const { cart, isCartOpen, loading } = useAppSelector((state) => state.cart);
  const [error, setError] = useState<string | null>(null);

  const getSessionId = useCallback(() => {
    const CART_SESSION_KEY = 'cart_session_id';
    let sessionId = localStorage.getItem(CART_SESSION_KEY);
    if (!sessionId) {
      sessionId = crypto.randomUUID();
      localStorage.setItem(CART_SESSION_KEY, sessionId);
    }
    return sessionId;
  }, []);

  const loadCart = useCallback(async () => {
    try {
      dispatch(setLoading(true));
      setError(null);
      const sessionId = getSessionId();
      const cartData = await api.getCart(sessionId);
      dispatch(setCart(cartData));
    } catch (err) {
      setError('Failed to load cart');
      console.error('Failed to load cart:', err);
    } finally {
      dispatch(setLoading(false));
    }
  }, [dispatch, getSessionId]);

  const addToCart = useCallback(
    async (request: AddToCartRequest) => {
      try {
        setError(null);
        const sessionId = getSessionId();
        await api.addToCart(sessionId, request);
        await loadCart();
      } catch (err) {
        setError('Failed to add item to cart');
        throw err;
      }
    },
    [getSessionId, loadCart]
  );

  const updateCart = useCallback(
    async (request: UpdateCartRequest) => {
      try {
        setError(null);
        const sessionId = getSessionId();
        await api.updateCart(sessionId, request);
        await loadCart();
      } catch (err) {
        setError('Failed to update cart');
        throw err;
      }
    },
    [getSessionId, loadCart]
  );

  const removeFromCart = useCallback(
    async (productId: number, toppingIds?: number[]) => {
      try {
        setError(null);
        const sessionId = getSessionId();
        await api.removeFromCart(sessionId, productId, toppingIds);
        await loadCart();
      } catch (err) {
        setError('Failed to remove item from cart');
        throw err;
      }
    },
    [getSessionId, loadCart]
  );

  const clearCart = useCallback(async () => {
    try {
      setError(null);
      const sessionId = getSessionId();
      await api.clearCart(sessionId);
      dispatch(clearCartAction());
    } catch (err) {
      setError('Failed to clear cart');
      throw err;
    }
  }, [dispatch, getSessionId]);

  const openCart = useCallback(() => {
    dispatch(setCartOpen(true));
  }, [dispatch]);

  const closeCart = useCallback(() => {
    dispatch(setCartOpen(false));
  }, [dispatch]);

  // Load cart on mount
  useEffect(() => {
    loadCart();
  }, [loadCart]);

  return {
    cart,
    isCartOpen,
    loading,
    error,
    addToCart,
    updateCart,
    removeFromCart,
    clearCart,
    openCart,
    closeCart,
    loadCart,
    refreshCart: loadCart, // alias for backward compatibility with useCart
  };
};
