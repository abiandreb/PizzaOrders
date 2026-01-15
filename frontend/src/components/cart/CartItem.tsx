import React from 'react';
import type { CartItem as CartItemType } from '../../types';

interface CartItemProps {
  item: CartItemType;
  onUpdateQuantity: (productId: number, quantity: number, toppingIds: number[]) => void;
  onRemove: (productId: number, toppingIds: number[]) => void;
}

export const CartItem: React.FC<CartItemProps> = ({ item, onUpdateQuantity, onRemove }) => {
  const handleQuantityChange = (newQuantity: number) => {
    if (newQuantity > 0) {
      onUpdateQuantity(item.productId, newQuantity, item.toppingIds);
    }
  };

  return (
    <div className="bg-white rounded-lg shadow p-4 flex items-center justify-between">
      <div className="flex-1">
        <h3 className="text-lg font-semibold">{item.productName}</h3>
        {item.toppings && item.toppings.length > 0 && (
          <div className="text-sm text-gray-600 mt-1">
            Toppings: {item.toppings.map((t) => t.toppingName).join(', ')}
          </div>
        )}
        <div className="text-sm text-gray-500 mt-1">
          Base Price: ${item.basePrice.toFixed(2)}
        </div>
      </div>

      <div className="flex items-center gap-4">
        <div className="flex items-center gap-2">
          <button
            onClick={() => handleQuantityChange(item.quantity - 1)}
            className="bg-gray-200 hover:bg-gray-300 w-8 h-8 rounded"
          >
            -
          </button>
          <span className="w-12 text-center">{item.quantity}</span>
          <button
            onClick={() => handleQuantityChange(item.quantity + 1)}
            className="bg-gray-200 hover:bg-gray-300 w-8 h-8 rounded"
          >
            +
          </button>
        </div>

        <div className="text-xl font-bold w-24 text-right">
          ${item.totalPrice.toFixed(2)}
        </div>

        <button
          onClick={() => onRemove(item.productId, item.toppingIds)}
          className="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded"
        >
          Remove
        </button>
      </div>
    </div>
  );
};
