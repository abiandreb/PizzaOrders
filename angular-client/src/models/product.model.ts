export enum ProductType {
  Pizza,
  Drink,
  Starter
}

export interface Topping {
  id: number;
  name: string;
  description: string;
  stock: number;
  price: number;
}

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  productType: ProductType;
  imageUrl: string;
  availableSizes?: string[];
  toppings?: Topping[];
}
