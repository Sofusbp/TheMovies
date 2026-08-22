using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Navigation;
using TheMovies.Models;
using System.Text.Json;
using System.IO;
using System.Linq.Expressions;
using System.Security.Policy;

namespace TheMovies.Repositories
{
    class FileMovieRepository
    {
        private string _fileName = "movies.json";

        public List<Movie> LoadMovies()
        {
            if (File.Exists(_fileName))
            {
                string json = File.ReadAllText(_fileName);
                return JsonSerializer.Deserialize<List<Movie>>(json) ?? new List<Movie>();
            }
            return new List<Movie>();
        }

        public void SaveMovie(List<Movie> movies) 
        {
            string moviesJson = JsonSerializer.Serialize(movies, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_fileName, moviesJson);
        }

    }
}

