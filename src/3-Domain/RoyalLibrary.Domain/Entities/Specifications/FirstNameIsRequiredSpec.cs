using Domain.Interfaces.Specification;
using RoyalLibrary.Domain.Entities;

namespace Domain.Entities.Specifications
{
    public class FirstNameIsRequiredSpec : ISpecification<Book>
    {
        public bool IsSatisfiedBy(Book entity)
        {
            return entity.FirstName.Trim().Length > 0;
        }
    }
}
