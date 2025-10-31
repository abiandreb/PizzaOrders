import React, { useState } from 'react';
import { useCart } from '../context/CartContext';
import { useAuth } from '../context/AuthContext';
import { apiService } from '../services/apiService';
import { useNavigate } from 'react-router-dom';

const CheckoutPage: React.FC = () => {
    const { cart, cartTotal, cartCount, clearCart, discount, finalTotal } = useCart();
    const { user } = useAuth();
    const navigate = useNavigate();
    const [customerDetails, setCustomerDetails] = useState({
        name: user?.name || '',
        address: '',
        phone: '',
    });
    const [isPlacingOrder, setIsPlacingOrder] = useState(false);
    const [error, setError] = useState('');

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setCustomerDetails(prev => ({ ...prev, [name]: value }));
    };

    const handlePlaceOrder = async (e: React.FormEvent) => {
        e.preventDefault();
        if (cart.length === 0) {
            setError('Your cart is empty.');
            return;
        }
        setIsPlacingOrder(true);
        setError('');
        try {
            const newOrder = await apiService.placeOrder(cart, user, customerDetails);
            clearCart();
            navigate(`/order-confirmation/${newOrder.id}`);
        } catch (err) {
            setError('Failed to place order. Please try again.');
        } finally {
            setIsPlacingOrder(false);
        }
    };
    
    return (
        <div className="bg-gray-100 min-h-screen py-12">
            <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
                <h1 className="text-4xl font-extrabold text-secondary text-center mb-8">Checkout</h1>
                {cartCount === 0 ? (
                    <div className="text-center bg-white p-8 rounded-lg shadow-md">
                        <h2 className="text-xl font-semibold text-secondary">Your cart is empty.</h2>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                        {/* Order Summary */}
                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <h2 className="text-2xl font-bold text-secondary mb-4">Order Summary</h2>
                            <div className="space-y-4">
                                {cart.map(item => (
                                    <div key={item.product.id} className="flex justify-between items-center">
                                        <div>
                                            <p className="font-semibold">{item.product.name}</p>
                                            <p className="text-sm text-gray-500">Qty: {item.quantity}</p>
                                        </div>
                                        <p className="font-semibold">${(item.product.price * item.quantity).toFixed(2)}</p>
                                    </div>
                                ))}
                            </div>
                            <hr className="my-4" />
                            <div className="space-y-2">
                                <div className="flex justify-between text-gray-600">
                                    <span>Subtotal</span>
                                    <span>${cartTotal.toFixed(2)}</span>
                                </div>
                                {discount > 0 && (
                                <div className="flex justify-between text-green-600">
                                    <span>Promo Discount</span>
                                    <span>-${discount.toFixed(2)}</span>
                                </div>
                                )}
                                <div className="flex justify-between font-bold text-lg text-secondary">
                                    <span>Total</span>
                                    <span>${finalTotal.toFixed(2)}</span>
                                </div>
                            </div>
                        </div>

                        {/* Delivery & Payment Form */}
                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <h2 className="text-2xl font-bold text-secondary mb-4">Delivery Details</h2>
                            <form onSubmit={handlePlaceOrder} className="space-y-4">
                                <div>
                                    <label htmlFor="name" className="block text-sm font-medium text-gray-700">Full Name</label>
                                    <input type="text" name="name" id="name" value={customerDetails.name} onChange={handleInputChange} required className="mt-1 block w-full px-3 py-2 bg-white border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-primary focus:border-primary" />
                                </div>
                                <div>
                                    <label htmlFor="address" className="block text-sm font-medium text-gray-700">Address</label>
                                    <input type="text" name="address" id="address" value={customerDetails.address} onChange={handleInputChange} required className="mt-1 block w-full px-3 py-2 bg-white border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-primary focus:border-primary" />
                                </div>
                                <div>
                                    <label htmlFor="phone" className="block text-sm font-medium text-gray-700">Phone Number</label>
                                    <input type="tel" name="phone" id="phone" value={customerDetails.phone} onChange={handleInputChange} required className="mt-1 block w-full px-3 py-2 bg-white border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-primary focus:border-primary" />
                                </div>
                                {error && <p className="text-red-500 text-sm">{error}</p>}
                                <button type="submit" disabled={isPlacingOrder} className="w-full bg-primary text-white font-bold py-3 px-4 rounded-lg hover:bg-primary-dark transition-colors disabled:bg-gray-400">
                                    {isPlacingOrder ? 'Placing Order...' : `Place Order ($${finalTotal.toFixed(2)})`}
                                </button>
                            </form>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
};

export default CheckoutPage;