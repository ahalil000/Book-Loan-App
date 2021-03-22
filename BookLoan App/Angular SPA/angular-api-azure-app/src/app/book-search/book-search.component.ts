import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, fromEvent, of, Subject, Subscription } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';
import { debounceTime, delay, distinctUntilChanged, map, switchMap, tap } from 'rxjs/operators';
import { Book } from '../models/Book';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-book-search',
  templateUrl: './book-search.component.html',
  styleUrls: ['./book-search.component.scss']
})
export class BookSearchComponent implements OnInit {

  book = {}
  books: Book[] = [];
  searchResults: Book[] = [];
  searchQueryUpdate = new Subject<string>();
  hasSearchResults$ = new BehaviorSubject<boolean>(false);
  subscription: Subscription;

  public consoleMessages: string[] = [];

  constructor(private api: ApiService, private router: Router) {}

  ngOnInit() {
    this.subscription = this.api.getBooks().pipe(delay(3000)).subscribe((res: Book[]) => {
      this.books = res
      console.log("read books from API");
    });

    this.getBooks();
  }

  ngOnDestroy()
  {
    this.subscription.unsubscribe();
  }

  // getMatchingBooks(val: string): Observable<Book[]>
  // {
  //   let book$:Observable<Book[]> = new Observable<Book[]>();   
  //   this.api.getBooks().pipe(delay(1000)).subscribe((res: Book[]) => {      
  //     book$ = of(res.filter(b => b.title.includes(val)));
  //     return book$;
  //     console.log("read books from API");
  //   });
  // }


  filteredBooks = keys => 
      this.books.filter(b => b.title.includes(keys));
  
  getBooks()
  {
    // Debounce search.
    this.searchQueryUpdate.pipe(
      debounceTime(400),
      distinctUntilChanged())
      .subscribe(value => {
        if (this.consoleMessages.length > 10) 
          this.consoleMessages = [];
        this.searchResults = [];
        if (value.length == 1 && value =='*')
        {
          this.books.forEach(b => { 
            this.searchResults.push(b);
           });
           this.hasSearchResults$.next(this.searchResults.length > 0);
        }
        else
          this.books.filter(b => b.title.includes(value)).forEach(b => {
          //if (!this.searchResults.find(c => c.id === b.id))
            this.searchResults.push(b);
            this.hasSearchResults$.next(this.searchResults.length > 0);
          })
    });
  }

  selectBookById(id) {
    this.router.navigate(['book-view', id]);     
  }

    // this.searchQueryUpdate.pipe(
    //   debounceTime(400),
    //   distinctUntilChanged(),
    //   switchMap(x => this.getMatchingBooks(x)),
    //   tap(t => {
    //     t.filter(b => b.title.includes(x)).forEach(b => {
    //       if (!this.searchResults.find(c => c.id === b.id))
    //         this.searchResults.push(b);
    //       this.hasSearchResults$.next(this.searchResults.length > 0);
    //   }));
      //)
      //.subscribe(value => {
    //     if (this.consoleMessages.length > 10) 
    //       this.consoleMessages = [];
    //     this.searchResults = [];
    //     this.books.filter(b => b.title.includes(value)).forEach(b => {
    //     if (!this.searchResults.find(c => c.id === b.id))
    //       this.searchResults.push(b);
    //     this.hasSearchResults$.next(this.searchResults.length > 0);
    //   })
    // });

  //}

}
