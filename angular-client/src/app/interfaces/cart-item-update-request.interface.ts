export interface CartItemUpdateRequest {
    productId: number;
    quantity: number;
    toppingIds?: number[];
}
