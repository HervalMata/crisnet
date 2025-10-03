import { Component, inject, OnInit } from '@angular/core';
import { HeaderComponent } from './layout/header/header.component';
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/product';
import { Pagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  imports: [HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  baseUrl = 'https://localhost:5255/api/';
  private http = inject(HttpClient);
  title = 'CrisNet';
  products: Product[] = [];

  ngOnInit() {
      this.http.get<any>(this.baseUrl + 'products').subscribe({
        next: response => this.products = response,

        error: error => console.log(error),
        complete: () => console.log('Request completed', this.products.length)
      });
      //console.log("Products", response.length);
  }
}
