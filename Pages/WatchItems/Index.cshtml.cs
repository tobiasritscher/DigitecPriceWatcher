using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesWatchItem.Models;

namespace DigitecPriceWatcher.Pages_WatchItems
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesWatchItemContext _context;

        public IndexModel(RazorPagesWatchItemContext context)
        {
            _context = context;
        }

        public IList<WatchItem> WatchItem { get;set; } = default!;

        public async Task OnGetViewAsync()
        {
            if (_context.WatchItem != null)
            {
                WatchItem = await _context.WatchItem.ToListAsync();
            }
        }
    }
}
