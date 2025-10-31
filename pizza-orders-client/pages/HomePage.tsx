import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Product } from '../types';
import { apiService } from '../services/apiService';
import { useCart } from '../context/CartContext';

const ProductCard: React.FC<{ product: Product }> = ({ product }) => {
    const { addToCart } = useCart();
    return (
        <div className="bg-white rounded-lg shadow-lg overflow-hidden transform hover:scale-105 transition-transform duration-300">
            <img className="w-full h-48 object-cover" src={product.imageUrl} alt={product.name} />
            <div className="p-6">
                <h3 className="text-xl font-bold text-secondary">{product.name}</h3>
                <p className="text-gray-600 mt-2 h-12">{product.description}</p>
                <div className="flex justify-between items-center mt-4">
                    <span className="text-2xl font-bold text-primary">${product.price.toFixed(2)}</span>
                    <button onClick={() => addToCart(product)} className="px-4 py-2 bg-primary text-white font-semibold rounded-lg hover:bg-primary-dark transition-colors">Add to Cart</button>
                </div>
            </div>
        </div>
    );
};

const HomePage: React.FC = () => {
    const [featuredProducts, setFeaturedProducts] = useState<Product[]>([]);

    useEffect(() => {
        const fetchProducts = async () => {
            const products = await apiService.getProducts();
            // Filter for only pizzas to be featured
            setFeaturedProducts(products.filter(p => p.category === 'veggie' || p.category === 'non-veggie').slice(0, 3));
        };
        fetchProducts();
    }, []);

    return (
        <div className="bg-light">
            {/* Hero Section */}
            <div className="relative bg-secondary text-white">
                <div className="absolute inset-0">
                    <img className="w-full h-full object-cover" src="https://picsum.photos/seed/pizzabg/1920/1080" alt="Pizza background" />
                    <div className="absolute inset-0 bg-black opacity-60"></div>
                </div>
                <div className="relative max-w-7xl mx-auto py-24 px-4 sm:py-32 sm:px-6 lg:px-8 text-center">
                    <h1 className="text-4xl font-extrabold tracking-tight sm:text-5xl lg:text-6xl">
                        <span className="block text-primary">Hot & Fresh</span>
                        <span className="block">Food Delivered to You</span>
                    </h1>
                    <p className="mt-6 max-w-lg mx-auto text-xl text-gray-300 sm:max-w-3xl">
                        Experience the taste of perfection. Made with the finest ingredients and delivered fast.
                    </p>
                    <div className="mt-10 max-w-sm mx-auto sm:max-w-none sm:flex sm:justify-center">
                        <Link to="/menu" className="w-full sm:w-auto px-8 py-3 border border-transparent text-base font-medium rounded-md text-white bg-primary hover:bg-primary-dark md:py-4 md:text-lg md:px-10 transition-transform transform hover:scale-105">
                            Order Now
                        </Link>
                    </div>
                </div>
            </div>

            {/* Featured Pizzas Section */}
            <div className="max-w-7xl mx-auto py-16 px-4 sm:px-6 lg:px-8">
                <div className="text-center">
                    <h2 className="text-3xl font-extrabold text-secondary sm:text-4xl">Our Most Popular Pizzas</h2>
                    <p className="mt-4 text-lg text-gray-600">Hand-picked by our customers. A must-try for everyone!</p>
                </div>
                <div className="mt-12 grid gap-8 md:grid-cols-2 lg:grid-cols-3">
                    {featuredProducts.map(product => (
                        <ProductCard key={product.id} product={product} />
                    ))}
                </div>
            </div>
        </div>
    );
};

export default HomePage;