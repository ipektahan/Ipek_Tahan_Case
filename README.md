# Insider QA Job Automation

## Overview
This project is a Selenium-based test automation suite for verifying Quality Assurance job listings on Insider's careers page. It includes tests to:
- Validate that the homepage and careers page load correctly.
- Filter job listings to display only "Quality Assurance" positions located in "Istanbul, Turkey."
- Verify that job applications open correctly in a new tab.
- Capture screenshots in case of test failures.

## Technologies Used
- **C#** (Programming Language)
- **Selenium WebDriver** (Browser Automation)
- **NUnit** (Testing Framework)
- **ChromeDriver / FirefoxDriver** (Browser Drivers)

## Installation & Setup
### Prerequisites
- Install **.NET SDK** ([Download](https://dotnet.microsoft.com/en-us/download))
- Install **ChromeDriver** or **FirefoxDriver** ([Download](https://sites.google.com/chromium.org/driver/))
- Install **Selenium WebDriver**
  ```sh
  dotnet add package Selenium.WebDriver
  dotnet add package Selenium.Support
  dotnet add package Selenium.WebDriver.ChromeDriver
  ```
- Install **NUnit** and NUnit Test Adapter
  ```sh
  dotnet add package NUnit
  dotnet add package NUnit3TestAdapter
  ```

## How to Run the Tests
1. Open the project in **Visual Studio** or **VS Code**.
2. Ensure that the required WebDriver (Chrome/Firefox) is installed and available in your system path.
3. Run the tests using the following command:
   ```sh
   dotnet test
   ```

## Test Cases
### 1. `TestHomePageAndCareersPage`
- Navigates to the Insider homepage.
- Clicks on "Company" -> "Careers".
- Verifies that the careers page is opened.

### 2. `TestQAJobFilteringAndApplication`
- Opens the Quality Assurance job listings page.
- Accepts cookies if prompted.
- Clicks on "See All QA Jobs" and applies the location filter "Istanbul, Turkey".
- Checks if there are job listings containing **"Quality Assurance"** and **"Istanbul, Turkey"**.
- Clicks on a job listing and verifies that the "View Role" button navigates to **Lever.co** job application page.

## Logging & Screenshots
- **Logging:** Console logs indicate whether job listings were found or not.
- **Screenshots:** If an exception occurs, a screenshot is saved in the `Screenshots/` directory with a timestamp.

## Configuration
The default browser is **Chrome**. To switch to Firefox, modify:
```csharp
private string browser = "firefox";
```

## Cleanup & Disposal
- The `Dispose()` method ensures that the WebDriver is properly closed after test execution.

## Notes
- Some elements take longer to load, so explicit waits and `Thread.Sleep` are used in necessary places.
- The test navigates between multiple tabs and ensures the correct one is opened.

## Author
_Ipek Tahan_

