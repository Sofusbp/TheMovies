# Brugertest for scenarie 3

## Formål

Brugertesten undersøger, om en biografmedarbejder kan registrere en reservation,
og om systemet forhindrer reservation af flere billetter, end der er ledige til
den valgte forestilling.

## Testdata

- Biograf: Testbiograf
- Sal: 1
- Kapacitet: 10 pladser
- Film: Testfilm
- Første reservation: 7 billetter
- Andet reservationsforsøg: 4 billetter
- Email: kunde@example.com
- Telefonnummer: 12345678

## Test 1 – Registrer en gyldig reservation

1. Opret Testbiograf med sal 1 og en kapacitet på 10 pladser.
2. Opret Testfilm og en forestilling i Testbiograf, sal 1.
3. Åbn reservationsvinduet.
4. Vælg forestillingen.
5. Indtast 7 billetter, email og telefonnummer.
6. Tryk på knappen for at registrere reservationen.

**Forventet resultat:** Reservationen bliver registreret og vist i listen.

**Faktisk resultat:** Reservationen på 7 billetter blev registreret og vist i
listen med den valgte film, biograf, sal, tid, email og telefonnummer.

**Godkendt/ikke godkendt:** Godkendt.

## Test 2 – Forsøg at overskride kapaciteten

1. Behold reservationen på 7 billetter fra test 1.
2. Vælg den samme forestilling igen.
3. Indtast 4 billetter, email og telefonnummer.
4. Tryk på knappen for at registrere reservationen.

**Forventet resultat:** Reservationen bliver afvist, fordi der kun er 3 ledige
pladser. Systemet viser beskeden: "Der er kun 3 ledige billetter".

**Faktisk resultat:** Reservationsforsøget på 4 billetter blev afvist, og
systemet viste beskeden: "Der er kun 3 ledige billetter".

**Godkendt/ikke godkendt:** Godkendt.

## Testperson og dato

- Testperson: Anika
- Dato: 29/08/2026
