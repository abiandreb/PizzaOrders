export interface ProductsResponse {
  id: number;
  name: string;
  description: string;
  price: number;
  hasToppings: boolean;
  imageUrl: string;
  productProperties: ProductProperties;
  productType: ProductType;
}

export interface ProductProperties {
  sizeOptions?: SizeOption[];
  defaultToppingIds?: number[];
  availableExtraToppingIds?: number[];
}

export interface SizeOption {
  size: string;
  price: number;
}

export enum ProductType {
  Pizza,
  Drink,
  Starter
}
