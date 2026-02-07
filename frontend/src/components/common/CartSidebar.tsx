import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';
import { useReduxCart } from '../../hooks/useReduxCart';
import { useConfirm } from '../../hooks/useConfirm';
import { ConfirmModal } from './ConfirmModal';
import { PaymentModal } from './PaymentModal';
import { api } from '../../services/api';

export const CartSidebar: React.FC = () => {
  const navigate = useNavigate();
  const { cart, isCartOpen, closeCart, removeFromCart, clearCart } = useReduxCart();
  const { confirm, isOpen, options, handleConfirm, handleCancel } = useConfirm();
  const [isPaymentOpen, setIsPaymentOpen] = useState(false);

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

  const handleCheckout = () => {
    setIsPaymentOpen(true);
  };

  const handlePaymentSuccess = async () => {
    try {
      if (cart) {
        await api.checkout(cart.sessionId);
        await clearCart();
        toast.success('Order placed successfully!', {
          duration: 5000,
        });
      }
      setIsPaymentOpen(false);
      closeCart();
    } catch (err: any) {
      toast.error(err.response?.data || 'Checkout failed. Please try again.');
    }
  };

  const handleViewCart = () => {
    closeCart();
    navigate('/cart');
  };

  const subtotal = cart?.totalPrice || 0;
  const deliveryFee = subtotal > 0 ? 3.99 : 0;
  const total = subtotal + deliveryFee;

  return (
    <>
      {/* Backdrop */}
      {isCartOpen && (
        <div
          className="fixed inset-0 bg-black/50 z-40 transition-opacity"
          onClick={closeCart}
        />
      )}

      {/* Sidebar */}
      <div
        className={`fixed inset-y-0 right-0 w-full sm:w-96 bg-white shadow-2xl z-50 transform transition-transform duration-300 ease-in-out ${
          isCartOpen ? 'translate-x-0' : 'translate-x-full'
        }`}
      >
        <div className="flex flex-col h-full">
          {/* Header */}
          <div className="px-6 py-4 border-b border-gray-100 flex justify-between items-center bg-white">
            <div className="flex items-center gap-2">
              <svg className="w-6 h-6 text-dominos-red" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z"></path>
              </svg>
              <h2 className="text-xl font-bold text-gray-900">Your Cart</h2>
            </div>
            <button
              onClick={closeCart}
              className="p-2 hover:bg-gray-100 rounded-full transition-colors"
            >
              <svg className="w-5 h-5 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M6 18L18 6M6 6l12 12"></path>
              </svg>
            </button>
          </div>

          {/* Cart Items */}
          <div className="flex-1 overflow-y-auto p-6">
            {!cart || cart.items.length === 0 ? (
              <div className="h-full flex flex-col items-center justify-center text-center">
                <div className="w-20 h-20 bg-gray-100 rounded-full flex items-center justify-center mb-4">
                  <svg className="w-10 h-10 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z"></path>
                  </svg>
                </div>
                <h3 className="font-semibold text-gray-900 mb-1">Your cart is empty</h3>
                <p className="text-gray-500 text-sm">
                  Add some delicious items to get started!
                </p>
              </div>
            ) : (
              <div className="space-y-4">
                {cart.items.map((item, index) => (
                  <div
                    key={`${item.productId}-${index}`}
                    className="flex gap-4 p-3 bg-gray-50 rounded-xl"
                  >
                    {/* Item Image Placeholder */}
                    <div className="w-16 h-16 rounded-lg bg-gray-200 flex items-center justify-center flex-shrink-0">
                      <span className="text-2xl">🍕</span>
                    </div>

                    {/* Item Details */}
                    <div className="flex-1 min-w-0">
                      <div className="flex justify-between items-start">
                        <h4 className="font-semibold text-gray-900 truncate pr-2">
                          {item.productName}
                        </h4>
                        <button
                          onClick={() => handleRemove(item.productId, item.toppingIds)}
                          className="text-gray-400 hover:text-dominos-red transition-colors"
                        >
                          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M6 18L18 6M6 6l12 12"></path>
                          </svg>
                        </button>
                      </div>

                      {item.toppings && item.toppings.length > 0 && (
                        <p className="text-xs text-gray-500 mt-0.5 truncate">
                          + {item.toppings.map((t) => t.toppingName).join(', ')}
                        </p>
                      )}

                      <div className="flex justify-between items-center mt-2">
                        <span className="text-xs text-gray-500">Qty: {item.quantity}</span>
                        <span className="font-bold text-dominos-blue">
                          ${item.totalPrice.toFixed(2)}
                        </span>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>

          {/* Footer */}
          {cart && cart.items.length > 0 && (
            <div className="p-6 border-t border-gray-100 bg-white">
              {/* Price Summary */}
              <div className="space-y-2 mb-4">
                <div className="flex justify-between text-sm text-gray-600">
                  <span>Subtotal</span>
                  <span>${subtotal.toFixed(2)}</span>
                </div>
                <div className="flex justify-between text-sm text-gray-600">
                  <span>Delivery</span>
                  <span>${deliveryFee.toFixed(2)}</span>
                </div>
                <div className="flex justify-between text-lg font-bold text-gray-900 pt-2 border-t border-gray-100">
                  <span>Total</span>
                  <span>${total.toFixed(2)}</span>
                </div>
              </div>

              {/* Actions */}
              <div className="space-y-2">
                <button
                  onClick={handleCheckout}
                  className="w-full py-3 bg-dominos-red text-white rounded-lg font-semibold hover:bg-dominos-red-dark transition-colors"
                >
                  Checkout
                </button>
                <button
                  onClick={handleViewCart}
                  className="w-full py-3 bg-gray-100 text-gray-700 rounded-lg font-semibold hover:bg-gray-200 transition-colors"
                >
                  View Cart
                </button>
              </div>
            </div>
          )}
        </div>
      </div>

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

      {isPaymentOpen && cart && (
        <PaymentModal
          total={total}
          onClose={() => setIsPaymentOpen(false)}
          onSuccess={handlePaymentSuccess}
        />
      )}
    </>
  );
};
