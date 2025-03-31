﻿namespace BichoApi.Presentation.DTO.Auth;

public class RegisterDto
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
}

public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class ResponseLoginDto
{
    public required string Token { get; set; }
}