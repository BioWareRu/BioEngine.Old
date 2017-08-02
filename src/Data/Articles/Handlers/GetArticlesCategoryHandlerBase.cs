using System;
using System.Collections.Generic;
using System.Text;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Requests;
using BioEngine.Data.Core;
using MediatR;

namespace BioEngine.Data.Articles.Handlers
{
    public abstract class GetArticlesCategoryHandlerBase<TRequest, TResponse> : RequestHandlerBase<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ParentEntityProvider _parentEntityProvider;

        protected GetArticlesCategoryHandlerBase(IMediator mediator, BWContext dbContext,
            ParentEntityProvider parentEntityProvider) : base(mediator, dbContext)
        {
            _parentEntityProvider = parentEntityProvider;
        }

        protected async void ProcessCat(ArticleCat cat, IGetArticlesCategoryRequest message)
        {
            await DBContext.Entry(cat).Collection(x => x.Children).LoadAsync();
            cat.Parent = message.Parent ?? await _parentEntityProvider.GetModelParent(cat);
            if (message.LoadLastItems != null)
            {
                cat.Items = (await Mediator.Send(new GetCategoryArticlesRequest(cat, (int) message.LoadLastItems)))
                    .articles;
            }
            if (message.LoadChildren)
            {
                foreach (var articleCat in cat.Children)
                {
                    ProcessCat(articleCat, message);
                }
            }
        }
    }
}