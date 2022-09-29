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
    public class DeleteModel : PageModel
    {
        private readonly RazorPagesWatchItemContext _context;

        public DeleteModel(RazorPagesWatchItemContext context)
        {
            _context = context;
        }

        [BindProperty]
      public WatchItem WatchItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.WatchItem == null)
            {
                return NotFound();
            }

            var watchitem = await _context.WatchItem.FirstOrDefaultAsync(m => m.ID == id);

            if (watchitem == null)
            {
                return NotFound();
            }
            else 
            {
                WatchItem = watchitem;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.WatchItem == null)
            {
                return NotFound();
            }
            var watchitem = await _context.WatchItem.FindAsync(id);

            if (watchitem != null)
            {
                WatchItem = watchitem;
                _context.WatchItem.Remove(WatchItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./../Index");
        }
    }
}
