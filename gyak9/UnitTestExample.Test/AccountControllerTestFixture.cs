using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnitTestExample.Controllers;


namespace UnitTestExample.Test
{
    public class AccountControllerTestFixture
    {

        [Test,
            
    TestCase("irf@uni-corvinus", "Abcd1234"),
    TestCase("irf.uni-corvinus.hu", "Abcd1234"),
    TestCase("irf@uni-corvinus.hu", "abcd1234"),
    TestCase("irf@uni-corvinus.hu", "ABCD1234"),
    TestCase("irf@uni-corvinus.hu", "abcdABCD"),
    TestCase("irf@uni-corvinus.hu", "Ab1234"),
            ]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            // Arrange
            var accountController = new AccountController();

            // Act
            var actualResult = accountController.ValidateEmail(email);

            // Assert
            NUnit.Framework.Assert.AreEqual(expectedResult, actualResult);

        }

        public void TestValidatePassword(string password, bool expectedResult)
        {
            // Arrange
            var accountController = new AccountController();

            // Act
            var actualResult = accountController.ValidatePassword(password);

            // Assert
            NUnit.Framework.Assert.AreEqual(expectedResult, actualResult);
        }

        public void TestRegisterHappyPath(string email, string password)
        {
            // Arrange
            var accountController = new AccountController();

            // Act
            var actualResult = accountController.Register(email, password);

            // Assert
            NUnit.Framework.Assert.AreEqual(email, actualResult.Email);
            NUnit.Framework.Assert.AreEqual(password, actualResult.Password);
            NUnit.Framework.Assert.AreNotEqual(Guid.Empty, actualResult.ID);
        }

        public void TestRegisterValidateException(string email, string password)
        {
            // Arrange
            var accountController = new AccountController();

            // Act
            try
            {
                var actualResult = accountController.Register(email, password);
                NUnit.Framework.Assert.Fail();
            }
            catch (Exception ex)
            {
                NUnit.Framework.Assert.IsInstanceOf<ValidationException>(ex);
            }

            // Assert
        }

    }
}
