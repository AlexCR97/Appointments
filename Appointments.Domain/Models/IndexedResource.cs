namespace Appointments.Domain.Models;

public struct IndexedResource
{
    public int Index { get; private set; }
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

    public override string ToString()
    {
        return @$"[{Index}] = ""{Path}""";
    }

    public static IndexedResource Default()
    {
        return new IndexedResource();
    }
}
