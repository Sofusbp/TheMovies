using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TheMovies.ViewModels;

namespace TheMovies.Views
{
    public partial class CinemaView : Window
    {
        public CinemaView()
        {
            InitializeComponent();

            DataContext = new CinemaViewModel();
        }
    }
}