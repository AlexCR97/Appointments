using Appointments.Common.Domain.Models;

namespace Appointments.Infrastructure.Mongo.Documents;

internal sealed record IndexedResourceDocument(
    int Index,
    string Path)
{
    internal static IndexedResourceDocument From(IndexedResource resource)
    {
        return new IndexedResourceDocument(
            resource.Index,
            resource.Path);
    }

    internal IndexedResource ToModel()
    {
        return new IndexedResource(
            Index,
            Path);
    }
}
