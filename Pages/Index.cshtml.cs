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
    public WatchItem watchItem { get; set; } = new WatchItem();

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
            watchItem.ItemUrl = WatchUrl;
            await GetInfosFromUrl();
        }
        else
        {
            Console.WriteLine($"Incorrect Input {WatchUrl}");
        }
        

        return RedirectToPage("./Index");
    }

    public IList<WatchItem> WatchItems { get;set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var context = SeedData.context;

        WatchItems = await context.WatchItem.ToListAsync();
        
        return Page();
    }

    private async Task GetInfosFromUrl() {        
        
        using (var client = new HttpClient())
        {
            // We'll use the GetAsync method to send 
            // a GET request to the specified URL
            var response = await client.GetAsync(WatchUrl);

            // If the response is successful, we'll
            // interpret the response as XML
            if (response.IsSuccessStatusCode)
            {
                var xml = await response.Content.ReadAsStringAsync();

                // We can then use the LINQ to XML API to query the XML
                var doc = XDocument.Parse(xml);

                // Let's query the XML to get all of the <title> elements
                var price = from el in doc.Descendants("strong")
                                select el.Value;
                watchItem.Price = toPrice(price.First());

                var name = from el in doc.Descendants("title")
                                select el.Value;
                watchItem.Name = name.First().Split(" - kaufen")[0];

                var cat = from el in doc.Descendants("img")
                                select el.FirstAttribute;
                watchItem.Category = cat.First().ToString().Split('"')[1];


                var available = from el in doc.Descendants("svg")
                                select el.Value;
                watchItem.isAvailable = Availability.inStock;

                Console.WriteLine(watchItem.ToString());
                
                
                var context = SeedData.context;
                context.WatchItem.Add(watchItem);
                await context.SaveChangesAsync();
                
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
