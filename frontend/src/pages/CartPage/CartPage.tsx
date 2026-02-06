import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';
import { Header } from '../../components/common/Header';
import { Footer } from '../../components/common/Footer';
import { CartSidebar } from '../../components/common/CartSidebar';
import { ConfirmModal } from '../../components/common/ConfirmModal';
import { useCart } from '../../hooks/useCart';
import { useConfirm } from '../../hooks/useConfirm';

export const CartPage: React.FC = () => {
  const { cart, loading, updateCart, removeFromCart, clearCart } = useCart();
  const navigate = useNavigate();
  const { confirm, isOpen, options, handleConfirm, handleCancel } = useConfirm();

  const handleUpdateQuantity = async (
    productId: number,
    newQuantity: number,
    toppingIds: number[]
  ) => {
    try {
      await updateCart({ productId, quantity: newQuantity, toppingIds });
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Failed to update cart');
    }
  };

  const handleRemove = async (productId: number, toppingIds: number[]) => {
    const confirmed = await confirm({
      title: 'Remove Item',
      message: 'Are you sure you want to remove this item from your cart?',
      confirmText: 'Remove',
      cancelText: 'Cancel',
      confirmButtonClass: 'bg-dominos-red hover:bg-dominos-red-dark',
    });

    if (!confirmed) return;

    try {
      await removeFromCart(productId, toppingIds);
      toast.success('Item removed from cart');
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Failed to remove item');
    }
  };

  const handleClearCart = async () => {
    const confirmed = await confirm({
      title: 'Clear Cart',
      message: 'Are you sure you want to clear your cart? All items will be removed.',
      confirmText: 'Clear Cart',
      cancelText: 'Cancel',
      confirmButtonClass: 'bg-dominos-red hover:bg-dominos-red-dark',
    });

    if (!confirmed) return;

    try {
      await clearCart();
      toast.success('Cart cleared');
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Failed to clear cart');
    }
  };

  const handleCheckout = () => {
    navigate('/checkout');
  };

  const subtotal = cart?.totalPrice || 0;
  const deliveryFee = subtotal > 0 ? 3.99 : 0;
  const total = subtotal + deliveryFee;

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 flex flex-col">
        <Header />
        <div className="flex-grow flex items-center justify-center">
          <div className="text-center">
            <div className="w-12 h-12 border-4 border-dominos-blue border-t-transparent rounded-full animate-spin mx-auto mb-4"></div>
            <p className="text-gray-500">Loading cart...</p>
          </div>
        </div>
        <Footer />
        <CartSidebar />
      </div>
    );
  }

  if (!cart || cart.items.length === 0) {
    return (
      <div className="min-h-screen bg-gray-50 flex flex-col">
        <Header />
        <div className="flex-grow flex items-center justify-center">
          <div className="text-center">
            <div className="w-24 h-24 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-6">
              <svg className="w-12 h-12 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z"></path>
              </svg>
            </div>
            <h1 className="text-2xl font-bold text-gray-900 mb-2">Your Cart is Empty</h1>
            <p className="text-gray-500 mb-6">Add some delicious items to your cart!</p>
            <Link
              to="/"
              className="inline-flex items-center px-6 py-3 bg-dominos-blue text-white rounded-lg font-semibold hover:bg-dominos-blue-dark transition-colors"
            >
              Browse Menu
            </Link>
          </div>
        </div>
        <Footer />
        <CartSidebar />
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <Header />

      <main className="flex-grow">
        {/* Page Header */}
        <div className="bg-white border-b border-gray-100">
          <div className="max-w-7xl mx-auto px-4 py-6">
            <div className="flex items-center justify-between">
              <div>
                <h1 className="text-2xl font-bold text-gray-900">Shopping Cart</h1>
                <p className="text-gray-500 text-sm mt-1">
                  {cart.items.reduce((sum, item) => sum + item.quantity, 0)} items in your cart
                </p>
              </div>
              <button
                onClick={handleClearCart}
                className="px-4 py-2 text-sm font-medium text-dominos-red hover:bg-red-50 rounded-lg transition-colors"
              >
                Clear Cart
              </button>
            </div>
          </div>
        </div>

        {/* Cart Content */}
        <div className="max-w-7xl mx-auto px-4 py-8">
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            {/* Cart Items */}
            <div className="lg:col-span-2 space-y-4">
              {cart.items.map((item, index) => (
                <div
                  key={`${item.productId}-${index}`}
                  className="bg-white rounded-xl p-4 shadow-sm border border-gray-100"
                >
                  <div className="flex gap-4">
                    {/* Item Image */}
                    <div className="w-24 h-24 bg-gray-100 rounded-lg flex items-center justify-center flex-shrink-0">
                      <span className="text-4xl">🍕</span>
                    </div>

                    {/* Item Details */}
                    <div className="flex-grow">
                      <div className="flex justify-between items-start">
                        <div>
                          <h3 className="font-semibold text-gray-900">{item.productName}</h3>
                          {item.toppings && item.toppings.length > 0 && (
                            <p className="text-sm text-gray-500 mt-0.5">
                              + {item.toppings.map((t) => t.toppingName).join(', ')}
                            </p>
                          )}
                        </div>
                        <button
                          onClick={() => handleRemove(item.productId, item.toppingIds)}
                          className="text-gray-400 hover:text-dominos-red transition-colors"
                        >
                          <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"></path>
                          </svg>
                        </button>
                      </div>

                      <div className="flex justify-between items-center mt-4">
                        {/* Quantity Controls */}
                        <div className="flex items-center gap-2 bg-gray-100 rounded-lg p-1">
                          <button
                            onClick={() => handleUpdateQuantity(item.productId, Math.max(1, item.quantity - 1), item.toppingIds)}
                            className="w-8 h-8 flex items-center justify-center bg-white rounded-md shadow-sm hover:bg-gray-50 font-bold text-gray-600"
                          >
                            -
                          </button>
                          <span className="w-8 text-center font-semibold">{item.quantity}</span>
                          <button
                            onClick={() => handleUpdateQuantity(item.productId, item.quantity + 1, item.toppingIds)}
                            className="w-8 h-8 flex items-center justify-center bg-white rounded-md shadow-sm hover:bg-gray-50 font-bold text-gray-600"
                          >
                            +
                          </button>
                        </div>

                        {/* Price */}
                        <span className="text-lg font-bold text-dominos-blue">
                          ${item.totalPrice.toFixed(2)}
                        </span>
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>

            {/* Order Summary */}
            <div className="lg:col-span-1">
              <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6 sticky top-24">
                <h2 className="text-lg font-bold text-gray-900 mb-4">Order Summary</h2>

                <div className="space-y-3 mb-4">
                  <div className="flex justify-between text-sm text-gray-600">
                    <span>Subtotal</span>
                    <span>${subtotal.toFixed(2)}</span>
                  </div>
                  <div className="flex justify-between text-sm text-gray-600">
                    <span>Delivery Fee</span>
                    <span>${deliveryFee.toFixed(2)}</span>
                  </div>
                  <div className="border-t border-gray-100 pt-3">
                    <div className="flex justify-between text-lg font-bold text-gray-900">
                      <span>Total</span>
                      <span>${total.toFixed(2)}</span>
                    </div>
                  </div>
                </div>

                <button
                  onClick={handleCheckout}
                  className="w-full py-3 bg-dominos-red text-white rounded-lg font-semibold hover:bg-dominos-red-dark transition-colors mb-3"
                >
                  Proceed to Checkout
                </button>

                <Link
                  to="/"
                  className="block text-center py-3 text-dominos-blue font-medium hover:text-dominos-blue-dark transition-colors"
                >
                  Continue Shopping
                </Link>
              </div>
            </div>
          </div>
        </div>
      </main>

      <Footer />
      <CartSidebar />

      <ConfirmModal
        isOpen={isOpen}
        title={options.title}
        message={options.message}
        confirmText={options.confirmText}
        cancelText={options.cancelText}
        confirmButtonClass={options.confirmButtonClass}
        onConfirm={handleConfirm}
        onCancel={handleCancel}
      />
    </div>
  );
};
