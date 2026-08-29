using TheMovies.Models;
using TheMovies.ViewModels;

namespace TheMovies.Tests
{
    public class ReservationInputValidationTests
    {
        [Fact]
        public void ValiderInput_ReturnererTrue_NaarInputErGyldigt()
        {
            // Arrange
            ReservationViewModel viewModel = new ReservationViewModel
            {
                SelectedScreening = OpretForestilling(),
                AntalBilletterInput = "2",
                Email = "kunde@example.com",
                Telefonnummer = "12345678"
            };

            // Act
            bool resultat = viewModel.ValiderInput(out int antalBilletter);

            // Assert
            Assert.True(resultat);
            Assert.Equal(2, antalBilletter);
            Assert.Equal(string.Empty, viewModel.Fejlbesked);
        }

        [Fact]
        public void ValiderInput_ReturnererFalse_NaarAntalBilletterIkkeErEtTal()
        {
            // Arrange
            ReservationViewModel viewModel = new ReservationViewModel
            {
                SelectedScreening = OpretForestilling(),
                AntalBilletterInput = "to",
                Email = "kunde@example.com",
                Telefonnummer = "12345678"
            };

            // Act
            bool resultat = viewModel.ValiderInput(out int antalBilletter);

            // Assert
            Assert.False(resultat);
            Assert.Equal(0, antalBilletter);
            Assert.Equal("Antal billetter skal være et tal", viewModel.Fejlbesked);
        }

        private static Screening OpretForestilling()
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
                Sal = new CinemaRoom { Nummer = 1, Kapacitet = 10 },
                StartTidspunkt = new DateTime(2026, 9, 1, 18, 0, 0),
                SlutTidspunkt = new DateTime(2026, 9, 1, 20, 0, 0)
            };
        }
    }
}
