using Medicine_V2.DBHelpers;
using Medicine_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine_V2
{
    class Program
    {
        private const string _MainMenu = "Главное меню\n" +
                "Выберите один из вариантов (указать номер):\n" +
                "1. Товары,\n" +
                "2. Аптеки,\n" +
                "3. Склады,\n" +
                "4. Партий,\n" +
                "0. Выход";

        private const string _Menu = "Выберите один из вариантов (указать номер):\n" +
            "1. Вывести все записи,\n" +
            "2. Добавить новую запись,\n" +
            "3. Удалить запись (по номеру),\n";

        private const string _PharmacyMenu = "4. Вывести на экран весь список товаров и его количество в выбранной аптеке (количество товара во всех складах аптеки),\n";

        private const string _EndMenu = "0. Назад";

        /// <summary>
        /// Товары
        /// </summary>
        private static ProductDBHelper _Product;
        /// <summary>
        /// Аптеки
        /// </summary>
        private static PharmacyDBHelper _Pharmacy;
        /// <summary>
        /// Склады
        /// </summary>
        private static WarehouseDBHelper _Warehouse;
        /// <summary>
        /// Партии
        /// </summary>
        private static BatchDBHelper _Batch;
        static void Main(string[] args)
        {
            Initialize();

            MainMenu();
        }

        static void Initialize()
        {
            DBProvider provider = new DBProvider();
            _Product = new ProductDBHelper(provider);
            _Pharmacy = new PharmacyDBHelper(provider);
            _Warehouse = new WarehouseDBHelper(provider);
            _Batch = new BatchDBHelper(provider);
        }

        /// <summary>
        /// Главное меню
        /// </summary>
        static void MainMenu()
        {
            int selectedNumber = -1;

            while (selectedNumber != 0)
            {
                Console.WriteLine(_MainMenu);
                if (int.TryParse(Console.ReadLine(), out selectedNumber))
                {
                    if (selectedNumber < 0 || selectedNumber > 4)
                    {
                        Console.WriteLine("Введен неверный номер!");
                        continue;
                    }
                    //Спуск в меню по иерархий
                    switch (selectedNumber)
                    {
                        case 1: //Товары
                            ProductMenu();
                            break;
                        case 2: //Аптеки
                            PharmacyMenu();
                            break;
                        case 3: //Склады
                            WarehouseMenu();
                            break;
                        case 4: //Партии
                            BatchMenu();
                            break;
                        case 0: //Выход
                            return;
                    }
                }
                else
                {
                    Console.WriteLine("Введен неверный номер!");
                }
            }
        }

        #region Products
        /// <summary>
        /// Меню товаров
        /// </summary>
        static void ProductMenu()
        {
            int selectedNumber = -1;

            while (selectedNumber != 0)
            {
                Console.WriteLine("Меню товаров:\n" + _Menu + _EndMenu);
                if (int.TryParse(Console.ReadLine(), out selectedNumber))
                {
                    if (selectedNumber < 0 || selectedNumber > 3)
                    {
                        Console.WriteLine("Введен неверный номер!");
                        continue;
                    }
                    //Спуск в меню по иерархий
                    switch (selectedNumber)
                    {
                        case 1: //Вывести все записи
                            WriteAllProducts();
                            break;
                        case 2: //Добавить новую запись
                            AddProduct();
                            break;
                        case 3: //Удалить запись
                            DeleteProduct();
                            break;
                        case 0: //Назад
                            return;
                    }
                }
                else
                {
                    Console.WriteLine("Введен неверный номер!");
                }
            }
        }

        static void WriteAllProducts()
        {
            Console.WriteLine("Вывод всех товаров:");
            var values = _Product.GetAllValues().ToList();
            for (int i = 0; i < values.Count; i++)
            {
                Console.WriteLine(
                    (i + 1) +
                    ".   Название: " + values[i].Name);
            }
        }

        static void AddProduct()
        {
            Console.WriteLine("Добавление нового товара:");
            Console.WriteLine("Напишите название:");
            string name = Console.ReadLine().Replace(" ", "");
            if (name.Length == 0)
            {
                Console.WriteLine("Название введено неверно!");
            }
            else
            {
                _Product.CreateItem(
                    new ProductModel(
                        1,
                        name));
            }
        }

        static void DeleteProduct()
        {
            var values = _Product.GetAllValues().ToList();
            Console.WriteLine("Удаление товара:");
            Console.WriteLine("Напишите номер удаляемого товара:");
            int number = -1;
            if (!int.TryParse(Console.ReadLine(), out number) ||
                values.FirstOrDefault(u => u.ID == number) != null) 
            {
                Console.WriteLine("Введен неверный номер!");
                return;
            }

            _Product.DeleteItem(number);
        }
        #endregion

        #region Pharmacies
        /// <summary>
        /// Меню аптек
        /// </summary>
        static void PharmacyMenu()
        {
            int selectedNumber = -1;

            while (selectedNumber != 0)
            {
                Console.WriteLine("Меню аптек:\n" + _Menu + _PharmacyMenu + _EndMenu);
                if (int.TryParse(Console.ReadLine(), out selectedNumber))
                {
                    if (selectedNumber < 0 || selectedNumber > 4)
                    {
                        Console.WriteLine("Введен неверный номер!");
                        continue;
                    }
                    //Спуск в меню по иерархий
                    switch (selectedNumber)
                    {
                        case 1: //Вывести все записи
                            WriteAllPharmacies();
                            break;
                        case 2: //Добавить новую запись
                            AddPharmacy();
                            break;
                        case 3: //Удалить запись
                            DeletePharmacy();
                            break;
                        case 4: //Доп. задание
                            GetAllProductsForSelectedPharmacy();
                            break;
                        case 0: //Назад
                            return;
                    }
                }
                else
                {
                    Console.WriteLine("Введен неверный номер!");
                }
            }
        }

        static void GetAllProductsForSelectedPharmacy()
        {
            Console.WriteLine("Вывод всего списка товаров и его количества для выбранной аптеки:");
            Console.WriteLine("Напишите номер аптеки:");

            int number = -1;
            if (!int.TryParse(Console.ReadLine(), out number)) 
            {
                Console.WriteLine("Введен неверный номер!");
                return;
            }

            var pharmacies = _Pharmacy.GetAllValues().ToList();
            var pharmacy = pharmacies.FirstOrDefault(u => u.ID == number);
            if (pharmacy == null)
            {
                Console.WriteLine("Введен неверный номер!");
                return;
            }

            //Получить все склады для данной аптеки
            List<WarehouseModel> warehouses = new List<WarehouseModel>();
            foreach (var model in _Warehouse.GetAllValues())
            {
                var warehouse = model as WarehouseModel;
                if (warehouse != null && warehouse.IDPharmacies == number)
                    warehouses.Add(warehouse);
            }

            //Получить все партий для данных складов
            List<BatchModel> batches = new List<BatchModel>();
            foreach (var model in _Batch.GetAllValues())
            {
                var batch = model as BatchModel;
                if (batch == null) continue;

                foreach (var warehouse in warehouses)
                    if (warehouse.ID == batch.IDWarehouse)
                        batches.Add(batch);
            }

            //Получить все товары для данных партий
            Dictionary<ProductModel, int> products = new Dictionary<ProductModel, int>();
            foreach (var model in _Product.GetAllValues())
            {
                var product = model as ProductModel;
                if (product == null) continue;

                foreach (var batch in batches)
                    if (product.ID == batch.IDProduct)
                    {
                        if (products.ContainsKey(product))
                        {
                            products[product] += batch.Count;
                        }
                        else
                        {
                            products.Add(product, batch.Count);
                        }
                    }
            }

            foreach (var product in products)
            {
                Console.WriteLine("1.   Название: " + product.Key.Name + ",     Количество: " + product.Value);
            }
        }

        static void WriteAllPharmacies()
        {
            Console.WriteLine("Вывод всех аптек:");
            var values = _Pharmacy.GetAllValues().ToList();
            for (int i = 0; i < values.Count; i++)
            {
                var pharmacy = values[i] as PharmacyModel;
                if (pharmacy != null)
                    Console.WriteLine(
                        (i + 1) +
                        ".  Название:  " + pharmacy.Name +
                        ",  адрес: " + pharmacy.Address +
                        ",   телефон:" + pharmacy.Phone);
            }
        }

        static void AddPharmacy()
        {
            Console.WriteLine("Добавление новой аптеки:");

            Console.WriteLine("Напишите название:");
            string name = Console.ReadLine().Replace(" ", "");
            if (name.Length == 0)
            {
                Console.WriteLine("Название введено неверно!");
                return;
            }

            Console.WriteLine("Напишите адрес:");
            string address = Console.ReadLine().Replace(" ", "");
            if (address.Length == 0)
            {
                Console.WriteLine("Адресс введен неверно!");
                return;
            }

            Console.WriteLine("Напишите телефон:");
            string phone = Console.ReadLine().Replace(" ", "");
            if (address.Length == 0 || address.Length > 20)
            {
                Console.WriteLine("Номер введен неверно!");
                return;
            }

            _Pharmacy.CreateItem(
                new PharmacyModel(
                    1,
                    name,
                    address,
                    phone));
        }

        static void DeletePharmacy()
        {
            var values = _Pharmacy.GetAllValues().ToList();
            Console.WriteLine("Удаление аптеки:");
            Console.WriteLine("Напишите номер удаляемой аптеки:");
            int number = -1;
            if (!int.TryParse(Console.ReadLine(), out number) ||
                values.FirstOrDefault(u => u.ID == number) != null)
            {
                Console.WriteLine("Введен неверный номер!");
                return;
            }

            _Pharmacy.DeleteItem(number);
        }
        #endregion

        #region Warehouses
        /// <summary>
        /// Меню складов
        /// </summary>
        static void WarehouseMenu()
        {
            int selectedNumber = -1;

            while (selectedNumber != 0)
            {
                Console.WriteLine("Меню складов:\n" + _Menu + _EndMenu);
                if (int.TryParse(Console.ReadLine(), out selectedNumber))
                {
                    if (selectedNumber < 0 || selectedNumber > 3)
                    {
                        Console.WriteLine("Введен неверный номер!");
                        continue;
                    }
                    //Спуск в меню по иерархий
                    switch (selectedNumber)
                    {
                        case 1: //Вывести все записи
                            WriteAllWarehouses();
                            break;
                        case 2: //Добавить новую запись
                            AddWarehouse();
                            break;
                        case 3: //Удалить запись
                            DeleteWarehouse();
                            break;
                        case 0: //Назад
                            return;
                    }
                }
                else
                {
                    Console.WriteLine("Введен неверный номер!");
                }
            }
        }

        static void WriteAllWarehouses()
        {
            var values = _Warehouse.GetAllValues().ToList();
            Console.WriteLine("Вывод всех складов:");
            for (int i = 0; i < values.Count; i++)
            {
                Console.WriteLine(
                    (i + 1) +
                    ".  Название:  " +  values[i].Name);
            }
        }

        static void AddWarehouse()
        {
            Console.WriteLine("Добавление нового склада:");

            Console.WriteLine("Напишите название:");
            string name = Console.ReadLine().Replace(" ", "");
            if (name.Length == 0)
            {
                Console.WriteLine("Название введено неверно!");
                return;
            }

            Console.WriteLine("Напишите номер аптеки:");
            int numberPharmacy;
            if (!int.TryParse(Console.ReadLine().Replace(" ", ""), out numberPharmacy))
            {
                Console.WriteLine("Введен неверный номер аптеки!");
                return;
            }

            var pharmacies = _Pharmacy.GetAllValues();
            var pharmacy = pharmacies.FirstOrDefault(u => u.ID == numberPharmacy);
            if (pharmacy == null)
            {
                Console.WriteLine("Введен неверный номер аптеки!");
                return;
            }

            _Warehouse.CreateItem(
                new WarehouseModel(
                    1,
                    numberPharmacy,
                    name));
        }

        static void DeleteWarehouse()
        {
            var values = _Warehouse.GetAllValues().ToList();
            Console.WriteLine("Удаление склада:");
            Console.WriteLine("Напишите номер удаляемого склада:");
            int number = -1;
            if (!int.TryParse(Console.ReadLine(), out number) ||
                values.FirstOrDefault(u => u.ID == number) != null)
            {
                Console.WriteLine("Введен неверный номер!");
                return;
            }

            _Warehouse.DeleteItem(number);
        }
        #endregion

        #region Batch
        /// <summary>
        /// Меню партий
        /// </summary>
        static void BatchMenu()
        {
            int selectedNumber = -1;

            while (selectedNumber != 0)
            {
                Console.WriteLine("Меню партии:\n" + _Menu + _EndMenu);
                if (int.TryParse(Console.ReadLine(), out selectedNumber))
                {
                    if (selectedNumber < 0 || selectedNumber > 3)
                    {
                        Console.WriteLine("Введен неверный номер!");
                        continue;
                    }
                    //Спуск в меню по иерархий
                    switch (selectedNumber)
                    {
                        case 1: //Вывести все записи
                            WriteAllBatches();
                            break;
                        case 2: //Добавить новую запись
                            AddBatch();
                            break;
                        case 3: //Удалить запись
                            DeleteBatch();
                            break;
                        case 0: //Назад
                            return;
                    }
                }
                else
                {
                    Console.WriteLine("Введен неверный номер!");
                }
            }
        }

        static void WriteAllBatches()
        {
            var values = _Batch.GetAllValues().ToList();
            Console.WriteLine("Вывод всех партий:");
            for (int i = 0; i <values.Count; i++)
            {
                var batch = values[i] as BatchModel;
                if (batch != null)
                    Console.WriteLine(
                        (i + 1) +
                        ".  Название:  " + batch.Name +
                        "   Количество: " + batch.Count);
            }
        }

        static void AddBatch()
        {
            Console.WriteLine("Добавление новой партий:");

            Console.WriteLine("Напишите название:");
            string name = Console.ReadLine().Replace(" ", "");
            if (name.Length == 0)
            {
                Console.WriteLine("Название введено неверно!");
                return;
            }

            Console.WriteLine("Напишите количество:");
            int count;
            if (!int.TryParse(Console.ReadLine().Replace(" ", ""), out count))
            {
                Console.WriteLine("Введено неверное количество!");
                return;
            }

            Console.WriteLine("Напишите номер товара:");
            int numberProduct;
            if (!int.TryParse(Console.ReadLine().Replace(" ", ""), out numberProduct))
            {
                Console.WriteLine("Введен неверный номер товара!");
                return;
            }

            var products = _Product.GetAllValues();
            var pharmacy = products.FirstOrDefault(u => u.ID == numberProduct);
            if (pharmacy == null)
            {
                Console.WriteLine("Введен неверный номер товара!");
                return;
            }

            Console.WriteLine("Напишите номер склада:");
            int numberWarehouse;
            if (!int.TryParse(Console.ReadLine().Replace(" ", ""), out numberWarehouse))
            {
                Console.WriteLine("Введен неверный номер склада!");
                return;
            }

            var warehouses = _Warehouse.GetAllValues();
            var warehouse = warehouses.FirstOrDefault(u => u.ID == numberProduct);
            if (warehouse == null)
            {
                Console.WriteLine("Введен неверный номер склада!");
                return;
            }

            _Batch.CreateItem(
                new BatchModel(
                    1,
                    numberProduct,
                    numberWarehouse,
                    count,
                    name));
        }

        static void DeleteBatch()
        {
            var values = _Batch.GetAllValues().ToList();
            Console.WriteLine("Удаление партии:");
            Console.WriteLine("Напишите номер удаляемой партии:");
            int number = -1;
            if (!int.TryParse(Console.ReadLine(), out number) ||
                values.FirstOrDefault(u => u.ID == number) != null)
            {
                Console.WriteLine("Введен неверный номер!");
                return;
            }

            _Batch.DeleteItem(number);
        }
        #endregion
    }
}
