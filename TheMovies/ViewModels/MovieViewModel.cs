using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TheMovies.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TheMovies.Repositories;
using System.Linq;


namespace TheMovies.ViewModels
{
    public class MovieViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        public ICommand RegistrerCommand { get; }
        public ICommand SletCommand { get; }

        private FileMovieRepository _repository;
        private Movie _selectedMovie;
        private string _titel;
        private string _varighedInput;
        private string _genre;
        private string _fejlTitel;
        private string _fejlVarighed;
        private string _fejlGenre;
        private string _instruktøer;
        private string _succesbesked = "";
        private DateTime _premieredato;


        public string Titel
        {
            get { return _titel; }
            set
            {
                _titel = value;

                if (string.IsNullOrWhiteSpace(value))
                {
                    FejlTitel = "Titel må ikke være tom";
                }
                else
                {
                    FejlTitel = "";
                }

                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("Titel"));
            }
        }

        public string VarighedInput
        {
            get { return _varighedInput; }
            set
            {
                _varighedInput = value;

                if (string.IsNullOrWhiteSpace(value))
                {
                    FejlVarighed = "Varighed må ikke være tom";
                }
                else if (!int.TryParse(value, out int varighed))
                {
                    FejlVarighed = "Varighed skal være et tal";
                }
                else if (varighed <= 0)
                {
                    FejlVarighed = "Varighed skal være større end 0";
                }
                else
                {
                    FejlVarighed = "";
                }

                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("VarighedInput"));
            }
        }

        public string Genre
        {
            get { return _genre; }
            set
            {
                _genre = value;

                if (string.IsNullOrWhiteSpace(value))
                {
                    FejlGenre = "Genre må ikke være tom";
                }
                else
                {
                    FejlGenre = "";
                }

                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("Genre"));
            }
        }

        public string Instruktøer
        {
            get { return _instruktøer; }
            set
            {
                _instruktøer = value;

                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("Instruktøer"));
            }
        }

        public DateTime Premieredato
        {
            get { return _premieredato; }
            set
            {
                _premieredato = value;

                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("Premieredato"));
            }
        }


        ObservableCollection<Movie> _movieList = new ObservableCollection<Movie>();

        public MovieViewModel()
        {
            _premieredato = DateTime.Today;

            _repository = new FileMovieRepository();

            foreach (var movie in _repository.LoadMovies())
            {
                _movieList.Add(movie);
            }

            RegistrerCommand = new RelayCommand(
                parameter => RegistrerFilm(),
                parameter => KanRegistrereFilm());
            SletCommand = new RelayCommand(
                parameter => SletFilm(),
                parameter => SelectedMovie != null);
        }


        public ObservableCollection<Movie> MovieList { get { return _movieList; } }

        public Movie SelectedMovie
        { 
            get { return _selectedMovie; }
            set
            {
                _selectedMovie = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedMovie"));
            }
        }

        public string FejlTitel
        { 
            get { return _fejlTitel; }
            set
            {
                _fejlTitel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FejlTitel"));
            }
        }

        public string FejlVarighed
        { 
            get { return _fejlVarighed; }
            set
            {
                _fejlVarighed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FejlVarighed"));
            }
        }

        public string FejlGenre
        { 
            get {return _fejlGenre; }
            set
            {
                _fejlGenre = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FejlGenre"));
            }
        }

        public string Succesbesked
        {
            get { return _succesbesked; }
            set { _succesbesked = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Succesbesked")); }
        }

        public void RegistrerFilm()
        {
            if (string.IsNullOrWhiteSpace(Titel))
            {
                FejlTitel = "Titel må ikke være tom";
                return;
            }

            if (string.IsNullOrWhiteSpace(VarighedInput))
            {
                FejlVarighed = "Varighed må ikke være tom";
                return;
            }

            if (!int.TryParse(VarighedInput, out int varighed))
            {
                FejlVarighed = "Varighed skal være et tal";
                return;
            }

            if (varighed <= 0)
            {
                FejlVarighed = "Varighed skal være større end 0";
                return;
            }

            if (string.IsNullOrWhiteSpace(Genre))
            {
                FejlGenre = "Genre må ikke være tom";
                return;
            }

            Movie movie = new Movie();

            movie.Titel = Titel;
            movie.Varighed = varighed;
            movie.Genre = Genre;
            movie.Instruktøer = Instruktøer;
            movie.Premieredato = Premieredato;

            _movieList.Add(movie);

            _repository.SaveMovie(_movieList.ToList());
            Succesbesked = "Filmen er registreret";
        }

        private bool KanRegistrereFilm()
        {
            return !string.IsNullOrWhiteSpace(Titel) &&
                   int.TryParse(VarighedInput, out int varighed) &&
                   varighed > 0 &&
                   !string.IsNullOrWhiteSpace(Genre);
        }

        public void SletFilm()
        {
            if (SelectedMovie != null)
            {
                _movieList.Remove(SelectedMovie);
                _repository.SaveMovie(_movieList.ToList());
            }
        }
    }
}
