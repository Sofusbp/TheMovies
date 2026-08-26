using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TheMovies.Models;

namespace TheMovies.Repositories
{
    public class FileScreeningRepository
    {
        private string _fileName = "screenings.json";

        public List<Screening> LoadScreenings()
        {
            if (File.Exists(_fileName))
            {
                string json = File.ReadAllText(_fileName);

                return JsonSerializer.Deserialize<List<Screening>>(json)
                       ?? new List<Screening>();
            }

            return new List<Screening>();
        }

        public void SaveScreenings(List<Screening> screenings)
        {
            string screeningsJson = JsonSerializer.Serialize(
                screenings,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_fileName, screeningsJson);
        }
    }
}