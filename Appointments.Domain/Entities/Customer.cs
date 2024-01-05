using Appointments.Common.Domain;
using Appointments.Common.Domain.Models;

namespace Appointments.Core.Domain.Entities;

public sealed class Customer : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email? Email { get; private set; }
    public string? PhoneNumber { get; private set; }

    public Customer(
        Guid id,
        DateTime createdAt,
        string createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        string firstName,
        string lastName,
        Email? email,
        string? phoneNumber)
    : base(
        id,
        createdAt,
        createdBy,
        updatedAt,
        updatedBy)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public void Update(
        string updatedBy,
        string firstName,
        string lastName,
        Email? email,
        string? phoneNumber)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;

        // TODO Add event
    }

    public static Customer Create(
        string createdBy,
        string firstName,
        string lastName,
        Email? email,
        string? phoneNumber)
    {
        var customer = new Customer(
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            firstName,
            lastName,
            email,
            phoneNumber);

        // TODO Add event

        return customer;
    }
}
