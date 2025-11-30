using System.Text.Json;
using GymSystem.Models;

namespace GymSystem.Data
{
    public static class JsonDb
    {
        private static readonly string FilePath = "gym_data.json";

        public static List<Client> Clients { get; set; } = new List<Client>();
        public static int NextId = 1;

        static JsonDb()
        {
            LoadData();
        }

        public static void LoadData()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    Clients = JsonSerializer.Deserialize<List<Client>>(json) ?? new List<Client>();
                    
                    if (Clients.Any())
                    {
                        NextId = Clients.Max(c => c.Id) + 1;
                    }
                }
            }
            else
            {
                Clients.Add(new Client { Id = 1, Name = "José Rafael", LastName = "Castillo Eve", Age = 20, Plan = "Premium" });
                SaveChanges();
            }
        }

        public static void SaveChanges()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(Clients, options);
            File.WriteAllText(FilePath, json);
        }
    }
}