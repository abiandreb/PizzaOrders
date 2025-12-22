import { Component, ChangeDetectionStrategy, signal, inject, OnInit, output } from '@angular/core';
import { Product, ProductType } from '../../models/product.model';
import { ApiService } from '../../services/api.service';
import { ProductCardComponent } from '../product-card/product-card.component';
import { NgOptimizedImage } from '@angular/common';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ProductCardComponent, NgOptimizedImage]
})
export class ProductListComponent implements OnInit {
  apiService = inject(ApiService);
  productSelected = output<Product>();

  products = signal<Product[]>([]);
  isLoading = signal(true);
  selectedType = signal<ProductType>(ProductType.Pizza);
  
  // Expose ProductType enum to the template
  productTypes = ProductType;

  ngOnInit() {
    this.loadProducts();
  }

  selectProductType(type: ProductType) {
    this.selectedType.set(type);
    this.loadProducts();
  }

  async loadProducts() {
    this.isLoading.set(true);
    this.products.set([]);
    const products = await this.apiService.getProducts(this.selectedType());
    this.products.set(products);
    this.isLoading.set(false);
  }

  onProductSelect(product: Product) {
    this.productSelected.emit(product);
  }
}
