using System.Web.Mvc;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StatCoach.Controllers;
using StatCoach.Models;
using WebMatrix.WebData;

namespace StatCoach.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Register_No_User()
        {
            // Arrange
            AccountController accountController = new AccountController();

            // Act
            ViewResult result = accountController.Register() as ViewResult;

            // Assert
            Assert.AreEqual(typeof(RegisterModel), result.Model.GetType());
        }

        [TestMethod]
        public void Register_With_User()
        {
            // Arrange
            AccountController accountController = new AccountController();
            RegisterModel model = new RegisterModel
            {
                
            }

            // Act
            ViewResult result = accountController.Register() as ViewResult;

            // Assert
            Assert.AreEqual(typeof(RegisterModel), result.Model.GetType());
        }
    }
}
