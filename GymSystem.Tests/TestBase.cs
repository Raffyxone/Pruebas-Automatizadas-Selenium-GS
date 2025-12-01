using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System;
using System.IO;

namespace GymSystem.Tests
{
    public class TestBase
    {
        protected IWebDriver driver;
        protected static ExtentReports extent;
        protected ExtentTest test;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            string path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Reporte_Selenium.html");
            var sparkReporter = new ExtentSparkReporter(path);
            extent = new ExtentReports();
            extent.AttachReporter(sparkReporter);
        }

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void Teardown()
        {
            try
            {
                var status = TestContext.CurrentContext.Result.Outcome.Status;
                
                string finalName = "Final_" + TestContext.CurrentContext.Test.Name;
                string screenshotPath = TakeScreenshot(finalName);
                
                if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    test.Fail("Fallo detectado", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                    test.Fail(TestContext.CurrentContext.Result.Message);
                }
                else
                {
                    test.Pass("Prueba Exitosa", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                }
            }
            finally
            {
                if (driver != null)
                {
                    driver.Quit();
                    driver.Dispose();
                }
            }
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            extent.Flush();
        }

        protected string TakeScreenshot(string name)
        {
            try 
            {
                ITakesScreenshot ts = (ITakesScreenshot)driver;
                Screenshot screenshot = ts.GetScreenshot();
                
                string finalName = name + ".png";
                
                string path = Path.Combine(TestContext.CurrentContext.TestDirectory, finalName);
                screenshot.SaveAsFile(path);
                return path;
            }
            catch
            {
                return "";
            }
        }
        
        protected void AgregarFotoAlReporte(string nombreExactoArchivo)
        {
            string path = TakeScreenshot(nombreExactoArchivo);
            test.Info("Evidencia: " + nombreExactoArchivo, MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
        }
    }
}