using System.Xml;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using DigitecPriceWatcher.Pages_WatchItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using RazorPagesWatchItem.Models;

namespace DigitecPriceWatcher.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public IList<WatchItem> WatchItems { get;set; } = default!;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    public string WatchUrl { get; set; } = default!;
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || WatchUrl == null)
        {
            return Page();
        }

        if (WatchUrl.Contains("digitec") || WatchUrl.Contains("galaxus"))
        {
            await GetInfosFromUrl();
        }
        else
        {
            Console.WriteLine($"Incorrect Input {WatchUrl}");
        }
    
        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var context = SeedData.context;

        WatchItems = await context.WatchItem.ToListAsync();

        if (WatchItems.Any()) 
        {
            foreach (var item in WatchItems)
            {
                await parseWebpage(item);

                context.Attach(item).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if ((context.WatchItem?.Any(e => e.ID == item.ID)).GetValueOrDefault())
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
        
        return Page();
    }

    private async Task GetInfosFromUrl()
    {
        WatchItem tempWatchItem = new WatchItem();
        tempWatchItem.ItemUrl = WatchUrl;
        await parseWebpage(tempWatchItem);

        var context = SeedData.context;
        context.WatchItem.Add(tempWatchItem);
        await context.SaveChangesAsync();
    }

    private async Task parseWebpage(WatchItem item)
    {
        using (var client = new HttpClient())
        {
            // We'll use the GetAsync method to send 
            // a GET request to the specified URL
            var response = await client.GetAsync(item.ItemUrl);

            // If the response is successful, we'll
            // interpret the response as XML
            if (response.IsSuccessStatusCode)
            {
                var xml = await response.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(xml);

                var price = from el in doc.Descendants("strong")
                            select el.Value;
                var parsedPrice = toPrice(price.First());
                item.Price = parsedPrice;

                var name = from el in doc.Descendants("title")
                           select el.Value;
                item.Name = name.First().Split(" - kaufen")[0];

                var cat = from el in doc.Descendants("img")
                          select el.FirstAttribute;
                item.Category = cat.First().ToString().Split('"')[1];

                var available = from el in doc.Descendants("svg")
                                select el.Value;
                item.isAvailable = Availability.inStock;
            }
        }
    }

    private decimal toPrice(string value)
    {
        var splitValues = value.Split('.');
        var franken = splitValues[0];
        var rappen = splitValues[1];
        if (rappen == "–") 
        {
            rappen = "00";
        }

        decimal price = decimal.MinValue;

        Decimal.TryParse(franken + "." + rappen, out price);
        return price;
    }
}
