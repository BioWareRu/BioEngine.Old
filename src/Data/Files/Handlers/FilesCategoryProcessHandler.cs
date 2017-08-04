using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Queries;
using JetBrains.Annotations;
using MediatR;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class FilesCategoryProcessHandler : CategoryProcessHandlerBase<FileCategoryProcessQuery, FileCat, File>
    {
        public FilesCategoryProcessHandler(IMediator mediator, BWContext dbContext,
            ParentEntityProvider parentEntityProvider) : base(mediator, dbContext, parentEntityProvider)
        {
        }

        protected override async Task<IEnumerable<File>> GetCatItems(FileCat cat, int count)
        {
            return (await Mediator.Send(new GetCategoryFilesQuery(cat) {PageSize = count})).files;
        }
    }
}