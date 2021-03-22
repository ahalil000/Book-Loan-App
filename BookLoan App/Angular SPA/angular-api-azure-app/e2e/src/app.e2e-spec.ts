import { AppPage } from './app.po';
import { browser, logging } from 'protractor';

describe('workspace-project App', () => {
  browser.waitForAngularEnabled(true);
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    console.log("checking page title..");
    page.getTitleText().then(txt => {
      console.log("page title=" + txt);
      expect(txt).toEqual('Angular Demo App');
    });
    //expect(page.getTitleText()).toEqual('Welcome to angular-azure-app!');
  });

  afterEach(async () => {
    // Assert that there are no errors emitted from the browser
    const logs = await browser.manage().logs().get(logging.Type.BROWSER);
    expect(logs).not.toContain(jasmine.objectContaining({
      level: logging.Level.SEVERE,
    } as logging.Entry));
  });
});
