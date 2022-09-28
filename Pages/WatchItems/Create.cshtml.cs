using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPagesWatchItem.Models;

namespace DigitecPriceWatcher.Pages_WatchItems
{
    public class CreateModel : PageModel
    {
        private readonly RazorPagesWatchItemContext _context;

        public CreateModel(RazorPagesWatchItemContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public WatchItem WatchItem { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.WatchItem == null || WatchItem == null)
            {
                return Page();
            }

            _context.WatchItem.Add(WatchItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
