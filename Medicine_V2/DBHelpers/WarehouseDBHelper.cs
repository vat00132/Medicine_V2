using Medicine_V2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine_V2.DBHelpers
{
    public class WarehouseDBHelper : DBHelper
    {
        /// <summary>
        /// Название таблицы складов в БД
        /// </summary>
        private const string WAREHOUSE_TABLE = "Warehouses";

        public WarehouseDBHelper(DBProvider provider)
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
            command.Parameters.AddWithValue("@Table", WAREHOUSE_TABLE);

            //Данной таблицы нет
            if ((int)command.ExecuteScalar() == 0)
            {
                command = _Provider.SqlCommand(
                    @"CREATE TABLE " + WAREHOUSE_TABLE + " (ID INT PRIMARY KEY IDENTITY, IDPharmacies INT NOT NULL, Name NVARCHAR(255) NOT NULL)");
                command.ExecuteNonQuery();
            }

            _Provider.CloseConnection();
        }

        /// <summary>
        /// Создать новый склад
        /// </summary>
        public override void CreateItem(Model model)
        {
            WarehouseModel warehouse = model as WarehouseModel;
            if (warehouse == null) return;

            _Provider.OpenConnection();

            var command = _Provider.SqlCommand(
                @"INSERT INTO " + WAREHOUSE_TABLE + " (IDPharmacies, Name) VALUES (@IDPharmacies, @Name)");
            command.Parameters.AddWithValue("@IDPharmacies", warehouse.IDPharmacies);
            command.Parameters.AddWithValue("@Name", warehouse.Name);
            command.ExecuteNonQuery();

            _Provider.CloseConnection();
        }

        /// <summary>
        /// Удаление склада по id
        /// </summary>
        /// <param name="id">id склада</param>
        public override void DeleteItem(int id)
        {
            _Provider.OpenConnection();

            var command = _Provider.SqlCommand(
                @"DELETE  FROM " + WAREHOUSE_TABLE + " WHERE ID=@ID");
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
            List<WarehouseModel> models = new List<WarehouseModel>();

            _Provider.OpenConnection();

            var command = _Provider.SqlCommand(
                @"SELECT * FROM " + WAREHOUSE_TABLE);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                //Если есть данные
                if (reader.HasRows)
                {
                    //Построчное считывание
                    while (reader.Read())
                    {
                        models.Add(
                            new WarehouseModel(
                                reader.GetInt32(0),  //id
                                reader.GetInt32(1),  //id аптеки
                                reader.GetString(2)));  //Название
                    }
                }
            }

            _Provider.CloseConnection();

            return models;
        }
    }
}
