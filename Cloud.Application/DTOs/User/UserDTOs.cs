﻿namespace Cloud.Application.DTOs.User;

public class UserDTOs
{
    public int userId { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string role { get; set; }
    public DateTime createAt { get; set; }

    public DateTime updateAt { get; set; }
}