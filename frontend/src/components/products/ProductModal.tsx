import React, { useState, useEffect } from 'react';
import toast from 'react-hot-toast';
import type { Product, Topping } from '../../types';
import { useReduxCart } from '../../hooks/useReduxCart';
import { api } from '../../services/api';

interface ProductModalProps {
  product: Product;
  onClose: () => void;
}

const SIZES = [
  { label: 'Small', multiplier: 0.8 },
  { label: 'Medium', multiplier: 1.0 },
  { label: 'Large', multiplier: 1.25 },
  { label: 'X-Large', multiplier: 1.5 },
];

export const ProductModal: React.FC<ProductModalProps> = ({ product, onClose }) => {
  const [sizeIndex, setSizeIndex] = useState(1); // Default to Medium
  const [selectedToppings, setSelectedToppings] = useState<Topping[]>([]);
  const [quantity, setQuantity] = useState(1);
  const [availableToppings, setAvailableToppings] = useState<Topping[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const { addToCart } = useReduxCart();

  useEffect(() => {
    const loadToppings = async () => {
      try {
        const toppings = await api.getAllToppings();
        setAvailableToppings(toppings);
      } catch (error) {
        console.error('Failed to load toppings:', error);
      }
    };

    if (product.hasToppings) {
      loadToppings();
    }
  }, [product.hasToppings]);

  const calculateTotal = () => {
    const baseWithMultiplier = product.basePrice * SIZES[sizeIndex].multiplier;
    const toppingsPrice = selectedToppings.reduce((acc, t) => acc + t.price, 0);
    return (baseWithMultiplier + toppingsPrice) * quantity;
  };

  const toggleTopping = (topping: Topping) => {
    setSelectedToppings((prev) =>
      prev.find((t) => t.id === topping.id)
        ? prev.filter((t) => t.id !== topping.id)
        : [...prev, topping]
    );
  };

  const handleAdd = async () => {
    setIsLoading(true);
    try {
      await addToCart({
        productId: product.id,
        quantity,
        toppingIds: selectedToppings.map((t) => t.id),
      });
      toast.success('Item added to cart!');
      onClose();
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Failed to add item to cart');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 z-[100] flex items-center justify-center p-4">
      {/* Backdrop */}
      <div className="absolute inset-0 bg-black/60 backdrop-blur-sm" onClick={onClose} />

      {/* Modal */}
      <div className="relative bg-white w-full max-w-4xl max-h-[90vh] rounded-2xl overflow-hidden shadow-2xl flex flex-col md:flex-row">
        {/* Left: Image */}
        <div className="w-full md:w-2/5 h-56 md:h-auto overflow-hidden bg-gray-100 flex-shrink-0 relative">
          {(product.productImage?.fullUrl || product.imageUrl) ? (
            <img
              src={product.productImage?.fullUrl || product.imageUrl}
              alt={product.name}
              className="w-full h-full object-cover"
              onError={(e) => {
                e.currentTarget.style.display = 'none';
                e.currentTarget.nextElementSibling?.classList.remove('hidden');
              }}
            />
          ) : null}
          <div className={`w-full h-full flex items-center justify-center bg-gradient-to-br from-gray-100 to-gray-200 ${(product.productImage?.fullUrl || product.imageUrl) ? 'hidden absolute inset-0' : ''}`}>
            <span className="text-8xl opacity-50">
              {product.productType === 0 ? '🍕' : product.productType === 1 ? '🥤' : '🍰'}
            </span>
          </div>
        </div>

        {/* Right: Options */}
        <div className="w-full md:w-3/5 p-6 overflow-y-auto bg-white flex flex-col max-h-[calc(90vh-14rem)] md:max-h-none">
          {/* Header */}
          <div className="flex justify-between items-start mb-4">
            <div>
              <h2 className="text-2xl font-bold text-gray-900">{product.name}</h2>
              <p className="text-gray-500 text-sm mt-1">{product.description}</p>
            </div>
            <button
              onClick={onClose}
              className="p-2 hover:bg-gray-100 rounded-full transition-colors"
            >
              <svg className="w-5 h-5 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M6 18L18 6M6 6l12 12"></path>
              </svg>
            </button>
          </div>

          <div className="space-y-6 flex-grow">
            {/* Size Selector */}
            {product.hasToppings && (
              <section>
                <h3 className="font-semibold text-gray-900 mb-3 text-sm uppercase tracking-wide">
                  Select Size
                </h3>
                <div className="grid grid-cols-2 sm:grid-cols-4 gap-2">
                  {SIZES.map((size, index) => (
                    <button
                      key={size.label}
                      onClick={() => setSizeIndex(index)}
                      className={`py-2.5 px-3 rounded-lg border-2 transition-all text-sm font-medium ${
                        sizeIndex === index
                          ? 'border-[#0066CC] bg-blue-50 text-[#0066CC]'
                          : 'border-gray-200 hover:border-gray-300 text-gray-600'
                      }`}
                    >
                      {size.label}
                    </button>
                  ))}
                </div>
              </section>
            )}

            {/* Toppings */}
            {product.hasToppings && availableToppings.length > 0 && (
              <section>
                <h3 className="font-semibold text-gray-900 mb-3 text-sm uppercase tracking-wide">
                  Extra Toppings
                </h3>
                <div className="grid grid-cols-2 gap-2">
                  {availableToppings.map((topping) => {
                    const isSelected = selectedToppings.find((st) => st.id === topping.id);
                    return (
                      <button
                        key={topping.id}
                        onClick={() => toggleTopping(topping)}
                        className={`flex items-center justify-between p-3 rounded-lg border-2 transition-all text-sm ${
                          isSelected
                            ? 'bg-[#0066CC] border-[#0066CC] text-white'
                            : 'bg-white border-gray-200 text-gray-700 hover:border-gray-300'
                        }`}
                      >
                        <span className="font-medium">{topping.name}</span>
                        <span className={isSelected ? 'text-blue-100' : 'text-gray-400'}>
                          +${topping.price.toFixed(2)}
                        </span>
                      </button>
                    );
                  })}
                </div>
              </section>
            )}

            {/* Quantity */}
            <section className="flex items-center justify-between">
              <h3 className="font-semibold text-gray-900 text-sm uppercase tracking-wide">
                Quantity
              </h3>
              <div className="flex items-center gap-3 bg-gray-100 p-1.5 rounded-lg">
                <button
                  onClick={() => setQuantity((q) => Math.max(1, q - 1))}
                  className="w-9 h-9 flex items-center justify-center bg-white rounded-md shadow-sm hover:bg-gray-50 font-bold text-gray-600"
                >
                  -
                </button>
                <span className="w-8 text-center font-bold text-lg">{quantity}</span>
                <button
                  onClick={() => setQuantity((q) => q + 1)}
                  className="w-9 h-9 flex items-center justify-center bg-white rounded-md shadow-sm hover:bg-gray-50 font-bold text-gray-600"
                >
                  +
                </button>
              </div>
            </section>
          </div>

          {/* Footer */}
          <div className="mt-6 pt-4 border-t border-gray-100 flex items-center justify-between gap-4">
            <div>
              <p className="text-gray-400 text-xs font-semibold uppercase tracking-wide mb-0.5">Total</p>
              <p className="text-2xl font-bold text-[#0066CC]">${calculateTotal().toFixed(2)}</p>
            </div>
            <button
              onClick={handleAdd}
              disabled={isLoading}
              className="flex-1 max-w-[200px] py-3 bg-[#E31837] text-white rounded-lg font-semibold shadow-lg hover:bg-[#C41230] transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {isLoading ? 'Adding...' : 'Add to Cart'}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
