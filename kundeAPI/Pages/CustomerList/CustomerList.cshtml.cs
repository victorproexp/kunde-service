using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using kundeAPI.Models;

namespace MyApp.Namespace
{
    public class CustomerListModel : PageModel
    {
        [BindProperty]
        public List<Kunde>? Kunder { get; set; }
        public async Task OnGetAsync()
        {
            using HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:80/")
            };

            Kunder = await client.GetFromJsonAsync<List<Kunde>>("api/kunde");
        }
    }
}
