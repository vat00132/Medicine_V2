using Medicine_V2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine_V2.DBHelpers
{
    public class BatchDBHelper : DBHelper
    {
        /// <summary>
        /// Название таблицы партий в БД
        /// </summary>
        private const string BATCH_TABLE = "Batches";

        public BatchDBHelper(DBProvider provider)
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
            command.Parameters.AddWithValue("@Table", BATCH_TABLE);

            //Данной таблицы нет
            if ((int)command.ExecuteScalar() == 0)
            {
                command = _Provider.SqlCommand(
                    @"CREATE TABLE " + BATCH_TABLE + " (ID INT PRIMARY KEY IDENTITY, IDProduct INT NOT NULL, IDWarehouse INT NOT NULL, Count INT NOT NULL, Name NVARCHAR(255) NOT NULL)");
                command.ExecuteNonQuery();
            }

            _Provider.CloseConnection();
        }

        /// <summary>
        /// Создание новой партий
        /// </summary>
        public override void CreateItem(Model model)
        {
            BatchModel batch = model as BatchModel;
            if (batch == null) return;

            _Provider.OpenConnection();

            var command = _Provider.SqlCommand(
                @"INSERT INTO " + BATCH_TABLE + " (IDProduct, IDWarehouse, Count, Name) VALUES (@IDProduct, @IDWarehouse, @Count, @Name)");
            command.Parameters.AddWithValue("@IDProduct", batch.IDProduct);
            command.Parameters.AddWithValue("@IDWarehouse", batch.IDWarehouse);
            command.Parameters.AddWithValue("@Count", batch.Count);
            command.Parameters.AddWithValue("@Name", batch.Name);
            command.ExecuteNonQuery();

            _Provider.CloseConnection();
        }

        /// <summary>
        /// Удаление партий по id
        /// </summary>
        /// <param name="id">id партий</param>
        public override void DeleteItem(int id)
        {
            _Provider.OpenConnection();

            var command = _Provider.SqlCommand(
                @"DELETE  FROM " + BATCH_TABLE + " WHERE ID=@ID");
            command.Parameters.AddWithValue("@ID", id);
            command.ExecuteNonQuery();

            _Provider.CloseConnection();
        }

        /// <summary>
        /// Получить все значения из таблицы
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Model> GetAllValues()
        {
            List<BatchModel> models = new List<BatchModel>();

            _Provider.OpenConnection();

            var command = _Provider.SqlCommand(
                @"SELECT * FROM " + BATCH_TABLE);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                //Если есть данные
                if (reader.HasRows)
                {
                    //Построчное считывание
                    while (reader.Read())
                    {
                        models.Add(
                            new BatchModel(
                                reader.GetInt32(0),  //id
                                reader.GetInt32(1),  //id товара
                                reader.GetInt32(2),  //id склада
                                reader.GetInt32(3), //Количество товара в партий
                                reader.GetString(4)));  //Название
                    }
                }
            }

            _Provider.CloseConnection();

            return models;
        }
    }
}
