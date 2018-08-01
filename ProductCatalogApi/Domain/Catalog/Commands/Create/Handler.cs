using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProductCatalogApi.Data;

namespace ProductCatalogApi.Domain.Catalog.Commands.Create
{
    public class Handler : IRequestHandler<Request, Result>
    {
        private readonly IMediator _mediator;
        
        private readonly CatalogContext _catalogContext;

        public Handler(IMediator mediator, CatalogContext catalogContext)
        {
            this._mediator = mediator;
            this._catalogContext = catalogContext;
        }

        public async Task<Result> Handle(Request request,  CancellationToken cancellationToken)
        {
            var item = new CatalogItem
            {
                CatalogBrandId = request.CatalogBrandId,
                Price = request.Price,
                CatalogTypeId = request.CatalogTypeId,
                Description = request.Description,
                Name = request.Name,
                PictureFileName = request.PictureFileName
            };
            _catalogContext.CatalogItems.Add(item);
            await _catalogContext.SaveChangesAsync(cancellationToken);
            
            /*
            await _mediator.Publish(new Notification
            {
                NomeFuncionario = solicitacao.Funcionario.Nome,
                ValorAntigo = solicitacao.Valor,
                Valor = request.Valor,
                DescricaoAntiga = request.Descricao,
                Descricao = solicitacao.Descricao
            }, cancellationToken);
            */
            
            return new Result<CatalogItem>(item);
        }
    }
}