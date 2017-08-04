using System.Threading.Tasks;
using BioEngine.Data.Base.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.ViewComponents
{
    public class CounterViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public CounterViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var block = await _mediator.Send(new GetBlockByIdRequest("counter"));
            return View(block);
        }
    }
}