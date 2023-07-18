﻿namespace Appointments.Domain.Entities;

public class User : Entity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsPasswordPlainText { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfileImage { get; set; }
}
