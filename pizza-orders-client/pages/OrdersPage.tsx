import React, { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import { apiService } from '../services/apiService';
import { Order } from '../types';

const OrderItem: React.FC<{ order: Order }> = ({ order }) => {
    const getStatusColor = (status: Order['status']) => {
        switch (status) {
            case 'Delivered': return 'bg-green-100 text-green-800';
            case 'Out for Delivery': return 'bg-blue-100 text-blue-800';
            case 'Preparing': return 'bg-yellow-100 text-yellow-800';
            case 'Received': return 'bg-gray-100 text-gray-800';
            default: return 'bg-gray-100 text-gray-800';
        }
    };
    return (
        <div className="bg-white shadow-md rounded-lg p-6 mb-6">
            <div className="flex flex-wrap justify-between items-center border-b pb-4 mb-4">
                <div>
                    <p className="font-bold text-secondary">Order ID: #{order.id}</p>
                    <p className="text-sm text-gray-500">Date: {new Date(order.date).toLocaleDateString()}</p>
                </div>
                <div>
                    <p className="font-bold text-secondary text-right">Total: ${order.total.toFixed(2)}</p>
                    <span className={`text-sm font-medium mr-2 px-2.5 py-0.5 rounded ${getStatusColor(order.status)}`}>
                        {order.status}
                    </span>
                </div>
            </div>
            <div>
                <h4 className="font-semibold text-secondary mb-2">Items:</h4>
                <ul className="space-y-2">
                    {order.items.map((item, index) => (
                        <li key={index} className="flex justify-between items-center text-sm text-gray-700">
                            <span>{item.quantity} x {item.product.name}</span>
                            <span>${(item.quantity * item.product.price).toFixed(2)}</span>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
};


const OrdersPage: React.FC = () => {
    const [orders, setOrders] = useState<Order[]>([]);
    const [loading, setLoading] = useState(true);
    const { user } = useAuth();

    useEffect(() => {
        const fetchOrders = async () => {
            if (user) {
                setLoading(true);
                try {
                    const userOrders = await apiService.getUserOrders(user.id);
                    setOrders(userOrders.sort((a,b) => new Date(b.date).getTime() - new Date(a.date).getTime()));
                } catch (error) {
                    console.error("Failed to fetch orders:", error);
                } finally {
                    setLoading(false);
                }
            }
        };
        fetchOrders();
    }, [user]);

    return (
        <div className="bg-gray-100 min-h-screen py-12">
            <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
                <h1 className="text-4xl font-extrabold text-secondary text-center mb-8">My Orders</h1>
                {loading ? (
                    <div className="text-center">
                         <div className="animate-spin rounded-full h-16 w-16 border-t-2 border-b-2 border-primary mx-auto"></div>
                    </div>
                ) : orders.length > 0 ? (
                    <div>
                        {orders.map(order => <OrderItem key={order.id} order={order} />)}
                    </div>
                ) : (
                    <div className="text-center bg-white p-8 rounded-lg shadow-md">
                        <h2 className="text-xl font-semibold text-secondary">No orders yet!</h2>
                        <p className="text-gray-600 mt-2">Looks like you haven't placed any orders. Check out our menu!</p>
                    </div>
                )}
            </div>
        </div>
    );
};

export default OrdersPage;