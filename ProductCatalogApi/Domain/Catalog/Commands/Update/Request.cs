using MediatR;

namespace ProductCatalogApi.Domain.Catalog.Commands.Update
{
    public class Request : Validatable, IRequest<Result>
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }

        public string PictureFileName { get; set; }
        
        public string PictureUrl { get; set; }

        public int CatalogTypeId { get; set; }

        public int CatalogBrandId { get; set; }
        
        public override void Validate()
        {
            if (Price <= 0)
                AddNotification("Price", "Valor inválido");

            if (string.IsNullOrEmpty(Name))
                AddNotification("Name", "Nome é obrigatória");
        }
    }
}