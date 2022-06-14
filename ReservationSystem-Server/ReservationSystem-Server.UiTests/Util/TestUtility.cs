using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using OpenQA.Selenium;

namespace ReservationSystem_Server.UiTests.Util;

[PublicAPI]
public static class TestUtility
{
    private static readonly Dictionary<DefaultAccount, (string, string)> Accounts = new()
    {
        { DefaultAccount.Admin, ("a@e.com", "Abc123!@#") },
        { DefaultAccount.Manager, ("m@e.com", "Abc123!@#") },
        { DefaultAccount.Employee, ("e@e.com", "Abc123!@#") },
        { DefaultAccount.Customer, ("u@e.com", "Abc123!@#") }
    };

    /**
     * Utility method that handles authentication for the test.
     */
    public static void Login(IWebDriver driver, string username, string password)
    {
        driver.Navigate().GoToUrl(Config.BaseUrl + "/Identity/Account/Login");
        
        driver.FindElement(By.Id("Input_Email")).SendKeys(username);
        driver.FindElement(By.Id("Input_Password")).SendKeys(password);
        driver.FindElement(By.Id("login-submit")).Click();
    }
    
    /**
     * Utility method that handles authentication as specific default account for the test.
     */
    public static void LoginAs(IWebDriver driver, DefaultAccount account)
    {
        (string username, string password) = Accounts[account];
        Login(driver, username, password);
    }
    
    public static void SendDateTime(IWebElement element, DateTime dateTime)
    {
        SendDate(element, DateOnly.FromDateTime(dateTime));
        element.SendKeys(Keys.ArrowRight);
        SendTime(element, TimeOnly.FromDateTime(dateTime));
    }
    
    public static void SendDate(IWebElement element, DateOnly date)
    {
        element.SendKeys(date.ToString("ddMMyyyy"));
    }
    
    public static void SendTime(IWebElement element, TimeOnly time)
    {
        element.SendKeys(time.ToString("hhmmtt"));
    }
    
    public static bool IsFullyLoaded(IWebDriver driver)
    {
        return ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete");
    }
    
    public static bool IsModalLoaded(IWebDriver driver)
    {
        return driver.FindElements(By.ClassName("lds-dual-ring")).Count <= 1; // There is always at least one on the page that acts as a prefab
    }
    
    public enum DefaultAccount
    {
        Admin,
        Manager,
        Employee,
        Customer
    }
}