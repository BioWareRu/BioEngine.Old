using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Queries;
using JetBrains.Annotations;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class FilesCategoryProcessHandler : CategoryProcessHandlerBase<FileCategoryProcessQuery, FileCat, File>
    {
        public FilesCategoryProcessHandler(HandlerContext<FilesCategoryProcessHandler> context,
            ParentEntityProvider parentEntityProvider) : base(context, parentEntityProvider)
        {
        }

        protected override async Task<IEnumerable<File>> GetCatItemsAsync(FileCat cat, int count)
        {
            return (await Mediator.Send(new GetCategoryFilesQuery(cat) {PageSize = count})).models;
        }
    }
}