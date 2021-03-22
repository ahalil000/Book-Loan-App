import { browser, by, element } from 'protractor';

export class LoginForm
{ 
  getHeading() {
    return element(by.id('heading'));
  };

  getHeadingText() {
    return element(by.id('heading')).getText();
  };

  getAccountMenu()
  {
    return element(by.id('btnAccount'));
  }

  getMenuLoginBtn()
  {
    return element(by.id('btnMenuLogin'));
  }

  getMenuLogoutBtn()
  {
    return element(by.id('btnMenuLogout'));
  }

  getLoginId()
  {
    return element(by.id("email"));
  }

  getLoginPwd()
  {
    return element(by.id("pwd"));
  }

  getLoginBtn()
  {
    return element(by.id('btnLogin'));
  }

  getBrowser() {
    return browser.get('http://localhost:4200');
  };
};
