using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Requests;
using MediatR;

namespace BioEngine.Data.Files.Handlers
{
    public class FilesCategoryProcessHandler : CategoryProcessHandlerBase<FileCat, File>
    {
        protected FilesCategoryProcessHandler(IMediator mediator, BWContext dbContext,
            ParentEntityProvider parentEntityProvider) : base(mediator, dbContext, parentEntityProvider)
        {
        }

        protected override async Task<IEnumerable<File>> GetCatItems(FileCat cat, int count)
        {
            return (await Mediator.Send(new GetCategoryFilesRequest(cat) {PageSize = count})).files;
        }
    }
}