using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Extensions;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace ReservationSystem_Server.UiTests.Util;

public class UiTestBase
{
    public enum DriverType
    {
        Chrome,
        Firefox,
        Edge
    }

    private static readonly Dictionary<DriverType, (Action, Func<IWebDriver>)> Drivers = new()
    {
        {
            DriverType.Chrome,
            (
                () => new DriverManager().SetUpDriver(new ChromeConfig()),
                () => new ChromeDriver(new ChromeOptions { AcceptInsecureCertificates = true }))
        },
        {
            DriverType.Edge,
            (
                () => new DriverManager().SetUpDriver(new EdgeConfig()),
                () => new EdgeDriver(new EdgeOptions { AcceptInsecureCertificates = true }))
        },
        {
            DriverType.Firefox,
            (
                () => new DriverManager().SetUpDriver(new FirefoxConfig()),
                () => new FirefoxDriver(new FirefoxOptions { AcceptInsecureCertificates = true }))
        }
    };

    protected IWebDriver Driver { get; private set; } = null!;

    private readonly DriverType _driverType;

    public UiTestBase(DriverType driver)
    {
        _driverType = driver;
    }

    [SetUp]
    public void SetUp()
    {
        if (!Drivers.ContainsKey(_driverType))
        {
            throw new InvalidOperationException("Driver type is not supported");
        }

        (Action setUpDriver, Func<IWebDriver> createDriver) = Drivers[_driverType];

        setUpDriver();
        Driver = createDriver();
    }

    [TearDown]
    public virtual void TearDown()
    {
        // Capture screenshot on failure
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            try
            {
                TestContext context = TestContext.CurrentContext;
                
                Directory.CreateDirectory(Config.FailureScreenshotPath);
                Driver.TakeScreenshot()
                    .SaveAsFile($"{Config.FailureScreenshotPath}/{context.Test.FullName}.png",
                        ScreenshotImageFormat.Png);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        Driver.Quit();
        Driver = null!;
    }
}