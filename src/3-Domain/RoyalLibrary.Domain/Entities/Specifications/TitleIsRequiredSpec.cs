using Domain.Interfaces.Specification;
using RoyalLibrary.Domain.Entities;

namespace Domain.Entities.Specifications
{
    public class TitleIsRequiredSpec : ISpecification<Book>
    {
        public bool IsSatisfiedBy(Book entity)
        {
            return entity.Title.Trim().Length > 0;
        }
    }
}
