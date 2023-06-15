﻿#pragma warning disable CS8618
namespace K.O.R_Server.Requests.Account;

// ReSharper disable once ClassNeverInstantiated.Global
public class RegistrationRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordSha512 { get; set; }
}