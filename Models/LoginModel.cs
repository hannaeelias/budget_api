﻿using Microsoft.AspNetCore.Mvc;

namespace budget_api.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
