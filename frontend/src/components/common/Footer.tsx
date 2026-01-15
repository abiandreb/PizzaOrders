import React from 'react';

export const Footer: React.FC = () => {
  return (
    <footer className="bg-gray-800 text-white mt-auto">
      <div className="container mx-auto px-4 py-6">
        <div className="text-center">
          <p className="text-sm">
            &copy; {new Date().getFullYear()} PizzaOrders. All rights reserved.
          </p>
          <p className="text-xs text-gray-400 mt-2">
            A pet project for learning Clean Architecture
          </p>
        </div>
      </div>
    </footer>
  );
};
