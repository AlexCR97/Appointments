using Appointments.Common.Domain.Models;

namespace Appointments.Api.Tenant.Models;

public sealed record IndexedResourceModel(
    int Index,
    string Path)
{
    internal static IndexedResourceModel From(IndexedResource resource)
    {
        return new IndexedResourceModel(
            resource.Index,
            resource.Path);
    }

    internal IndexedResource ToModel()
    {
        return new IndexedResource(Index, Path);
    }
}
