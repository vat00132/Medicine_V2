using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine_V2.Models
{
    public class WarehouseModel : Model
    {
        /// <summary>
        /// id аптеки
        /// </summary>
        public int IDPharmacies { get; private set; }
        /// <summary>
        /// Конструктор для склада
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="idPharmacies">id аптеки</param>
        /// <param name="name">Название</param>
        public WarehouseModel(int id, int idPharmacies, string name)
            : base(id, name)
        {
            IDPharmacies = idPharmacies;
        }
    }
}
