// conf.js
exports.config = {
    framework: 'jasmine',
    seleniumAddress: 'http://localhost:4444/wd/hub',
    specs: [
         'spec.js' 
         //'../e2e/book-list/*spec.js'  
       ],
    //specs: [
      //'./**/*.e2e-spec.ts'
    //  './e2e/book-list/*spec.ts'
    //],
    //suites: {
    //    bookUpdate: 'spec.js',
    //    bookUpdate2: ['./e2e/book-list/*spec.ts']
    //  },      
    capabilities: {
      browserName: 'chrome'
    }
  }