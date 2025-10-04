import { Component, inject, OnInit } from '@angular/core';
import { ProductItemComponent } from './product-item/product-item.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatSelectionList, MatListOption } from '@angular/material/list';
import { FormsModule } from '@angular/forms';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { ShopService } from '../../core/services/shop.service';
import { MatDialog } from '@angular/material/dialog';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';
import { ShopParams } from '../../shared/models/shopParams';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';

@Component({
  selector: 'app-shop',
  imports: [
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator,
    FormsModule
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);
  products?: Product[];
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' }
  ];
  shopParams = new ShopParams();
  pageSizeOptions = [5, 10, 15, 20];
  pageEvent?: PageEvent;

  ngOnInit() {
    this.initializeShop();
  }

  initializeShop() {
    this.getProducts();
    this.shopService.getBrands();
    this.shopService.getTypes();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: response => this.products = response,
      error: error => console.log(error)
    });
  }

  onSortChange(event: any) {
    this.shopParams.pageNumber = 1;
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.shopParams.sort = selectedOption.value;
      this.getProducts();
    }
  }

  onSearchChange() {
    this.shopParams.pageNumber = 1;
    this.shopService.getProducts(this.shopParams).subscribe({
      next: response => this.products = response,
      error: error => console.log(error)
    });
  }

  openFilterDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      width: '500px',
      data: { selectBrands: this.shopParams.brands, selectTypes: this.shopParams.types },
    });
    dialogRef.afterClosed().subscribe(
      result => {
      if (result) {
        this.shopParams.pageNumber = 1;
        this.shopParams.brands = result.selectBrands;
        this.shopParams.types = result.selectTypes;
        this.getProducts();
      }
      },
    );
  }

  handlePageEvent(event: PageEvent) {
    this.shopParams.pageNumber = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }
}
