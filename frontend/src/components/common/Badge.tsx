import React from 'react';

export type BadgeType = 'new' | 'vege' | 'hot' | 'bestseller' | 'customizable';

interface BadgeProps {
  type: BadgeType;
  className?: string;
}

const badgeConfig: Record<BadgeType, { label: string; className: string }> = {
  new: {
    label: 'New',
    className: 'bg-badge-new text-white',
  },
  vege: {
    label: 'Vege',
    className: 'bg-badge-vege text-white',
  },
  hot: {
    label: 'Hot',
    className: 'bg-badge-hot text-white',
  },
  bestseller: {
    label: 'Bestseller',
    className: 'bg-badge-bestseller text-gray-900',
  },
  customizable: {
    label: 'Customizable',
    className: 'bg-dominos-blue text-white',
  },
};

export const Badge: React.FC<BadgeProps> = ({ type, className = '' }) => {
  const config = badgeConfig[type];

  return (
    <span
      className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-semibold ${config.className} ${className}`}
    >
      {config.label}
    </span>
  );
};
