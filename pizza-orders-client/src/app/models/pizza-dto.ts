export interface PizzaDto {
  id?: number;          // int?  -> optional number
  name: string;         // string -> required string
  description: string;  // string -> required string
  price: number;        // decimal -> number
}
