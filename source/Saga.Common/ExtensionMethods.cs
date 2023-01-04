using FluentValidation;
using FluentValidation.Results;

namespace Saga.Common
{
    public static class ExtensionMethods
    {
        public static List<ValidationFailure> GetErrors<TModel>(this IValidator<TModel> validator, TModel request)
        {
            var result = validator.Validate(request);

            return result.Errors.Where(x => x != null).ToList();
        }
    }
}
