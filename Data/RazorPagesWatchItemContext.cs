using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorPagesWatchItem.Models;

    public class RazorPagesWatchItemContext : DbContext
    {
        public RazorPagesWatchItemContext (DbContextOptions<RazorPagesWatchItemContext> options)
            : base(options)
        {
        }

        public DbSet<RazorPagesWatchItem.Models.WatchItem> WatchItem { get; set; } = default!;
    }
