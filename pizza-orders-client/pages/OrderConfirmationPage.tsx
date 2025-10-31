import React from 'react';
import { useParams, Link } from 'react-router-dom';
import { CheckCircleIcon } from '../components/Icons';

const OrderConfirmationPage: React.FC = () => {
    const { orderId } = useParams<{ orderId: string }>();

    return (
        <div className="bg-gray-100 min-h-screen flex items-center justify-center py-12">
            <div className="max-w-md mx-auto px-4 sm:px-6 lg:px-8 text-center">
                <div className="bg-white p-8 sm:p-12 rounded-2xl shadow-lg">
                    <CheckCircleIcon className="h-24 w-24 text-green-500 mx-auto" />
                    <h1 className="text-3xl sm:text-4xl font-extrabold text-secondary mt-6">Order Placed Successfully!</h1>
                    <p className="mt-4 text-lg text-gray-600">Thank you for your purchase. Your order is being processed.</p>
                    <div className="mt-8 bg-gray-50 p-4 rounded-lg border border-gray-200">
                        <p className="text-gray-700">Your Order ID is:</p>
                        <p className="text-2xl font-bold text-primary tracking-wider mt-1">#{orderId}</p>
                    </div>
                    <p className="mt-6 text-sm text-gray-500">You will receive an email confirmation shortly.</p>
                    <div className="mt-10">
                        <Link 
                            to="/menu" 
                            className="w-full inline-flex justify-center py-3 px-6 border border-transparent rounded-md shadow-sm text-base font-medium text-white bg-primary hover:bg-primary-dark focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary"
                        >
                            Continue Shopping
                        </Link>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default OrderConfirmationPage;
