using TheMovies.Models;
using TheMovies.ViewModels;

namespace TheMovies.Tests
{
    public class ReservationCapacityTests
    {
        [Fact]
        public void ValiderKapacitet_ReturnererTrue_NaarDerErPlads()
        {
            // Arrange
            Screening forestilling = OpretForestilling(10);
            ReservationViewModel viewModel = new ReservationViewModel
            {
                SelectedScreening = forestilling
            };

            viewModel.ReservationList.Clear();

            // Act
            bool resultat = viewModel.ValiderKapacitet(4);

            // Assert
            Assert.True(resultat);
            Assert.Equal(string.Empty, viewModel.Fejlbesked);
        }

        [Fact]
        public void ValiderKapacitet_ReturnererFalse_NaarDerIkkeErPlads()
        {
            // Arrange
            Screening forestilling = OpretForestilling(10);
            ReservationViewModel viewModel = new ReservationViewModel
            {
                SelectedScreening = forestilling
            };

            viewModel.ReservationList.Clear();
            viewModel.ReservationList.Add(new Reservation
            {
                Forestilling = forestilling,
                AntalBilletter = 7,
                Email = "kunde@example.com",
                Telefonnummer = "12345678"
            });

            // Act
            bool resultat = viewModel.ValiderKapacitet(4);

            // Assert
            Assert.False(resultat);
            Assert.Equal("Der er kun 3 ledige billetter", viewModel.Fejlbesked);
        }

        private static Screening OpretForestilling(int kapacitet)
        {
            return new Screening
            {
                Film = new Movie
                {
                    Titel = "Testfilm",
                    Varighed = 90,
                    Genre = "Drama",
                    Instruktøer = "Testinstruktør"
                },
                Biograf = new Cinema { Navn = "Testbiograf" },
                Sal = new CinemaRoom { Nummer = 1, Kapacitet = kapacitet },
                StartTidspunkt = new DateTime(2026, 9, 1, 18, 0, 0),
                SlutTidspunkt = new DateTime(2026, 9, 1, 20, 0, 0)
            };
        }
    }
}
