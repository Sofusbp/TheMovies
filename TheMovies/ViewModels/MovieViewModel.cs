using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TheMovies.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TheMovies.ViewModels
{
    public class MovieViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand RegistrerCommand { get; }

        public string Titel
        {
            get { return _movie.Titel; }
            set { _movie.Titel = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Titel")); }
            
        }

        public int? Varighed
        {
            get { return _movie.Varighed == 0 ? null : _movie.Varighed; }
            set { _movie.Varighed = value ?? 0; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs ("Varighed")); }
        }

        public string Genre
        { 
            get { return _movie.Genre; }
            set { _movie.Genre = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Genre")); }
        }

        private Movie _movie;
        public MovieViewModel(string Titel, int Varighed, string Genre)
        {
            _movie = new Movie();
            _movie.Titel = Titel;
            _movie.Varighed = Varighed;
            _movie.Genre = Genre;

            RegistrerCommand = new RelayCommand(Parameter => RegistrerFilm());
        }

        ObservableCollection<Movie> _movieList = new ObservableCollection<Movie>();

        public ObservableCollection<Movie> MovieList { get { return _movieList; } }
        

        public void RegistrerFilm()
        {
            _movieList.Add(_movie);
        }
    }
}
