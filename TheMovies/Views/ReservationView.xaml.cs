using System.Windows;
using System.Windows.Controls;
using TheMovies.ViewModels;

namespace TheMovies.Views
{
    public partial class ReservationView : UserControl
    {
        public ReservationView()
        {
            InitializeComponent();

            DataContext = new ReservationViewModel();
        }
    }
}
