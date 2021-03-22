import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Book } from '../models/Book';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/internal/operators/map';
import { LookupService } from '../services/lookup.service';
import { Genre } from '../models/Genre';


@Component({
  selector: 'book-detail',
  templateUrl: './book-detail.component.html',
  styleUrls: ['./book-detail.component.scss']
})
export class BookDetailComponent implements OnInit {

  book: Book = new Book();
  genres = [];
  id: any;
  selectedGenre: string;

  constructor(private api: ApiService, private lookupService: LookupService, 
      private activatedRoute: ActivatedRoute, 
      router: Router) {}

  ngOnInit() {
    if (!this.activatedRoute)
      return;
    this.id = this.activatedRoute.snapshot.params["id"];
    this.genres = ["Childrens", "Family sage", "Fantasy", "Fiction", "Folklore", "Horror", "Mystery", "Non-fiction", "Sci-fi"];
    this.selectedGenre = "Fiction";
    // this.activatedRoute.queryParams.subscribe(params => {
    //   this.id = params['id'];
    // });

    //var id = this.activatedRoute.snapshot.params["id"];
    console.log("BookDetailComponent instantiated with the following id : " + this.id);

    //let id = this.activatedRoute.snapshot.paramMap.get('id');
    // this.api.getBook(this.id).subscribe((res: Book) => {
    //   this.book = res;  

    //   this.selectedGenre = this.book.genre;
    //   this.selectedGenre = this.selectedGenre.charAt(0).toUpperCase() + this.selectedGenre.slice(1);
      
    //   this.lookupService.getGenres().subscribe((gen: Genre[]) => {
    //     this.genres = gen.map(g => g.genre);
    //   });
      
    //   console.log("read book " + this.id + " from API");
    // });
    
    const getBook$ = this.api.getBook(this.id);
    const getGenres$ = this.lookupService.getGenres();

    combineLatest(
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
