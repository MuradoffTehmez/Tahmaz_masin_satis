using System;
using System.Collections.Generic;

namespace Tahmaz_masin_satis
{
    class Program
    {
        static List<User> users = new List<User>();
        static List<Car> cars = new List<Car>();
        static List<Sale> sales = new List<Sale>();
        static decimal balance = 100000;
        static bool isAdmin = false;

        class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        class Car
        {
            public string Brand { get; set; }
            public int Year { get; set; }
            public int Horsepower { get; set; }
            public string IBAN { get; set; }
            public decimal AlisQiymeti { get; set; }
            public decimal SatisQiymeti { get; set; }
            public int Sayi { get; set; }
        }

        class Sale
        {
            public string Brand { get; set; }
            public int Year { get; set; }
            public int Quantity { get; set; }
            public DateTime SaleDate { get; set; }
        }

        static void Main()
        {
            Console.WriteLine("Xoş gəlmisiniz!");

            Console.Write("İstifadəçi adınızı daxil edin: ");
            string username = Console.ReadLine();

            Console.Write("Şifrənizi daxil edin (8 simvoldan çox olmalıdır): ");
            string password = Console.ReadLine();

            Console.Write("Şifrəni təkrar daxil edin: ");
            string confirmPassword = Console.ReadLine();

            if (password.Length < 8 || password != confirmPassword)
            {
                Console.WriteLine("Şifrə səhvdir və ya təsdiq şifrəsi uyğun deyil. Qeydiyyat başa çatmadı.");
                return;
            }
            else
            {
                Console.WriteLine("Giriş uğurlu oldu, " + username);
                if (username == "admin" && password == "admin123")
                {
                    isAdmin = true;
                }
            }

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("Zəhmət olmasa aşağıdakı seçimlərdən birini edin:");
                Console.WriteLine("1. Maşın Rüsumunu Hesabla");
                Console.WriteLine("2. Maşını Sat");
                Console.WriteLine("3. Maşın Əlavə Et");
                Console.WriteLine("4. Balansı Yoxla");
                Console.WriteLine("5. Maşınları Axtar");
                Console.WriteLine("6. Satış Tarixçəsi");
                Console.WriteLine("7. Balans Əməliyyatları");
                Console.WriteLine("8. Çıxış");

                int choice;
                bool validChoice;

                do
                {
                    validChoice = int.TryParse(Console.ReadLine(), out choice);
                    if (!validChoice || choice < 1 || choice > 8)
                    {
                        Console.WriteLine("Yanlış seçim. Zəhmət olmasa düzgün bir əməliyyatı seçin.");
                    }
                } while (!validChoice || choice < 1 || choice > 8);

                switch (choice)
                {
                    case 1:

                        CalculateCarPrice();
                        break;
                    case 2:
                        SellCar();
                        break;
                    case 3:
                        if (isAdmin)
                        {
                            AddCar();
                        }
                        else
                        {
                            Console.WriteLine("Satıcı hesabı ilə maşın əlavə etmək üçün icazəniz yoxdur.");
                        }
                        break;
                    case 4:
                        CheckBalance();
                        break;
                    case 5:
                        SearchCars();
                        break;
                    case 6:
                        ShowSaleHistory();
                        break;
                    case 7:
                        PerformBalanceOperation();
                        break;
                    case 8:
                        Console.WriteLine("Təşəkkür edirik ki, Market tətbiqini istifadə etdiyiniz üçün!");
                        isRunning = false;
                        break;
                }
            }
        }

        static void CalculateCarPrice()
        {
            Console.Write("Maşının markasını daxil edin (Mercedes, BMW, Hyundai, Kia və s.): ");
            string brand = Console.ReadLine();

            Console.Write("Maşının ilini daxil edin: ");
            int year;
            while (!int.TryParse(Console.ReadLine(), out year))
            {
                Console.WriteLine("Yanlış ədəd daxil etdiniz. İli təkrar daxil edin:");
            }

            decimal rüsum = 0;

            if ((brand == "Mercedes" || brand == "BMW") && year < 2015)
            {
                rüsum = 300;
            }
            else if ((brand == "Mercedes" || brand == "BMW") && year >= 2015)
            {
                rüsum = 500;
            }
            else if ((brand == "Hyundai" || brand == "Kia") && year < 2017)
            {
                rüsum = 250;
            }
            else if ((brand == "Hyundai" || brand == "Kia") && year >= 2017)
            {
                rüsum = 150;
            }
            else if (year >= 2015 && year <= 2021)
            {
                rüsum = 100;
            }
            else
            {
                rüsum = 49.99M;
            }

            Console.WriteLine($"Rüsum: {rüsum:C}");
        }

        static void SellCar()
        {
            Console.Write("Maşının İBAN nömrəsini daxil edin: ");
            string iban = Console.ReadLine();

            var car = cars.Find(c => c.IBAN == iban);

            if (car == null)
            {
                Console.WriteLine("Belə bir maşın qeydiyyatda yoxdur.");
                return;
            }

            Console.Write("Satış sayını daxil edin: ");
            int satışSayi;
            while (!int.TryParse(Console.ReadLine(), out satışSayi))
            {
                Console.WriteLine("Yanlış ədəd daxil etdiniz. Satış sayını təkrar daxil edin:");
            }


            if (satışSayi <= 0)
            {
                Console.WriteLine("Satış sayı 0-dan kiçik ola bilməz.");
                return;
            }

            if (car.Sayi < satışSayi)
            {
                Console.WriteLine($"Anbarda {car.Sayi} ədəd {car.Brand} mövcuddur. Daxil etdiyiniz say anbardakı saydan çoxdur.");
                return;
            }

            decimal satisQiymeti = car.SatisQiymeti * satışSayi;

            if (satisQiymeti > balance)
            {
                Console.WriteLine("Balansınız kifayət qədər deyil.");
                return;
            }

            balance -= satisQiymeti;
            car.Sayi -= satışSayi;

            if (car.Sayi == 0)
            {
                cars.Remove(car);
            }

            Sale sale = new Sale
            {
                Brand = car.Brand,
                Year = car.Year,
                Quantity = satışSayi,
                SaleDate = DateTime.Now
            };

            sales.Add(sale);

            Console.WriteLine($"{satışSayi} {car.Brand} satıldı. Qalıq balans: {balance:C}");
        }

        static void AddCar()
        {
            Car yeniMaşın = new Car();
            Console.Write("Maşının markasını daxil edin: ");

            yeniMaşın.Brand = Console.ReadLine();
            Console.Write("Maşının ilini daxil edin: ");
            int year;
            while (!int.TryParse(Console.ReadLine(), out year))
            {
                Console.WriteLine("Yanlış ədəd daxil etdiniz. İli təkrar daxil edin:");
            }
            yeniMaşın.Year = year;
            Console.Write("Maşının mator gücünü daxil edin: ");
            int horsepower;
            while (!int.TryParse(Console.ReadLine(), out horsepower))
            {
                Console.WriteLine("Yanlış ədəd daxil etdiniz. Mator gücünü təkrar daxil edin:");
            }
            yeniMaşın.Horsepower = horsepower;
            Console.Write("Maşının İBAN nömrəsini daxil edin: ");
            yeniMaşın.IBAN = Console.ReadLine();
            Console.Write("Maşının alış qiymətini daxil edin: ");
            decimal alisQiymeti;
            while (!decimal.TryParse(Console.ReadLine(), out alisQiymeti))
            {
                Console.WriteLine("Yanlış ədəd daxil etdiniz. Alış qiymətini təkrar daxil edin:");
            }
            yeniMaşın.AlisQiymeti = alisQiymeti;
            Console.Write("Maşının satış qiymətini daxil edin: ");
            decimal satisQiymeti;
            while (!decimal.TryParse(Console.ReadLine(), out satisQiymeti))
            {
                Console.WriteLine("Yanlış ədəd daxil etdiniz. Satış qiymətini təkrar daxil edin:");
            }
            yeniMaşın.SatisQiymeti = satisQiymeti;
            Console.Write("Maşının sayını daxil edin: ");
            int sayi;
            while (!int.TryParse(Console.ReadLine(), out sayi))
            {
                Console.WriteLine("Yanlış ədəd daxil etdiniz. Sayını təkrar daxil edin:");
            }
            yeniMaşın.Sayi = sayi;
            cars.Add(yeniMaşın);
            Console.WriteLine($"{yeniMaşın.Brand} maşın əlavə edildi.");
        }

        static void CheckBalance()
        {
            Console.WriteLine($"Balansınız: {balance:C}");
        }

        static void SearchCars()
        {
            Console.WriteLine("Axtarmağa başlamaq üçün aşağıdakı seçimlərdən birini edin:");
            Console.WriteLine("1. Markaya görə axtar");
            Console.WriteLine("2. İle görə axtar");
            Console.WriteLine("3. Mator gücünə görə axtar");
            Console.WriteLine("4. Geriyə qayıt");

            int searchChoice;
            bool validChoice;

            do
            {
                validChoice = int.TryParse(Console.ReadLine(), out searchChoice);
                if (!validChoice || searchChoice < 1 || searchChoice > 4)
                {
                    Console.WriteLine("Yanlış seçim. Zəhmət olmasa düzgün bir əməliyyatı seçin.");
                }
            } while (!validChoice || searchChoice < 1 || searchChoice > 4);

            switch (searchChoice)
            {
                case 1:
                    SearchByBrand();
                    break;
                case 2:
                    SearchByYear();
                    break;
                case 3:
                    SearchByHorsepower();
                    break;
                case 4:
                    break;
            }
        }

        static void SearchByBrand()
        {
            Console.Write("Axtardığınız maşının markasını daxil edin: ");
            string brandToSearch = Console.ReadLine();

            var matchingCars = cars.FindAll(c => c.Brand.ToLower() == brandToSearch.ToLower());

            if (matchingCars.Count == 0)
            {
                Console.WriteLine($"Daxil etdiyiniz markaya uyğun maşın tapılmadı.");
                return;

            }

            Console.WriteLine("Tapılan maşınlar:");
            foreach (var car in matchingCars)
            {
                Console.WriteLine($"Marka: {car.Brand}, İl: {car.Year}, Mator gücü: {car.Horsepower}, Sayı: {car.Sayi}, Alış qiyməti: {car.AlisQiymeti:C}, Satış qiyməti: {car.SatisQiymeti:C}");
            }
        }

        static void SearchByYear()
        {
            Console.Write("Axtardığınız il aralığını daxil edin (Başlanğıc il - Bitiş il): ");
            string yearRange = Console.ReadLine();

            string[] years = yearRange.Split('-');

            if (years.Length != 2 || !int.TryParse(years[0], out int startYear) || !int.TryParse(years[1], out int endYear))
            {
                Console.WriteLine("Yanlış ili daxil etdiniz.");
                return;
            }

            var matchingCars = cars.FindAll(c => c.Year >= startYear && c.Year <= endYear);

            if (matchingCars.Count == 0)
            {
                Console.WriteLine($"Daxil etdiyiniz il aralığına uyğun maşın tapılmadı.");
                return;
            }

            Console.WriteLine("Tapılan maşınlar:");
            foreach (var car in matchingCars)
            {
                Console.WriteLine($"Marka: {car.Brand}, İl: {car.Year}, Mator gücü: {car.Horsepower}, Sayı: {car.Sayi}, Alış qiyməti: {car.AlisQiymeti:C}, Satış qiyməti: {car.SatisQiymeti:C}");
            }
        }

        static void SearchByHorsepower()
        {
            Console.Write("Axtardığınız mator gücünü daxil edin: ");
            int horsepowerToSearch;
            while (!int.TryParse(Console.ReadLine(), out horsepowerToSearch))
            {
                Console.WriteLine("Yanlış mator gücü daxil etdiniz. Mator gücünü təkrar daxil edin:");

            }

            var matchingCars = cars.FindAll(c => c.Horsepower == horsepowerToSearch);

            if (matchingCars.Count == 0)
            {
                Console.WriteLine($"Daxil etdiyiniz mator gücünə uyğun maşın tapılmadı.");
                return;
            }

            Console.WriteLine("Tapılan maşınlar:");
            foreach (var car in matchingCars)
            {
                Console.WriteLine($"Marka: {car.Brand}, İl: {car.Year}, Mator gücü: {car.Horsepower}, Sayı: {car.Sayi}, Alış qiyməti: {car.AlisQiymeti:C}, Satış qiyməti: {car.SatisQiymeti:C}");
            }
        }

        static void ShowSaleHistory()
        {
            if (sales.Count == 0)
            {
                Console.WriteLine("Hələ heç bir satış edilməyib.");
                return;
            }

            Console.WriteLine("Satış tarixçəsi:");

            foreach (var sale in sales)
            {
                Console.WriteLine($"Tarix: {sale.SaleDate}, Marka: {sale.Brand}, İl: {sale.Year}, Sayı: {sale.Quantity}");
            }
        }

        static void PerformBalanceOperation()
        {
            Console.WriteLine("Balans əməliyyatını seçin:");
            Console.WriteLine("1. Balansa qoşulmaq");
            Console.WriteLine("2. Balanstan çıxmaq");
            Console.WriteLine("3. Geriyə qayıt");

            int operationChoice;
            bool validChoice;

            do
            {

                validChoice = int.TryParse(Console.ReadLine(), out operationChoice);
                if (!validChoice || operationChoice < 1 || operationChoice > 3)
                {
                    Console.WriteLine("Yanlış seçim. Zəhmət olmasa düzgün bir əməliyyatı seçin.");
                }
            } while (!validChoice || operationChoice < 1 || operationChoice > 3);

            switch (operationChoice)
            {
                case 1:
                    AddToBalance();
                    break;
                case 2:
                    WithdrawFromBalance();
                    break;
                case 3:
                    break;
            }
        }

        static void AddToBalance()
        {
            Console.Write("Balansa əlavə etmək istədiyiniz məbləği daxil edin: ");
            decimal amountToAdd;
            while (!decimal.TryParse(Console.ReadLine(), out amountToAdd) || amountToAdd <= 0)
            {
                Console.WriteLine("Yanlış məbləq daxil etdiniz. Məbləği təkrar daxil edin:");
            }

            balance += amountToAdd;
            Console.WriteLine($"Balansınıza {amountToAdd:C} əlavə olundu. Yeni balans: {balance:C}");
        }

        static void WithdrawFromBalance()
        {
            Console.Write("Balanstan çıxarmaq istədiyiniz məbləği daxil edin: ");
            decimal amountToWithdraw;
            while (!decimal.TryParse(Console.ReadLine(), out amountToWithdraw) || amountToWithdraw <= 0)
            {
                Console.WriteLine("Yanlış məbləq daxil etdiniz. Məbləği təkrar daxil edin:");
            }

            if (amountToWithdraw > balance)
            {
                Console.WriteLine("Balansınız kifayət qədər deyil.");

            }
            else
            {
                balance -= amountToWithdraw;
                Console.WriteLine($"{amountToWithdraw:C} balansdan çıxarıldı. Yeni balans: {balance:C}");
            }
        }
    }

}
