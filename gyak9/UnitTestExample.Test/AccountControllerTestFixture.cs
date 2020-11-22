using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnitTestExample.Controllers;
using Moq;
using UnitTestExample.Abstractions;
using UnitTestExample.Entities;

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
                var actualResault = accountController.Register(email, password);
                NUnit.Framework.Assert.Fail();
            }
            catch (Exception ex)
            {
                NUnit.Framework.Assert.IsInstanceOf<ValidationException>(ex);
            }

            // Assert
            var accountServiceMock = new Mock<IAccountManager>(MockBehavior.Strict);
            accountServiceMock
                .Setup(m => m.CreateAccount(It.IsAny<Account>()))
                .Returns<Account>(a => a);
            var accountController = new AccountController();
            accountController.AccountManager = accountServiceMock.Object;

            // Act
            var actualResult = accountController.Register(email, password);

            // Assert
            NUnit.Framework.Assert.AreEqual(email, actualResult.Email);
            NUnit.Framework.Assert.AreEqual(password, actualResult.Password);
            NUnit.Framework.Assert.AreNotEqual(Guid.Empty, actualResult.ID);
            accountServiceMock.Verify(m => m.CreateAccount(actualResult), Times.Once);
        }

        public void TestRegisterApplicationException(string newEmail, string newPassword)
        {
            // Arrange
            var accountServiceMock = new Mock<IAccountManager>(MockBehavior.Strict);
            accountServiceMock
                .Setup(m => m.CreateAccount(It.IsAny<Account>()))
                .Throws<ApplicationException>();
            var accountController = new AccountController();
            accountController.AccountManager = accountServiceMock.Object;

            // Act
            try
            {
                var actualResult = accountController.Register(newEmail, newPassword);
                NUnit.Framework.Assert.Fail();
            }
            catch (Exception ex)
            {
                NUnit.Framework.Assert.IsInstanceOf<ApplicationException>(ex);
            }

            // Assert
        }

    }
}
