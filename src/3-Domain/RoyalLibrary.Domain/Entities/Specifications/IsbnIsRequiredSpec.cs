using Domain.Interfaces.Specification;
using RoyalLibrary.Domain.Entities;

namespace Domain.Entities.Specifications
{
    class IsbnIsRequiredSpec : ISpecification<Book>
    {
        public bool IsSatisfiedBy(Book entity)
        {
            return entity.Isbn.Trim().Length > 0;
        }
    }
}
