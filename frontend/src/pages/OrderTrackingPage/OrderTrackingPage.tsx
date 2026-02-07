import React, { useCallback, useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import toast from 'react-hot-toast';
import { Header } from '../../components/common/Header';
import { Footer } from '../../components/common/Footer';
import { CartSidebar } from '../../components/common/CartSidebar';
import { api } from '../../services/api';
import { useOrderSignalR } from '../../hooks/useOrderSignalR';
import type { OrderDetail } from '../../types';

const happyPathStatuses = ['Paid', 'Accepted', 'Preparing', 'Ready', 'Delivering', 'Delivered', 'Completed'];

const statusColors: Record<string, string> = {
  New: 'bg-gray-100 text-gray-700',
  PaymentPending: 'bg-yellow-100 text-yellow-700',
  Paid: 'bg-blue-100 text-blue-700',
  Accepted: 'bg-blue-100 text-blue-700',
  Preparing: 'bg-orange-100 text-orange-700',
  Ready: 'bg-teal-100 text-teal-700',
  Delivering: 'bg-indigo-100 text-indigo-700',
  Delivered: 'bg-green-100 text-green-700',
  Completed: 'bg-green-100 text-green-700',
  Cancelled: 'bg-red-100 text-red-700',
  Failed: 'bg-red-100 text-red-700',
};

function StatusTracker({ status }: { status: string }) {
  const isTerminal = status === 'Cancelled' || status === 'Failed';
  const isPrePayment = status === 'New' || status === 'PaymentPending';

  if (isTerminal) {
    return (
      <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
        <h2 className="text-lg font-semibold text-gray-900 mb-4">Order Status</h2>
        <div className="flex items-center gap-3 p-4 bg-red-50 rounded-lg">
          <div className="w-10 h-10 bg-red-500 rounded-full flex items-center justify-center">
            <svg className="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </div>
          <div>
            <p className="font-semibold text-red-700">{status}</p>
            <p className="text-sm text-red-600">
              {status === 'Cancelled' ? 'This order has been cancelled.' : 'This order has failed.'}
            </p>
          </div>
        </div>
      </div>
    );
  }

  if (isPrePayment) {
    return (
      <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
        <h2 className="text-lg font-semibold text-gray-900 mb-4">Order Status</h2>
        <div className="flex items-center gap-3 p-4 bg-yellow-50 rounded-lg">
          <div className="w-10 h-10 bg-yellow-500 rounded-full flex items-center justify-center">
            <svg className="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
          </div>
          <div>
            <p className="font-semibold text-yellow-700">{status === 'New' ? 'Order Created' : 'Payment Pending'}</p>
            <p className="text-sm text-yellow-600">
              {status === 'New' ? 'Your order has been created and is awaiting payment.' : 'Payment is being processed.'}
            </p>
          </div>
        </div>
      </div>
    );
  }

  const currentIndex = happyPathStatuses.indexOf(status);

  return (
    <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
      <h2 className="text-lg font-semibold text-gray-900 mb-6">Order Status</h2>
      <div className="flex items-center justify-between">
        {happyPathStatuses.map((step, index) => {
          const isCompleted = index <= currentIndex;
          const isCurrent = index === currentIndex;
          const isLast = index === happyPathStatuses.length - 1;

          return (
            <React.Fragment key={step}>
              <div className="flex flex-col items-center relative">
                <div
                  className={`w-8 h-8 rounded-full flex items-center justify-center text-xs font-bold transition-all ${
                    isCurrent
                      ? 'bg-[#0066CC] text-white ring-4 ring-blue-100'
                      : isCompleted
                        ? 'bg-green-500 text-white'
                        : 'bg-gray-200 text-gray-500'
                  }`}
                >
                  {isCompleted && !isCurrent ? (
                    <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M5 13l4 4L19 7" />
                    </svg>
                  ) : (
                    index + 1
                  )}
                </div>
                <span className={`text-xs mt-2 font-medium whitespace-nowrap ${isCurrent ? 'text-[#0066CC]' : isCompleted ? 'text-green-600' : 'text-gray-400'}`}>
                  {step}
                </span>
              </div>
              {!isLast && (
                <div className={`flex-1 h-0.5 mx-1 ${index < currentIndex ? 'bg-green-500' : 'bg-gray-200'}`} />
              )}
            </React.Fragment>
          );
        })}
      </div>
    </div>
  );
}

export const OrderTrackingPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [order, setOrder] = useState<OrderDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchOrder = async () => {
      if (!id) return;
      try {
        const data = await api.getMyOrderById(Number(id));
        setOrder(data);
      } catch {
        setError('Failed to load order details.');
      } finally {
        setLoading(false);
      }
    };
    fetchOrder();
  }, [id]);

  const handleStatusUpdate = useCallback((update: { status: string; updatedAt: string }) => {
    setOrder(prev => prev ? { ...prev, status: update.status, updatedAt: update.updatedAt } : prev);
    toast.success(`Order status updated to ${update.status}`);
  }, []);

  useOrderSignalR(id ? Number(id) : undefined, handleStatusUpdate);

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <Header />

      <main className="flex-grow">
        <div className="bg-white border-b border-gray-100">
          <div className="max-w-4xl mx-auto px-4 py-6">
            <Link to="/orders" className="inline-flex items-center gap-1 text-sm text-[#0066CC] hover:text-[#004C99] font-medium mb-3">
              <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15 19l-7-7 7-7" />
              </svg>
              Back to Orders
            </Link>
            <h1 className="text-2xl font-bold text-gray-900">
              Order #{id}
            </h1>
            {order && (
              <div className="flex items-center gap-3 mt-2">
                <span className={`px-2.5 py-0.5 rounded-full text-xs font-semibold ${statusColors[order.status] || 'bg-gray-100 text-gray-700'}`}>
                  {order.status}
                </span>
                <span className="text-gray-500 text-sm">
                  {new Date(order.orderDate).toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' })}
                </span>
              </div>
            )}
          </div>
        </div>

        <div className="max-w-4xl mx-auto px-4 py-8">
          {loading && (
            <div className="flex items-center justify-center py-16">
              <svg className="animate-spin h-8 w-8 text-[#0066CC]" viewBox="0 0 24 24">
                <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" fill="none" />
                <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
              </svg>
            </div>
          )}

          {error && (
            <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
              {error}
            </div>
          )}

          {!loading && !error && order && (
            <div className="space-y-6">
              {/* Status Tracker */}
              <StatusTracker status={order.status} />

              {/* Order Items */}
              <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
                <h2 className="text-lg font-semibold text-gray-900 mb-4">
                  Items ({order.itemCount})
                </h2>
                <div className="divide-y divide-gray-100">
                  {order.items.map((item, index) => (
                    <div key={index} className="py-4 first:pt-0 last:pb-0">
                      <div className="flex items-start justify-between">
                        <div className="flex-1">
                          <p className="font-medium text-gray-900">{item.productName}</p>
                          <div className="flex items-center gap-3 mt-1 text-sm text-gray-500">
                            <span>{item.quantity} x ${item.unitPrice.toFixed(2)}</span>
                            {item.size && (
                              <span className="px-2 py-0.5 bg-gray-100 rounded text-xs font-medium text-gray-600">
                                {item.size}
                              </span>
                            )}
                          </div>
                          {item.modifiers.length > 0 && (
                            <div className="mt-1.5">
                              {item.modifiers.map((mod, modIdx) => (
                                <span key={modIdx} className="text-xs text-gray-500">
                                  {modIdx > 0 && ', '}
                                  + {mod.toppingName} (${mod.price.toFixed(2)})
                                </span>
                              ))}
                            </div>
                          )}
                        </div>
                        <span className="font-semibold text-gray-900 ml-4">
                          ${item.totalPrice.toFixed(2)}
                        </span>
                      </div>
                    </div>
                  ))}
                </div>
              </div>

              {/* Payment Summary */}
              <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
                <h2 className="text-lg font-semibold text-gray-900 mb-4">Payment Summary</h2>
                <div className="space-y-2">
                  <div className="flex justify-between text-sm text-gray-600">
                    <span>Subtotal</span>
                    <span>${order.items.reduce((sum, i) => sum + i.totalPrice, 0).toFixed(2)}</span>
                  </div>
                  <div className="flex justify-between text-lg font-bold text-gray-900 pt-2 border-t border-gray-100">
                    <span>Total</span>
                    <span>${order.totalPrice.toFixed(2)}</span>
                  </div>
                </div>
              </div>
            </div>
          )}
        </div>
      </main>

      <Footer />
      <CartSidebar />
    </div>
  );
};
