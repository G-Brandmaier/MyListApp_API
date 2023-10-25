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

    public ListServiceTests()
    {

    }

    [Fact]
    public void CreateUserList_ShouldCreateUserList_ReturnCreatedUserList()
    {
        //Arrange
        var userListDto = new UserListDto
        {
            Title = "Test List",
            UserId = Guid.NewGuid()
        };

        //Act


        //Assert
        
    }
}
