import React, { useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { useReduxCart } from '../../hooks/useReduxCart';
import { AuthModal } from './AuthModal';
import { ProductType } from '../../types';

interface HeaderProps {
  selectedType?: ProductType;
  onTypeChange?: (type: ProductType) => void;
}

export const Header: React.FC<HeaderProps> = ({ selectedType, onTypeChange }) => {
  const { isAuthenticated, isAdmin, user, logout } = useAuth();
  const { cart, openCart } = useReduxCart();
  const [isAuthModalOpen, setIsAuthModalOpen] = useState(false);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const location = useLocation();

  const itemCount = cart?.items.reduce((sum, item) => sum + item.quantity, 0) || 0;

  const isMenuPage = location.pathname === '/';

  const categories = [
    { type: ProductType.Pizza, label: 'Pizza', icon: null },
    { type: ProductType.Drink, label: 'Drinks', icon: null },
    { type: ProductType.Dessert, label: 'Desserts', icon: null },
  ];

  return (
    <>
      {/* Main Header */}
      <header className="fixed top-0 left-0 right-0 z-50 bg-white shadow-md">
        {/* Top Navigation Bar */}
        <div className="border-b border-gray-100">
          <div className="max-w-7xl mx-auto px-4">
            <div className="h-16 flex items-center justify-between">
              {/* Logo */}
              <Link to="/" className="flex items-center gap-2 group">
                <div className="w-10 h-10 bg-[#E31837] rounded-full flex items-center justify-center">
                  <span className="text-white text-xl font-bold">P</span>
                </div>
                <span className="text-xl font-bold text-[#0066CC] hidden sm:block">
                  Pizza<span className="text-[#E31837]">Orders</span>
                </span>
              </Link>

              {/* Desktop Navigation */}
              <nav className="hidden md:flex items-center gap-8">
                <Link
                  to="/"
                  className="text-sm font-semibold text-gray-700 hover:text-[#0066CC] transition-colors"
                >
                  Menu
                </Link>
                {isAuthenticated && (
                  <Link
                    to="/orders"
                    className="text-sm font-semibold text-gray-700 hover:text-[#0066CC] transition-colors"
                  >
                    My Orders
                  </Link>
                )}
                {isAdmin && (
                  <Link
                    to="/admin"
                    className="text-sm font-semibold text-gray-700 hover:text-[#0066CC] transition-colors flex items-center gap-1"
                  >
                    <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z"></path>
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"></path>
                    </svg>
                    Admin
                  </Link>
                )}
              </nav>

              {/* Right Side Actions */}
              <div className="flex items-center gap-3">
                {/* Auth Section */}
                {isAuthenticated ? (
                  <div className="flex items-center gap-2">
                    <div className="hidden sm:flex items-center gap-2 px-3 py-1.5 bg-gray-50 rounded-full">
                      <div className="w-6 h-6 bg-[#0066CC] rounded-full flex items-center justify-center text-white text-xs font-bold">
                        {user?.email?.charAt(0).toUpperCase()}
                      </div>
                      <span className="text-sm font-medium text-gray-700 max-w-[120px] truncate">
                        {user?.email}
                      </span>
                    </div>
                    <button
                      onClick={logout}
                      className="p-2 text-gray-500 hover:text-[#E31837] hover:bg-red-50 rounded-full transition-all"
                      title="Logout"
                    >
                      <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"></path>
                      </svg>
                    </button>
                  </div>
                ) : (
                  <button
                    onClick={() => setIsAuthModalOpen(true)}
                    className="flex items-center gap-2 px-4 py-2 bg-[#0066CC] text-white rounded-full font-semibold text-sm hover:bg-[#004C99] transition-colors"
                  >
                    <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path>
                    </svg>
                    <span className="hidden sm:inline">Login</span>
                  </button>
                )}

                {/* Cart Button */}
                <button
                  onClick={openCart}
                  className="relative flex items-center gap-2 px-4 py-2 bg-[#E31837] text-white rounded-full font-semibold text-sm hover:bg-[#C41230] transition-colors"
                >
                  <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z"></path>
                  </svg>
                  {itemCount > 0 && (
                    <span className="absolute -top-1 -right-1 w-5 h-5 bg-white text-[#E31837] text-xs font-bold flex items-center justify-center rounded-full">
                      {itemCount}
                    </span>
                  )}
                </button>

                {/* Mobile Menu Button */}
                <button
                  onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
                  className="md:hidden p-2 text-gray-700 hover:bg-gray-100 rounded-lg"
                >
                  <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    {isMobileMenuOpen ? (
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M6 18L18 6M6 6l12 12" />
                    ) : (
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 6h16M4 12h16M4 18h16" />
                    )}
                  </svg>
                </button>
              </div>
            </div>
          </div>
        </div>

        {/* Category Navigation - Only show on menu page */}
        {isMenuPage && onTypeChange && (
          <div className="bg-gray-50 border-b border-gray-200">
            <div className="max-w-7xl mx-auto px-4">
              <div className="flex overflow-x-auto scrollbar-hide -mx-4 px-4 md:mx-0 md:px-0">
                {categories.map((category) => (
                  <button
                    key={category.type}
                    onClick={() => onTypeChange(category.type)}
                    className={`flex-shrink-0 px-6 py-3 text-sm font-semibold transition-all border-b-2 ${
                      selectedType === category.type
                        ? 'text-[#0066CC] border-[#0066CC] bg-white'
                        : 'text-gray-600 border-transparent hover:text-[#0066CC] hover:bg-white'
                    }`}
                  >
                    {category.label}
                  </button>
                ))}
              </div>
            </div>
          </div>
        )}

        {/* Mobile Menu Dropdown */}
        {isMobileMenuOpen && (
          <div className="md:hidden bg-white border-t border-gray-100 shadow-lg">
            <div className="px-4 py-3 space-y-2">
              <Link
                to="/"
                className="block px-4 py-2 text-gray-700 hover:bg-gray-50 rounded-lg font-medium"
                onClick={() => setIsMobileMenuOpen(false)}
              >
                Menu
              </Link>
              {isAuthenticated && (
                <Link
                  to="/orders"
                  className="block px-4 py-2 text-gray-700 hover:bg-gray-50 rounded-lg font-medium"
                  onClick={() => setIsMobileMenuOpen(false)}
                >
                  My Orders
                </Link>
              )}
              {isAdmin && (
                <Link
                  to="/admin"
                  className="block px-4 py-2 text-gray-700 hover:bg-gray-50 rounded-lg font-medium"
                  onClick={() => setIsMobileMenuOpen(false)}
                >
                  Admin Panel
                </Link>
              )}
            </div>
          </div>
        )}
      </header>

      {/* Spacer to prevent content from going under fixed header */}
      <div className={`${isMenuPage && onTypeChange ? 'h-28' : 'h-16'}`} />

      <AuthModal isOpen={isAuthModalOpen} onClose={() => setIsAuthModalOpen(false)} />
    </>
  );
};
