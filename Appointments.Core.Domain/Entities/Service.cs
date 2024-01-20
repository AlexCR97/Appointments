using Appointments.Common.Domain;
using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;

namespace Appointments.Core.Domain.Entities;

public sealed class Service : Entity
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }

    private readonly List<IndexedResource> _images = new();
    public IReadOnlyList<IndexedResource> Images
    {
        get
        {
            return _images;
        }
        private set
        {
            _images.Clear();
            _images.AddRange(value);
        }
    }

    private readonly List<string> _termsAndConditions = new();
    public IReadOnlyList<string> TermsAndConditions
    {
        get
        {
            return _termsAndConditions;
        }
        private set
        {
            _termsAndConditions.Clear();
            _termsAndConditions.AddRange(value);
        }
    }
    public string? Notes { get; private set; }

    /// <summary>
    /// The amount of time it takes for the customer to receive the service.
    /// </summary>
    public TimeSpan? CustomerDuration { get; private set; }

    /// <summary>
    /// The amount of time the service occupies in the calendar.
    /// </summary>
    public TimeSpan? CalendarDuration { get; private set; }

    public Service(
        Guid id,
        DateTime createdAt,
        string createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        Guid tenantId,
        string name,
        string? description,
        decimal price,
        IReadOnlyList<IndexedResource> images,
        IReadOnlyList<string> termsAndConditions,
        string? notes,
        TimeSpan? customerDuration,
        TimeSpan? calendarDuration)
    : base(
        id,
        createdAt,
        createdBy,
        updatedAt,
        updatedBy)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
        Price = price;
        Images = images;
        TermsAndConditions = termsAndConditions;
        Notes = notes;
        CustomerDuration = customerDuration;
        CalendarDuration = calendarDuration;
    }

    public void AddImage(string updatedBy, IndexedResource image)
    {
        _images.Add(image);

        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        // TODO Add event
    }

    public IndexedResource GetImage(int index)
    {
        var imageIndex = _images.FindIndex(x => x.Index == index);

        return imageIndex == -1
            ? throw new NotFoundException(nameof(IndexedResource), nameof(IndexedResource.Index), index.ToString())
            : _images[imageIndex];
    }

    public void RemoveImage(string updatedBy, int index)
    {
        var listIndex = _images.FindIndex(x => x.Index == index);

        if (listIndex != -1)
        {
            _images.RemoveAt(listIndex);

            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
            // TODO Add event
        }
    }

    public void Update(
        string updatedBy,
        string name,
        string? description,
        decimal price,
        IReadOnlyList<IndexedResource> images,
        IReadOnlyList<string> termsAndConditions,
        string? notes,
        TimeSpan? customerDuration,
        TimeSpan? calendarDuration)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        Name = name;
        Description = description;
        Price = price;
        Images = images;
        TermsAndConditions = termsAndConditions;
        Notes = notes;
        CustomerDuration = customerDuration;
        CalendarDuration = calendarDuration;

        // TODO Add event
    }

    public void UpdateImages(
        string updatedBy,
        List<IndexedResource> images)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        Images = images;

        // TODO Add event
    }

    public static Service Create(
        string createdBy,
        Guid tenantId,
        string name,
        string? description,
        decimal price,
        IReadOnlyList<IndexedResource> images,
        IReadOnlyList<string> termsAndConditions,
        string? notes,
        TimeSpan? customerDuration,
        TimeSpan? calendarDuration)
    {
        var service = new Service(
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            tenantId,
            name,
            description,
            price,
            images,
            termsAndConditions,
            notes,
            customerDuration,
            calendarDuration);

        // TODO Add event

        return service;
    }
}
