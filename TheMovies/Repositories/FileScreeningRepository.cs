using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
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

                List<Screening> screenings =
                    JsonSerializer.Deserialize<List<Screening>>(json)
                    ?? new List<Screening>();

                using JsonDocument document = JsonDocument.Parse(json);

                bool manglerId = document.RootElement.ValueKind ==
                                 JsonValueKind.Array &&
                                 document.RootElement.EnumerateArray().Any(item =>
                                     !item.TryGetProperty("Id", out JsonElement id) ||
                                     id.ValueKind != JsonValueKind.String ||
                                     !Guid.TryParse(id.GetString(), out Guid resultat) ||
                                     resultat == Guid.Empty);

                if (manglerId)
                {
                    foreach (Screening screening in screenings)
                    {
                        if (screening.Id == Guid.Empty)
                        {
                            screening.Id = Guid.NewGuid();
                        }
                    }

                    SaveScreenings(screenings);
                }

                return screenings;
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
