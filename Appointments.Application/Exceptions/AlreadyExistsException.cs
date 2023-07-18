namespace Appointments.Application.Exceptions;

public class AlreadyExistsException<TResource> : ApplicationException
{
    public Type ResourceType { get; }
    public string PropertyName { get; }
    public object PropertyValue { get; }

    public AlreadyExistsException(string propertyName, object propertyValue)
        : base("AlreadyExists", $"A resource of type {typeof(TResource).Name} with {propertyName}={propertyValue} already exists.")
    {
        ResourceType = typeof(TResource);
        PropertyName = propertyName;
        PropertyValue = propertyValue;
    }
}
