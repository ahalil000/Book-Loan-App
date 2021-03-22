import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Book } from '../models/Book';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/internal/operators/map';
import { LookupService } from '../services/lookup.service';
import { Genre } from '../models/Genre';
import { Loan } from '../models/Loan';
import { AuthService } from '../security/auth.service';


@Component({
  selector: 'book-borrow',
  templateUrl: './book-borrow.component.html',
  styleUrls: ['./book-borrow.component.scss']
})
export class BookBorrowComponent implements OnInit {

  heading = "Book Loan";
  
  book: Book = new Book();
  loan: Loan = new Loan();
  genres = [];
  id: any;
  selectedGenre: string;

  constructor(private api: ApiService, 
      private lookupService: LookupService, 
      private activatedRoute: ActivatedRoute,
      private authService: AuthService,  
      private router: Router) {}

  ngOnInit() {
    if (!this.activatedRoute)
      return;
    this.id = this.activatedRoute.snapshot.params["id"];
    this.genres = ["Childrens", "Family sage", "Fantasy", "Fiction", "Folklore", "Horror", "Mystery", "Non-fiction", "Sci-fi"];
    this.selectedGenre = "Fiction";

    console.log("BookBorrowComponent instantiated with the following id : " + this.id);
    
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

       let currDate = new Date();
       const currMonthNum = currDate.getMonth() + 1;
       let currDateString = currMonthNum.toString() + '/' + currDate.getDate().toString() + '/' + currDate.getFullYear().toString();
       
       let returnDate = new Date();
       returnDate.setDate(returnDate.getDate() + 14); 
       const returnMonthNum = returnDate.getMonth() + 1;

       let returnDateString = currMonthNum.toString() + '/' + returnDate.getDate().toString() + '/' + returnDate.getFullYear().toString();

       this.loan = { 
         id: 0, 
         dateLoaned: currDateString, 
         dateDue: returnDateString, 
         daysLoaned: 14, 
         bookId: book.id, 
         borrowerMemberShipNo: "", 
         loanedBy: this.authService.currLoggedInUserValue 
        };

      }
    );

  }

  eventProcessLoan(event)
  {
    this.api.loanBook(this.loan).subscribe(res => {
      console.log("book " + this.id + " has been loaned");
      this.router.navigate(['book-view', this.id]);
    })
  }

  eventCancelLoan(event)
  {
    this.router.navigate(['book-view', this.id]);     
  }

  eventSelection(value)
  {
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
