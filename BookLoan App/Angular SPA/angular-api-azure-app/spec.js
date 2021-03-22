const { Console, time } = require("console");
const { browser } = require("protractor");

// spec.js
describe('Protractor Test - Angular Record List Form', function() {
  var heading = element(by.id('heading'));
  var newValue = undefined;

  beforeEach(function() {
    browser.get('http://localhost:4200/books');
  });

  it('should have a title', function() {
      expect(element(by.id('heading')).getText()).toEqual('Book List');
    }
  );

  it('record can be updated', function() {
    browser.get('http://localhost:4200/books');

    element.all(by.css('.mat-list-item')).then(function(items) {
      expect(items.length).toBeGreaterThan(0);
      expect(items[1].getText()).toBe('The Lord of the Rings');

      // Select the first item
      var selectItem = element(by.id("book_1"));
      selectItem.click();
      browser.sleep(2000);

      // Test if the current form is named 
      expect(element(by.id('heading')).getText()).toEqual('Book Details'); 

      // Change ISBN value..
      var today = new Date();
      var increment = today.getSeconds();

      var isbnField = element(by.id('isbn'));
      var isbnValue = undefined;
      var EC = protractor.ExpectedConditions;
      browser.wait(EC.visibilityOf(isbnField), 5000);

      isbnField.getAttribute('value').then(text => {
        isbnValue = text;
        console.log("original isbn=" + text);
        newValue = parseInt(isbnValue) + increment;
        console.log("new isbn=" + newValue);
        console.log("increment=" + increment);
        isbnField.clear();
        isbnField.sendKeys(newValue.toString());
        browser.sleep(2000);   
      });
      
      // update the record..
      console.log("updating record..");
      var btnUpdate = element(by.id('btnUpdate'));
      btnUpdate.click();
      console.log("record updated!");
      browser.sleep(2000);
    });
  });  

  it('record update is consistent', function() {

    // reselect record and verify the ISBN field is the same as the updated value..
    browser.get('http://localhost:4200/books');

    element.all(by.css('.mat-list-item')).then(function(items) {
      expect(items.length).toBeGreaterThan(0);
      expect(items[1].getText()).toBe('The Lord of the Rings');
  
      // Select the first item
      var selectItem = element(by.id("book_1"));
      selectItem.click();
      browser.sleep(2000);
  
      // Test if the current form is named 
      expect(element(by.id('heading')).getText()).toEqual('Book Details'); 

      // Compare ISBN value from data source against value updated on initial form. 
      var isbnField = element(by.id('isbn'));
      var EC = protractor.ExpectedConditions;
      browser.wait(EC.visibilityOf(isbnField), 5000);

      isbnField.getAttribute('value').then(text => {
        console.log("updated isbn=" + text);
        console.log("compare isbn=" + newValue);
      });

      expect(isbnField.getAttribute('value')).toEqual(newValue.toString());
      browser.sleep(2000); 
    });
  });  

});
