import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';
import { Layout } from '../../components/common/Layout';
import { CartItem } from '../../components/cart/CartItem';
import { useCart } from '../../hooks/useCart';

export const CartPage: React.FC = () => {
  const { cart, loading, updateCart, removeFromCart, clearCart } = useCart();
  const navigate = useNavigate();

  const handleUpdateQuantity = async (
    productId: number,
    quantity: number,
    toppingIds: number[]
  ) => {
    try {
      await updateCart({ productId, quantity, toppingIds });
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Failed to update cart');
    }
  };

  const handleRemove = async (productId: number, toppingIds: number[]) => {
    try {
      await removeFromCart(productId, toppingIds);
      toast.success('Item removed from cart');
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Failed to remove item');
    }
  };

  const handleClearCart = async () => {
    if (window.confirm('Are you sure you want to clear your cart?')) {
      try {
        await clearCart();
        toast.success('Cart cleared');
      } catch (err: any) {
        toast.error(err.response?.data?.message || 'Failed to clear cart');
      }
    }
  };

  const handleCheckout = () => {
    navigate('/checkout');
  };

  if (loading) {
    return (
      <Layout>
        <div className="text-center py-12">
          <div className="text-xl">Loading cart...</div>
        </div>
      </Layout>
    );
  }

  if (!cart || cart.items.length === 0) {
    return (
      <Layout>
        <div className="text-center py-12">
          <h1 className="text-3xl font-bold mb-4">Your Cart is Empty</h1>
          <p className="text-gray-600 mb-6">Add some delicious items to your cart!</p>
          <Link
            to="/"
            className="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-6 rounded"
          >
            Browse Products
          </Link>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div>
        <div className="flex items-center justify-between mb-6">
          <h1 className="text-4xl font-bold">Shopping Cart</h1>
          <button
            onClick={handleClearCart}
            className="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded"
          >
            Clear Cart
          </button>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <div className="lg:col-span-2 space-y-4">
            {cart.items.map((item, index) => (
              <CartItem
                key={`${item.productId}-${index}`}
                item={item}
                onUpdateQuantity={handleUpdateQuantity}
                onRemove={handleRemove}
              />
            ))}
          </div>

          <div className="lg:col-span-1">
            <div className="bg-white rounded-lg shadow p-6 sticky top-4">
              <h2 className="text-2xl font-bold mb-4">Order Summary</h2>
              <div className="space-y-2 mb-4">
                <div className="flex justify-between text-gray-600">
                  <span>Items:</span>
                  <span>{cart.items.reduce((sum, item) => sum + item.quantity, 0)}</span>
                </div>
                <div className="border-t pt-2 mt-2">
                  <div className="flex justify-between text-xl font-bold">
                    <span>Total:</span>
                    <span>${cart.totalPrice.toFixed(2)}</span>
                  </div>
                </div>
              </div>
              <button
                onClick={handleCheckout}
                className="w-full bg-green-500 hover:bg-green-600 text-white font-bold py-3 px-6 rounded"
              >
                Proceed to Checkout
              </button>
            </div>
          </div>
        </div>
      </div>
    </Layout>
  );
};
