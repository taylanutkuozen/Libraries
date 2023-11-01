using FluentValidation;
using LibrariesFluentValidation.Models;
namespace LibrariesFluentValidation.FluentValidators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public string NotEmptyMessage { get; } = "{PropertyName} alanı boş olamaz";
        public CustomerValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty().WithMessage(NotEmptyMessage);
            RuleFor(x => x.CustomerMail).NotEmpty().WithMessage(NotEmptyMessage).EmailAddress().WithMessage("Email alanı doğru formatta olmalıdır.");
            RuleFor(x => x.CustomerAge).NotEmpty().WithMessage(NotEmptyMessage).InclusiveBetween(18, 60).WithMessage("Age alanı 18 ile 60 arasında olmalıdır.");
            RuleFor(x => x.BirthDay).NotEmpty().WithMessage(NotEmptyMessage).Must(x =>
            {
                return DateTime.Now.AddYears(-18) >= x;
            }).WithMessage("18 yaşından büyük olmalısınız.");
            RuleFor(x => x.Gender).IsInEnum().WithMessage("{PropertyName} alanı Erkek=1, Bayan=2 olmalıdır.");
            RuleForEach(x => x.Addresses).SetValidator(new AddressValidator());
        }
    }
}