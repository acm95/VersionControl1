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
            TestCase("abcd1234", false),
    TestCase("irf@uni-corvinus", false),
    TestCase("irf.uni-corvinus.hu", false),
    TestCase("irf@uni-corvinus.hu", true),
            TestCase("abcdefgh", false),
            TestCase("ABCD1234", false),
            TestCase("abcdefgh", false),
            TestCase("abcd", false),
            TestCase("AbCd1234", true)
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

    }
}
