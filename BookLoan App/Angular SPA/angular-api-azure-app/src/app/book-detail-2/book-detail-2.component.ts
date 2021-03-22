import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Book } from '../models/Book';
import { combineLatest } from 'rxjs';
//import { map } from 'rxjs/internal/operators/map';
import { LookupService } from '../services/lookup.service';
//import { Genre } from '../models/Genre';


@Component({
  selector: 'book-detail-2',
  templateUrl: './book-detail-2.component.html',
  styleUrls: ['./book-detail-2.component.scss']
})
export class BookDetail2Component implements OnInit {

  book: Book = new Book();
  genres = [];
  id: any;
  selectedGenre: string;

  constructor(private api: ApiService, private lookupService: LookupService, 
      private route: ActivatedRoute,
      private activatedRoute: ActivatedRoute, 
      router: Router) {}

  ngOnInit() {
    if (!this.activatedRoute)
      return;
    this.id = this.activatedRoute.snapshot.params["id"];

    try {
      this.id = this.route.snapshot.data.pageData.id;
      this.book = new Book();
      this.book.title = this.route.snapshot.data.pageData.title;
      this.book.author = this.route.snapshot.data.pageData.author;
      this.book.yearPublished = this.route.snapshot.data.pageData.yearPublished;
      this.book.genre = this.route.snapshot.data.pageData.genre;
      this.book.edition = this.route.snapshot.data.pageData.edition;
      this.book.isbn = this.route.snapshot.data.pageData.isbn;
      this.book.location = this.route.snapshot.data.pageData.location;
      this.genres = this.route.snapshot.data.pageData.genres.map(g => g.genre);
    } catch (ex) {
    }

    this.selectedGenre = "Fiction";
    
    console.log("BookDetail2Component instantiated with the following id : " + this.id);
  }

  eventUpdateRecord(event)
  {
    this.api.updateBook(this.book).subscribe(res => {
      console.log("book " + this.id + " updated");
    })

  }

  eventSelection(event){
    this.selectedGenre = event;
    this.book.genre = event;
  }

  selectBookById(id) {
    this.api.getBook(id).subscribe((res: Book) => {
      this.book = res;
      console.log("read book " + id + " from API");
    })  
  }


}
