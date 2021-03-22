// import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

// import { TestBed } from '@angular/core/testing';
// import { HttpClient, HttpResponse, HttpErrorResponse } from '@angular/common/http';

// import { Book } from 'src/app/models/Book';
// import { getBooks } from './test/test-books';
// import { of } from 'rxjs';
// import { ApiService } from './api.service';

// describe ('ApiService (with spies)', () => {
//     let httpClientSpy: { get: jasmine.Spy };
//     let apiService: ApiService;
  
//     expect(true).toBe(true);

//      beforeEach(() => {
//        httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
//        apiService = new ApiService(httpClientSpy as any);
//      });
  
//     it('should return expected books (HttpClient called once)', () => {
//        const expectedBooks: Book[] = getBooks();

    
//     //    expect(true).toBe(true);

//        httpClientSpy.get.and.returnValue(of(expectedBooks));
  
//        apiService.getBooks().subscribe(
//          books => expect(books).toEqual(expectedBooks, 'expected books'),
//          fail
//        );
//        expect(httpClientSpy.get.calls.count()).toBe(1, 'one call');
//     });

//     it('should return expected a book (HttpClient called once)', () => {
//         const expectedBook: Book = getBooks().find(b => b.id === 1);

//     //    expect(true).toBe(true);

//          httpClientSpy.get.and.returnValue(of(expectedBook));
    
//          apiService.getBook(1).subscribe(
//            book => expect(book).toEqual(expectedBook, 'expected book'),
//            fail
//          );
//          expect(httpClientSpy.get.calls.count()).toBe(1, 'one call');
//       });
  

//     it('should return an error when the server returns a 404', () => {
//       const errorResponse = new HttpErrorResponse({
//         error: 'test 404 error',
//         status: 404, statusText: 'Not Found'
//       });
  
      
//       httpClientSpy.get.and.returnValue(of(errorResponse));
  
//       apiService.getBooks().subscribe(
//         books => fail('expected an error, not books'),
//         error  => expect(error.message).toContain('test 404 error')
//       );
//     });
  
//   });
  