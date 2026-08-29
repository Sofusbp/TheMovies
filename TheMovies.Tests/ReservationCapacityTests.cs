using System.IO;
using TheMovies.Models;
using TheMovies.Repositories;
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

        [Fact]
        public void ValiderKapacitet_MedregnerReservationMedTidligereId()
        {
            // Arrange
            Screening valgtForestilling = OpretForestilling(10);
            Screening gemtForestilling = OpretForestilling(10);
            ReservationViewModel viewModel = new ReservationViewModel
            {
                SelectedScreening = valgtForestilling
            };

            viewModel.ReservationList.Clear();
            viewModel.ReservationList.Add(new Reservation
            {
                Forestilling = gemtForestilling,
                AntalBilletter = 7,
                Email = "kunde@example.com",
                Telefonnummer = "12345678"
            });

            // Act
            bool resultat = viewModel.ValiderKapacitet(4);

            // Assert
            Assert.NotEqual(valgtForestilling.Id, gemtForestilling.Id);
            Assert.False(resultat);
            Assert.Equal("Der er kun 3 ledige billetter", viewModel.Fejlbesked);
        }

        [Fact]
        public void RegistrerReservation_GenindlaeserReservationerFoerKapacitetskontrol()
        {
            // Arrange
            string filsti = Path.Combine(
                Path.GetTempPath(),
                $"reservation-test-{Guid.NewGuid()}.json");

            try
            {
                FileReservationRepository repository =
                    new FileReservationRepository(filsti);

                Screening forestilling = OpretForestilling(10);
                ReservationViewModel viewModel =
                    new ReservationViewModel(repository)
                    {
                        SelectedScreening = forestilling,
                        AntalBilletterInput = "4",
                        Email = "ny@example.com",
                        Telefonnummer = "87654321"
                    };

                repository.SaveReservations(new List<Reservation>
                {
                    new Reservation
                    {
                        Forestilling = forestilling,
                        AntalBilletter = 7,
                        Email = "kunde@example.com",
                        Telefonnummer = "12345678"
                    }
                });

                // Act
                viewModel.RegistrerReservation();

                // Assert
                Assert.Equal(
                    "Der er kun 3 ledige billetter",
                    viewModel.Fejlbesked);
                Assert.Single(repository.LoadReservations());
            }
            finally
            {
                if (File.Exists(filsti))
                {
                    File.Delete(filsti);
                }
            }
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
