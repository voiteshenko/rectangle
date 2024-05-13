using Common.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rubiconmp.Api.Application.Queries;
using Rubiconmp.Api.Dtos.Request;
using Rubiconmp.Api.Dtos.Response;

namespace Rubiconmp.Api.Host.Controllers
{
    public class RectangleController : BaseApiController
    {
        private readonly IMediator _mediator;

        public RectangleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> GetRectanglesBySegment(GetRectanglesBySegmentRequest request)
        {
            var result = await _mediator.Send(new GetRectanglesBySegmentQuery(request.X1, request.Y1, request.X2, request.Y2));

            return result.Any()
                ? Ok(result.Select(r => new GetRectanglesBySegmentResponse
                {
                    Id = r.Id,
                    Height = r.Height,
                    Width = r.Width,
                    X = r.X,
                    Y = r.Y
                }).ToList())
                : NotFound();
        }
    }
}