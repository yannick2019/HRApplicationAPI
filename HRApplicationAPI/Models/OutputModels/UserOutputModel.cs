﻿using HRApplicationAPI.Models.DbModels;

namespace HRApplicationAPI.Models.OutputModels
{
    public class UserOutputModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public List<UserEvent> UserEvents { get; set; } = [];
    }
}
