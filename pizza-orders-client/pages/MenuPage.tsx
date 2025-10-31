import React, { useState, useEffect } from 'react';
import { Product } from '../types';
import { apiService } from '../services/apiService';
import { useCart } from '../context/CartContext';

const ProductCard: React.FC<{ product: Product, onAddToCart: (product: Product) => void }> = ({ product, onAddToCart }) => {
    return (
        <div className="bg-white rounded-lg shadow-md overflow-hidden flex flex-col transform hover:scale-105 transition-transform duration-300">
            <img className="w-full h-48 object-cover" src={product.imageUrl} alt={product.name} />
            <div className="p-4 flex flex-col flex-grow">
                <h3 className="text-lg font-bold text-secondary">{product.name}</h3>
                <p className="text-gray-600 mt-2 text-sm flex-grow">{product.description}</p>
                <div className="flex justify-between items-center mt-4">
                    <span className="text-xl font-bold text-primary">${product.price.toFixed(2)}</span>
                    <button onClick={() => onAddToCart(product)} className="px-4 py-2 bg-primary text-white text-sm font-semibold rounded-lg hover:bg-primary-dark transition-colors">
                        Add to Cart
                    </button>
                </div>
            </div>
        </div>
    );
};

const MenuPage: React.FC = () => {
    const [products, setProducts] = useState<Product[]>([]);
    const [loading, setLoading] = useState(true);
    const [filter, setFilter] = useState<'all' | 'veggie' | 'non-veggie' | 'drink'>('all');
    const { addToCart } = useCart();

    useEffect(() => {
        const fetchProducts = async () => {
            setLoading(true);
            try {
                const data = await apiService.getProducts();
                setProducts(data);
            } catch (error) {
                console.error("Failed to fetch products:", error);
            } finally {
                setLoading(false);
            }
        };
        fetchProducts();
    }, []);
    
    const filteredProducts = products.filter(product => {
        if (filter === 'all') return true;
        return product.category === filter;
    });

    return (
        <div className="bg-gray-100 min-h-screen">
            <div className="max-w-7xl mx-auto py-12 px-4 sm:px-6 lg:px-8">
                <div className="text-center mb-12">
                    <h1 className="text-4xl font-extrabold text-secondary">Our Menu</h1>
                    <p className="mt-2 text-lg text-gray-600">Choose from our wide range of delicious pizzas and drinks.</p>
                </div>

                <div className="flex justify-center mb-8">
                    <div className="flex flex-wrap space-x-2 bg-gray-200 p-1 rounded-full">
                        <button onClick={() => setFilter('all')} className={`px-4 py-2 text-sm font-medium rounded-full ${filter === 'all' ? 'bg-primary text-white shadow' : 'text-gray-700'}`}>All</button>
                        <button onClick={() => setFilter('veggie')} className={`px-4 py-2 text-sm font-medium rounded-full ${filter === 'veggie' ? 'bg-primary text-white shadow' : 'text-gray-700'}`}>Veggie</button>
                        <button onClick={() => setFilter('non-veggie')} className={`px-4 py-2 text-sm font-medium rounded-full ${filter === 'non-veggie' ? 'bg-primary text-white shadow' : 'text-gray-700'}`}>Non-Veggie</button>
                        <button onClick={() => setFilter('drink')} className={`px-4 py-2 text-sm font-medium rounded-full ${filter === 'drink' ? 'bg-primary text-white shadow' : 'text-gray-700'}`}>Drinks</button>
                    </div>
                </div>

                {loading ? (
                    <div className="text-center">
                        <div className="animate-spin rounded-full h-16 w-16 border-t-2 border-b-2 border-primary mx-auto"></div>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8">
                        {filteredProducts.map(product => (
                            <ProductCard key={product.id} product={product} onAddToCart={addToCart} />
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};

export default MenuPage;