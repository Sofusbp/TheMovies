using System;

namespace TheMovies.Models
{
    public class Screening
    {
        public Movie Film
        { get; set; }

        public Cinema Biograf
        { get; set; }

        public CinemaRoom Sal
        { get; set; }

        public DateTime StartTidspunkt
        { get; set; }

        public DateTime SlutTidspunkt
        { get; set; }
    }
}
