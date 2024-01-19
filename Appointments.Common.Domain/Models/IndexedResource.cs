using FluentValidation;

namespace Appointments.Common.Domain.Models;

// TODO Rename to IndexedAsset?
public struct IndexedResource
{
    public int Index { get; private set; }

    // TODO Use AssetPath model instead?
    public string Path { get; }

    public IndexedResource()
    {
        Index = 0;
        Path = string.Empty;
    }

    public IndexedResource(int index, string path)
    {
        Index = index;
        Path = path;
    }

    public void SetIndex(int index)
    {
        Index = index;
    }

    public override readonly string ToString()
    {
        return @$"[{Index}] = ""{Path}""";
    }

    public static IndexedResource Default()
    {
        return new IndexedResource();
    }
}

public sealed class IndexedResourceValidator : AbstractValidator<IndexedResource>
{
    public IndexedResourceValidator()
    {
        RuleFor(x => x.Index)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Path)
            .NotEmpty();
    }
}
