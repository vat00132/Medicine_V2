using Medicine_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine_V2.DBHelpers
{
    public abstract class DBHelper
    {
        public DBProvider _Provider { get; private set; }

        public DBHelper(DBProvider provider)
        {
            _Provider = provider;
        }

        protected abstract void CheckTable();
        public abstract void CreateItem(Model model);
        public abstract void DeleteItem(int id);
        public abstract IEnumerable<Model> GetAllValues();
    }
}
