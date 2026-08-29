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

        private void ReservationView_IsVisibleChanged(
            object sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                DataContext = new ReservationViewModel();
            }
        }
    }
}
