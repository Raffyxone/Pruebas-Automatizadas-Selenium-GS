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
        
        private static string reportPath;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            var binPath = TestContext.CurrentContext.TestDirectory;
            
            var projectPath = Directory.GetParent(binPath)!.Parent!.Parent!.FullName;
            
            reportPath = Path.Combine(projectPath, "Reportes");
            
            if (!Directory.Exists(reportPath))
            {
                Directory.CreateDirectory(reportPath);
            }

            string htmlFile = Path.Combine(reportPath, "Reporte_Selenium.html");
            var sparkReporter = new ExtentSparkReporter(htmlFile);
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
                
                string finalName = "test_case_" + TestContext.CurrentContext.Test.Name;
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
                
                // Usamos la variable estática reportPath que definimos arriba
                string path = Path.Combine(reportPath, finalName);
                
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