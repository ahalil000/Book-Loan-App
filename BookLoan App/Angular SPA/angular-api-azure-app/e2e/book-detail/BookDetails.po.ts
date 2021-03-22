import { browser, by, element } from 'protractor';

export class BookDetailsPage
{
  getHeading() {
    return element(by.id('heading'));
  };

  getHeadingText() {
    return element(by.id('heading')).getText();
  };

  getBtnUpdate() {
    return element(by.id('btnUpdate'));
  };

  getISBN() {
    return element(by.id('isbn'));
  };

  getISBNText() {
    return element(by.id('isbn')).getText();
  };
};
