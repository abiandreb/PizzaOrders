import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {catchError, Observable, of, throwError} from 'rxjs';
import {ProductsResponse} from '../interfaces/ProductResponse';

@Injectable({
  providedIn: 'root',
})
export class ProductService {

  private baseUrl: string = 'http://localhost:5062';

  constructor(private http: HttpClient) {}

  getProductsByType(type: number): Observable<ProductsResponse[]>{

    const options =
      { params: new HttpParams().set('name', type) };

    return this.http.get<ProductsResponse[]>(this.baseUrl + "/Product").pipe(
      catchError(err => {
        console.error('Error occurred', err);
        return throwError(err);
      })
    );
  }
}
