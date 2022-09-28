using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesWatchItem.Models;

namespace DigitecPriceWatcher.Pages_WatchItems
{
    public class EditModel : PageModel
    {
        private readonly RazorPagesWatchItemContext _context;

        public EditModel(RazorPagesWatchItemContext context)
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

            var watchitem =  await _context.WatchItem.FirstOrDefaultAsync(m => m.ID == id);
            if (watchitem == null)
            {
                return NotFound();
            }
            WatchItem = watchitem;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(WatchItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WatchItemExists(WatchItem.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool WatchItemExists(int id)
        {
          return (_context.WatchItem?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
