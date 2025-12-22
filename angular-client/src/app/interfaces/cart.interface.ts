export interface SelectedItemTopping {
    toppingId: number;
    price: number;
}

export interface ItemModifiers {
    size?: string;
    extraToppings: SelectedItemTopping[];
}

export interface CartItem {
    productId: number;
    modifiers: ItemModifiers;
    quantity: number;
    totalPrice: number;
}

export interface Cart {
    sessionId: string;
    items: CartItem[];
}
