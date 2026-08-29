using System.Windows;
using TheMovies.ViewModels;

namespace TheMovies.Views
{
    public partial class ReservationView : Window
    {
        public ReservationView()
        {
            InitializeComponent();

            DataContext = new ReservationViewModel();
        }
    }
}
