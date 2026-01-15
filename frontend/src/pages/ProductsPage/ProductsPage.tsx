import React, { useState, useEffect } from 'react';
import { Layout } from '../../components/common/Layout';
import { ProductCard } from '../../components/products/ProductCard';
import type { Product } from '../../types';
import { ProductType } from '../../types';
import { api } from '../../services/api';
import { useCart } from '../../hooks/useCart';

export const ProductsPage: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [selectedType, setSelectedType] = useState<ProductType>(ProductType.Pizza);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const { addToCart } = useCart();

  useEffect(() => {
    loadProducts();
  }, [selectedType]);

  const loadProducts = async () => {
    try {
      setLoading(true);
      setError('');
      const data = await api.getProductsByType(selectedType);
      setProducts(data);
    } catch (err) {
      setError('Failed to load products');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleAddToCart = async (productId: number, quantity: number, toppingIds: number[]) => {
    try {
      await addToCart({ productId, quantity, toppingIds });
      alert('Item added to cart!');
    } catch (err) {
      alert('Failed to add item to cart');
    }
  };

  return (
    <Layout>
      <div>
        <h1 className="text-4xl font-bold mb-6">Our Menu</h1>

        <div className="flex gap-4 mb-8">
          <button
            onClick={() => setSelectedType(ProductType.Pizza)}
            className={`px-6 py-2 rounded-lg font-semibold ${
              selectedType === ProductType.Pizza
                ? 'bg-blue-500 text-white'
                : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
            }`}
          >
            Pizzas
          </button>
          <button
            onClick={() => setSelectedType(ProductType.Drink)}
            className={`px-6 py-2 rounded-lg font-semibold ${
              selectedType === ProductType.Drink
                ? 'bg-blue-500 text-white'
                : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
            }`}
          >
            Drinks
          </button>
          <button
            onClick={() => setSelectedType(ProductType.Dessert)}
            className={`px-6 py-2 rounded-lg font-semibold ${
              selectedType === ProductType.Dessert
                ? 'bg-blue-500 text-white'
                : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
            }`}
          >
            Desserts
          </button>
        </div>

        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        {loading ? (
          <div className="text-center py-12">
            <div className="text-xl">Loading products...</div>
          </div>
        ) : products.length === 0 ? (
          <div className="text-center py-12">
            <div className="text-xl text-gray-500">No products available</div>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {products.map((product) => (
              <ProductCard key={product.id} product={product} onAddToCart={handleAddToCart} />
            ))}
          </div>
        )}
      </div>
    </Layout>
  );
};
