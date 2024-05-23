using Domain.Interfaces.Specification;
using RoyalLibrary.Domain.Entities;

namespace Domain.Entities.Specifications
{
    public class TypeIsRequiredSpec : ISpecification<Book>
    {
        public bool IsSatisfiedBy(Book entity)
        {
            return entity.Type.Trim().Length > 0;
        }
    }
}
