using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TheMovies.Models;
using TheMovies.Repositories;

namespace TheMovies.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand RegistrerCommand { get; }

        private FileReservationRepository _reservationRepository;
        private FileScreeningRepository _screeningRepository;

        private ObservableCollection<Reservation> _reservationList;
        private ObservableCollection<Screening> _screeningList;

        private Screening _selectedScreening;
        private string _antalBilletterInput;
        private string _email;
        private string _telefonnummer;
        private string _fejlbesked;

        public ReservationViewModel()
        {
            _reservationRepository = new FileReservationRepository();
            _screeningRepository = new FileScreeningRepository();

            _reservationList = new ObservableCollection<Reservation>(
                _reservationRepository.LoadReservations());

            _screeningList = new ObservableCollection<Screening>(
                _screeningRepository.LoadScreenings());

            RegistrerCommand = new RelayCommand(
                parameter => RegistrerReservation());
        }

        public ObservableCollection<Reservation> ReservationList
        {
            get { return _reservationList; }
        }

        public ObservableCollection<Screening> ScreeningList
        {
            get { return _screeningList; }
        }

        public Screening SelectedScreening
        {
            get { return _selectedScreening; }
            set
            {
                _selectedScreening = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("SelectedScreening"));
            }
        }

        public string AntalBilletterInput
        {
            get { return _antalBilletterInput; }
            set
            {
                _antalBilletterInput = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("AntalBilletterInput"));
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("Email"));
            }
        }

        public string Telefonnummer
        {
            get { return _telefonnummer; }
            set
            {
                _telefonnummer = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("Telefonnummer"));
            }
        }

        public string Fejlbesked
        {
            get { return _fejlbesked; }
            set
            {
                _fejlbesked = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("Fejlbesked"));
            }
        }

        public bool ValiderInput(out int antalBilletter)
        {
            antalBilletter = 0;

            if (SelectedScreening == null)
            {
                Fejlbesked = "Vælg en forestilling";
                return false;
            }

            if (!int.TryParse(AntalBilletterInput, out antalBilletter))
            {
                Fejlbesked = "Antal billetter skal være et tal";
                return false;
            }

            if (antalBilletter <= 0)
            {
                Fejlbesked = "Antal billetter skal være større end 0";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                Fejlbesked = "Email må ikke være tom";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Telefonnummer))
            {
                Fejlbesked = "Telefonnummer må ikke være tomt";
                return false;
            }

            Fejlbesked = "";
            return true;
        }

        public bool ValiderKapacitet(int antalBilletter)
        {
            if (SelectedScreening == null)
            {
                Fejlbesked = "Vælg en forestilling";
                return false;
            }

            if (SelectedScreening.Sal.Kapacitet <= 0)
            {
                Fejlbesked = "Salen har ingen gyldig kapacitet";
                return false;
            }

            int reserveredeBilletter = _reservationList
                .Where(reservation =>
                    reservation.Forestilling.Id == SelectedScreening.Id)
                .Sum(reservation => reservation.AntalBilletter);

            int ledigeBilletter =
                SelectedScreening.Sal.Kapacitet - reserveredeBilletter;

            if (antalBilletter > ledigeBilletter)
            {
                Fejlbesked = $"Der er kun {ledigeBilletter} ledige billetter";
                return false;
            }

            Fejlbesked = "";
            return true;
        }

        public void RegistrerReservation()
        {
            if (!ValiderInput(out int antalBilletter))
            {
                return;
            }

            if (!ValiderKapacitet(antalBilletter))
            {
                return;
            }

            Reservation reservation = new Reservation();

            reservation.Forestilling = SelectedScreening;
            reservation.AntalBilletter = antalBilletter;
            reservation.Email = Email;
            reservation.Telefonnummer = Telefonnummer;

            _reservationList.Add(reservation);

            _reservationRepository.SaveReservations(
                _reservationList.ToList());

            AntalBilletterInput = "";
            Email = "";
            Telefonnummer = "";
        }
    }
}
