import { Component } from '@angular/core'
import { ApiService } from '../services/api.service';
import { Router } from '@angular/router';
import { BehaviorSubject, Subscription } from 'rxjs';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { delay } from 'rxjs/operators';
import { Book } from '../models/Book';

@Component({
    selector: 'books',
    templateUrl: './books.component.html'
})
export class BooksComponent {

    heading  = "Book List";

    book = {}
    books
    
    bookView: Book[];

    hasLoaded$ = new BehaviorSubject<boolean>(false);
    mode: ProgressSpinnerMode = 'indeterminate';
    value = 50;

    numberBooks = 0;

    apidelay = 0;

    subscription: Subscription;

    constructor(private api: ApiService, private router: Router) {
        console.log("books component created");
    }

    ngOnInit() {
        this.subscription = this.api.getBooks().pipe(delay(this.apidelay)).subscribe(res => {
            this.hasLoaded$.next(true);
            this.books = res;
            this.bookView = this.books;
            this.numberBooks = this.books.length;
            console.log("read books from API");
        })
    }

    ngOnDestroy()
    {
        this.subscription.unsubscribe();
    }

    selectBookById(id) {
        this.router.navigate(['book-detail-3', id]);
    }

    selectBook(book) {
        this.api.selectBook(book);
    }

    eventNewRecord(event)
    {
        this.router.navigate(['book-new']);     
        console.log("new book data entry form being opened.");
    }

    eventPageChanged(event: any)
    {
        console.log("page event firing!");
    }

    eventPagedOutputChanged(event: any)
    {
        console.log("paginator output firing!");
        this.bookView = event;
    }

}