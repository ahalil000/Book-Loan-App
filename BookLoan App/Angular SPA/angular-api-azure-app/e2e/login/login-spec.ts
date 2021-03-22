import { LoginForm } from './LoginForm.po';
import { protractor, browser, logging } from 'protractor';

describe('Protractor Test - Angular Login Form', function() {
  browser.waitForAngularEnabled(true);

  let loginForm: LoginForm;

  beforeEach(() => {
    loginForm = new LoginForm();
  });
  
  beforeEach(function() {
    loginForm.getBrowser();
  });

  it('should have a title', function() {
    loginForm.getHeadingText().then(txt => 
      {
        console.log("page title=" + txt);
        expect(loginForm.getHeadingText()).toEqual('Login Form');
      });
    }
  );

  it('user can login', function() {
    var acctMenu = loginForm.getAccountMenu();
    acctMenu.click();
    browser.sleep(1000);

    var email = 'admin@bookloan.com';
    var pwd = 'Pass4Admin';

    var loginMenuBtn = loginForm.getMenuLoginBtn();
    loginMenuBtn.click();
    browser.sleep(1000);

    var loginEmail = loginForm.getLoginId();
    loginEmail.clear();
    loginEmail.sendKeys(email);
    browser.sleep(1000);   

    var loginPwd = loginForm.getLoginPwd();
    loginPwd.clear();
    loginPwd.sendKeys(pwd);
    browser.sleep(1000);   

    var loginFormBtn = loginForm.getLoginBtn();
    loginFormBtn.click();
    browser.waitForAngularEnabled(true);
    browser.sleep(5000);

    console.log("logged in!");

    var EC = protractor.ExpectedConditions;
    browser.wait(EC.visibilityOf(acctMenu), 2000);

    acctMenu.click();

    var btnMenuLogout = loginForm.getMenuLogoutBtn();
    expect(btnMenuLogout.isPresent()).toEqual(true);
  });  
});
