using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimRacingTelemetryLogger.Logger.GT7
{
    public static class GT7Utils
    {
        public static Dictionary<int, string> CarsManufacturers = new Dictionary<int, string>
        {
            { 3, "Alfa Romeo" },
            { 4, "Aston Martin" },
            { 5, "Audi" },
            { 6, "BMW" },
            { 7, "Chevrolet" },
            { 9, "Citroen" },
            { 10, "Daihatsu" },
            { 11, "Dodge" },
            { 12, "Fiat" },
            { 13, "Ford" },
            { 15, "Honda" },
            { 16, "Hyundai" },
            { 17, "Jaguar" },
            { 18, "Lancia" },
            { 21, "Mazda" },
            { 22, "Mercedes-Benz" },
            { 25, "Mitsubishi" },
            { 28, "Nissan" },
            { 30, "Pagani" },
            { 32, "Peugeot" },
            { 33, "Gran Turismo" },
            { 34, "Renault" },
            { 35, "RUF" },
            { 36, "Shelby" },
            { 38, "Subaru" },
            { 39, "Suzuki" },
            { 43, "Toyota" },
            { 44, "TVR" },
            { 46, "Volkswagen" },
            { 49, "Infiniti" },
            { 50, "Lexus" },
            { 51, "MINI" },
            { 52, "Pontiac" },
            { 55, "Plymouth" },
            { 57, "Autobianchi" },
            { 59, "Amuse" },
            { 65, "DMC" },
            { 69, "Volvo" },
            { 80, "Caterham" },
            { 86, "Alpine" },
            { 87, "RE Amemiya" },
            { 93, "Chaparral" },
            { 110, "Ferrari" },
            { 112, "Lamborghini" },
            { 113, "Bugatti" },
            { 114, "Renault Sport" },
            { 116, "Maserati" },
            { 117, "McLaren" },
            { 119, "Tesla" },
            { 121, "KTM" },
            { 125, "Abarth" },
            { 127, "SRT" },
            { 134, "Zagato" },
            { 135, "Italdesign" },
            { 136, "Porsche" },
            { 138, "Fittipaldi Motors" },
            { 139, "GT Awards (SEMA)" },
            { 140, "De Tomaso" },
            { 141, "Radical" },
            { 143, "Super Formula (Dallara)" },
            { 144, "BAC" },
            { 145, "Willys" },
            { 146, "Jeep" },
            { 147, "Chris Holstrom Concepts" },
            { 148, "Eckert's Rod & Custom" },
            { 149, "Greddy" },
            { 150, "Wicked Fabrication" },
            { 151, "Roadster Shop" },
            { 152, "Genesis" },
            { 153, "AMG" },
            { 154, "DS AUTOMOBILES" },
            { 155, "NISMO" },
            { 156, "Greening Auto Company" },
            { 157, "Garage RCR" },
            { 158, "BVLGARI" },
            { 159, "Skoda" },
            { 160, "AFEELA" }
        };

        public static Dictionary<int, (string name, int manufacturerId)> CarsModels = new Dictionary<int, (string, int)>
        {
            {24, ("180SX Type X '96", 28) },
            {31, ("Camaro Z28 '69", 7)},
            {41, ("Corvette Stingray (C3) '69", 7)},
            {48, ("Fairlady 240ZG (HS30) '71", 28)},
            {63, ("Corolla Levin 1600GT APEX (AE86) '83", 43)},
            {82, ("Supra RZ '97", 43)},
            {104, ("Sileighty '98", 28)},
            {105, ("205 Turbo 16 Evolution 2 '86", 32)},
            {135, ("S800 '66", 15)},
            {137, ("Beat '91", 15)},
            {145, ("Copen '02", 10)},
            {173, ("R5 Turbo '80", 34)},
            {187, ("Tuscan Speed 6 '00", 44)},
            {201, ("Eunos Roadster (NA) '89", 21)},
            {203, ("Integra Type R (DC2) '98", 15)},
            {204, ("Civic Type R (EK) '98", 15)},
            {205, ("RX-7 Spirit R Type A (FD) '02", 21)},
            {207, ("MR2 GT-S '97", 43)},
            {210, ("R34 GT-R V-spec II Nur '02", 28)},
            {211, ("Lancer Evolution V GSR '98", 25)},
            {216, ("McLaren F1 GTR Race Car '97", 6)},
            {293, ("NSX Type R '92", 15)},
            {296, ("787B '91", 21)},
            {301, ("Lancer Evolution IV GSR '96", 25)},
            {315, ("Cobra 427 '66", 36)},
            {334, ("Clio V6 24V '00", 34)},
            {345, ("Sprinter Trueno 1600GT APEX (S.Shigeno Version)", 43)},
            {365, ("155 2.5 V6 TI '93", 3)},
            {374, ("RX-7 GT-X (FC) '90", 21)},
            {379, ("Impreza Coupe WRX Type R STi Ver.VI '99", 38)},
            {387, ("300 SL Coupe '54", 22)},
            {396, ("NSX Type R '02", 15)},
            {451, ("Impreza 22B-STi '98", 38)},
            {485, ("GT-R GT500 '99", 28)},
            {489, ("R33 GT-R V-spec '97", 28)},
            {514, ("S2000 '99", 15)},
            {533, ("Stratos '73", 18)},
            {604, ("2000GT '67", 43)},
            {665, ("Superbird '70", 55)},
            {688, ("Integra Type R (DC2) '95", 15)},
            {709, ("Fairlady Z 300ZX TT 2seater '89", 28)},
            {773, ("R32 GT-R V-spec II '94", 28)},
            {810, ("Sprinter Trueno 1600GT APEX (AE86) '83", 43)},
            {818, ("Corvette (C2) '63", 7)},
            {821, ("Civic Type R (EK) '97", 15)},
            {829, ("Delta HF Integrale Evoluzione '91", 18)},
            {837, ("Skyline Hard Top 2000GT-R (KPGC10) '70", 28)},
            {843, ("Supra 3.0GT Turbo A '88", 43)},
            {919, ("Silvia Q's (S13) '88", 28)},
            {942, ("Corvette ZR-1 (C4) '89", 7)},
            {954, ("R92CP '92", 28)},
            {959, ("TT Coupe 3.2 quattro '03", 5)},
            {998, ("Sauber Mercedes C9 '89", 22)}
        };

        public static string GetCarPrettyName(int carId)
        {
            try
            {
                return $"{CarsManufacturers[CarsModels[carId].manufacturerId]} {CarsModels[carId]}";
            }
            catch
            {
                return "?";
            }
        }
    }
}
