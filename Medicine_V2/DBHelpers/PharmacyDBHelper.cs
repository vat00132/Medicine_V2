using Medicine_V2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine_V2.DBHelpers
{
    public class PharmacyDBHelper : DBHelper
    {
        /// <summary>
        /// Название таблицы аптек в БД
        /// </summary>
        private const string PHARMACY_TABLE = "Pharmacies";

        public PharmacyDBHelper(DBProvider provider)
            : base(provider)
        {
            CheckTable();
        }

        /// <summary>
        /// Проверка на наличие таблицы
        /// </summary>
        protected override void CheckTable()
        {
            _Provider.OpenConnection();

            var command = _Provider.SqlCommand(
                @"SELECT COUNT(*) FROM sys.sysobjects WHERE name=@Table");
            command.Parameters.AddWithValue("@Table", PHARMACY_TABLE);

            //Данной таблицы нет
            if ((int)command.ExecuteScalar() == 0)
            {
                command = _Provider.SqlCommand(
                    @"CREATE TABLE " + PHARMACY_TABLE + " (ID INT PRIMARY KEY IDENTITY, Name NVARCHAR(255) NOT NULL, Address NVARCHAR(255) NOT NULL, Phone NVARCHAR(20) NOT NULL)");
                command.ExecuteNonQuery();
            }

            _Provider.CloseConnection();
        }

        /// <summary>
        /// Создать аптеку
        /// </summary>
        /// <param name="name">Название</param>
        public override void CreateItem(Model model)
        {
            PharmacyModel pharmacy = model as PharmacyModel;
            if (pharmacy == null)
                return;

            _Provider.OpenConnection();

            var command = _Provider.SqlCommand(
                @"INSERT INTO " + PHARMACY_TABLE + " (Name, Address, Phone) VALUES (@Name, @Address, @Phone)");
            command.Parameters.AddWithValue("@Name", pharmacy.Name);
            command.Parameters.AddWithValue("@Address", pharmacy.Address);
            command.Parameters.AddWithValue("@Phone", pharmacy.Phone);
            command.ExecuteNonQuery();

            _Provider.CloseConnection();
        }

        /// <summary>
        /// Удаление аптеки по id
        /// </summary>
        /// <param name="id">id аптеки</param>
        public override void DeleteItem(int id)
        {
            _Provider.OpenConnection();

            var command = _Provider.SqlCommand(
                @"DELETE  FROM " + PHARMACY_TABLE + " WHERE ID=@ID");
            command.Parameters.AddWithValue("@ID", id);
            command.ExecuteNonQuery();

            _Provider.CloseConnection();
        }


        /// <summary>
        /// Получить все записи из таблицы
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Model> GetAllValues()
        {
            List<PharmacyModel> models = new List<PharmacyModel>();

            _Provider.OpenConnection();

            var command = _Provider.SqlCommand(
                @"SELECT * FROM " + PHARMACY_TABLE);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                //Если есть данные
                if (reader.HasRows)
                {
                    //Построчное считывание
                    while (reader.Read())
                    {
                        models.Add(
                            new PharmacyModel(
                                reader.GetInt32(0),  //id
                                reader.GetString(1),    //Название
                                reader.GetString(2),    //Адрес
                                reader.GetString(3)));  //Телефон
                    }
                }
            }

            _Provider.CloseConnection();

            return models;
        }
    }
}
