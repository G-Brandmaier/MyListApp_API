using Microsoft.OpenApi.Any;
using Moq;
using MyListApp_API.Models;
using MyListApp_API.Repository;
using MyListApp_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyListApp_API_Tests.UnitTests.Services;

public class ListServiceTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly ListService _listService;
    private readonly ListRepo _listRepo;
    public ListServiceTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _listRepo = new ListRepo();
        _listService = new ListService(_listRepo, _userServiceMock.Object);
    }

    [Fact]
    public void CreateUserList_ShouldCreateUserList_ReturnCreatedUserList()
    {

    }
}
