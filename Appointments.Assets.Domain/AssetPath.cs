using FluentValidation;

namespace Appointments.Assets.Domain;

public readonly struct AssetPath
{
    public string Value { get; }

    public AssetPath()
    {
        Value = string.Empty;
        new AssetPathValidator().ValidateAndThrow(this);
    }

    public AssetPath(string value)
    {
        Value = value.Trim().ToLower();
        new AssetPathValidator().ValidateAndThrow(this);
    }

    public override string ToString()
    {
        return Value;
    }
}

public sealed class AssetPathValidator : AbstractValidator<AssetPath>
{
    public AssetPathValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .Must(path => path.First() != '/')
                .WithMessage("Path must not start with '/'")
            .Must(path => path.Last() != '/')
                .WithMessage("Path must not end with '/'")
            .Must(path =>
            {
                var pathParts = path.Split('/');
                var fileName = pathParts.LastOrDefault();

                if (fileName is null)
                    return false;

                var fileNameParts = fileName.Split('.');

                return fileNameParts.Length == 2;
            })
            .WithMessage("Path must have file name with extension, e.g.: my-image.png");
    }
}
