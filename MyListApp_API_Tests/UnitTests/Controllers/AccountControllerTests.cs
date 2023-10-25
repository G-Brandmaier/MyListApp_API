using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using MyListApp_API.Controllers;
using MyListApp_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyListApp_API_Tests.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        private readonly AccountController _accountController;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<AccountController>> _logger;

        public AccountControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _logger = new Mock<ILogger<AccountController>>();
            _accountController = new AccountController(_userServiceMock.Object, _logger.Object);
            
        }
    }
}
