import {Component, OnInit} from '@angular/core';
import {ProductService} from '../services/product-service';
import {ProductsResponse} from '../interfaces/ProductResponse';
import {JsonPipe} from '@angular/common';

@Component({
  selector: 'app-products',
  imports: [
    JsonPipe
  ],
  templateUrl: './products.html',
  styleUrl: './products.css',
})
export class Products implements OnInit {

  public productResponse: ProductsResponse[] = [];

  constructor(private productService: ProductService) {
  }

  ngOnInit() {
    this.productService.getProductsByType(0).subscribe(products => {
      this.productResponse = products;
    })
  }
}
