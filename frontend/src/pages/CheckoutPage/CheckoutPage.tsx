import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Layout } from '../../components/common/Layout';
import { useCart } from '../../hooks/useCart';
import { api } from '../../services/api';

export const CheckoutPage: React.FC = () => {
  const { cart, clearCart } = useCart();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleCheckout = async () => {
    if (!cart || cart.items.length === 0) {
      alert('Cart is empty');
      navigate('/cart');
      return;
    }

    try {
      setLoading(true);
      setError('');

      const order = await api.checkout(cart.sessionId);

      await clearCart();

      alert(`Order placed successfully! Order ID: ${order.orderId}`);
      navigate('/');
    } catch (err: any) {
      setError(err.response?.data || 'Checkout failed. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  if (!cart || cart.items.length === 0) {
    return (
      <Layout>
        <div className="text-center py-12">
          <h1 className="text-3xl font-bold mb-4">Your cart is empty</h1>
          <button
            onClick={() => navigate('/')}
            className="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-6 rounded"
          >
            Continue Shopping
          </button>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="max-w-3xl mx-auto">
        <h1 className="text-4xl font-bold mb-6">Checkout</h1>

        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        <div className="bg-white rounded-lg shadow p-6 mb-6">
          <h2 className="text-2xl font-bold mb-4">Order Summary</h2>
          <div className="space-y-3">
            {cart.items.map((item, index) => (
              <div key={index} className="flex justify-between border-b pb-2">
                <div>
                  <div className="font-semibold">{item.productName}</div>
                  <div className="text-sm text-gray-600">
                    Quantity: {item.quantity} × ${item.basePrice.toFixed(2)}
                  </div>
                  {item.toppings && item.toppings.length > 0 && (
                    <div className="text-sm text-gray-500">
                      + {item.toppings.map((t) => t.toppingName).join(', ')}
                    </div>
                  )}
                </div>
                <div className="font-bold">${item.totalPrice.toFixed(2)}</div>
              </div>
            ))}
          </div>
          <div className="border-t mt-4 pt-4 flex justify-between text-2xl font-bold">
            <span>Total:</span>
            <span>${cart.totalPrice.toFixed(2)}</span>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6 mb-6">
          <h2 className="text-2xl font-bold mb-4">Payment Information</h2>
          <p className="text-gray-600 mb-4">
            This is a demo application. Payment is automatically approved.
          </p>
        </div>

        <div className="flex gap-4">
          <button
            onClick={() => navigate('/cart')}
            className="flex-1 bg-gray-300 hover:bg-gray-400 text-gray-800 font-bold py-3 px-6 rounded"
          >
            Back to Cart
          </button>
          <button
            onClick={handleCheckout}
            disabled={loading}
            className="flex-1 bg-green-500 hover:bg-green-600 text-white font-bold py-3 px-6 rounded disabled:opacity-50"
          >
            {loading ? 'Processing...' : 'Place Order'}
          </button>
        </div>
      </div>
    </Layout>
  );
};
