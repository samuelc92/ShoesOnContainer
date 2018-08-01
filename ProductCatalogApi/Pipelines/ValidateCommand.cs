using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ProductCatalogApi.Pipelines
{
    public class ValidateCommand<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : Domain.Result
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is ProductCatalogApi.Domain.Validatable validatable)
            {
                validatable.Validate();
                if (validatable.Invalid)
                {
                    Domain.Result validations = new Domain.Result();
                    foreach (Flunt.Notifications.Notification notification in validatable.Notifications)
                        validations.AddValidation(notification.Message);

                    var response = validations as TResponse;
                    return response;
                }
            }

            TResponse result = await next();
            return result;
        }
    }
}