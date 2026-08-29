# Krav til scenarie 3

## Scenariebeskrivelse

Den 24. i hver måned åbner biografmedarbejderne for reservationer til den næste
måned. En kunde ringer til biografen og får oplyst, hvornår en ønsket film bliver
vist. Kunden vælger en forestilling, og medarbejderen registrerer reservationen.

Datoen den 24. beskriver biografens arbejdsgang. Scenariets systemkrav handler
om selve registreringen og om at overholde salens kapacitet.

## Funktionelle krav

### S3-K1 – Registrering af reservation

Systemet skal gøre det muligt for en biografmedarbejder at registrere en
billetreservation til en valgt forestilling.

Reservationen skal indeholde:

- den valgte forestilling med film, tidspunkt, biograf og sal;
- antal billetter;
- kundens email;
- kundens telefonnummer.

**Acceptkriterium:** En reservation med gyldige oplysninger bliver gemt og vist
i listen over registrerede reservationer.

### S3-K2 – Kontrol af ledige billetter

Systemet må ikke registrere flere billetter, end der er ledige pladser til den
valgte forestilling.

Antallet af ledige billetter beregnes som salens kapacitet minus summen af
allerede reserverede billetter til samme forestilling.

**Acceptkriterium:** Hvis det ønskede antal billetter overstiger de ledige
pladser, bliver reservationen afvist, og medarbejderen får vist, hvor mange
billetter der er tilbage.
