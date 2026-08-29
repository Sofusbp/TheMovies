using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TheMovies.Models;
using TheMovies.Repositories;

namespace TheMovies.ViewModels
{
    public class CinemaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand OpretBiografCommand { get; }
        public ICommand OpretSalCommand { get; }

        private FileCinemaRepository _repository;

        private string _navn;
        private string _nummerInput;
        private string _kapacitetInput;
        private string _fejlbesked = "";
        private string _succesbesked = "";

        private Cinema _selectedCinema;

        private ObservableCollection<Cinema> _cinemaList;

        public CinemaViewModel()
        {
            _repository = new FileCinemaRepository();

            _cinemaList = new ObservableCollection<Cinema>(
                _repository.LoadCinemas());

            OpretBiografCommand =
                new RelayCommand(parameter => OpretBiograf());

            OpretSalCommand =
                new RelayCommand(parameter => OpretSal());
        }

        public ObservableCollection<Cinema> CinemaList
        {
            get { return _cinemaList; }
        }

        public string Navn
        {
            get { return _navn; }
            set
            {
                _navn = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("Navn"));
            }
        }

        public string NummerInput
        {
            get { return _nummerInput; }
            set
            {
                _nummerInput = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("NummerInput"));
            }
        }

        public string KapacitetInput
        {
            get { return _kapacitetInput; }
            set
            {
                _kapacitetInput = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("KapacitetInput"));
            }
        }

        public Cinema SelectedCinema
        {
            get { return _selectedCinema; }
            set
            {
                _selectedCinema = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("SelectedCinema"));
            }
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

        public void OpretBiograf()
        {
            if (string.IsNullOrWhiteSpace(Navn))
            {
                Fejlbesked = "Indtast et navn til biografen";
                return;
            }

            Cinema cinema = new Cinema();

            cinema.Navn = Navn;

            _cinemaList.Add(cinema);

            _repository.SaveCinemas(
                _cinemaList.ToList());

            Navn = "";
            Fejlbesked = "";
            Succesbesked = "Biografen er oprettet";
        }

        public void OpretSal()
        {
            if (SelectedCinema == null)
            {
                Fejlbesked = "Vælg en biograf";
                return;
            }

            if (!int.TryParse(NummerInput, out int nummer))
            {
                Fejlbesked = "Salnummer skal være et tal";
                return;
            }

            if (!int.TryParse(KapacitetInput, out int kapacitet))
            {
                Fejlbesked = "Kapacitet skal være et tal";
                return;
            }

            if (kapacitet <= 0)
            {
                Fejlbesked = "Kapacitet skal være større end 0";
                return;
            }

            if (SelectedCinema.Sale.Any(room => room.Nummer == nummer))
            {
                Fejlbesked = "Salnummeret findes allerede i biografen";
                return;
            }

            CinemaRoom room = new CinemaRoom();

            room.Nummer = nummer;
            room.Kapacitet = kapacitet;

            SelectedCinema.Sale.Add(room);

            _repository.SaveCinemas(
                _cinemaList.ToList());

            NummerInput = "";
            KapacitetInput = "";
            Fejlbesked = "";
            Succesbesked = "Salen er oprettet";
        }
    }
}
