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
        public ICommand SletBiografCommand { get; }

        private FileCinemaRepository _repository;

        private string _navn;
        private string _nummerInput;
        private string _kapacitetInput;
        private string _fejlbesked = "";
        private string _succesbesked = "";

        private Cinema _selectedCinema;

        private ObservableCollection<Cinema> _cinemaList;
        private ObservableCollection<CinemaRoomOverview> _cinemaRoomList;

        public CinemaViewModel()
        {
            _repository = new FileCinemaRepository();

            _cinemaList = new ObservableCollection<Cinema>(
                _repository.LoadCinemas());
            _cinemaRoomList = new ObservableCollection<CinemaRoomOverview>();
            OpdaterCinemaRoomList();

            OpretBiografCommand =
                new RelayCommand(
                    parameter => OpretBiograf(),
                    parameter => !string.IsNullOrWhiteSpace(Navn));

            OpretSalCommand =
                new RelayCommand(
                    parameter => OpretSal(),
                    parameter => KanOpretSal());
            SletBiografCommand = new RelayCommand(
                parameter => SletBiograf(),
                parameter => SelectedCinema != null);
        }

        public ObservableCollection<Cinema> CinemaList
        {
            get { return _cinemaList; }
        }

        public ObservableCollection<CinemaRoomOverview> CinemaRoomList
        {
            get { return _cinemaRoomList; }
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

            OpdaterCinemaRoomList();

            _repository.SaveCinemas(
                _cinemaList.ToList());

            NummerInput = "";
            KapacitetInput = "";
            Fejlbesked = "";
            Succesbesked = "Salen er oprettet";
        }

        private bool KanOpretSal()
        {
            return SelectedCinema != null &&
                   int.TryParse(NummerInput, out int nummer) && nummer > 0 &&
                   int.TryParse(KapacitetInput, out int kapacitet) && kapacitet > 0;
        }

        private void SletBiograf()
        {
            if (System.Windows.MessageBox.Show(
                "Vil du slette biografen og dens sale?",
                "Bekræft sletning",
                System.Windows.MessageBoxButton.YesNo) !=
                System.Windows.MessageBoxResult.Yes) return;
            _cinemaList.Remove(SelectedCinema);
            OpdaterCinemaRoomList();
            _repository.SaveCinemas(_cinemaList.ToList());
            Succesbesked = "Biografen er slettet";
        }

        private void OpdaterCinemaRoomList()
        {
            _cinemaRoomList.Clear();

            foreach (Cinema cinema in _cinemaList)
            {
                foreach (CinemaRoom room in cinema.Sale)
                {
                    _cinemaRoomList.Add(new CinemaRoomOverview
                    {
                        BiografNavn = cinema.Navn,
                        SalNummer = room.Nummer,
                        Kapacitet = room.Kapacitet
                    });
                }
            }
        }
    }

    public class CinemaRoomOverview
    {
        public string BiografNavn { get; set; } = "";
        public int SalNummer { get; set; }
        public int Kapacitet { get; set; }
    }
}
