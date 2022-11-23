using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RESTClientCMSShoppingCart.Models;
using System.Text;

namespace RESTClientCMSShoppingCart.Controllers
{
    public class PagesController : Controller
    {
        // GET /pages
        public async Task<IActionResult> Index()
        {
            List<Page> pages = new List<Page>();
            using (var httpClient = new HttpClient())
            {
                using var request = await httpClient.GetAsync("https://localhost:7226/api/pages");
                string response = await request.Content.ReadAsStringAsync();
                pages = JsonConvert.DeserializeObject<List<Page>>(response);
            }
            return View(pages);
        }
        
        // GET /pages/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Page page = new Page();
            using (var httpClient = new HttpClient())
            {
                using var request = await httpClient.GetAsync($"https://localhost:7226/api/pages/{id}");
                string response = await request.Content.ReadAsStringAsync();
                page = JsonConvert.DeserializeObject<Page>(response);
            }
            return View(page);
        }

        // POST /pages/edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Page page )
        {
            page.Slug = page.Title.Replace(" ", "-").ToLower();

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(page), Encoding.UTF8, "application/json");
                var request = await httpClient.PutAsync($"https://localhost:7226/api/pages/{page.Id}", content);
                string response = await request.Content.ReadAsStringAsync();
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // GET /pages/create 
        public IActionResult Create() => View();

        // POST /pages/create
        [HttpPost]
        public async Task<IActionResult> Create(Page page)
        {
            page.Slug = page.Title.Replace(" ", "-").ToLower();
            page.Sorting = 100;
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(page), Encoding.UTF8, "application/json");
                var request = await httpClient.PostAsync($"https://localhost:7226/api/pages", content);
                string response = await request.Content.ReadAsStringAsync();
            }
            return RedirectToAction("Index");
        }


        // GET /pages/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var request = await httpClient.DeleteAsync($"https://localhost:7226/api/pages/{id}");
            }
            return RedirectToAction("Index");

        }

    }
}
