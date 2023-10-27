using MyListApp_API.Models;

namespace MyListApp_API.Repository;

public interface IListRepo
{
    public List<UserList> UserList { get; set; }
}
