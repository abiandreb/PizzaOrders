import React, { useState, useEffect } from 'react';
import toast from 'react-hot-toast';
import { Header } from '../../components/common/Header';
import { Footer } from '../../components/common/Footer';
import { ProductCard } from '../../components/products/ProductCard';
import { ProductModal } from '../../components/products/ProductModal';
import { CartSidebar } from '../../components/common/CartSidebar';
import type { Product } from '../../types';
import { ProductType } from '../../types';
import { api } from '../../services/api';

export const ProductsPage: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [selectedType, setSelectedType] = useState<ProductType>(ProductType.Pizza);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);

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
      toast.error('Failed to load products');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleProductSelect = (product: Product) => {
    setSelectedProduct(product);
  };

  const getCategoryTitle = () => {
    switch (selectedType) {
      case ProductType.Pizza:
        return 'Our Pizzas';
      case ProductType.Drink:
        return 'Refreshing Drinks';
      case ProductType.Dessert:
        return 'Sweet Desserts';
      default:
        return 'Our Menu';
    }
  };

  const getCategoryDescription = () => {
    switch (selectedType) {
      case ProductType.Pizza:
        return 'Hand-crafted pizzas made with fresh ingredients and baked to perfection';
      case ProductType.Drink:
        return 'Cool off with our selection of refreshing beverages';
      case ProductType.Dessert:
        return 'End your meal on a sweet note with our delicious desserts';
      default:
        return '';
    }
  };

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <Header selectedType={selectedType} onTypeChange={setSelectedType} />

      <main className="flex-grow">
        {/* Page Header */}
        <div className="bg-white border-b border-gray-100">
          <div className="max-w-7xl mx-auto px-4 py-8">
            <h1 className="text-3xl font-bold text-gray-900 mb-2">
              {getCategoryTitle()}
            </h1>
            <p className="text-gray-500">
              {getCategoryDescription()}
            </p>
          </div>
        </div>

        {/* Products Section */}
        <div className="max-w-7xl mx-auto px-4 py-8">
          {error && (
            <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6">
              {error}
            </div>
          )}

          {loading ? (
            <div className="flex flex-col items-center justify-center py-16">
              <div className="w-12 h-12 border-4 border-dominos-blue border-t-transparent rounded-full animate-spin mb-4"></div>
              <p className="text-gray-500">Loading delicious options...</p>
            </div>
          ) : products.length === 0 ? (
            <div className="text-center py-16">
              <div className="text-6xl mb-4">
                {selectedType === ProductType.Pizza ? '🍕' : selectedType === ProductType.Drink ? '🥤' : '🍰'}
              </div>
              <h2 className="text-xl font-semibold text-gray-700 mb-2">No products available</h2>
              <p className="text-gray-500">Check back later for new items!</p>
            </div>
          ) : (
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
              {products.map((product) => (
                <ProductCard
                  key={product.id}
                  product={product}
                  onSelect={handleProductSelect}
                />
              ))}
            </div>
          )}
        </div>
      </main>

      <Footer />
      <CartSidebar />

      {/* Product Modal */}
      {selectedProduct && (
        <ProductModal
          product={selectedProduct}
          onClose={() => setSelectedProduct(null)}
        />
      )}
    </div>
  );
};
