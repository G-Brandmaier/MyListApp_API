using MyListApp_API.Models;
using MyListApp_API.Repository;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace MyListApp_API.Services;

public class ListService : IListService
{
    // Fake database for lists all users create
    private readonly ListRepo _listRepo;

    public ListService(ListRepo listRepo)
    {
        _listRepo = listRepo;
    }

    public UserList CreateUserList(UserListDto dto) //Lägg till att checka om användare existerar innan
    {
        if(dto != null)
        {
            var userList = new UserList();
            userList = dto;
            _listRepo.UserList.Add(userList);
            return userList;
        }
        return null;
    }

    public UserList AddToUserList(ListItemDto dto)
    {
        if(dto != null)
        {
            var listResult = _listRepo.UserList.Where(x => x.Id == dto.UserListId && x.UserId == dto.UserId).SingleOrDefault();
            if(listResult != null)
            {
                listResult.ListContent.Add(dto.Content);
                return listResult;
            }
        }
        return null;
    }
    //Get Lists
    public List<UserList> GetAllLists()
    {
        return _listRepo.UserList;
    }

    //Delete list
    public bool DeleteList(Guid listId)
    {
        var listRemove = _listRepo.UserList.FirstOrDefault(x => x.Id == listId);
        if(listRemove != null)
        {
            return false; //list is not found
        }
        _listRepo.UserList.Remove(listRemove);
        return true; // list sucessfully moved
    }
}
