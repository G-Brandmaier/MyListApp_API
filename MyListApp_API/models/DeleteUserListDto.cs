namespace MyListApp_API.models
{
    public class DeleteUserListDto
    {
        public Guid UserListId { get; set; }
        public Guid UserId { get; set; }
    }
}
