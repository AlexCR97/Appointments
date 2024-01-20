using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;
using Appointments.Infrastructure.Mongo.Documents;

namespace Appointments.Core.Infrastructure.Mongo.Customers;

internal sealed class CustomerDocument : MongoDocument
{
    public const string CollectionName = "customers";

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CustomerDocument()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // Required by Mongo Client library
    }

    public CustomerDocument(
        Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy, string firstName, string lastName, string? email, string? phoneNumber)
        : base(id, createdAt, createdBy, updatedAt, updatedBy)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    internal static CustomerDocument From(Customer entity)
    {
        return new CustomerDocument(
            entity.Id,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.FirstName,
            entity.LastName,
            entity.Email?.Value,
            entity.PhoneNumber);
    }

    internal Customer ToEntity()
    {
        return new Customer(
            Id,
            CreatedAt,
            CreatedBy,
            UpdatedAt,
            UpdatedBy,
            FirstName,
            LastName,
            Email is not null
                ? new Email(Email)
                : null,
            PhoneNumber);
    }
}
