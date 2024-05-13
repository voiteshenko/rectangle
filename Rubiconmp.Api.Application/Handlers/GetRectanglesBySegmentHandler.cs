using Common.DataLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rubiconmp.Api.Application.Queries;
using Rubiconmp.Api.DataLayer;
using Z.EntityFramework.Plus;

namespace Rubiconmp.Api.Application.Handlers
{
    public class GetRectanglesBySegmentHandler : EntityService<Rectangle>, IRequestHandler<GetRectanglesBySegmentQuery, IEnumerable<Rectangle>>
    {
        public GetRectanglesBySegmentHandler(IRepository<Rectangle> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }

        public async Task<IEnumerable<Rectangle>> Handle(GetRectanglesBySegmentQuery request, CancellationToken cancellationToken)
        {
            var rectangles = await Repository.Where(r => request.X1 >= r.X && request.X2 <= r.X + r.Width && request.Y1 >= r.Y && request.Y2 <= r.Y + r.Height).ToListAsync();

            return rectangles;
        }
    }
}