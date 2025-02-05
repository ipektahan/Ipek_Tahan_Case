using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Ipek_Tahan_Case
{
    public class InsiderTests : IDisposable
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string browser = "chrome"; // "firefox" olarak değiştirilebilir.

        [SetUp]
        public void Setup()
        {
            if (browser.ToLower() == "chrome")
            {
                driver = new ChromeDriver();
            }
            else if (browser.ToLower() == "firefox")
            {
                driver = new FirefoxDriver();
            }
            else
            {
                throw new Exception("Geçersiz tarayıcı seçimi!");
            }
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void TestHomePageAndCareersPage()
        {
            try
            {
                // 1. Insider ana sayfasının açıldığını doğrula
                driver.Navigate().GoToUrl("https://useinsider.com/");
                Assert.IsTrue(driver.Title.Contains("Insider"), "Ana sayfa yüklenmedi.");

                // 2. Company -> Careers sayfasının açıldığını doğrula
                IWebElement companyMenu = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//nav//a[contains(text(), 'Company')]")));
                companyMenu.Click();
                IWebElement careersLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(), 'Careers')]")));
                careersLink.Click();
                Assert.IsTrue(driver.Url.Contains("careers"), "Careers sayfası açılmadı.");
            }
            catch (Exception ex)
            {
                TakeScreenshot();
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void TestQAJobFilteringAndApplication()
        {
            try
            {
                // QA iş ilanları sayfasına gider
                driver.Navigate().GoToUrl("https://useinsider.com/careers/quality-assurance/");
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 250);"); //scroll yapar


                IWebElement acceptCookies = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("wt-cli-accept-all-btn")));
                acceptCookies.Click();

                // QA iş ilanlarını listeler
                IWebElement seeAllQaJobs = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[@class='btn btn-outline-secondary rounded text-medium mt-2 py-3 px-lg-5 w-100 w-md-50']")));
                seeAllQaJobs.Click();
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 500);");
                Thread.Sleep(13000); // İş ilanı filtreleme dropdownında performans sorunu oluştuğu için thread sleep koymak zorunda kaldım.

                IWebElement locationFilter = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@id='select2-filter-by-location-container']")));
                locationFilter.Click();
                IWebElement option = driver.FindElement(By.XPath("//li[text()='Istanbul, Turkey']"));
                option.Click();
                Thread.Sleep(3000);

                // İş ilanlarında Quality Assurance ve Istanbul, Turkey içeren iş ilanlarının olup olmadığını kontrol et.
                var jobListings = driver.FindElements(By.ClassName("position-list")); 
                bool qualityAssuranceFound = jobListings.Any(job =>
                  job.Text.Contains("Quality Assurance", StringComparison.OrdinalIgnoreCase) &&
                  job.Text.Contains("Istanbul, Turkey", StringComparison.OrdinalIgnoreCase));

                if (qualityAssuranceFound)
                {
                    Console.WriteLine("Quality Assurance ve Istanbul, Turkey içeren iş ilanları listelenmiş.");
                }
                else
                {
                    Console.WriteLine("Quality Assurance ve Istanbul, Turkey içeren iş ilanları bulunamadı.");
                }


                //  "View Role" butonuna tıklayıp başvuru formuna yönlendirildiğini doğrula
                IWebElement cardClick = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"jobs-list\"]/div[1]/div")));
                cardClick.Click();

                string mainWindow = driver.CurrentWindowHandle;

                IWebElement viewRolebutton = driver.FindElement(By.XPath("//*[@id='jobs-list']/div[1]/div/a"));
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].style.display='block'; arguments[0].style.visibility='visible';", viewRolebutton);
                viewRolebutton.Click();

                wait.Until(driver => driver.WindowHandles.Count > 1);

                foreach (string window in driver.WindowHandles)
                {
                    if (window != mainWindow)
                    {
                        driver.SwitchTo().Window(window);
                        break;
                    }
                }
                Assert.IsTrue(driver.Url.Contains("https://jobs.lever.co/"), "Yeni sekme açılmadı veya yanlış URL yüklendi.");

                driver.SwitchTo().Window(mainWindow);

            }
            catch (Exception ex)
            {
                TakeScreenshot();
                Assert.Fail(ex.Message);
            }
        }

        private void TakeScreenshot()
        {
            //string screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "screenshot.png");
            //Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            //screenshot.SaveAsFile(screenshotPath);
            //Console.WriteLine("Hata oluştu! Screenshot alındı: " + screenshotPath);

            string screenshotDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
            if (!Directory.Exists(screenshotDirectory))
            {
                Directory.CreateDirectory(screenshotDirectory);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string screenshotPath = Path.Combine(screenshotDirectory, $"screenshot_{timestamp}.png");

            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(screenshotPath);
            Console.WriteLine("Hata oluştu! Screenshot alındı: " + screenshotPath);
        }

        [TearDown]
        public void TearDown()
        {

        }

        //Test sonuçlandıktan sonra sayfayı kapatır.
        public void Dispose()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
        public bool IsTextPresent(string text)
        {
            try
            {
                return driver.PageSource.Contains(text);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}