import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Book } from '../models/Book';
import { combineLatest, Subscription } from 'rxjs';
import { LookupService } from '../services/lookup.service';


@Component({
  selector: 'book-detail-3',
  templateUrl: './book-detail-3.component.html',
  styleUrls: ['./book-detail-3.component.scss']
})
export class BookDetail3Component implements OnInit {

  heading = "Book Details";
  
  book: Book = new Book();
  genres = [];
  id: any;
  selectedGenre: string;
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

    console.log("BookDetailComponent instantiated with the following id : " + this.id);
    
    const getBook$ = this.api.getBook(this.id);
    const getGenres$ = this.lookupService.getGenres();

    this.subscription = combineLatest(
      [
        getBook$, 
        getGenres$
      ]
    ).subscribe(([book, genres]) => {
        if (!book || !genres)
          return;
        console.log("number of genres  = " + genres.length);
        console.log("book name = " + book['title']);        

       this.genres = genres.map(g => g.genre);
       this.book = book;  
       this.selectedGenre = this.book.genre;
       this.selectedGenre = this.selectedGenre.charAt(0).toUpperCase() + this.selectedGenre.slice(1);
      }
    );
  }

  ngOnDestroy()
  {
    if (!this.subscription)
      return;
    this.subscription.unsubscribe();
  }

  eventUpdateRecord(event)
  {
    this.api.updateBook(this.book).subscribe(res => {
      console.log("book " + this.id + " updated");
    })

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

  eventBack(event)
  {
    this.router.navigate(['books']);     
  }

}
