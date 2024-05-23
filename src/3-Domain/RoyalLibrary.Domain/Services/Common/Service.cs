using Domain.Interfaces.Validation;
using Domain.Validation;

namespace Domain.Services.Common
{
    public class Service<TEntity> where TEntity : class
    {
        public Service()
        {            
            ValidationResult = new ValidationResult();
        }

        protected ValidationResult ValidationResult { get; }
        public virtual ValidationResult InsertUpdate(TEntity entity)
        {
            if (!ValidationResult.IsValid)
                return ValidationResult;

            if (entity is ISelfValidation selfValidationEntity && !selfValidationEntity.IsValid)
                return selfValidationEntity.ValidationResult;

            return ValidationResult;
        }       
    }
}
