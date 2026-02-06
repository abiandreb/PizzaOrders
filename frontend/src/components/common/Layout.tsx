import React from 'react';
import type { ReactNode } from 'react';
import { Header } from './Header';
import { Footer } from './Footer';
import { CartSidebar } from './CartSidebar';

interface LayoutProps {
  children: ReactNode;
}

export const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <div className="flex flex-col min-h-screen bg-gray-50">
      <Header />
      <main className="flex-1">{children}</main>
      <Footer />
      <CartSidebar />
    </div>
  );
};
