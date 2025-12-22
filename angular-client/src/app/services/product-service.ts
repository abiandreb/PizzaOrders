import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {catchError, Observable, of, throwError} from 'rxjs';
import {ProductsResponse} from '../interfaces/ProductResponse';

import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProductService {

  private baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getProductsByType(type: number): Observable<ProductsResponse[]>{

    const options =
      { params: new HttpParams().set('productType', type.toString()) };

    return this.http.get<ProductsResponse[]>(this.baseUrl + "/Product", options).pipe(
      catchError(err => {
        console.error('Error occurred', err);
        return throwError(err);
      })
    );
  }
}
