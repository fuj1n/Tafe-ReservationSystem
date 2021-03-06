using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ReservationSystem_Server.UiTests.Util;
using static ReservationSystem_Server.UiTests.Util.Config;
using static ReservationSystem_Server.UiTests.Util.TestUtility;

namespace ReservationSystem_Server.UiTests;

[TestFixture(DriverType.Chrome)]
[TestFixture(DriverType.Firefox)]
[TestFixture(DriverType.Edge)]
public class SittingUiTest : UiTestBase
{
    public SittingUiTest(DriverType driver) : base(driver)
    {
    }

    [Test]
    public void Test_Unauthorized_Redirect_Login()
    {
        Driver.Navigate().GoToUrl(BaseUrl + "/Admin/Sitting");

        Assert.That(Driver.Url, Does.Contain("Account/Login"));
    }

    [Test]
    public void Test_WrongRole_Access_Denied()
    {
        LoginAs(Driver, DefaultAccount.Employee);

        Driver.Navigate().GoToUrl(BaseUrl + "/Admin/Sitting");

        Assert.That(Driver.FindElements(By.TagName("h1")).Select(w => w.Text), Does.Contain("Access denied"));
    }

    [Test]
    public void Test_Authorized_No_Redirect()
    {
        LoginAs(Driver, DefaultAccount.Manager);

        Driver.Navigate().GoToUrl(BaseUrl + "/Admin/Sitting");

        Assert.That(Driver.Url, Does.Contain("/Admin/Sitting"));
    }

    [Test]
    public void Test_Show_Past_Toggle_Changes_Url()
    {
        LoginAs(Driver, DefaultAccount.Manager);

        Driver.Navigate().GoToUrl(BaseUrl + "/Admin/Sitting");

        Driver.FindElement(By.Id("pastSittings")).Click();
        Thread.Sleep(500); // Wait for animation
        Assert.That(Driver.Url, Does.Contain("?pastSittings=true"));

        Driver.FindElement(By.Id("pastSittings")).Click();
        Thread.Sleep(500); // Wait for animation
        Assert.That(Driver.Url, Does.Not.Contain("?pastSittings=true"));
    }

    [Test]
    public void Test_Create_Button_Opens_Create_Page()
    {
        LoginAs(Driver, DefaultAccount.Manager);

        Driver.Navigate().GoToUrl(BaseUrl + "/Admin/Sitting");
        Driver.FindElement(By.PartialLinkText("Create")).Click();

        WebDriverWait wait = new(Driver, TimeSpan.FromSeconds(5));
        wait.Until(IsModalLoaded);
        
        Assert.That(Driver.FindElement(By.Id("modal-title")).Text, Does.Contain("Create"));
        Assert.That(Driver.FindElement(By.Id("btn-save")).Displayed, Is.True);
    }
    
    [Test]
    public void Test_Create_Sitting_With_Invalid_Data_Shows_Validation_Errors()
    {
        LoginAs(Driver, DefaultAccount.Manager);

        Driver.Navigate().GoToUrl(BaseUrl + "/Admin/Sitting");
        Driver.FindElement(By.PartialLinkText("Create")).Click();
        
        WebDriverWait wait = new(Driver, TimeSpan.FromSeconds(5));
        wait.Until(IsModalLoaded);
        
        Assert.That(() => Driver.FindElement(By.ClassName("validation-summary-errors")), Throws.TypeOf<NoSuchElementException>());
        
        Thread.Sleep(500);
        Driver.FindElement(By.Id("btn-save")).Click();

        Thread.Sleep(500);
        wait.Until(IsModalLoaded);
        
        Assert.That(Driver.FindElements(By.Id("btn-save")), Is.Not.Empty);
        Assert.That(Driver.FindElements(By.ClassName("validation-summary-errors")), Is.Not.Empty);
    }

    [Test]
    public void Test_Create_Sitting_With_Valid_Data_Created_Successfully()
    {
        DateOnly sittingDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1));
        
        LoginAs(Driver, DefaultAccount.Manager);

        Driver.Navigate().GoToUrl(BaseUrl + "/Admin/Sitting");
        ReadOnlyCollection<IWebElement>? beforeRows = Driver.FindElements(By.TagName("tr"));
        
        Driver.FindElement(By.PartialLinkText("Create")).Click();
        
        WebDriverWait wait = new(Driver, TimeSpan.FromSeconds(5));
        wait.Until(IsModalLoaded);

        IWebElement startTime = Driver.FindElement(By.Id("StartTime"));
        IWebElement endTime = Driver.FindElement(By.Id("EndTime"));
        IWebElement capacity = Driver.FindElement(By.Id("Capacity"));
        IWebElement sittingType = Driver.FindElement(By.Id("SittingTypeId"));
        
        SendDateTime(startTime, sittingDate.ToDateTime(new TimeOnly(7, 30)));
        SendDateTime(endTime, sittingDate.ToDateTime(new TimeOnly(11, 30)));
        
        capacity.Clear();
        capacity.SendKeys("1337");
        
        sittingType.SendKeys("B");

        Thread.Sleep(500);
        Driver.FindElement(By.Id("btn-save")).Click();
        Thread.Sleep(500);
        wait.Until(IsModalLoaded);
        Driver.FindElement(By.Id("btn-dismiss")).Click();
        Thread.Sleep(500);
        wait.Until(IsModalLoaded);
        
        Assert.That(() => Driver.FindElement(By.ClassName("validation-summary-errors")), Throws.TypeOf<NoSuchElementException>());
        Assert.That(Driver.Url, Does.Not.Contain("/Admin/Sitting/Create"));
        Assert.That(Driver.Url, Does.Contain("/Admin/Sitting"));

        // Wait for modal system to finish up
        Thread.Sleep(500);
        wait.Until(IsModalLoaded);
        Thread.Sleep(500);
        
        ReadOnlyCollection<IWebElement>? afterRows = Driver.FindElements(By.TagName("tr"));
        Assert.That(afterRows.Count, Is.GreaterThan(beforeRows.Count));
    }
}