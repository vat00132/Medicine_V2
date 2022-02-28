using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine_V2.DBHelpers
{
    public class DBProvider
    {
        /// <summary>
        /// Источник данных
        /// </summary>
        private const string _DataSource = "DESKTOP-6CO4U8L\\SQLEXPRESS";
        /// <summary>
        /// БД
        /// </summary>
        private const string _InitialCatalog = "Medicine_V2";
        /// <summary>
        /// Строка подключения
        /// </summary>
        private readonly string _ConnectionString = new SqlConnectionStringBuilder
        {
            DataSource = _DataSource,
            InitialCatalog = _InitialCatalog,
            IntegratedSecurity = true
        }.ConnectionString;
        /// <summary>
        /// Подключение к БД
        /// </summary>
        private SqlConnection SqlConnection;

        /// <summary>
        /// Конструктор
        /// </summary>
        public DBProvider()
        {
            SqlConnection = new SqlConnection(_ConnectionString);
        }

        /// <summary>
        /// Создание sql запроса
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <returns></returns>
        public SqlCommand SqlCommand(string query)
        {
            var command = new SqlCommand
            {
                Connection = SqlConnection,
                CommandText = query
            };
            return command;
        }

        /// <summary>
        /// Открыть соединение
        /// </summary>
        public void OpenConnection()
        {
            SqlConnection.Open();
        }

        /// <summary>
        /// Закрыть соединение
        /// </summary>
        public void CloseConnection()
        {
            SqlConnection.Close();
        }
    }
}
