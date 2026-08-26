using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TheMovies.Models
{
    public class Cinema
    {
        public string Navn
        { get; set; }

        public ObservableCollection<CinemaRoom> Sale
        {  get; set; } = new ObservableCollection<CinemaRoom>();
    }
}