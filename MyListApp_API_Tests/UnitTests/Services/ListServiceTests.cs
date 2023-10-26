using Moq;
using MyListApp_API.Models;
using MyListApp_API.Repository;
using MyListApp_API.Services;

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

    #region Gabriella testar 11st

    [Fact]
    public void CreateUserList_ShouldCreateUserList_ReturnCreatedUserList()
    {
        //Arrange
        var userListDto = new UserListDto
        {
            Title = "Test List",
            UserId = Guid.NewGuid()
        };
        _userServiceMock.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(new User { Id = It.IsAny<Guid>(), UserName = "test@test.com", Email = "test@test.com", Password = "Test123!" });

        //Act
        var result = _listService.CreateUserList(userListDto);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<UserList>(result);
    }

    [Fact]
    public void CreateUserList_InvalidUserIdReceived_ReturnNull()
    {
        //Arrange
        var userListDto = new UserListDto
        {
            Title = "Test List",
            UserId = Guid.NewGuid()
        };

        User user = null;
        _userServiceMock.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(user);

        //Act
        var result = _listService.CreateUserList(userListDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void CreateUserList_MaxLengthReachedForUserListDtoTitle_ReturnNull()
    {
        //Arrange
        var userListDto = new UserListDto { UserId = Guid.NewGuid() };

        char[] fixedSizeString = new char[30];
        for (int i = 0; i < fixedSizeString.Length; i++)
        {
            fixedSizeString[i] = 'A';
        }
        userListDto.Title = new string(fixedSizeString);
        _userServiceMock.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(new User { Id = It.IsAny<Guid>(), UserName = "test@test.com", Email = "test@test.com", Password = "Test123!" });

        //Act
        var result = _listService.CreateUserList(userListDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void CreateUserList_MaxLengthNotReachedForUserListDtoTitle_ReturnCreatedUserList()
    {
        //Arrange
        var userListDto = new UserListDto { UserId = Guid.NewGuid() };

        char[] fixedSizeString = new char[15];
        for (int i = 0; i < fixedSizeString.Length; i++)
        {
            fixedSizeString[i] = 'A';
        }
        userListDto.Title = new string(fixedSizeString);
        _userServiceMock.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(new User { Id = It.IsAny<Guid>(), UserName = "test@test.com", Email = "test@test.com", Password = "Test123!" });

        //Act
        var result = _listService.CreateUserList(userListDto);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<UserList>(result);
        Assert.Equal(userListDto.Title, result.Title);
    }

    [Fact]
    public void CreateUserList_ShouldCreateUserListWithEmptyList_ReturnCreatedUserList()
    {
        //Arrange
        var userListDto = new UserListDto { UserId = Guid.NewGuid(), Title = "Att göra" };

        _userServiceMock.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(new User { Id = It.IsAny<Guid>(), UserName = "test@test.com", Email = "test@test.com", Password = "Test123!" });

        //Act
        var result = _listService.CreateUserList(userListDto);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<UserList>(result);
        Assert.Empty(result.ListContent);
    }

    [Fact]
    public void AddToUserList_ShouldAddStringToListInUserList_ReturnUpdatedUserList()
    {
        //Arrange
        var listItemDto = new ListItemDto{ UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), Content = "Hämta ut paket" };
        var userList = new UserList{ Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        _listRepo.UserList.Add(userList);

        //Act
        var result = _listService.AddToUserList(listItemDto);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<UserList>(result);
        Assert.Single<string>(result.ListContent);
    }

    [Fact]
    public void AddToUserList_UserListIdDoesNotExist_ReturnNull()
    {
        //Arrange
        var listItemDto = new ListItemDto { UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), Content = "Hämta ut paket" };
        var userList = new UserList { Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        _listRepo.UserList = new List<UserList>{ userList };

        //Act
        var result = _listService.AddToUserList(listItemDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void AddToUserList_UserIdDoesNotExist_ReturnNull()
    {
        //Arrange
        var listItemDto = new ListItemDto { UserId = new Guid("22a643af-a171-4ce3-9b55-a7e017607497"), UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), Content = "Hämta ut paket" };
        var userList = new UserList { Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        _listRepo.UserList = new List<UserList> { userList };

        //Act
        var result = _listService.AddToUserList(listItemDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void AddToUserList_ListItemDtoIsNull_ReturnNull()
    {
        //Arrange
        ListItemDto listItemDto = null;
        var userList = new UserList { Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        _listRepo.UserList = new List<UserList> { userList };

        //Act
        var result = _listService.AddToUserList(listItemDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void AddToUserList_MaxLengthReachedForListItemDtoContent_ReturnNull()
    {
        //Arrange
        var listItemDto = new ListItemDto { UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        var userList = new UserList { Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        _listRepo.UserList.Add(userList);

        char[] fixedSizeString = new char[85];
        for (int i = 0; i < fixedSizeString.Length; i++)
        {
            fixedSizeString[i] = 'A';
        }
        listItemDto.Content = new string(fixedSizeString);

        //Act
        var result = _listService.AddToUserList(listItemDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void AddToUserList_MaxLengthNotReachedForListItemDtoContent_ReturnUpdatedUserList()
    {
        //Arrange
        var listItemDto = new ListItemDto { UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        var userList = new UserList { Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        _listRepo.UserList.Add(userList);

        char[] fixedSizeString = new char[5];
        for (int i = 0; i < fixedSizeString.Length; i++)
        {
            fixedSizeString[i] = 'A';
        }
        listItemDto.Content = new string(fixedSizeString);

        //Act
        var result = _listService.AddToUserList(listItemDto);

        //Assert
        Assert.NotNull(result);
        Assert.Single<string>(result.ListContent);
    }

    #endregion
}
