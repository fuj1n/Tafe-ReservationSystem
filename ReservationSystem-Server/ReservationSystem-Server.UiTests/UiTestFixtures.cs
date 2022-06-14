using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using NUnit.Framework;
using static ReservationSystem_Server.UiTests.Util.Config;

namespace ReservationSystem_Server.UiTests;

[SetUpFixture]
public class UiTestFixtures
{
    [OneTimeSetUp]
    public void SetUp()
    {
        using HttpClient client = new();
            
        try
        {
            client.Send(new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}"));
        }
        catch (HttpRequestException)
        {
            throw new InvalidOperationException("Attempted to run tests whilst server is not running");
        }

        try
        {
            if (Directory.Exists(FailureScreenshotPath))
            {
                Directory.Delete(FailureScreenshotPath, true);
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }
    
    [OneTimeTearDown]
    public void TearDown()
    {
        if (!Directory.Exists(FailureScreenshotPath)) return;
        
        // Open the directory containing the screenshots if any
        try
        {
            Process.Start("explorer.exe", Path.GetFullPath(FailureScreenshotPath));
        }
        catch (Exception)
        {
            // ignored
        }
    }
}