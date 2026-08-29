using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TheMovies.Models;

namespace TheMovies.Repositories
{
    public class FileReservationRepository
    {
        private string _fileName;

        public FileReservationRepository(
            string fileName = "reservations.json")
        {
            _fileName = fileName;
        }

        public List<Reservation> LoadReservations()
        {
            if (File.Exists(_fileName))
            {
                string json = File.ReadAllText(_fileName);

                return JsonSerializer.Deserialize<List<Reservation>>(json)
                       ?? new List<Reservation>();
            }

            return new List<Reservation>();
        }

        public void SaveReservations(List<Reservation> reservations)
        {
            string reservationsJson = JsonSerializer.Serialize(
                reservations,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_fileName, reservationsJson);
        }
    }
}
