using MediatR;
using Rubiconmp.Api.DataLayer;

namespace Rubiconmp.Api.Application.Queries
{
    public record GetRectanglesBySegmentQuery(double X1, double Y1, double X2, double Y2) : IRequest<IEnumerable<Rectangle>>;
}