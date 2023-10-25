﻿using System.ComponentModel.DataAnnotations;

namespace MyListApp_API.Models;

public class UserListDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public string Title { get; set; } = null!;

    public static implicit operator UserList(UserListDto dto)
    {
        return new UserList
        {
            UserId = dto.UserId,
            Title = dto.Title
        };
    }
}
