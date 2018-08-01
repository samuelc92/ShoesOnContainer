using MediatR;

namespace ProductCatalogApi.Domain.Catalog.Commands.Delete
{
    public class Request: Validatable, IRequest<Result>
    {
        public int Id { get; set; }
        
        public override void Validate()
        {
            if (Id <= 0)
                AddNotification("Id", "Valor inválido");
        }
        
    }
}