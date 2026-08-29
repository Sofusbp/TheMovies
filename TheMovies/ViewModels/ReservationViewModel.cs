using System.Collections.ObjectModel;
using System.ComponentModel;
using TheMovies.Models;
using TheMovies.Repositories;

namespace TheMovies.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private FileReservationRepository _reservationRepository;
        private FileScreeningRepository _screeningRepository;

        private ObservableCollection<Reservation> _reservationList;
        private ObservableCollection<Screening> _screeningList;

        private Screening _selectedScreening;
        private string _antalBilletterInput;
        private string _email;
        private string _telefonnummer;

        public ReservationViewModel()
        {
            _reservationRepository = new FileReservationRepository();
            _screeningRepository = new FileScreeningRepository();

            _reservationList = new ObservableCollection<Reservation>(
                _reservationRepository.LoadReservations());

            _screeningList = new ObservableCollection<Screening>(
                _screeningRepository.LoadScreenings());
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
    }
}
