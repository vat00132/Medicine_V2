using Medicine_V2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine_V2.DBHelpers
{
    public class ProductDBHelper : DBHelper
    {
        /// <summary>
        /// Название таблицы товаров в БД
        /// </summary>
        private const string PRODUCT_TABLE = "Products";

        public ProductDBHelper(DBProvider provider)
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
            command.Parameters.AddWithValue("@Table", PRODUCT_TABLE);

            //Данной таблицы нет
            if ((int)command.ExecuteScalar() == 0)
            {
                command = _Provider.SqlCommand(
                    @"CREATE TABLE " + PRODUCT_TABLE + " (ID INT PRIMARY KEY IDENTITY, Name NVARCHAR(255) NOT NULL)");
                command.ExecuteNonQuery();
            }

            _Provider.CloseConnection();
        }

        /// <summary>
        /// Создать товар
        /// </summary>
        /// <param name="name">Название</param>
        public override void CreateItem(Model model)
        {
            ProductModel product = model as ProductModel;
            if (model == null)
                return;

            _Provider.OpenConnection();

            var command = _Provider.SqlCommand(
                @"INSERT INTO " + PRODUCT_TABLE + " (Name) VALUES (@Name)");
            command.Parameters.AddWithValue("@Name", product.Name);
            command.ExecuteNonQuery();

            _Provider.CloseConnection();
        }

        /// <summary>
        /// Удаление товара по id
        /// </summary>
        /// <param name="id">id товара</param>
        public override void DeleteItem(int id)
        {
            _Provider.OpenConnection();

            var command = _Provider.SqlCommand(
                @"DELETE  FROM " + PRODUCT_TABLE + " WHERE ID=@ID");
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
            List<ProductModel> models = new List<ProductModel>();

            _Provider.OpenConnection();

            var command = _Provider.SqlCommand(
                @"SELECT * FROM " + PRODUCT_TABLE);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                //Если есть данные
                if (reader.HasRows)
                {
                    //Построчное считывание
                    while (reader.Read())
                    {
                        models.Add(
                            new ProductModel(
                                reader.GetInt32(0),  //id
                                reader.GetString(1)));  //Название
                    }
                }
            }

            _Provider.CloseConnection();

            return models;
        }
    }
}
