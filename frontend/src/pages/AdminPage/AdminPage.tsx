import React, { useState, useEffect } from 'react';
import toast from 'react-hot-toast';
import { Layout } from '../../components/common/Layout';
import { ConfirmModal } from '../../components/common/ConfirmModal';
import { useConfirm } from '../../hooks/useConfirm';
import type { Product, Topping, OrderAdminDto } from '../../types';
import { ProductType } from '../../types';
import { api } from '../../services/api';

export const AdminPage: React.FC = () => {
  const [activeTab, setActiveTab] = useState<'products' | 'toppings' | 'orders'>('products');
  const [products, setProducts] = useState<Product[]>([]);
  const [toppings, setToppings] = useState<Topping[]>([]);
  const [orders, setOrders] = useState<OrderAdminDto[]>([]);
  const [orderStatuses, setOrderStatuses] = useState<string[]>([]);
  const [statusFilter, setStatusFilter] = useState<string>('');
  const [loading, setLoading] = useState(false);
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  const [editingTopping, setEditingTopping] = useState<Topping | null>(null);
  const { confirm, isOpen, options, handleConfirm, handleCancel } = useConfirm();

  useEffect(() => {
    loadData();
  }, [activeTab, statusFilter]);

  useEffect(() => {
    // Load order statuses on mount
    api.getAvailableOrderStatuses()
      .then(setOrderStatuses)
      .catch(() => {});
  }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      if (activeTab === 'products') {
        const data = await api.getAllProductsForAdmin();
        setProducts(data);
      } else if (activeTab === 'toppings') {
        const data = await api.getAllToppings();
        setToppings(data);
      } else if (activeTab === 'orders') {
        const data = await api.getAllOrdersForAdmin(statusFilter || undefined);
        setOrders(data);
      }
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Failed to load data');
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteProduct = async (id: number) => {
    const confirmed = await confirm({
      title: 'Delete Product',
      message: 'Are you sure you want to delete this product? This action cannot be undone.',
      confirmText: 'Delete',
      cancelText: 'Cancel',
    });

    if (!confirmed) return;

    try {
      await api.deleteProduct(id);
      toast.success('Product deleted successfully');
      await loadData();
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Failed to delete product');
    }
  };

  const handleDeleteTopping = async (id: number) => {
    const confirmed = await confirm({
      title: 'Delete Topping',
      message: 'Are you sure you want to delete this topping? This action cannot be undone.',
      confirmText: 'Delete',
      cancelText: 'Cancel',
    });

    if (!confirmed) return;

    try {
      await api.deleteTopping(id);
      toast.success('Topping deleted successfully');
      await loadData();
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Failed to delete topping');
    }
  };

  const handleSaveProduct = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const formData = new FormData(e.currentTarget);

    const productData = {
      name: formData.get('name') as string,
      description: formData.get('description') as string,
      basePrice: parseFloat(formData.get('basePrice') as string),
      hasToppings: formData.get('hasToppings') === 'true',
      productType: parseInt(formData.get('productType') as string) as ProductType,
      imageUrl: formData.get('imageUrl') as string || undefined,
    };

    try {
      if (editingProduct) {
        await api.updateProduct({ ...productData, id: editingProduct.id });
        toast.success('Product updated successfully');
      } else {
        await api.createProduct(productData);
        toast.success('Product created successfully');
      }
      setEditingProduct(null);
      await loadData();
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Failed to save product');
    }
  };

  const handleSaveTopping = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const formData = new FormData(e.currentTarget);

    const toppingData = {
      name: formData.get('name') as string,
      description: formData.get('description') as string,
      stock: parseInt(formData.get('stock') as string),
      price: parseFloat(formData.get('price') as string),
    };

    try {
      if (editingTopping) {
        await api.updateTopping({ ...toppingData, id: editingTopping.id });
        toast.success('Topping updated successfully');
      } else {
        await api.createTopping(toppingData);
        toast.success('Topping created successfully');
      }
      setEditingTopping(null);
      await loadData();
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Failed to save topping');
    }
  };

  const getProductTypeLabel = (type: ProductType) => {
    switch (type) {
      case ProductType.Pizza: return 'Pizza';
      case ProductType.Drink: return 'Drink';
      case ProductType.Dessert: return 'Dessert';
      default: return 'Unknown';
    }
  };

  const getProductTypeIcon = (type: ProductType) => {
    switch (type) {
      case ProductType.Pizza: return '🍕';
      case ProductType.Drink: return '🥤';
      case ProductType.Dessert: return '🍰';
      default: return '📦';
    }
  };

  const handleUpdateOrderStatus = async (orderId: number, newStatus: string) => {
    try {
      await api.updateOrderStatus(orderId, { status: newStatus });
      toast.success(`Order #${orderId} status updated to ${newStatus}`);
      await loadData();
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Failed to update order status');
    }
  };

  const getStatusColor = (status: string) => {
    switch (status.toLowerCase()) {
      case 'new': return 'bg-blue-100 text-blue-700';
      case 'paymentpending': return 'bg-yellow-100 text-yellow-700';
      case 'paid': return 'bg-green-100 text-green-700';
      case 'accepted': return 'bg-indigo-100 text-indigo-700';
      case 'preparing': return 'bg-orange-100 text-orange-700';
      case 'ready': return 'bg-teal-100 text-teal-700';
      case 'delivering': return 'bg-purple-100 text-purple-700';
      case 'delivered': return 'bg-emerald-100 text-emerald-700';
      case 'completed': return 'bg-green-100 text-green-700';
      case 'cancelled': return 'bg-red-100 text-red-700';
      case 'failed': return 'bg-red-100 text-red-700';
      default: return 'bg-gray-100 text-gray-700';
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleString();
  };

  return (
    <Layout>
      <div className="max-w-6xl mx-auto px-4 py-8">
        {/* Page Header */}
        <div className="mb-8">
          <div className="flex items-center gap-3 mb-2">
            <div className="w-10 h-10 bg-[#0066CC] rounded-lg flex items-center justify-center">
              <svg className="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z"></path>
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"></path>
              </svg>
            </div>
            <div>
              <h1 className="text-2xl font-bold text-gray-900">Admin Panel</h1>
              <p className="text-sm text-gray-500">Manage products, toppings, and orders</p>
            </div>
          </div>
        </div>

        {/* Tab Navigation */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 mb-6">
          <div className="flex border-b border-gray-200">
            <button
              onClick={() => setActiveTab('products')}
              className={`flex-1 px-6 py-4 text-sm font-semibold transition-colors ${
                activeTab === 'products'
                  ? 'text-[#0066CC] border-b-2 border-[#0066CC] bg-blue-50/50'
                  : 'text-gray-600 hover:text-gray-900 hover:bg-gray-50'
              }`}
            >
              <div className="flex items-center justify-center gap-2">
                <span>🍕</span>
                Products
                <span className="bg-gray-100 text-gray-600 text-xs px-2 py-0.5 rounded-full">
                  {products.length}
                </span>
              </div>
            </button>
            <button
              onClick={() => setActiveTab('toppings')}
              className={`flex-1 px-6 py-4 text-sm font-semibold transition-colors ${
                activeTab === 'toppings'
                  ? 'text-[#0066CC] border-b-2 border-[#0066CC] bg-blue-50/50'
                  : 'text-gray-600 hover:text-gray-900 hover:bg-gray-50'
              }`}
            >
              <div className="flex items-center justify-center gap-2">
                <span>🧀</span>
                Toppings
                <span className="bg-gray-100 text-gray-600 text-xs px-2 py-0.5 rounded-full">
                  {toppings.length}
                </span>
              </div>
            </button>
            <button
              onClick={() => setActiveTab('orders')}
              className={`flex-1 px-6 py-4 text-sm font-semibold transition-colors ${
                activeTab === 'orders'
                  ? 'text-[#0066CC] border-b-2 border-[#0066CC] bg-blue-50/50'
                  : 'text-gray-600 hover:text-gray-900 hover:bg-gray-50'
              }`}
            >
              <div className="flex items-center justify-center gap-2">
                <span>📋</span>
                Orders
                <span className="bg-gray-100 text-gray-600 text-xs px-2 py-0.5 rounded-full">
                  {orders.length}
                </span>
              </div>
            </button>
          </div>

          <div className="p-6">
            {/* Products Tab */}
            {activeTab === 'products' && (
              <div>
                {/* Add Button */}
                <div className="flex justify-between items-center mb-6">
                  <h2 className="text-lg font-semibold text-gray-900">
                    {editingProduct ? (editingProduct.id ? 'Edit Product' : 'New Product') : 'All Products'}
                  </h2>
                  {!editingProduct && (
                    <button
                      onClick={() => setEditingProduct({} as Product)}
                      className="flex items-center gap-2 px-4 py-2 bg-[#0066CC] text-white rounded-lg font-medium text-sm hover:bg-[#004C99] transition-colors"
                    >
                      <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 4v16m8-8H4"></path>
                      </svg>
                      Add Product
                    </button>
                  )}
                </div>

                {/* Product Form */}
                {editingProduct && (
                  <div className="bg-gray-50 rounded-xl p-6 mb-6 border border-gray-200">
                    <form onSubmit={handleSaveProduct} className="space-y-4">
                      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <div>
                          <label className="block text-sm font-medium text-gray-700 mb-1">Name</label>
                          <input
                            name="name"
                            type="text"
                            defaultValue={editingProduct.name}
                            required
                            className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#0066CC] focus:border-transparent outline-none"
                            placeholder="Product name"
                          />
                        </div>
                        <div>
                          <label className="block text-sm font-medium text-gray-700 mb-1">Base Price</label>
                          <input
                            name="basePrice"
                            type="number"
                            step="0.01"
                            defaultValue={editingProduct.basePrice}
                            required
                            className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#0066CC] focus:border-transparent outline-none"
                            placeholder="0.00"
                          />
                        </div>
                      </div>
                      <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">Description</label>
                        <textarea
                          name="description"
                          defaultValue={editingProduct.description}
                          rows={2}
                          className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#0066CC] focus:border-transparent outline-none resize-none"
                          placeholder="Product description"
                        />
                      </div>
                      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                        <div>
                          <label className="block text-sm font-medium text-gray-700 mb-1">Type</label>
                          <select
                            name="productType"
                            defaultValue={editingProduct.productType}
                            className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#0066CC] focus:border-transparent outline-none bg-white"
                          >
                            <option value={ProductType.Pizza}>🍕 Pizza</option>
                            <option value={ProductType.Drink}>🥤 Drink</option>
                            <option value={ProductType.Dessert}>🍰 Dessert</option>
                          </select>
                        </div>
                        <div>
                          <label className="block text-sm font-medium text-gray-700 mb-1">Has Toppings</label>
                          <select
                            name="hasToppings"
                            defaultValue={editingProduct.hasToppings ? 'true' : 'false'}
                            className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#0066CC] focus:border-transparent outline-none bg-white"
                          >
                            <option value="true">Yes</option>
                            <option value="false">No</option>
                          </select>
                        </div>
                        <div>
                          <label className="block text-sm font-medium text-gray-700 mb-1">Image URL</label>
                          <input
                            name="imageUrl"
                            type="text"
                            defaultValue={editingProduct.imageUrl}
                            className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#0066CC] focus:border-transparent outline-none"
                            placeholder="https://..."
                          />
                        </div>
                      </div>
                      <div className="flex gap-3 pt-2">
                        <button
                          type="submit"
                          className="px-6 py-2.5 bg-[#0066CC] text-white rounded-lg font-medium text-sm hover:bg-[#004C99] transition-colors"
                        >
                          {editingProduct.id ? 'Update Product' : 'Create Product'}
                        </button>
                        <button
                          type="button"
                          onClick={() => setEditingProduct(null)}
                          className="px-6 py-2.5 bg-gray-200 text-gray-700 rounded-lg font-medium text-sm hover:bg-gray-300 transition-colors"
                        >
                          Cancel
                        </button>
                      </div>
                    </form>
                  </div>
                )}

                {/* Products List */}
                {loading ? (
                  <div className="flex items-center justify-center py-12">
                    <div className="w-8 h-8 border-4 border-[#0066CC] border-t-transparent rounded-full animate-spin"></div>
                  </div>
                ) : products.length === 0 ? (
                  <div className="text-center py-12">
                    <div className="text-4xl mb-3">📦</div>
                    <p className="text-gray-500">No products yet. Add your first product!</p>
                  </div>
                ) : (
                  <div className="overflow-x-auto">
                    <table className="w-full">
                      <thead>
                        <tr className="border-b border-gray-200">
                          <th className="text-left py-3 px-4 text-xs font-semibold text-gray-500 uppercase tracking-wider">Product</th>
                          <th className="text-left py-3 px-4 text-xs font-semibold text-gray-500 uppercase tracking-wider">Type</th>
                          <th className="text-left py-3 px-4 text-xs font-semibold text-gray-500 uppercase tracking-wider">Price</th>
                          <th className="text-left py-3 px-4 text-xs font-semibold text-gray-500 uppercase tracking-wider">Toppings</th>
                          <th className="text-right py-3 px-4 text-xs font-semibold text-gray-500 uppercase tracking-wider">Actions</th>
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-gray-100">
                        {products.map((product) => (
                          <tr key={product.id} className="hover:bg-gray-50 transition-colors">
                            <td className="py-3 px-4">
                              <div className="flex items-center gap-3">
                                <div className="w-10 h-10 bg-gray-100 rounded-lg flex items-center justify-center text-lg">
                                  {getProductTypeIcon(product.productType)}
                                </div>
                                <div>
                                  <p className="font-medium text-gray-900">{product.name}</p>
                                  <p className="text-xs text-gray-500 truncate max-w-[200px]">{product.description}</p>
                                </div>
                              </div>
                            </td>
                            <td className="py-3 px-4">
                              <span className="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-medium bg-gray-100 text-gray-700">
                                {getProductTypeLabel(product.productType)}
                              </span>
                            </td>
                            <td className="py-3 px-4">
                              <span className="font-semibold text-gray-900">${product.basePrice.toFixed(2)}</span>
                            </td>
                            <td className="py-3 px-4">
                              {product.hasToppings ? (
                                <span className="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-medium bg-green-100 text-green-700">
                                  Yes
                                </span>
                              ) : (
                                <span className="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-medium bg-gray-100 text-gray-500">
                                  No
                                </span>
                              )}
                            </td>
                            <td className="py-3 px-4 text-right">
                              <div className="flex items-center justify-end gap-2">
                                <button
                                  onClick={() => setEditingProduct(product)}
                                  className="p-2 text-gray-500 hover:text-[#0066CC] hover:bg-blue-50 rounded-lg transition-colors"
                                  title="Edit"
                                >
                                  <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"></path>
                                  </svg>
                                </button>
                                <button
                                  onClick={() => handleDeleteProduct(product.id)}
                                  className="p-2 text-gray-500 hover:text-[#E31837] hover:bg-red-50 rounded-lg transition-colors"
                                  title="Delete"
                                >
                                  <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"></path>
                                  </svg>
                                </button>
                              </div>
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                )}
              </div>
            )}

            {/* Toppings Tab */}
            {activeTab === 'toppings' && (
              <div>
                {/* Add Button */}
                <div className="flex justify-between items-center mb-6">
                  <h2 className="text-lg font-semibold text-gray-900">
                    {editingTopping ? (editingTopping.id ? 'Edit Topping' : 'New Topping') : 'All Toppings'}
                  </h2>
                  {!editingTopping && (
                    <button
                      onClick={() => setEditingTopping({} as Topping)}
                      className="flex items-center gap-2 px-4 py-2 bg-[#0066CC] text-white rounded-lg font-medium text-sm hover:bg-[#004C99] transition-colors"
                    >
                      <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 4v16m8-8H4"></path>
                      </svg>
                      Add Topping
                    </button>
                  )}
                </div>

                {/* Topping Form */}
                {editingTopping && (
                  <div className="bg-gray-50 rounded-xl p-6 mb-6 border border-gray-200">
                    <form onSubmit={handleSaveTopping} className="space-y-4">
                      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <div>
                          <label className="block text-sm font-medium text-gray-700 mb-1">Name</label>
                          <input
                            name="name"
                            type="text"
                            defaultValue={editingTopping.name}
                            required
                            className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#0066CC] focus:border-transparent outline-none"
                            placeholder="Topping name"
                          />
                        </div>
                        <div>
                          <label className="block text-sm font-medium text-gray-700 mb-1">Description</label>
                          <input
                            name="description"
                            type="text"
                            defaultValue={editingTopping.description}
                            required
                            className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#0066CC] focus:border-transparent outline-none"
                            placeholder="Topping description"
                          />
                        </div>
                      </div>
                      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <div>
                          <label className="block text-sm font-medium text-gray-700 mb-1">Price</label>
                          <input
                            name="price"
                            type="number"
                            step="0.01"
                            defaultValue={editingTopping.price}
                            required
                            className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#0066CC] focus:border-transparent outline-none"
                            placeholder="0.00"
                          />
                        </div>
                        <div>
                          <label className="block text-sm font-medium text-gray-700 mb-1">Stock</label>
                          <input
                            name="stock"
                            type="number"
                            defaultValue={editingTopping.stock}
                            required
                            className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#0066CC] focus:border-transparent outline-none"
                            placeholder="100"
                          />
                        </div>
                      </div>
                      <div className="flex gap-3 pt-2">
                        <button
                          type="submit"
                          className="px-6 py-2.5 bg-[#0066CC] text-white rounded-lg font-medium text-sm hover:bg-[#004C99] transition-colors"
                        >
                          {editingTopping.id ? 'Update Topping' : 'Create Topping'}
                        </button>
                        <button
                          type="button"
                          onClick={() => setEditingTopping(null)}
                          className="px-6 py-2.5 bg-gray-200 text-gray-700 rounded-lg font-medium text-sm hover:bg-gray-300 transition-colors"
                        >
                          Cancel
                        </button>
                      </div>
                    </form>
                  </div>
                )}

                {/* Toppings List */}
                {loading ? (
                  <div className="flex items-center justify-center py-12">
                    <div className="w-8 h-8 border-4 border-[#0066CC] border-t-transparent rounded-full animate-spin"></div>
                  </div>
                ) : toppings.length === 0 ? (
                  <div className="text-center py-12">
                    <div className="text-4xl mb-3">🧀</div>
                    <p className="text-gray-500">No toppings yet. Add your first topping!</p>
                  </div>
                ) : (
                  <div className="overflow-x-auto">
                    <table className="w-full">
                      <thead>
                        <tr className="border-b border-gray-200">
                          <th className="text-left py-3 px-4 text-xs font-semibold text-gray-500 uppercase tracking-wider">Topping</th>
                          <th className="text-left py-3 px-4 text-xs font-semibold text-gray-500 uppercase tracking-wider">Price</th>
                          <th className="text-left py-3 px-4 text-xs font-semibold text-gray-500 uppercase tracking-wider">Stock</th>
                          <th className="text-right py-3 px-4 text-xs font-semibold text-gray-500 uppercase tracking-wider">Actions</th>
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-gray-100">
                        {toppings.map((topping) => (
                          <tr key={topping.id} className="hover:bg-gray-50 transition-colors">
                            <td className="py-3 px-4">
                              <div>
                                <p className="font-medium text-gray-900">{topping.name}</p>
                                <p className="text-xs text-gray-500">{topping.description}</p>
                              </div>
                            </td>
                            <td className="py-3 px-4">
                              <span className="font-semibold text-gray-900">+${topping.price.toFixed(2)}</span>
                            </td>
                            <td className="py-3 px-4">
                              <span className={`inline-flex items-center px-2.5 py-1 rounded-full text-xs font-medium ${
                                topping.stock > 50
                                  ? 'bg-green-100 text-green-700'
                                  : topping.stock > 10
                                    ? 'bg-yellow-100 text-yellow-700'
                                    : 'bg-red-100 text-red-700'
                              }`}>
                                {topping.stock} units
                              </span>
                            </td>
                            <td className="py-3 px-4 text-right">
                              <div className="flex items-center justify-end gap-2">
                                <button
                                  onClick={() => setEditingTopping(topping)}
                                  className="p-2 text-gray-500 hover:text-[#0066CC] hover:bg-blue-50 rounded-lg transition-colors"
                                  title="Edit"
                                >
                                  <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"></path>
                                  </svg>
                                </button>
                                <button
                                  onClick={() => handleDeleteTopping(topping.id)}
                                  className="p-2 text-gray-500 hover:text-[#E31837] hover:bg-red-50 rounded-lg transition-colors"
                                  title="Delete"
                                >
                                  <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"></path>
                                  </svg>
                                </button>
                              </div>
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                )}
              </div>
            )}

            {/* Orders Tab */}
            {activeTab === 'orders' && (
              <div>
                {/* Filter and Header */}
                <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 mb-6">
                  <h2 className="text-lg font-semibold text-gray-900">All Orders</h2>
                  <div className="flex items-center gap-3">
                    <label className="text-sm text-gray-600">Filter by status:</label>
                    <select
                      value={statusFilter}
                      onChange={(e) => setStatusFilter(e.target.value)}
                      className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-[#0066CC] focus:border-transparent outline-none bg-white text-sm"
                    >
                      <option value="">All Statuses</option>
                      {orderStatuses.map((status) => (
                        <option key={status} value={status}>{status}</option>
                      ))}
                    </select>
                  </div>
                </div>

                {/* Orders List */}
                {loading ? (
                  <div className="flex items-center justify-center py-12">
                    <div className="w-8 h-8 border-4 border-[#0066CC] border-t-transparent rounded-full animate-spin"></div>
                  </div>
                ) : orders.length === 0 ? (
                  <div className="text-center py-12">
                    <div className="text-4xl mb-3">📋</div>
                    <p className="text-gray-500">
                      {statusFilter ? `No orders with status "${statusFilter}"` : 'No orders yet'}
                    </p>
                  </div>
                ) : (
                  <div className="space-y-4">
                    {orders.map((order) => (
                      <div key={order.id} className="bg-gray-50 rounded-xl p-4 border border-gray-200">
                        {/* Order Header */}
                        <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-3 mb-4">
                          <div className="flex items-center gap-3">
                            <div className="w-10 h-10 bg-[#0066CC] rounded-lg flex items-center justify-center text-white font-bold text-sm">
                              #{order.id}
                            </div>
                            <div>
                              <p className="font-semibold text-gray-900">Order #{order.id}</p>
                              <p className="text-xs text-gray-500">{formatDate(order.createdAt)}</p>
                            </div>
                          </div>
                          <div className="flex items-center gap-3">
                            <span className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-semibold ${getStatusColor(order.status)}`}>
                              {order.status}
                            </span>
                            <span className="text-lg font-bold text-gray-900">${order.totalPrice.toFixed(2)}</span>
                          </div>
                        </div>

                        {/* Customer Info */}
                        <div className="flex items-center gap-2 mb-4 text-sm text-gray-600">
                          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path>
                          </svg>
                          <span>{order.userEmail || 'Guest Order'}</span>
                        </div>

                        {/* Order Items */}
                        <div className="bg-white rounded-lg p-3 mb-4">
                          <p className="text-xs font-semibold text-gray-500 uppercase tracking-wider mb-2">Items</p>
                          <div className="space-y-2">
                            {order.items.map((item, index) => (
                              <div key={index} className="flex justify-between items-center text-sm">
                                <div className="flex items-center gap-2">
                                  <span className="text-gray-600">{item.quantity}x</span>
                                  <span className="font-medium text-gray-900">{item.productName}</span>
                                </div>
                                <span className="text-gray-600">${item.totalPrice.toFixed(2)}</span>
                              </div>
                            ))}
                          </div>
                        </div>

                        {/* Status Update */}
                        <div className="flex flex-col sm:flex-row items-start sm:items-center gap-3 pt-3 border-t border-gray-200">
                          {order.nextStatuses.length > 0 ? (
                            <>
                              <label className="text-sm font-medium text-gray-700">Actions:</label>
                              <div className="flex flex-wrap gap-2">
                                {order.nextStatuses.map((status) => (
                                  <button
                                    key={status}
                                    onClick={() => handleUpdateOrderStatus(order.id, status)}
                                    className={`px-3 py-1.5 text-xs font-medium rounded-lg transition-colors ${
                                      status === 'Cancelled'
                                        ? 'bg-red-50 border border-red-300 text-red-700 hover:bg-red-100'
                                        : 'bg-green-50 border border-green-300 text-green-700 hover:bg-green-100'
                                    }`}
                                  >
                                    {status === 'Cancelled' ? 'Cancel' : status}
                                  </button>
                                ))}
                              </div>
                            </>
                          ) : (
                            <span className={`text-xs font-semibold ${
                              order.status === 'Completed' ? 'text-green-600' :
                              order.status === 'Cancelled' ? 'text-red-600' :
                              'text-gray-500'
                            }`}>
                              {order.status}
                            </span>
                          )}
                        </div>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            )}
          </div>
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
    </Layout>
  );
};
