using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace GymSystem.Tests
{
    public class GymTests : TestBase
    {
        private const string BaseUrl = "http://localhost:5000"; 

        //HISTORIA 1: LOGIN

        [Test]
        public void Historia1_Login_Exitoso()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            
            driver.FindElement(By.Id("inputUser")).SendKeys("admin");
            driver.FindElement(By.Id("inputPass")).SendKeys("1234");
            
            // FOTO 1: Login Correcto
            AgregarFotoAlReporte("test_case_login_success");
            
            driver.FindElement(By.Id("btnLogin")).Click();
            Thread.Sleep(2000); 

            Assert.That(driver.Title, Does.Contain("Clientes").Or.Contain("Panel"));
        }

        [Test]
        public void Historia1_Login_Fallido()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            driver.FindElement(By.Id("inputUser")).SendKeys("admin");
            driver.FindElement(By.Id("inputPass")).SendKeys("MAL");
            driver.FindElement(By.Id("btnLogin")).Click();

            Thread.Sleep(2000);

            var errorMsg = driver.FindElement(By.ClassName("validation-summary-errors")).Text;
            
            AgregarFotoAlReporte("test_case_login_failed");
            
            Assert.That(errorMsg, Does.Contain("Credenciales inválidas"));
        }

        //HISTORIA 2: CREAR CLIENTE

        [Test]
        public void Historia2_CrearCliente_Exitoso()
        {
            Loguearse();
            
            driver.FindElement(By.Id("btnCrearNuevo")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.Id("inputNombre")).SendKeys("Selenium");
            driver.FindElement(By.Id("inputApellido")).SendKeys("Lento");
            driver.FindElement(By.Id("inputEdad")).SendKeys("25");
            
            driver.FindElement(By.Id("btnGuardar")).Click();
            Thread.Sleep(2000);

            AgregarFotoAlReporte("test_case_create_success");

            Assert.That(driver.Url, Does.Contain("Index"));
            Assert.That(driver.PageSource, Does.Contain("Selenium"));
        }

        [Test]
        public void Historia2_CrearCliente_EdadInvalida()
        {
            Loguearse();
            driver.FindElement(By.Id("btnCrearNuevo")).Click();

            driver.FindElement(By.Id("inputNombre")).SendKeys("Error");
            driver.FindElement(By.Id("inputApellido")).SendKeys("Test");
            driver.FindElement(By.Id("inputEdad")).SendKeys("-10"); 
            
            driver.FindElement(By.Id("btnGuardar")).Click();
            Thread.Sleep(2000);

            bool hayError = driver.PageSource.Contains("La edad debe estar entre");
            
            AgregarFotoAlReporte("test_case_create_invalid_age");
            
            Assert.That(hayError, Is.True);
        }

        //HISTORIA 3: LEER LISTA

        [Test]
        public void Historia3_VerLista_TablaVisible()
        {
            Loguearse();
            Thread.Sleep(1500);
            
            var tabla = driver.FindElement(By.Id("tablaClientes"));
            
            AgregarFotoAlReporte("test_case_read_list");
            
            Assert.That(tabla.Displayed, Is.True);
        }

        //HISTORIA 4: EDITAR

        [Test]
        public void Historia4_EditarCliente_Exitoso()
        {
            Loguearse();
            
            driver.FindElement(By.CssSelector(".btnEditar")).Click();
            Thread.Sleep(1000);
            
            var inputNombre = driver.FindElement(By.Id("inputNombre"));
            inputNombre.Clear();
            inputNombre.SendKeys("EditadoVideo");
            
            AgregarFotoAlReporte("test_case_update_1_form");

            driver.FindElement(By.Id("btnGuardar")).Click();
            Thread.Sleep(2000);

            AgregarFotoAlReporte("test_case_update_2_result");

            Assert.That(driver.PageSource, Does.Contain("EditadoVideo"));
        }

        //HISTORIA 5: ELIMINAR

        [Test]
        public void Historia5_EliminarCliente_Exitoso()
        {
            Loguearse();

            driver.FindElement(By.CssSelector(".btnEliminar")).Click();
            Thread.Sleep(1500);
            
            AgregarFotoAlReporte("test_case_delete_client_confirm");

            driver.FindElement(By.Id("btnConfirmarEliminar")).Click();
            Thread.Sleep(2000);

            AgregarFotoAlReporte("test_case_delete_client_done");

            Assert.That(driver.Url, Does.Contain("Index"));
        }

        private void Loguearse()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            driver.FindElement(By.Id("inputUser")).SendKeys("admin");
            driver.FindElement(By.Id("inputPass")).SendKeys("1234");
            driver.FindElement(By.Id("btnLogin")).Click();
            
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("Index") || d.Url.Contains("Clients"));
        }
    }
}