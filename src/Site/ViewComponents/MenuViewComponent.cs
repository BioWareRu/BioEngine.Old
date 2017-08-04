using System.Threading.Tasks;
using BioEngine.Data.Base.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public MenuViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(string key)
        {
            var menu = await _mediator.Send(new GetMenuByKeyQuery(key));
            return View(menu.GetMenu());
        }
    }
}