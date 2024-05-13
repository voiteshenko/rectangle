using Microsoft.EntityFrameworkCore;

namespace Rubiconmp.Api.DataLayer
{
    public class ShapeContext : DbContext
    {
        public ShapeContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Rectangle> Rectangles { get; set; }
    }
}