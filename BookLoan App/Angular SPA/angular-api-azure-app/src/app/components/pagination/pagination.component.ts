import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { PageEvent } from '@angular/material';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent implements OnInit {

  _currentPage: number = 0;
  _numberOfPages: number;
  _itemsPerPage: number = 10;
  _totalItems: number;
  _dataSource: any;
  _pagedOutput: any;

  @Input("itemsPerPage")
  set itemsPerPage(value: number) 
  { 
    this._itemsPerPage = value; 
    this.paginator.pageSize = value; 
  }

  @Input("totalItens")
  set totalItems(value: number) 
  { 
    this._totalItems = value; 
    this.paginator.length = value;  
  } 

  get itemsPerPage() { return this._itemsPerPage; }
  get numberOfPages() { return Math.ceil(this._totalItems / this._itemsPerPage); }
  get currentPage() { return this._currentPage; }
  get totalItems() { return this._totalItems; }

  @Input("dataSource")
  set dataSource(value: any) 
  { 
    this._dataSource = value; 
  } 

  @Output() pageChange: EventEmitter<PageEvent> = new EventEmitter();
  @Output() pagedOutputChange: EventEmitter<any> = new EventEmitter();

  @ViewChild("paginator", {static: true}) paginator;

  constructor() { }

  ngOnInit() {     
  }

  onSelect(value: PageEvent)
  {
    this._currentPage = this.paginator.pageIndex;
    this._numberOfPages = this.paginator.getNumberOfPages();
    this._itemsPerPage = this.paginator.pageSize;
    this.pageChange.emit(this.paginator);
    this.getPagedView();
  }

  getPagedView()
  {
    this._pagedOutput = [];
    let itemIndex = 0;
    this._dataSource.forEach(element => {
        if ((itemIndex >= this.paginator.pageIndex*this.paginator.pageSize) && 
            (itemIndex < (this.paginator.pageIndex+1)*this.paginator.pageSize))
        {
            this._pagedOutput.push(element);
        }
        itemIndex++;
    }); 
    this.pagedOutputChange.emit(this._pagedOutput);
  }

}
