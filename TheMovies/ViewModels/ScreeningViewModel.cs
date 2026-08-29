using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TheMovies.Models;
using TheMovies.Repositories;

namespace TheMovies.ViewModels
{
    public class ScreeningViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand RegistrerCommand { get; }

        private FileScreeningRepository _repository;
        private FileMovieRepository _movieRepository;
        private FileCinemaRepository _cinemaRepository;

        private ObservableCollection<Screening> _screeningList;
        private ObservableCollection<Movie> _movieList;
        private ObservableCollection<Cinema> _cinemaList;
        private ObservableCollection<CinemaRoom> _roomList;

        private Movie _selectedMovie;
        private Cinema _selectedCinema;
        private CinemaRoom _selectedRoom;

        private DateTime _startTidspunkt;
        private DateTime _dato;
        private string _tidInput = "18:00";
        private string _beregnetSluttid = "";
        private string _fejlbesked = "";
        private string _succesbesked = "";

        public ScreeningViewModel()
        {
            _repository = new FileScreeningRepository();
            _movieRepository = new FileMovieRepository();
            _cinemaRepository = new FileCinemaRepository();

            _screeningList = new ObservableCollection<Screening>(
            _repository.LoadScreenings()
            .OrderBy(screening => screening.StartTidspunkt));

            _movieList = new ObservableCollection<Movie>(
                _movieRepository.LoadMovies());

            _cinemaList = new ObservableCollection<Cinema>(
                _cinemaRepository.LoadCinemas());

            _roomList = new ObservableCollection<CinemaRoom>();

            _startTidspunkt = DateTime.Today.AddHours(18);
            _dato = DateTime.Today;

            RegistrerCommand = new RelayCommand(
                parameter => RegistrerForestilling());
        }


        public ObservableCollection<Screening> ScreeningList
        {
            get { return _screeningList; }
        }


        public ObservableCollection<Movie> MovieList
        {
            get { return _movieList; }
        }


        public ObservableCollection<Cinema> CinemaList
        {
            get { return _cinemaList; }
        }


        public ObservableCollection<CinemaRoom> RoomList
        {
            get { return _roomList; }
        }


        public Movie SelectedMovie
        {
            get { return _selectedMovie; }

            set
            {
                _selectedMovie = value;
                OpdaterBeregnetSluttid();

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("SelectedMovie"));
            }
        }


        public Cinema SelectedCinema
        {
            get { return _selectedCinema; }

            set
            {
                _selectedCinema = value;

                _roomList.Clear();

                if (_selectedCinema != null)
                {
                    foreach (CinemaRoom room in _selectedCinema.Sale)
                    {
                        _roomList.Add(room);
                    }
                }

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("SelectedCinema"));

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("RoomList"));
            }
        }


        public CinemaRoom SelectedRoom
        {
            get { return _selectedRoom; }

            set
            {
                _selectedRoom = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("SelectedRoom"));
            }
        }


        public DateTime StartTidspunkt
        {
            get { return _startTidspunkt; }

            set
            {
                _startTidspunkt = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("StartTidspunkt"));
            }
        }

        public DateTime Dato
        {
            get { return _dato; }
            set { _dato = value; OpdaterStarttidspunkt(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Dato")); }
        }

        public string TidInput
        {
            get { return _tidInput; }
            set { _tidInput = value; OpdaterStarttidspunkt(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TidInput")); }
        }

        public string BeregnetSluttid
        {
            get { return _beregnetSluttid; }
            set { _beregnetSluttid = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BeregnetSluttid")); }
        }

        public string Fejlbesked
        {
            get { return _fejlbesked; }
            set { _fejlbesked = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Fejlbesked")); }
        }

        public string Succesbesked
        {
            get { return _succesbesked; }
            set { _succesbesked = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Succesbesked")); }
        }


        public void RegistrerForestilling()
        {
            if (SelectedMovie == null)
            {
                Fejlbesked = "Vælg en film";
                return;
            }

            if (SelectedCinema == null)
            {
                Fejlbesked = "Vælg en biograf";
                return;
            }

            if (SelectedRoom == null)
            {
                Fejlbesked = "Vælg en sal";
                return;
            }

            if (!TimeSpan.TryParse(TidInput, out TimeSpan tid))
            {
                Fejlbesked = "Indtast tidspunktet som f.eks. 18:30";
                return;
            }

            StartTidspunkt = Dato.Date.Add(tid);

            DateTime slutTidspunkt = StartTidspunkt
                .AddMinutes(SelectedMovie.Varighed)
                .AddMinutes(15)
                .AddMinutes(15);

            bool overlap = _screeningList.Any(screening =>
                (screening.Biograf == null ||
                 screening.Biograf.Navn == SelectedCinema.Navn) &&
                screening.Sal.Nummer == SelectedRoom.Nummer &&
                StartTidspunkt < screening.SlutTidspunkt &&
                slutTidspunkt > screening.StartTidspunkt);

            if (overlap)
            {
                Fejlbesked = "Salen er allerede optaget på dette tidspunkt";
                return;
            }

            Screening screening = new Screening();

            screening.Film = SelectedMovie;
            screening.Biograf = SelectedCinema;
            screening.Sal = SelectedRoom;
            screening.StartTidspunkt = StartTidspunkt;
            screening.SlutTidspunkt = slutTidspunkt;

            _screeningList.Add(screening);

            var sorteretListe = _screeningList
                .OrderBy(screening => screening.StartTidspunkt)
                .ToList();

            _screeningList.Clear();

            foreach (Screening item in sorteretListe)
            {
                _screeningList.Add(item);
            }

            _repository.SaveScreenings(
                _screeningList.ToList());
            Fejlbesked = "";
            Succesbesked = "Forestillingen er registreret";
        }

        private void OpdaterStarttidspunkt()
        {
            if (TimeSpan.TryParse(TidInput, out TimeSpan tid))
            {
                StartTidspunkt = Dato.Date.Add(tid);
                OpdaterBeregnetSluttid();
            }
            else
            {
                BeregnetSluttid = "";
            }
        }

        private void OpdaterBeregnetSluttid()
        {
            if (SelectedMovie == null)
            {
                BeregnetSluttid = "";
                return;
            }

            DateTime slut = StartTidspunkt
                .AddMinutes(SelectedMovie.Varighed + 30);
            BeregnetSluttid = $"Forventet sluttid: {slut:HH:mm} (inkl. reklamer og rengøring)";
        }
    }
}
