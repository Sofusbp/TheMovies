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

        public void OpretBiograf()
        {
            if (string.IsNullOrWhiteSpace(Navn))
            {
                return;
            }

            Cinema cinema = new Cinema();

            cinema.Navn = Navn;

            _cinemaList.Add(cinema);

            _repository.SaveCinemas(
                _cinemaList.ToList());

            Navn = "";
        }

        public void OpretSal()
        {
            if (SelectedCinema == null)
            {
                return;
            }

            if (!int.TryParse(NummerInput, out int nummer))
            {
                return;
            }

            if (SelectedCinema.Sale.Any(room => room.Nummer == nummer))
            {
                return;
            }

            CinemaRoom room = new CinemaRoom();

            room.Nummer = nummer;

            SelectedCinema.Sale.Add(room);

            _repository.SaveCinemas(
                _cinemaList.ToList());

            NummerInput = "";
        }
    }
}