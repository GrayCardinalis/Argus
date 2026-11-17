using Argus.Constants.Database;
using Argus.Dtos.SupportRequest.Comments;
using FluentValidation;

namespace Argus.Validators.SupportRequest.Comments
{
    public class CreateSupportRequestCommentDtoValidator
        : AbstractValidator<CreateSupportRequestCommentDto>
    {
        public CreateSupportRequestCommentDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Message)
                .NotEmpty()
                .MaximumLength(SupportRequestCommentFieldLengths.MessageMaxLength);
        }
    }
}