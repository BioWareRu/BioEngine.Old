using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using MediatR;

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    internal class GetParentByUrlHandler : QueryHandlerBase<GetParentByUrlQuery, IParentModel>
    {
        private readonly ParentEntityProvider _provider;

        public GetParentByUrlHandler(IMediator mediator, BWContext dbContext,
            ParentEntityProvider provider) : base(mediator, dbContext)
        {
            _provider = provider;
        }

        public override async Task<IParentModel> Handle(GetParentByUrlQuery message)
        {
            return await _provider.GetParenyByUrl(message.Url);
        }
    }
}