using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Data.Base.Requests;
using BioEngine.Data.Core;
using MediatR;

namespace BioEngine.Data.Base.Handlers
{
    public class GetParentByUrlHandler : RequestHandlerBase<GetParentByUrlRequest, IParentModel>
    {
        private readonly ParentEntityProvider _provider;

        public GetParentByUrlHandler(IMediator mediator, BWContext dbContext,
            ParentEntityProvider provider) : base(mediator, dbContext)
        {
            _provider = provider;
        }

        public override async Task<IParentModel> Handle(GetParentByUrlRequest message)
        {
            return await _provider.GetParenyByUrl(message.Url);
        }
    }
}