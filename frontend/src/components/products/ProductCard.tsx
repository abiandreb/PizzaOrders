import React, { useState } from 'react';
import type { Product } from '../../types';

interface ProductCardProps {
  product: Product;
  onAddToCart: (productId: number, quantity: number, toppingIds: number[]) => void;
}

export const ProductCard: React.FC<ProductCardProps> = ({ product, onAddToCart }) => {
  const [quantity, setQuantity] = useState(1);
  const [loading, setLoading] = useState(false);

  const handleAddToCart = async () => {
    setLoading(true);
    try {
      await onAddToCart(product.id, quantity, []);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="bg-white rounded-lg shadow-md p-4 hover:shadow-lg transition-shadow">
      {product.imageUrl && (
        <img
          src={product.imageUrl}
          alt={product.name}
          className="w-full h-48 object-cover rounded-md mb-4"
        />
      )}
      <h3 className="text-xl font-bold mb-2">{product.name}</h3>
      {product.description && <p className="text-gray-600 mb-3">{product.description}</p>}
      <div className="flex items-center justify-between mb-4">
        <span className="text-2xl font-bold text-blue-600">${product.basePrice.toFixed(2)}</span>
        {product.hasToppings && (
          <span className="text-sm text-green-600">Customizable</span>
        )}
      </div>
      <div className="flex items-center gap-2">
        <input
          type="number"
          min="1"
          value={quantity}
          onChange={(e) => setQuantity(Math.max(1, parseInt(e.target.value) || 1))}
          className="border rounded px-2 py-1 w-16 text-center"
        />
        <button
          onClick={handleAddToCart}
          disabled={loading}
          className="flex-1 bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded disabled:opacity-50"
        >
          {loading ? 'Adding...' : 'Add to Cart'}
        </button>
      </div>
    </div>
  );
};
