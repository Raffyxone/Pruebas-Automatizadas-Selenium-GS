using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GymSystem.Data;
using GymSystem.Models;

namespace GymSystem.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        public IActionResult Index()
        {
            return View(JsonDb.Clients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Client client)
        {
            if (!ModelState.IsValid) return View(client);

            client.Id = JsonDb.NextId++;
            JsonDb.Clients.Add(client);
            JsonDb.SaveChanges(); 

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var client = JsonDb.Clients.FirstOrDefault(c => c.Id == id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost]
        public IActionResult Edit(Client client)
        {
            if (!ModelState.IsValid) return View(client);

            var existing = JsonDb.Clients.FirstOrDefault(c => c.Id == client.Id);
            if (existing != null)
            {
                existing.Name = client.Name;
                existing.LastName = client.LastName;
                existing.Age = client.Age;
                existing.Plan = client.Plan;
                JsonDb.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var client = JsonDb.Clients.FirstOrDefault(c => c.Id == id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var client = JsonDb.Clients.FirstOrDefault(c => c.Id == id);
            if (client != null) 
            {
                JsonDb.Clients.Remove(client);
                JsonDb.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}