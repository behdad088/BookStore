export interface Edit{
    Id: number;
    Author: string;
    Title: string;
    Price: string[];
    inStock: string;
}

export interface Book{
    Id: number;
    Author: string;
    Title: string;
    Price: number;
    inStock: number;
}

export interface Product {
  id: number;
  name: string;
}