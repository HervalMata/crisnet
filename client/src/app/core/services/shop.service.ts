import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';
import { ShopParams } from '../../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
    baseUrl = 'https://localhost:5255/api/';
    private http = inject(HttpClient);
    brands: string[] = [];
    types: string[] = [];

    getProducts(shopParams: ShopParams) {
        let params = new HttpParams();

        if (shopParams.brands.length > 0) {
            params = params.append('brands', shopParams.brands.join(','));
        }

        if (shopParams.types.length > 0) {
            params = params.append('types', shopParams.types.join(','));
        }

        if (shopParams.sort.length > 0) {
            params = params.append('sort', shopParams.sort);
        }

        if (shopParams.search.length > 0) {
            params = params.append('search', shopParams.search);
        }

        params = params.append('pageIndex', shopParams.pageNumber);
        params = params.append('pageSize', shopParams.pageSize);

        return this.http.get<Product[]>(this.baseUrl + 'products', { params});
    }

    getBrands() {
        if (this.brands.length > 0) return;
        return this.http.get<string[]>(this.baseUrl + 'products/brands').subscribe({
            next: response => this.brands = response,
        });
    }

    getTypes() {
        if (this.types.length > 0) return;
        return this.http.get<string[]>(this.baseUrl + 'products/types').subscribe({
            next: response => this.types = response,
        });
    }
}
