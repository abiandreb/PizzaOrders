import React, { useState, useEffect, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { useCart } from '../context/CartContext';
import { apiService } from '../services/apiService';
import { ConstructorOptions, ConstructorOption, Product } from '../types';

const ConstructorPage: React.FC = () => {
    const [options, setOptions] = useState<ConstructorOptions | null>(null);
    const [loading, setLoading] = useState(true);
    const { addToCart } = useCart();
    const navigate = useNavigate();

    // State for user selections
    const [selectedBase, setSelectedBase] = useState<ConstructorOption | null>(null);
    const [selectedSauce, setSelectedSauce] = useState<ConstructorOption | null>(null);
    const [selectedCheese, setSelectedCheese] = useState<ConstructorOption | null>(null);
    const [selectedVeggies, setSelectedVeggies] = useState<ConstructorOption[]>([]);
    const [selectedMeats, setSelectedMeats] = useState<ConstructorOption[]>([]);
    
    useEffect(() => {
        const fetchOptions = async () => {
            try {
                const data = await apiService.getConstructorOptions();
                setOptions(data);
                // Set default selections
                if (data.bases.length > 0) setSelectedBase(data.bases[0]);
                if (data.sauces.length > 0) setSelectedSauce(data.sauces[0]);
                if (data.cheeses.length > 0) setSelectedCheese(data.cheeses[0]);
            } catch (error) {
                console.error("Failed to fetch constructor options", error);
            } finally {
                setLoading(false);
            }
        };
        fetchOptions();
    }, []);

    const handleCheckboxChange = (option: ConstructorOption, type: 'veggie' | 'meat') => {
        const state = type === 'veggie' ? selectedVeggies : selectedMeats;
        const setState = type === 'veggie' ? setSelectedVeggies : setSelectedMeats;
        
        if (state.find(item => item.id === option.id)) {
            setState(state.filter(item => item.id !== option.id));
        } else {
            setState([...state, option]);
        }
    };

    const totalPrice = useMemo(() => {
        let total = 0;
        if (selectedBase) total += selectedBase.price;
        if (selectedSauce) total += selectedSauce.price;
        if (selectedCheese) total += selectedCheese.price;
        selectedVeggies.forEach(v => total += v.price);
        selectedMeats.forEach(m => total += m.price);
        return total;
    }, [selectedBase, selectedSauce, selectedCheese, selectedVeggies, selectedMeats]);
    
    const handleAddToCart = () => {
        if (!selectedBase || !selectedSauce || !selectedCheese) {
            alert('Please select a base, sauce, and cheese.');
            return;
        }
        
        const allToppings = [...selectedVeggies, ...selectedMeats];
        const description = [selectedBase.name, selectedSauce.name, selectedCheese.name, ...allToppings.map(t => t.name)].join(', ');

        const customPizza: Product = {
            id: `custom-${new Date().getTime()}`,
            name: 'Custom Built Pizza',
            description: description,
            price: totalPrice,
            imageUrl: 'https://picsum.photos/seed/custompizza/400/300',
            category: 'custom',
        };

        addToCart(customPizza);
        navigate('/menu');
    };

    if (loading) return <div className="text-center py-10"><div className="animate-spin rounded-full h-16 w-16 border-t-2 border-b-2 border-primary mx-auto"></div></div>;
    if (!options) return <div className="text-center py-10">Could not load pizza options.</div>;

    const renderRadioGroup = (title: string, items: ConstructorOption[], selected: ConstructorOption | null, setSelected: (item: ConstructorOption) => void) => (
        <div className="mb-6">
            <h3 className="text-xl font-bold text-secondary mb-3">{title}</h3>
            <div className="space-y-2">
                {items.map(item => (
                    <label key={item.id} className="flex items-center p-3 bg-gray-50 rounded-lg border has-[:checked]:bg-blue-50 has-[:checked]:border-primary">
                        <input type="radio" name={title} value={item.id} checked={selected?.id === item.id} onChange={() => setSelected(item)} className="h-4 w-4 text-primary focus:ring-primary border-gray-300"/>
                        <span className="ml-3 text-gray-700">{item.name}</span>
                        <span className="ml-auto font-medium text-gray-800">${item.price.toFixed(2)}</span>
                    </label>
                ))}
            </div>
        </div>
    );
    
    const renderCheckboxGroup = (title: string, items: ConstructorOption[], type: 'veggie' | 'meat') => (
        <div className="mb-6">
            <h3 className="text-xl font-bold text-secondary mb-3">{title}</h3>
            <div className="grid grid-cols-2 gap-2">
                {items.map(item => (
                    <label key={item.id} className="flex items-center p-3 bg-gray-50 rounded-lg border has-[:checked]:bg-blue-50 has-[:checked]:border-primary">
                        <input type="checkbox" onChange={() => handleCheckboxChange(item, type)} className="h-4 w-4 text-primary focus:ring-primary border-gray-300 rounded"/>
                        <span className="ml-3 text-gray-700">{item.name}</span>
                        <span className="ml-auto font-medium text-gray-800">${item.price.toFixed(2)}</span>
                    </label>
                ))}
            </div>
        </div>
    );

    return (
        <div className="bg-gray-100 py-12">
            <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="text-center mb-10">
                    <h1 className="text-4xl font-extrabold text-secondary">Build Your Own Pizza</h1>
                    <p className="mt-2 text-lg text-gray-600">Create your masterpiece from our fresh ingredients.</p>
                </div>
                <div className="bg-white p-8 rounded-lg shadow-lg">
                    {renderRadioGroup('Choose Your Crust', options.bases, selectedBase, setSelectedBase)}
                    {renderRadioGroup('Choose Your Sauce', options.sauces, selectedSauce, setSelectedSauce)}
                    {renderRadioGroup('Choose Your Cheese', options.cheeses, selectedCheese, setSelectedCheese)}
                    <h3 className="text-2xl font-bold text-secondary mt-8 mb-4 border-b pb-2">Add Toppings</h3>
                    {renderCheckboxGroup('Veggies', options.veggies, 'veggie')}
                    {renderCheckboxGroup('Meats', options.meats, 'meat')}

                    <div className="mt-8 sticky bottom-0 bg-white py-4 -mx-8 px-8 border-t">
                        <div className="flex justify-between items-center">
                            <div>
                                <span className="text-2xl font-bold text-secondary">Total:</span>
                                <span className="text-3xl font-extrabold text-primary ml-2">${totalPrice.toFixed(2)}</span>
                            </div>
                            <button onClick={handleAddToCart} className="px-8 py-3 bg-primary text-white font-bold rounded-lg hover:bg-primary-dark transition-colors text-lg">
                                Add to Cart
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ConstructorPage;