import { useState, useEffect } from 'react';
import type { Cart, AddToCartRequest, UpdateCartRequest } from '../types';
import { api } from '../services/api';

const CART_SESSION_KEY = 'cart_session_id';

export const useCart = () => {
  const [cart, setCart] = useState<Cart | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const getOrCreateSessionId = (): string => {
    let sessionId = localStorage.getItem(CART_SESSION_KEY);
    if (!sessionId) {
      sessionId = crypto.randomUUID();
      localStorage.setItem(CART_SESSION_KEY, sessionId);
    }
    return sessionId;
  };

  const loadCart = async () => {
    try {
      setLoading(true);
      setError(null);
      const sessionId = getOrCreateSessionId();
      const cartData = await api.getCart(sessionId);
      setCart(cartData);
    } catch (err) {
      setError('Failed to load cart');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const addToCart = async (data: AddToCartRequest) => {
    try {
      setError(null);
      const sessionId = getOrCreateSessionId();
      await api.addToCart(sessionId, data);
      await loadCart();
    } catch (err) {
      setError('Failed to add item to cart');
      throw err;
    }
  };

  const updateCart = async (data: UpdateCartRequest) => {
    try {
      setError(null);
      const sessionId = getOrCreateSessionId();
      await api.updateCart(sessionId, data);
      await loadCart();
    } catch (err) {
      setError('Failed to update cart');
      throw err;
    }
  };

  const removeFromCart = async (productId: number, toppingIds?: number[]) => {
    try {
      setError(null);
      const sessionId = getOrCreateSessionId();
      await api.removeFromCart(sessionId, productId, toppingIds);
      await loadCart();
    } catch (err) {
      setError('Failed to remove item from cart');
      throw err;
    }
  };

  const clearCart = async () => {
    try {
      setError(null);
      const sessionId = getOrCreateSessionId();
      await api.clearCart(sessionId);
      setCart(null);
    } catch (err) {
      setError('Failed to clear cart');
      throw err;
    }
  };

  useEffect(() => {
    loadCart();
  }, []);

  return {
    cart,
    loading,
    error,
    addToCart,
    updateCart,
    removeFromCart,
    clearCart,
    refreshCart: loadCart,
  };
};
