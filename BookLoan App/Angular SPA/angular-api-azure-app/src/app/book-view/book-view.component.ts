import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Book } from '../models/Book';
import { combineLatest, Subscription } from 'rxjs';
//import { map } from 'rxjs/internal/operators/map';
import { LookupService } from '../services/lookup.service';
//import { Genre } from '../models/Genre';


@Component({
  selector: 'book-view',
  templateUrl: './book-view.component.html',
  styleUrls: ['./book-view.component.scss']
})
export class BookViewComponent implements OnInit {

  heading = "Book View";
  
  book: Book = new Book();
  genres = [];
  id: any;
  selectedGenre: string;
  loanstatus: string;
  subscription: Subscription;

  constructor(private api: ApiService, private lookupService: LookupService, 
      private activatedRoute: ActivatedRoute, 
      private router: Router) {}

  ngOnInit() {
    if (!this.activatedRoute)
      return;
    this.id = this.activatedRoute.snapshot.params["id"];
    this.genres = ["Childrens", "Family sage", "Fantasy", "Fiction", "Folklore", "Horror", "Mystery", "Non-fiction", "Sci-fi"];
    this.selectedGenre = "Fiction";

    console.log("BookViewComponent instantiated with the following id : " + this.id);
    
    const getBook$ = this.api.getBook(this.id);
    const getBookLoanStatus$ = this.api.getBookLoanStatus(this.id);
    const getGenres$ = this.lookupService.getGenres();

    this.subscription = combineLatest(
      [
        getBook$, 
        getBookLoanStatus$,
        getGenres$
      ]
    ).subscribe(([book, bookloanstatus, genres]) => {
          if (!book || !bookloanstatus || !genres)
          return;
        console.log("number of genres  = " + genres.length);
        console.log("book name = " + book.title);        
        if (bookloanstatus)
          console.log("loan status = " + bookloanstatus.status);        

       this.genres = genres.map(g => g.genre);
       this.book = book;  
       this.selectedGenre = this.book.genre;
       this.selectedGenre = this.selectedGenre.charAt(0).toUpperCase() + this.selectedGenre.slice(1);
       this.loanstatus = bookloanstatus.status;
      }
    );

  }

  ngOnDestroy()
  {
    if (!this.subscription)
      return;
    this.subscription.unsubscribe();
  }

  eventBorrowRecord(event)
  {
    this.router.navigate(['book-borrow', this.book.id]);     
  }

  eventReturnRecord(event)
  {
    this.router.navigate(['book-return', this.book.id]);     
  }

  eventBack(event)
  {
    this.router.navigate(['book-search']);     
  }

  eventSelection(value){
    this.selectedGenre = value.currentTarget.innerText;
    this.book.genre = value.currentTarget.innerText;
  }

  selectBookById(id) {
    this.api.getBook(id).subscribe((res: Book) => {
      this.book = res;
      console.log("read book " + id + " from API");
    })  
  }

}
