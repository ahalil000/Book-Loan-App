import { browser, by, element } from 'protractor';

export class BookListPage
{ 
  getHeading() {
    return element(by.id('heading'));
  };

  getHeadingText() {
    return element(by.id('heading')).getText();
  };

  getListItems()
  {
    return element.all(by.css('.mat-list-item'));
  }

  getFirstBook()
  {
    return element(by.id("book_1"));
  }

  getBrowser() {
    return browser.get('http://localhost:4200/books');
  };
};
