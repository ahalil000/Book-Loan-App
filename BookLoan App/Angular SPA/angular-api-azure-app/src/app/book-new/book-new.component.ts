import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Book } from '../models/Book';
import { ApiService } from '../services/api.service';
import { LookupService } from '../services/lookup.service';
import { publishedYearValidator } from '../validators/published-year.directive';

@Component({
  selector: 'app-book-new',
  templateUrl: './book-new.component.html',
  styleUrls: ['./book-new.component.scss']
})
export class BookNewComponent implements OnInit {

  heading = "Book Details";
  
  book: Book = new Book();
  genres = [];
  id: any;
  selectedGenre: string;
  genre$: Subscription;

  bookForm: any;
  submitted: boolean = false;

  constructor(private api: ApiService, private lookupService: LookupService, 
    private router: Router, private fb: FormBuilder) { 
      this.initForm(fb);
  }

  private initForm(fb: FormBuilder) {
    this.bookForm = fb.group({
      title: ['', Validators.required],
      author: ['', Validators.required],
      yearPublished: [0, 
        [
          Validators.required, 
          publishedYearValidator()
      ]],  
      edition: ['', Validators.required],
      isbn: ['', Validators.required],
      location: ['', Validators.required],
      genre: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.selectedGenre = "Fiction";

    console.log("BookNewComponent instantiated");
    
    this.genre$ = this.lookupService.getGenres().subscribe(
      genres => {
          if (!genres)
            return;
          console.log("number of genres  = " + genres.length);
  
         this.genres = genres.map(g => g.genre);
         this.selectedGenre = this.genres[0];
         this.selectedGenre = this.selectedGenre.charAt(0).toUpperCase() + this.selectedGenre.slice(1);
        }
      );
  }

  get title() { return this.bookForm.get('title'); }
  get author() { return this.bookForm.get('author'); }
  get yearPublished() { return this.bookForm.get('yearPublished'); }
  get isbn() { return this.bookForm.get('isbn'); }
  get location() { return this.bookForm.get('location'); }
  get edition() { return this.bookForm.get('edition'); }

  ngOnDestroy()
  {
      this.genre$.unsubscribe();
  }

  eventSelection(value: any){
    this.selectedGenre = value;
  }

  eventBack(event)
  {
    this.router.navigate(['books']);     
  }

  eventCreateRecord(event)
  {
    this.submitted = true;

    if (!this.bookForm.valid)
      return;
  
    console.log('new book record created!');
  }

}
