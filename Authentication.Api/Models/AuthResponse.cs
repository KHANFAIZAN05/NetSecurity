﻿namespace Authentication.Api.Models
{

    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }

    }
}
