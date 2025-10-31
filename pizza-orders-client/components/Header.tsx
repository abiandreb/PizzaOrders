import React, { useState } from 'react';
import { NavLink, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { useCart } from '../context/CartContext';
import { PizzaIcon, ShoppingCartIcon, UserCircleIcon, MenuIcon, XIcon, LogoutIcon } from './Icons';

const Header: React.FC = () => {
  const { user, logout } = useAuth();
  const { cartCount } = useCart();
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isProfileOpen, setIsProfileOpen] = useState(false);

  const navLinkClass = ({ isActive }: { isActive: boolean }) =>
    `px-3 py-2 rounded-md text-sm font-medium transition-colors ${
      isActive ? 'bg-primary text-white' : 'text-gray-300 hover:bg-secondary-light hover:text-white'
    }`;

  const mobileNavLinkClass = ({ isActive }: { isActive: boolean }) =>
    `block px-3 py-2 rounded-md text-base font-medium transition-colors ${
      isActive ? 'bg-primary text-white' : 'text-gray-300 hover:bg-secondary-light hover:text-white'
    }`;
    
  return (
    <nav className="bg-secondary shadow-lg sticky top-0 z-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          <div className="flex items-center">
            <Link to="/" className="flex-shrink-0 flex items-center text-white">
              <PizzaIcon className="h-8 w-8 text-primary" />
              <span className="ml-2 text-xl font-bold">PizzaOrders</span>
            </Link>
            <div className="hidden md:block">
              <div className="ml-10 flex items-baseline space-x-4">
                <NavLink to="/" className={navLinkClass}>Home</NavLink>
                <NavLink to="/menu" className={navLinkClass}>Menu</NavLink>
                <NavLink to="/constructor" className={navLinkClass}>Pizza Builder</NavLink>
                <NavLink to="/contact" className={navLinkClass}>Contact</NavLink>
                {user && <NavLink to="/orders" className={navLinkClass}>My Orders</NavLink>}
                {user?.role === 'admin' && <NavLink to="/admin" className={navLinkClass}>Admin</NavLink>}
              </div>
            </div>
          </div>
          <div className="hidden md:block">
            <div className="ml-4 flex items-center md:ml-6 space-x-4">
              <Link to="/checkout" className="relative p-1 rounded-full text-gray-400 hover:text-white focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-secondary focus:ring-white">
                <ShoppingCartIcon className="h-6 w-6" />
                {cartCount > 0 && (
                  <span className="absolute -top-2 -right-2 inline-flex items-center justify-center px-2 py-1 text-xs font-bold leading-none text-white transform translate-x-1/2 -translate-y-1/2 bg-accent rounded-full">
                    {cartCount}
                  </span>
                )}
              </Link>
              {user ? (
                <div className="ml-3 relative">
                  <div>
                    <button onClick={() => setIsProfileOpen(!isProfileOpen)} className="max-w-xs bg-secondary rounded-full flex items-center text-sm focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-secondary focus:ring-white" id="user-menu-button" aria-expanded="false" aria-haspopup="true">
                      <span className="sr-only">Open user menu</span>
                      <UserCircleIcon className="h-8 w-8 text-gray-400"/>
                    </button>
                  </div>
                  {isProfileOpen && (
                    <div className="origin-top-right absolute right-0 mt-2 w-48 rounded-md shadow-lg py-1 bg-white ring-1 ring-black ring-opacity-5 focus:outline-none" role="menu" aria-orientation="vertical" aria-labelledby="user-menu-button">
                      <div className="px-4 py-2 text-sm text-gray-700 border-b">{user.name}</div>
                      <a href="#" onClick={(e) => { e.preventDefault(); logout(); setIsProfileOpen(false); }} className="flex items-center w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100" role="menuitem">
                        <LogoutIcon className="h-4 w-4 mr-2" />
                        Sign out
                      </a>
                    </div>
                  )}
                </div>
              ) : (
                <NavLink to="/auth" className={navLinkClass}>Login</NavLink>
              )}
            </div>
          </div>
          <div className="-mr-2 flex md:hidden">
            <Link to="/checkout" className="relative p-1 mr-2 rounded-full text-gray-400 hover:text-white focus:outline-none">
              <ShoppingCartIcon className="h-6 w-6" />
              {cartCount > 0 && (
                <span className="absolute -top-2 -right-2 inline-flex items-center justify-center px-2 py-1 text-xs font-bold leading-none text-white transform translate-x-1/2 -translate-y-1/2 bg-accent rounded-full">{cartCount}</span>
              )}
            </Link>
            <button onClick={() => setIsMenuOpen(!isMenuOpen)} type="button" className="bg-secondary-light inline-flex items-center justify-center p-2 rounded-md text-gray-400 hover:text-white hover:bg-secondary focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-secondary focus:ring-white" aria-controls="mobile-menu" aria-expanded="false">
              <span className="sr-only">Open main menu</span>
              {isMenuOpen ? <XIcon className="block h-6 w-6" /> : <MenuIcon className="block h-6 w-6" />}
            </button>
          </div>
        </div>
      </div>

      {isMenuOpen && (
        <div className="md:hidden" id="mobile-menu">
          <div className="px-2 pt-2 pb-3 space-y-1 sm:px-3">
            <NavLink to="/" className={mobileNavLinkClass} onClick={() => setIsMenuOpen(false)}>Home</NavLink>
            <NavLink to="/menu" className={mobileNavLinkClass} onClick={() => setIsMenuOpen(false)}>Menu</NavLink>
            <NavLink to="/constructor" className={mobileNavLinkClass} onClick={() => setIsMenuOpen(false)}>Pizza Builder</NavLink>
            <NavLink to="/contact" className={mobileNavLinkClass} onClick={() => setIsMenuOpen(false)}>Contact</NavLink>
            {user && <NavLink to="/orders" className={mobileNavLinkClass} onClick={() => setIsMenuOpen(false)}>My Orders</NavLink>}
            {user?.role === 'admin' && <NavLink to="/admin" className={mobileNavLinkClass} onClick={() => setIsMenuOpen(false)}>Admin Panel</NavLink>}
             {user ? (
                <div className="pt-4 pb-3 border-t border-secondary-light">
                    <div className="flex items-center px-5">
                      <UserCircleIcon className="h-10 w-10 text-gray-400"/>
                      <div className="ml-3">
                        <div className="text-base font-medium leading-none text-white">{user.name}</div>
                        <div className="text-sm font-medium leading-none text-gray-400">{user.email}</div>
                      </div>
                    </div>
                    <div className="mt-3 px-2 space-y-1">
                      <a href="#" onClick={(e) => { e.preventDefault(); logout(); setIsMenuOpen(false); }} className="flex items-center w-full text-left px-3 py-2 rounded-md text-base font-medium text-gray-400 hover:text-white hover:bg-secondary-light">
                        <LogoutIcon className="h-5 w-5 mr-2" />
                        Sign out
                      </a>
                    </div>
                </div>
              ) : (
                <NavLink to="/auth" className={mobileNavLinkClass} onClick={() => setIsMenuOpen(false)}>Login</NavLink>
              )}
          </div>
        </div>
      )}
    </nav>
  );
};

export default Header;