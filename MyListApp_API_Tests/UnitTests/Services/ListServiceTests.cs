using Moq;
using MyListApp_API.Models;
using MyListApp_API.Repository;
using MyListApp_API.Services;
using NuGet.Frameworks;
using System.Xml.Serialization;
using System.Linq;
using MyListApp_API.models;

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

    #region Gabriella testar 20st

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
        var user = new User { Id = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        var listItemDto = new ListItemDto{ UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), Content = "Hämta ut paket" };
        var userList = new UserList{ Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });
        _userServiceMock.Setup(x => x.GetUserById(user.Id)).Returns(user);

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
        var user = new User { Id = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        var listItemDto = new ListItemDto { UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), Content = "Hämta ut paket" };
        var userList = new UserList { Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });
        _userServiceMock.Setup(x => x.GetUserById(user.Id)).Returns(user);

        //Act
        var result = _listService.AddToUserList(listItemDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void AddToUserList_UserIdDoesNotExist_ReturnNull()
    {
        //Arrange
        User user = null;
        var listItemDto = new ListItemDto { UserId = new Guid("22a643af-a171-4ce3-9b55-a7e017607497"), UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), Content = "Hämta ut paket" };
        var userList = new UserList { Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });
        _userServiceMock.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(user);

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
        var user = new User { Id = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        var listItemDto = new ListItemDto { UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        var userList = new UserList { Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });
        _userServiceMock.Setup(x => x.GetUserById(user.Id)).Returns(user);

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
        var user = new User { Id = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        var userList = new UserList { Title = "Test List", UserId = user.Id, Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });
        _userServiceMock.Setup(x => x.GetUserById(user.Id)).Returns(user);

        //Act
        var result = _listService.GetAllUserListsById(user.Id);

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
        var user = new User { Id = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        var updatedListItemDto = new UpdateListItemDto { UserId = user.Id, UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ContentPosition = 1, NewContent = "Hämta ut paket" };
        var userList = new UserList { Title = "Test List", UserId = user.Id, Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ListContent = new List<string> { "Handla", "Städa", "Träna" }};
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });
        _userServiceMock.Setup(x => x.GetUserById(user.Id)).Returns(user);

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
        User user = null;
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
        var user = new User { Id = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        var updatedListItemDto = new UpdateListItemDto { UserId = user.Id, UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ContentPosition = 1, NewContent = "Hämta ut paket" };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList>());
        _userServiceMock.Setup(x => x.GetUserById(user.Id)).Returns(user);

        //Act
        var result = _listService.UpdateUserListContent(updatedListItemDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void UpdateUserListContent_ContentPostitionIsZero_ReturnNull()
    {
        //Arrange
        var user = new User { Id = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        var updatedListItemDto = new UpdateListItemDto { UserId = user.Id, UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ContentPosition = 0, NewContent = "Hämta ut paket" };
        _userServiceMock.Setup(x => x.GetUserById(user.Id)).Returns(user);
        var userList = new UserList { Title = "Test List", UserId = Guid.NewGuid(), Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ListContent = new List<string> { "Handla", "Städa", "Träna" } };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });

        //Act
        var result = _listService.UpdateUserListContent(updatedListItemDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void UpdateUserListContent_ContentPostitionIsNegativeNumber_ReturnNull()
    {
        //Arrange
        var user = new User { Id = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        var updatedListItemDto = new UpdateListItemDto { UserId = user.Id, UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ContentPosition = -8, NewContent = "Hämta ut paket" };
        _userServiceMock.Setup(x => x.GetUserById(user.Id)).Returns(user);
        var userList = new UserList { Title = "Test List", UserId = Guid.NewGuid(), Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ListContent = new List<string> { "Handla", "Städa", "Träna" } };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });

        //Act
        var result = _listService.UpdateUserListContent(updatedListItemDto);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void UpdateUserListContent_MaxLengthReachedForUpdateListItemDtoNewContent_ReturnNull()
    {
        //Arrange
        var user = new User { Id = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        var updatedListItemDto = new UpdateListItemDto { UserId = user.Id, UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ContentPosition = 1 };
        _userServiceMock.Setup(x => x.GetUserById(user.Id)).Returns(user);
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
        var user = new User { Id = new Guid("12a643af-a171-4ce3-9b55-a7e017607497") };
        var updatedListItemDto = new UpdateListItemDto { UserId = user.Id, UserListId = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ContentPosition = 1 };
        var userList = new UserList { Title = "Test List", UserId = user.Id, Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c"), ListContent = new List<string> { "Handla", "Städa", "Träna" } };
        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList> { userList });
        _userServiceMock.Setup(x => x.GetUserById(user.Id)).Returns(user);
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


    #region Steff testar ListService, GetAllLists (8 st)

    //Test 1. när listan är tom
    [Fact]
    public void GetAllLists_WhenNoLists_ReturnEmptyList()
    {
        //Arrange
        var mockList = new List<UserList>();
        _listRepoMock.Setup(r => r.UserList).Returns(mockList); 

        //Act
        var result = _listService.GetAllLists();

        //Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    //Test 2. Testar att alla listor returneras
    [Fact]
    public void GetAllLists_WhenMultipleListsExcist_ReturnAllLists()
    {
        //Arrange
        var list1 = new UserList { Title = "List 1", UserId = Guid.NewGuid() };
        var list2 = new UserList { Title = "List 2", UserId = Guid.NewGuid() };
        var mockList = new List<UserList> { list1, list2 };
        _listRepoMock.Setup(r => r.UserList).Returns(mockList);

        //Act
        var result = _listService.GetAllLists();

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(list1, result);
        Assert.Contains(list2, result);
    }

    //Test 3. Att man ej kan påverka hämtade listor externt
    [Fact]
    public void GetAllLists_ModifyReturnedLists()
    {
        //Arrange
        var list1 = new UserList { Title = "List 1", UserId = Guid.NewGuid() };
        var mockList = new List<UserList> { list1 };
        _listRepoMock.Setup(r => r.UserList).Returns(mockList);

        //Act
        var result = _listService.GetAllLists();
        result.Clear();

        //Assert
        Assert.Empty(_listRepoMock.Object.UserList); //att den ej är null
        Assert.Contains(list1, _listRepoMock.Object.UserList);
    }

    //Test 4. Lägg till lista och se om den returneras korrekt
    [Fact]
    public void GetAllLists_WhenOneListExists_ReturnThatList()
    {
        //Arrange
        var list1 = new UserList { Title = "List 1", UserId = Guid.NewGuid() };
        var mockList = new List<UserList> { list1 };
        _listRepoMock.Setup(r => r.UserList).Returns(mockList);

        //Act
        var result = _listService.GetAllLists();

        //Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(list1, result);
    }

    //Test 5. Testa ordningen på listorna, att de returnerar i den ordning de läggs till
    [Fact]
    public void GetAllLists_WhenMultipleListsAdded_ReturnsInCorrectOrder()
    {
        //Arrange
        var list1 = new UserList { Title = "Title 1", UserId = Guid.NewGuid() };
        var list2 = new UserList { Title = "Title 2", UserId = Guid.NewGuid() };
        var mockList = new List<UserList> { list1, list2 };
        _listRepoMock.Setup(r => r.UserList).Returns(mockList);

        //Act
        var result = _listService.GetAllLists();

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(list1, result[0]); //pos.1
        Assert.Equal(list2, result[1]); //pos.2 
    }

    //Test 6. Hantera null-värden från Repo
    [Fact]
    public void GetAllLists_WhenRepoReturnsNull_ReturnEmptyList()
    {
        //Arrange
        _listRepoMock.Setup(r => r.UserList).Returns((List<UserList>)null);

        //Act
        var result = _listService.GetAllLists();

        //Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    //Test 7. Säkerställa onödiga anrop och se så att listan hämtas enndast 1 gång
    [Fact]
    public void GetAllLists_RepoIsCalledOnlyOnce()
    {
        //Arrange
        var mockList = new List<UserList>();
        _listRepoMock.Setup(r => r.UserList).Returns(mockList);
        
        //Act
        var result = _listService.GetAllLists();

        //Assert
        _listRepoMock.Verify(r => r.UserList, Times.Once());
    }

    //Test 8. Kan metoden hantera stora listor utan problem?
    [Fact]
    public void GetAllLists_WithLargeAmmountOfData_ReturnsAllData()
    {
        //Arrange
        var largeList = Enumerable.Range(0, 10000).Select(i => new UserList { Title = $"List {i}", UserId = Guid.NewGuid() }).ToList();
        _listRepoMock.Setup(r => r.UserList).Returns(largeList);

        //Act
        var result = _listService.GetAllLists();

        //Assert
        Assert.NotNull(result);
        Assert.Equal(largeList.Count, result.Count);
    }
    #endregion

    #region Steff testar ListService DeleteAllLists (11 st)

    //Test 1. Ta bort lista med giltigt listid
    [Fact]
    public void DeleteList_WithValidId_ReturnsTrueAndRemovesList()
    {
        // Arrange
        var validUserId = Guid.NewGuid();
        var listToDeleteId = Guid.NewGuid();
        var userList = new UserList { Id = listToDeleteId, Title = "Test list", UserId = validUserId };

        _listRepoMock.Setup(r => r.UserList).Returns(new List<UserList> { userList });

        var deleteUserListDto = new DeleteUserListDto
        {
            UserListId = listToDeleteId,
            UserId = validUserId
        };

        // Act
        var result = _listService.DeleteList(deleteUserListDto);

        //Assert
        Assert.True(result);
        _listRepoMock.Verify(r => r.UserList.Remove(It.IsAny<UserList>()), Times.Once());  
    }

    //Test 2. Returnera false om lista ej finns
    [Fact]
    public void DeleteList_WithInvalidId_ReturnFalse()
    {
        //Assert
        var invalidId = Guid.NewGuid();
        _listRepoMock.Setup(r => r.UserList).Returns(new List<UserList>());

        var deleteUserlistDto = new DeleteUserListDto
        {
            UserListId = invalidId,
            UserId = Guid.NewGuid()
        };

        //Act
        var result = _listService.DeleteList(deleteUserlistDto);

        //Assert
        Assert.False(result);
    }

    //Test 3. Att bara listan man vill ta bort påverkas
    [Fact]
    public void DeleteList_WithInvalidId_RemovesOnlyTheOneList()
    {
        //Arrange
        var validUserId = Guid.NewGuid();
        var listToDelete = Guid.NewGuid();
        var userList1 = new UserList { Id = listToDelete, Title = "List 1", UserId = validUserId };
        var userList2 = new UserList { Id = Guid.NewGuid(), Title = "List 2", UserId = validUserId };

        _listRepoMock.Setup(r => r.UserList).Returns(new List<UserList> { userList1, userList2 });
        
        var deleteUserListDto = new DeleteUserListDto
        {
            UserListId = listToDelete,
            UserId = validUserId
        };

        //Act
        _listService.DeleteList(deleteUserListDto);

        //Assert
        _listRepoMock.Verify(r => r.UserList.Remove(It.Is<UserList>(x => x.Id == listToDelete)), Times.Once());
    }

    //Test 4. Kontrollera att metoden hanterar null-argument korrekt
    [Fact]
    public void DeleteList_withNullDto_ReturnsFalse()
    {
        // Arrange
        var mockList = new List<UserList>
    {
        new UserList { Id = Guid.NewGuid(), Title = "List 1", UserId = Guid.NewGuid() },
        new UserList { Id = Guid.NewGuid(), Title = "List 2", UserId = Guid.NewGuid() }
    };

        // mocken  returnerar en förbestämd lista
        _listRepoMock.Setup(r => r.UserList).Returns(mockList);

        // Act
        var result = _listService.DeleteList(null);

        // Assert
        Assert.False(result); // förväntningar

        // Verifiera att Remove-metoden aldrig anropades på mocken
        _listRepoMock.Verify(r => r.UserList.Remove(It.IsAny<UserList>()), Times.Never);
    }

    //Test 5
    [Fact]
    public void DeleteList_WithInvalidListIdAndValidUserId_ReturnsFalse()
    {
        //Arrange
        var validUserId = Guid.NewGuid();
        var userList = new UserList { Id = Guid.NewGuid(), Title = "Test list", UserId = validUserId };
        var mockList = new List<UserList> { userList };
        _listRepoMock.Setup(r => r.UserList).Returns(mockList);

        var deleteUserListDto = new DeleteUserListDto
        {
            UserListId = Guid.NewGuid(), //Annat list-Id
            UserId = validUserId
        };

        //Act
        var result = _listService.DeleteList(deleteUserListDto);

        //Assert
        Assert.False(result);
        _listRepoMock.Verify(r => r.UserList, Times.Once);
    }
  
    //Test 6
    [Fact]
    public void DeleteList_NullDto_ReturnFalse()
    {
        //Act
        var result = _listService.DeleteList(null);

        //Assert
        Assert.False(result);
    }

    //Test 7. Tar bort lista utan matchande id, returnera false
    [Fact]
    public void DeleteList_WithNoMatchUserId_ReturnFalse()
    {
        //Arrange
        var listIdToDelete = Guid.NewGuid();
        var userIdToMatch = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        var userList = new UserList
        {
            Id = listIdToDelete,
            Title = "Test list",
            UserId = userIdToMatch
        };

        var mockListRepo = new Mock<IListRepo>();
        mockListRepo.Setup(r => r.UserList).Returns(new List<UserList>() { userList });

        var mockUserService = new Mock<IUserService>();
        mockUserService.Setup(u => u.GetUserById(It.IsAny<Guid>())).Returns(new User());

        var listService = new ListService(mockListRepo.Object, mockUserService.Object);

        var dto = new DeleteUserListDto { UserListId = listIdToDelete, UserId = otherUserId };

        //Act
        var result = listService.DeleteList(dto);

        //Assert
        Assert.False(result);
        Assert.Empty(_listRepoMock.Object.UserList);
    }

    //Tets 8. Ta bort lista med rätt id men tomt repo, returnera false
    [Fact]
    public void DeleteList_WithValidIdAndEmptyRepo_ReturnFalse()
    {
        //Arrange
        var dto = new DeleteUserListDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid() };
        var mockList = new List<UserList>();

        _listRepoMock.Setup(r => r.UserList).Returns(mockList);

        //Act
        var result = _listService.DeleteList(dto);

        //Assert
        Assert.False(result);
    }

    //Test 9. tar bort spec lista baserat på matchande userid & listid
    [Fact]
    public void DeleteList_MultipleListsSameUserId_RemovesOnlyOneList()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var listIdToDelete = Guid.NewGuid();
        var userList1 = new UserList { Id = listIdToDelete, Title = "List 1", UserId = Guid.NewGuid() };
        var userList2 = new UserList { Id = listIdToDelete, Title = "List 2", UserId = Guid.NewGuid() };

        var mockList = new List<UserList> { userList1, userList2 };
        _listRepoMock.Setup(r => r.UserList).Returns(mockList);

        var dto = new DeleteUserListDto { UserId = userId, UserListId = listIdToDelete };

        //Act
        _listService.DeleteList(dto);

        //Assert
        Assert.DoesNotContain(userList1, _listRepoMock.Object.UserList);
        Assert.DoesNotContain(userList2, _listRepoMock.Object.UserList);
    }

    //Test. 10
    [Fact]
    public void DeleteList_ReturnsTrue_WhenListExistsForGivenUser()
    {
        //Arrange
        var dto = new DeleteUserListDto
        {
            UserListId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };
        var mockList = new List<UserList>
        {
            new UserList { Id = dto.UserListId, UserId = dto.UserId }
        };

        _listRepoMock.Setup(r => r.UserList).Returns(mockList);

        //Act
        bool result = _listService.DeleteList(dto);

        //Asset
        Assert.True(result);
    }

    //Test 11.
    [Fact]
    public void DeleteList_ReturnsFalse_WhenListDoesNotExistOrUserIdMismatch()
    {
        //Arrange
        var dto = new DeleteUserListDto
        {
            UserListId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };

        _listRepoMock.Setup(x => x.UserList).Returns(new List<UserList>());

        //Act
        bool result = _listService.DeleteList(dto);

        //Assert
        Assert.False(result);
    }


    #endregion
}
