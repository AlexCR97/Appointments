using Appointments.Application.Requests.Services;
using FluentValidation;

namespace Appointments.Application.Validations.Services;

internal sealed class UploadImagesRequestValidator : AbstractValidator<UploadImagesRequest>
{
    public UploadImagesRequestValidator()
    {
        RuleForEach(x => x.Images)
            .SetValidator(new IndexedImageValidator());
    }

    private class IndexedImageValidator : AbstractValidator<UploadImagesRequest.IndexedImage>
    {
        public IndexedImageValidator()
        {
            RuleFor(x => x.Index)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.FileName)
                .NotEmpty();

            RuleFor(x => x.File)
                .NotEmpty();
        }
    }
}
