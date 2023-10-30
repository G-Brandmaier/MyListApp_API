using Moq;
using MyListApp_API.Models;
using MyListApp_API.Repository;
using MyListApp_API.Services;

namespace MyListApp_API_Tests.UnitTests.Services;

public class ListServiceTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly ListService _listService;
    private readonly Mock<IListRepo> _listRepoMock;
    public ListServiceTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _listRepoMock = new Mock<IListRepo>();
        _listService = new ListService(_listRepoMock.Object, _userServiceMock.Object);
    }

    #region Gabriella testar 11st

    #region Testar metoden CreateUserList
    [Fact]
    public void CreateUserList_ShouldCreateUserList_ReturnCreatedUserList()
    {
        //Arrange
        var userListDto = new UserListDto
        {
            Title = "Test List",
            UserId = Guid.NewGuid()
        };
        var userList = new UserList { Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });
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
        var userList = new UserList { Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });

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
        var userList = new UserList { Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });
        _userServiceMock.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(new User { Id = It.IsAny<Guid>(), UserName = "test@test.com", Email = "test@test.com", Password = "Test123!" });

        //Act
        var result = _listService.CreateUserList(userListDto);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<UserList>(result);
        Assert.Empty(result.ListContent);
    }
    #endregion

    #region Testar metoden AddToUserList
    [Fact]
    public void AddToUserList_ShouldAddStringToUserListListContent_ReturnUpdatedUserList()
    {
        //Arrange
        var listItemDto = new ListItemDto{ UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), Content = "Hämta ut paket" };
        var userList = new UserList{ Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });

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
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });

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
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });

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
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });

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
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });

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
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });

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

    #region Testar metoden GetAllUserListsById

    [Fact]
    public void GetAllUserListsById_ShouldGetAllUserListsByUserId_ReturnListOfAllUserLists()
    {
        //Arrange
        Guid userId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497");
        var user = new User { Id = userId };
        var userList = new UserList { Title = "Test List", UserId = userId, Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });
        _userServiceMock.Setup(x => x.GetUserById(userId)).Returns(user);

        //Act
        var result = _listService.GetAllUserListsById(userId);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<List<UserList>>(result);
        Assert.True(result.Count == 1);
    }

    [Fact]
    public void GetAllUserListsById_UserIdDoesNotExist_ReturnNull()
    {
        //Arrange
        Guid userId = Guid.NewGuid();
        var user = new User();
        user = null;
        _userServiceMock.Setup(x => x.GetUserById(userId)).Returns(user);

        //Act
        var result = _listService.GetAllUserListsById(userId);

        //Assert
        Assert.Null(result);
    }

    #endregion

    #region Testar metoden UpdateUserListContent

    [Fact]
    public void UpdateUserListContent_ShouldUpdateStringInUserListListContent_ReturnUpdatedUserList()
    {
        //Arrange
        Guid userId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497");
        var user = new User { Id = userId };
        var updatedListItemDto = new UpdateListItemDto { UserId = userId, UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ContentPosition = 1, NewContent = "Hämta ut paket" };
        var userList = new UserList { Title = "Test List", UserId = userId, Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ListContent = new List<string> { "Handla", "Städa", "Träna" }};
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });
        _userServiceMock.Setup(x => x.GetUserById(userId)).Returns(user);
        //Act
        var result = _listService.UpdateUserListContent(updatedListItemDto);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<UserList>(result);
        Assert.True(result.ListContent.Count == 3);
        Assert.Equal(updatedListItemDto.NewContent, result.ListContent[updatedListItemDto.ContentPosition - 1]);
    }

    [Fact]
    public void UpdateUserListContent_UserIdDoesNotExist_ReturnNull()
    {
        //Arrange
        var user = new User();
        user = null;
        var updatedListItemDto = new UpdateListItemDto { UserId = Guid.NewGuid(), UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ContentPosition = 1, NewContent = "Hämta ut paket" };
        var userList = new UserList { Title = "Test List", UserId = Guid.NewGuid(), Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ListContent = new List<string> { "Handla", "Städa", "Träna" } };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });
        _userServiceMock.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(user);

        //Act
        var result = _listService.UpdateUserListContent(updatedListItemDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void UpdateUserListContent_UserListIdDoesNotExist_ReturnNull()
    {
        //Arrange
        Guid userId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497");
        var user = new User { Id = userId };
        var updatedListItemDto = new UpdateListItemDto { UserId = userId, UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ContentPosition = 1, NewContent = "Hämta ut paket" };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList>());
        _userServiceMock.Setup(x => x.GetUserById(userId)).Returns(user);

        //Act
        var result = _listService.UpdateUserListContent(updatedListItemDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void UpdateUserListContent_ContentPostitionIsZero_ReturnNull()
    {
        //Arrange
        Guid userId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497");
        var user = new User { Id = userId };
        var updatedListItemDto = new UpdateListItemDto { UserId = userId, UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ContentPosition = 0, NewContent = "Hämta ut paket" };
        _userServiceMock.Setup(x => x.GetUserById(userId)).Returns(user);

        //Act
        var result = _listService.UpdateUserListContent(updatedListItemDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void UpdateUserListContent_MaxLengthReachedForUpdateListItemDtoNewContent_ReturnNull()
    {
        //Arrange
        Guid userId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497");
        var user = new User { Id = userId };
        var updatedListItemDto = new UpdateListItemDto { UserId = userId, UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ContentPosition = 1 };
        _userServiceMock.Setup(x => x.GetUserById(userId)).Returns(user);
        char[] fixedSizeString = new char[90];
        for (int i = 0; i < fixedSizeString.Length; i++)
        {
            fixedSizeString[i] = 'A';
        }
        updatedListItemDto.NewContent = new string(fixedSizeString);
        //Act
        var result = _listService.UpdateUserListContent(updatedListItemDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void UpdateUserListContent_MaxLengthNotReachedForUpdateListItemDtoNewContent_ReturnUpdatedUserList()
    {
        //Arrange
        Guid userId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497");
        var user = new User { Id = userId };
        var updatedListItemDto = new UpdateListItemDto { UserId = userId, UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ContentPosition = 1 };
        var userList = new UserList { Title = "Test List", UserId = userId, Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ListContent = new List<string> { "Handla", "Städa", "Träna" } };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });
        _userServiceMock.Setup(x => x.GetUserById(userId)).Returns(user);
        char[] fixedSizeString = new char[79];
        for (int i = 0; i < fixedSizeString.Length; i++)
        {
            fixedSizeString[i] = 'A';
        }
        updatedListItemDto.NewContent = new string(fixedSizeString);

        //Act
        var result = _listService.UpdateUserListContent(updatedListItemDto);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(updatedListItemDto.NewContent, result.ListContent[updatedListItemDto.ContentPosition - 1]);
        Assert.IsType<UserList>(result);
    }

    #endregion

    #endregion
}
