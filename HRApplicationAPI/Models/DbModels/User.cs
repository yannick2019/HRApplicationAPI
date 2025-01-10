﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HRApplicationAPI.Models.DbModels
{
    public class User
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public IList<UserEvent> UserEvents { get; set; } = [];
    }
}
