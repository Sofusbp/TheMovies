using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TheMovies.ViewModels;

namespace TheMovies.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MovieViewModel();
        }

        private void ÅbnBiografer(object sender, RoutedEventArgs e)
        {
            CinemaView cinemaView = new CinemaView();
            cinemaView.Show();
        }

        private void ÅbnForestillinger(object sender, RoutedEventArgs e)
        {
            ScreeningView screeningView = new ScreeningView();
            screeningView.DataContext = new ScreeningViewModel();
            screeningView.Show();
        }

        private void ÅbnReservationer(object sender, RoutedEventArgs e)
        {
            ReservationView reservationView = new ReservationView();
            reservationView.Show();
        }

    }
}
