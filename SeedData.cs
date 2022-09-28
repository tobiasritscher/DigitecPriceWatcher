using Microsoft.EntityFrameworkCore;

namespace RazorPagesWatchItem.Models
{
    public static class SeedData
    {
        public static RazorPagesWatchItemContext? context { get; set; }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            context = new RazorPagesWatchItemContext(serviceProvider.GetRequiredService<DbContextOptions<RazorPagesWatchItemContext>>());
            
            if (context == null || context.WatchItem == null)
            {
                throw new ArgumentNullException("Null RazorPagesWatchItemContext");
            }

            // Look for any movies.
            if (context.WatchItem.Any())
            {
                return;   // DB has been seeded
            }
            
        }
    }
}