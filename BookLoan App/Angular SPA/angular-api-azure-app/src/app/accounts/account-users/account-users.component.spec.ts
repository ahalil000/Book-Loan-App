import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialModule } from '../../material/material.module';
import { AccountUsersComponent } from './account-users.component';
import { TestApiService } from '../../services/test-api.service';
import { ApiService } from '../../services/api.service';
import { Router } from '@angular/router';
import { getBooks } from '../../services/test/test-books';
import { of } from 'rxjs';

const books = getBooks();

describe('BooksComponent', () => {
  let component: AccountUsersComponent;
  let fixture: ComponentFixture<AccountUsersComponent>;
  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };
  const mockService = jasmine.createSpyObj('TestApiService', ['getBooks', 'getBook'] );
  mockService.getBooks.and.returnValue(of(books));
  mockService.getBook.and.returnValue(of(books.find(b => b.id === 1)));  
  
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountUsersComponent ],
      imports: [ MaterialModule ],
      providers: [
        { provide: ApiService, useClass: TestApiService }, // useValue: mockService }, //  
        { provide: Router, useValue: mockRouter }
      ]      
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display books', () => {
    expect(books.length).toBeGreaterThan(0);
  });

  //it('books list should be selectable', () => {
    //expect(component.books.length).toBeGreaterThan(0);
  //});

}); 
