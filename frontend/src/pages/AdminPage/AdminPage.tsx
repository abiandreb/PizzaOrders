import React, { useState, useEffect } from 'react';
import toast from 'react-hot-toast';
import { Layout } from '../../components/common/Layout';
import { ConfirmModal } from '../../components/common/ConfirmModal';
import { useConfirm } from '../../hooks/useConfirm';
import type { Product, Topping } from '../../types';
import { ProductType } from '../../types';
import { api } from '../../services/api';

export const AdminPage: React.FC = () => {
  const [activeTab, setActiveTab] = useState<'products' | 'toppings'>('products');
  const [products, setProducts] = useState<Product[]>([]);
  const [toppings, setToppings] = useState<Topping[]>([]);
  const [loading, setLoading] = useState(false);
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  const [editingTopping, setEditingTopping] = useState<Topping | null>(null);
  const { confirm, isOpen, options, handleConfirm, handleCancel } = useConfirm();

  useEffect(() => {
    loadData();
  }, [activeTab]);

  const loadData = async () => {
    setLoading(true);
    try {
      if (activeTab === 'products') {
        const data = await api.getAllProductsForAdmin();
        setProducts(data);
      } else {
        const data = await api.getAllToppings();
        setToppings(data);
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

  return (
    <Layout>
      <div>
        <h1 className="text-4xl font-bold mb-6">Admin Panel</h1>

        <div className="flex gap-4 mb-6">
          <button
            onClick={() => setActiveTab('products')}
            className={`px-6 py-2 rounded-lg font-semibold ${
              activeTab === 'products'
                ? 'bg-blue-500 text-white'
                : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
            }`}
          >
            Products
          </button>
          <button
            onClick={() => setActiveTab('toppings')}
            className={`px-6 py-2 rounded-lg font-semibold ${
              activeTab === 'toppings'
                ? 'bg-blue-500 text-white'
                : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
            }`}
          >
            Toppings
          </button>
        </div>

        {activeTab === 'products' && (
          <div>
            <div className="mb-6">
              <button
                onClick={() => setEditingProduct({} as Product)}
                className="bg-green-500 hover:bg-green-600 text-white font-bold py-2 px-4 rounded"
              >
                Add New Product
              </button>
            </div>

            {editingProduct && (
              <div className="bg-white p-6 rounded-lg shadow mb-6">
                <h2 className="text-2xl font-bold mb-4">
                  {editingProduct.id ? 'Edit Product' : 'New Product'}
                </h2>
                <form onSubmit={handleSaveProduct} className="space-y-4">
                  <div>
                    <label className="block text-sm font-bold mb-2">Name</label>
                    <input
                      name="name"
                      type="text"
                      defaultValue={editingProduct.name}
                      required
                      className="w-full border rounded px-3 py-2"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-bold mb-2">Description</label>
                    <textarea
                      name="description"
                      defaultValue={editingProduct.description}
                      className="w-full border rounded px-3 py-2"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-bold mb-2">Base Price</label>
                    <input
                      name="basePrice"
                      type="number"
                      step="0.01"
                      defaultValue={editingProduct.basePrice}
                      required
                      className="w-full border rounded px-3 py-2"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-bold mb-2">Product Type</label>
                    <select
                      name="productType"
                      defaultValue={editingProduct.productType}
                      className="w-full border rounded px-3 py-2"
                    >
                      <option value={ProductType.Pizza}>Pizza</option>
                      <option value={ProductType.Drink}>Drink</option>
                      <option value={ProductType.Dessert}>Dessert</option>
                    </select>
                  </div>
                  <div>
                    <label className="block text-sm font-bold mb-2">Has Toppings</label>
                    <select
                      name="hasToppings"
                      defaultValue={editingProduct.hasToppings ? 'true' : 'false'}
                      className="w-full border rounded px-3 py-2"
                    >
                      <option value="true">Yes</option>
                      <option value="false">No</option>
                    </select>
                  </div>
                  <div>
                    <label className="block text-sm font-bold mb-2">Image URL (optional)</label>
                    <input
                      name="imageUrl"
                      type="text"
                      defaultValue={editingProduct.imageUrl}
                      className="w-full border rounded px-3 py-2"
                    />
                  </div>
                  <div className="flex gap-4">
                    <button
                      type="submit"
                      className="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded"
                    >
                      Save
                    </button>
                    <button
                      type="button"
                      onClick={() => setEditingProduct(null)}
                      className="bg-gray-300 hover:bg-gray-400 text-gray-800 font-bold py-2 px-4 rounded"
                    >
                      Cancel
                    </button>
                  </div>
                </form>
              </div>
            )}

            {loading ? (
              <div className="text-center py-12">Loading...</div>
            ) : (
              <div className="bg-white rounded-lg shadow overflow-hidden">
                <table className="min-w-full">
                  <thead className="bg-gray-100">
                    <tr>
                      <th className="px-6 py-3 text-left">Name</th>
                      <th className="px-6 py-3 text-left">Type</th>
                      <th className="px-6 py-3 text-left">Price</th>
                      <th className="px-6 py-3 text-left">Toppings</th>
                      <th className="px-6 py-3 text-right">Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {products.map((product) => (
                      <tr key={product.id} className="border-b">
                        <td className="px-6 py-4">{product.name}</td>
                        <td className="px-6 py-4">
                          {product.productType === ProductType.Pizza ? 'Pizza' :
                           product.productType === ProductType.Drink ? 'Drink' : 'Dessert'}
                        </td>
                        <td className="px-6 py-4">${product.basePrice.toFixed(2)}</td>
                        <td className="px-6 py-4">{product.hasToppings ? 'Yes' : 'No'}</td>
                        <td className="px-6 py-4 text-right space-x-2">
                          <button
                            onClick={() => setEditingProduct(product)}
                            className="bg-yellow-500 hover:bg-yellow-600 text-white px-3 py-1 rounded"
                          >
                            Edit
                          </button>
                          <button
                            onClick={() => handleDeleteProduct(product.id)}
                            className="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded"
                          >
                            Delete
                          </button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        )}

        {activeTab === 'toppings' && (
          <div>
            <div className="mb-6">
              <button
                onClick={() => setEditingTopping({} as Topping)}
                className="bg-green-500 hover:bg-green-600 text-white font-bold py-2 px-4 rounded"
              >
                Add New Topping
              </button>
            </div>

            {editingTopping && (
              <div className="bg-white p-6 rounded-lg shadow mb-6">
                <h2 className="text-2xl font-bold mb-4">
                  {editingTopping.id ? 'Edit Topping' : 'New Topping'}
                </h2>
                <form onSubmit={handleSaveTopping} className="space-y-4">
                  <div>
                    <label className="block text-sm font-bold mb-2">Name</label>
                    <input
                      name="name"
                      type="text"
                      defaultValue={editingTopping.name}
                      required
                      className="w-full border rounded px-3 py-2"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-bold mb-2">Description</label>
                    <input
                      name="description"
                      type="text"
                      defaultValue={editingTopping.description}
                      required
                      className="w-full border rounded px-3 py-2"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-bold mb-2">Price</label>
                    <input
                      name="price"
                      type="number"
                      step="0.01"
                      defaultValue={editingTopping.price}
                      required
                      className="w-full border rounded px-3 py-2"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-bold mb-2">Stock</label>
                    <input
                      name="stock"
                      type="number"
                      defaultValue={editingTopping.stock}
                      required
                      className="w-full border rounded px-3 py-2"
                    />
                  </div>
                  <div className="flex gap-4">
                    <button
                      type="submit"
                      className="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded"
                    >
                      Save
                    </button>
                    <button
                      type="button"
                      onClick={() => setEditingTopping(null)}
                      className="bg-gray-300 hover:bg-gray-400 text-gray-800 font-bold py-2 px-4 rounded"
                    >
                      Cancel
                    </button>
                  </div>
                </form>
              </div>
            )}

            {loading ? (
              <div className="text-center py-12">Loading...</div>
            ) : (
              <div className="bg-white rounded-lg shadow overflow-hidden">
                <table className="min-w-full">
                  <thead className="bg-gray-100">
                    <tr>
                      <th className="px-6 py-3 text-left">Name</th>
                      <th className="px-6 py-3 text-left">Description</th>
                      <th className="px-6 py-3 text-left">Price</th>
                      <th className="px-6 py-3 text-left">Stock</th>
                      <th className="px-6 py-3 text-right">Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {toppings.map((topping) => (
                      <tr key={topping.id} className="border-b">
                        <td className="px-6 py-4">{topping.name}</td>
                        <td className="px-6 py-4">{topping.description}</td>
                        <td className="px-6 py-4">${topping.price.toFixed(2)}</td>
                        <td className="px-6 py-4">{topping.stock}</td>
                        <td className="px-6 py-4 text-right space-x-2">
                          <button
                            onClick={() => setEditingTopping(topping)}
                            className="bg-yellow-500 hover:bg-yellow-600 text-white px-3 py-1 rounded"
                          >
                            Edit
                          </button>
                          <button
                            onClick={() => handleDeleteTopping(topping.id)}
                            className="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded"
                          >
                            Delete
                          </button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        )}
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
