using Domain.Entities.Specifications;
using Domain.Validation;
using RoyalLibrary.Domain.Entities;
using RoyalLibrary.Domain.Entities.Validations;

namespace Domain.Entities.Validations
{
    public class BookValidations : Validation<Book>
    {
        public BookValidations()
        {
            base.AddRule(new ValidationRule<Book>(new CategoryIsRequiredSpec(), ValidationMessages.CategoryRequired));
            base.AddRule(new ValidationRule<Book>(new FirstNameIsRequiredSpec(), ValidationMessages.FirstNameRequired));
            base.AddRule(new ValidationRule<Book>(new IsbnIsRequiredSpec(), ValidationMessages.IsbnRequired));
            base.AddRule(new ValidationRule<Book>(new LastNameIsRequiredSpec(), ValidationMessages.LastNameRequired));
            base.AddRule(new ValidationRule<Book>(new TitleIsRequiredSpec(), ValidationMessages.TitleRequired));
            base.AddRule(new ValidationRule<Book>(new TotalCopiesSize(), ValidationMessages.TotalCopiesSizeRequired));
            base.AddRule(new ValidationRule<Book>(new TypeIsRequiredSpec(), ValidationMessages.TypeIsRequired));            
        }
    }
}
