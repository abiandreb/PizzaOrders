import React from 'react';
import type { Product } from '../../types';
import { Badge } from '../common/Badge';

interface ProductCardProps {
  product: Product;
  onSelect: (product: Product) => void;
}

export const ProductCard: React.FC<ProductCardProps> = ({ product, onSelect }) => {
  // Determine which badges to show
  const getBadges = () => {
    const badges: Array<'new' | 'vege' | 'hot' | 'bestseller' | 'customizable'> = [];

    // Check description for keywords to determine badges
    const desc = (product.description || '').toLowerCase();
    if (desc.includes('vege') || desc.includes('vegetarian')) {
      badges.push('vege');
    }
    if (desc.includes('spicy') || desc.includes('hot') || desc.includes('jalapeno')) {
      badges.push('hot');
    }
    if (product.hasToppings) {
      badges.push('customizable');
    }

    return badges;
  };

  const badges = getBadges();

  // Use productImage.mediumUrl if available, fallback to imageUrl
  const imageUrl = product.productImage?.mediumUrl || product.imageUrl;

  return (
    <div
      onClick={() => onSelect(product)}
      className="flex bg-white rounded-xl shadow-sm hover:shadow-md transition-shadow border border-gray-100 cursor-pointer h-[280px]"
    >
      {/* Image Section - 40% width */}
      <div className="w-2/5 relative bg-gray-100 flex-shrink-0 h-full overflow-hidden rounded-l-xl">
        {imageUrl ? (
          <img
            src={imageUrl}
            alt={product.name}
            className="w-full h-full object-cover"
            onError={(e) => {
              // Hide broken image and show fallback
              e.currentTarget.style.display = 'none';
              e.currentTarget.nextElementSibling?.classList.remove('hidden');
            }}
          />
        ) : null}
        <div className={`w-full h-full flex items-center justify-center bg-gradient-to-br from-gray-100 to-gray-200 absolute inset-0 ${imageUrl ? 'hidden' : ''}`}>
          <span className="text-6xl opacity-50">
            {product.productType === 0 ? '🍕' : product.productType === 1 ? '🥤' : '🍰'}
          </span>
        </div>
      </div>

      {/* Content Section - 60% width */}
      <div className="w-3/5 p-4 flex flex-col h-full">
        {/* Product Name */}
        <h3 className="font-bold text-lg text-gray-900 mb-1 line-clamp-1">
          {product.name}
        </h3>

        {/* Price */}
        <p className="text-dominos-blue font-bold text-lg mb-1">
          from ${product.basePrice.toFixed(2)}
        </p>

        {/* Description */}
        <p className="text-gray-500 text-sm line-clamp-2 mb-2">
          {product.description || 'Delicious treat waiting for you!'}
        </p>

        {/* Badges */}
        {badges.length > 0 && (
          <div className="flex flex-wrap gap-1.5 mb-2">
            {badges.map((badge) => (
              <Badge key={badge} type={badge} />
            ))}
          </div>
        )}

        {/* Spacer to push button to bottom */}
        <div className="flex-grow"></div>

        {/* Select Button */}
        <button
          onClick={(e) => {
            e.stopPropagation();
            onSelect(product);
          }}
          className="w-full py-3 bg-[#0066CC] text-white rounded-lg font-semibold hover:bg-[#004C99] transition-colors"
        >
          Select
        </button>
      </div>
    </div>
  );
};
