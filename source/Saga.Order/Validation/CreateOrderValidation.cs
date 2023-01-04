using FluentValidation;
using Saga.Order.Dtos;

namespace Saga.Order.Validation
{

    public class CreateOrderValidation : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidation()
        {
            RuleFor(customer => customer.UserId).NotNull();
            RuleFor(customer => customer.Currency).NotNull();
            RuleFor(customer => customer.Items).Custom((value, context) =>
            {
                if(value.Count() == 0)
                {
                    context.AddFailure(nameof(CreateOrderRequest.Items), "Must not be empty.");
                }
            });
        }
    }
}
