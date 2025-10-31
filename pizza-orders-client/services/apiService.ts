import { Product, Order, CartItem, User, ConstructorOptions } from '../types';
import { authService } from './authService';

// Mock Data
const PRODUCTS: Product[] = [
  { id: 'p1', name: 'Margherita', description: 'Classic delight with 100% real mozzarella cheese', price: 8.99, imageUrl: 'https://picsum.photos/seed/margherita/400/300', category: 'veggie' },
  { id: 'p2', name: 'Pepperoni', description: 'A classic pepperoni pizza with a zesty tomato sauce', price: 10.99, imageUrl: 'https://picsum.photos/seed/pepperoni/400/300', category: 'non-veggie' },
  { id: 'p3', name: 'Veggie Supreme', description: 'A delicious mix of fresh vegetables and mozzarella', price: 11.99, imageUrl: 'https://picsum.photos/seed/veggie/400/300', category: 'veggie' },
  { id: 'p4', name: 'Meat Lover\'s', description: 'Packed with pepperoni, sausage, ham, and bacon', price: 12.99, imageUrl: 'https://picsum.photos/seed/meat/400/300', category: 'non-veggie' },
  { id: 'p5', name: 'BBQ Chicken', description: 'Grilled chicken, red onions, and cilantro with BBQ sauce', price: 12.49, imageUrl: 'https://picsum.photos/seed/bbq/400/300', category: 'non-veggie' },
  { id: 'p6', name: 'Hawaiian', description: 'A tropical blend of ham, pineapple, and mozzarella', price: 11.49, imageUrl: 'https://picsum.photos/seed/hawaiian/400/300', category: 'non-veggie' },
  { id: 'p7', name: 'Mushroom Truffle', description: 'Creamy truffle sauce with a mix of exotic mushrooms', price: 13.99, imageUrl: 'https://picsum.photos/seed/mushroom/400/300', category: 'veggie' },
  { id: 'p8', name: 'Spicy Sausage', description: 'Hot Italian sausage, jalapenos, and red pepper flakes', price: 12.99, imageUrl: 'https://picsum.photos/seed/spicy/400/300', category: 'non-veggie' },
  { id: 'd1', name: 'Coca-Cola', description: 'A 330ml can of refreshing Coca-Cola', price: 1.99, imageUrl: 'https://picsum.photos/seed/coke/400/300', category: 'drink' },
];

const CONSTRUCTOR_DATA: ConstructorOptions = {
    bases: [
        { id: 'b1', name: 'Thin Crust', price: 2.00, type: 'base' },
        { id: 'b2', name: 'Thick Crust', price: 2.50, type: 'base' },
        { id: 'b3', name: 'Stuffed Crust', price: 3.50, type: 'base' },
    ],
    sauces: [
        { id: 's1', name: 'Tomato Sauce', price: 1.00, type: 'sauce' },
        { id: 's2', name: 'BBQ Sauce', price: 1.50, type: 'sauce' },
        { id: 's3', name: 'White Garlic Sauce', price: 1.50, type: 'sauce' },
    ],
    cheeses: [
        { id: 'c1', name: 'Mozzarella', price: 1.50, type: 'cheese' },
        { id: 'c2', name: 'Extra Mozzarella', price: 2.50, type: 'cheese' },
    ],
    veggies: [
        { id: 'v1', name: 'Onions', price: 0.50, type: 'veggie' },
        { id: 'v2', name: 'Bell Peppers', price: 0.50, type: 'veggie' },
        { id: 'v3', name: 'Mushrooms', price: 0.75, type: 'veggie' },
        { id: 'v4', name: 'Olives', price: 0.75, type: 'veggie' },
        { id: 'v5', name: 'Jalapenos', price: 0.75, type: 'veggie' },
    ],
    meats: [
        { id: 'm1', name: 'Pepperoni', price: 1.50, type: 'meat' },
        { id: 'm2', name: 'Sausage', price: 1.50, type: 'meat' },
        { id: 'm3', name: 'Bacon', price: 1.75, type: 'meat' },
        { id: 'm4', name: 'Chicken', price: 1.75, type: 'meat' },
    ]
};

let MOCK_ORDERS: Order[] = [
    { id: 'o1', userId: '2', items: [{ product: PRODUCTS[1], quantity: 2 }], total: 21.98, status: 'Delivered', date: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000).toISOString(), customer: { name: 'Regular User', address: '123 Pizza Lane', phone: '555-1234' } },
    { id: 'o2', userId: '2', items: [{ product: PRODUCTS[2], quantity: 1 }, { product: PRODUCTS[0], quantity: 1 }], total: 20.98, status: 'Delivered', date: new Date(Date.now() - 5 * 24 * 60 * 60 * 1000).toISOString(), customer: { name: 'Regular User', address: '123 Pizza Lane', phone: '555-1234' } },
];

const apiRequest = <T,>(requestFunc: () => T, delay = 500): Promise<T> => {
    return new Promise((resolve) => {
        setTimeout(() => {
            resolve(requestFunc());
        }, delay);
    });
};


export const apiService = {
    getProducts: (): Promise<Product[]> => {
        return apiRequest(() => PRODUCTS);
    },

    getConstructorOptions: (): Promise<ConstructorOptions> => {
        return apiRequest(() => CONSTRUCTOR_DATA);
    },

    getUserOrders: (userId: string): Promise<Order[]> => {
        return apiRequest(() => MOCK_ORDERS.filter(o => o.userId === userId));
    },

    getAllOrders: (): Promise<Order[]> => {
        return apiRequest(() => [...MOCK_ORDERS].sort((a,b) => new Date(b.date).getTime() - new Date(a.date).getTime()));
    },

    placeOrder: (cart: CartItem[], user: User | null, customerDetails: {name: string, address: string, phone: string}): Promise<Order> => {
        return apiRequest(() => {
            const newOrder: Order = {
                id: `o${MOCK_ORDERS.length + 1}`,
                userId: user ? user.id : 'guest',
                items: cart,
                total: cart.reduce((sum, item) => sum + item.product.price * item.quantity, 0),
                status: 'Received',
                date: new Date().toISOString(),
                customer: customerDetails,
            };
            MOCK_ORDERS.push(newOrder);
            return newOrder;
        }, 1000);
    },

    updateOrderStatus: (orderId: string, status: Order['status']): Promise<Order> => {
        return apiRequest(() => {
            const orderIndex = MOCK_ORDERS.findIndex(o => o.id === orderId);
            if (orderIndex === -1) {
                throw new Error('Order not found');
            }
            MOCK_ORDERS[orderIndex].status = status;
            return MOCK_ORDERS[orderIndex];
        }, 300);
    }
};