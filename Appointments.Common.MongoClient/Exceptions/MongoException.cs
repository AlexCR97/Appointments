namespace Appointments.Common.MongoClient.Exceptions;

public class MongoException : Exception
{
    public string Code { get; }
    
    public MongoException(string code, string message)
        : base(message)
    {
        Code = code;
    }
}
