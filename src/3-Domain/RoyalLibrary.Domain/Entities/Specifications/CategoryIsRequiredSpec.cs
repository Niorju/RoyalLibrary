using Domain.Interfaces.Specification;
using RoyalLibrary.Domain.Entities;

namespace Domain.Entities.Specifications
{
    public class CategoryIsRequiredSpec : ISpecification<Book>
    {
        public bool IsSatisfiedBy(Book entity)
        {
            return entity.Category.Trim().Length > 0;
        }
    }
}
