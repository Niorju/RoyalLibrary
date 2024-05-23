using Domain.Entities.Validations;
using Domain.Interfaces.Validation;
using Domain.Validation;

namespace RoyalLibrary.Domain.Entities
{
    public class Book : ISelfValidation
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalCopies { get; set; } = 0;
        public int CopiesInUse { get; set; } = 0;
        public string Type { get; set; }
        public string Isbn { get; set; }
        public string Category { get; set; }
        public ValidationResult ValidationResult { get; private set; }

        public bool IsValid
        {
            get
            {
                var checkBook = new BookValidations();
                ValidationResult = checkBook.Valid(this);
                return ValidationResult.IsValid;
            }
        }
    }
}
