using Domain.Interfaces.Specification;
using RoyalLibrary.Domain.Entities;

namespace Domain.Entities.Specifications
{
    public class TotalCopiesSize : ISpecification<Book>
    {
        public bool IsSatisfiedBy(Book entity)
        {
            return entity.TotalCopies > 0;
        }
    }
}
