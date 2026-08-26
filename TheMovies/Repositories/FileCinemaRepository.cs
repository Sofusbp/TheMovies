using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TheMovies.Models;

namespace TheMovies.Repositories
{
    public class FileCinemaRepository
    {
        private string _fileName = "cinemas.json";

        public List<Cinema> LoadCinemas()
        {
            if (File.Exists(_fileName))
            {
                string json = File.ReadAllText(_fileName);

                return JsonSerializer.Deserialize<List<Cinema>>(json)
                       ?? new List<Cinema>();
            }

            return new List<Cinema>();
        }

        public void SaveCinemas(List<Cinema> cinemas)
        {
            string cinemasJson = JsonSerializer.Serialize(
                cinemas,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_fileName, cinemasJson);
        }
    }
}