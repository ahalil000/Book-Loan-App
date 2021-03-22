import { BookListPage } from './BookList.po';
import { BookDetailsPage } from '../book-detail/BookDetails.po';
import { protractor, browser, logging } from 'protractor';

describe('Protractor Test - Angular Record List Form', function() {
  browser.waitForAngularEnabled(true);

  let bookListPage: BookListPage;
  let bookDetailPage: BookDetailsPage;
  let newValue = undefined;

  beforeEach(() => {
    bookListPage = new BookListPage();
    bookDetailPage = new BookDetailsPage();
  });
  
  beforeEach(function() {
    bookListPage.getBrowser();
  });

  it('should have a title', function() {
    var EC = protractor.ExpectedConditions;
    browser.wait(EC.visibilityOf(bookListPage.getHeading()), 5000);
    expect(bookListPage.getHeadingText()).toEqual('Book List');
    }
  );

  it('record can be updated', function() {
    bookListPage.getListItems().then(function(items) {
      expect(items.length).toBeGreaterThan(0);
      expect(items[1].getText()).toBe('The Lord of the Rings');

      // Select the first item
      var selectItem = bookListPage.getFirstBook();
      selectItem.click();
      browser.sleep(2000);

      // Test if the current form is named 
      expect(bookDetailPage.getHeadingText()).toEqual('Book Details'); 

      // Change ISBN value..
      var today = new Date();
      var increment = today.getSeconds();

      var isbnField = bookDetailPage.getISBN(); 
      var isbnValue = undefined;
      var EC = protractor.ExpectedConditions;
      browser.wait(EC.visibilityOf(isbnField), 2000);

      isbnField.getAttribute('value').then(text => {
        isbnValue = text;
        console.log("original isbn=" + text);
        newValue = parseInt(isbnValue) + increment;
        console.log("new isbn=" + newValue);
        console.log("increment=" + increment);
        isbnField.clear();
        isbnField.sendKeys(newValue.toString());
        browser.sleep(1000);   
      });
      
      // update the record..
      console.log("updating record..");
      var btnUpdate = bookDetailPage.getBtnUpdate(); 
      btnUpdate.click();
      console.log("record updated!");
      browser.sleep(1000);
    });
  });  

  it('record update is consistent', function() {

    // reselect record and verify the ISBN field is the same as the updated value..
    browser.get('http://localhost:4200/books');

    bookListPage.getListItems().then(function(items) {
      expect(items.length).toBeGreaterThan(0);
      expect(items[1].getText()).toBe('The Lord of the Rings');
  
      // Select the first item
      var selectItem = bookListPage.getFirstBook();
      selectItem.click();
      browser.sleep(1000);
  
      // Test if the current form is named 
      expect(bookDetailPage.getHeadingText()).toEqual('Book Details'); 

      // Compare ISBN value from data source against value updated on initial form. 
      var isbnField = bookDetailPage.getISBN();
      var EC = protractor.ExpectedConditions;
      browser.wait(EC.visibilityOf(isbnField), 2000);

      isbnField.getAttribute('value').then(text => {
        console.log("updated isbn=" + text);
        console.log("compare isbn=" + newValue);
      });

      expect(isbnField.getAttribute('value')).toEqual(newValue.toString());
      browser.sleep(1000); 
    });
  });  

});
