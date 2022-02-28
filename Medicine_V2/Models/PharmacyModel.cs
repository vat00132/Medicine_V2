using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine_V2.Models
{
    public class PharmacyModel : Model
    {
        /// <summary>
        /// Адрес аптеки
        /// </summary>
        public string Address { get; private set; }
        /// <summary>
        /// Телефон аптеки
        /// </summary>
        public string Phone { get; private set; }
        /// <summary>
        /// Конструктор аптеки
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="name">Название</param>
        /// <param name="address">Адрес</param>
        /// <param name="phone">Телефон</param>
        public PharmacyModel(int id, string name, string address, string phone)
            : base(id, name)
        {
            Address = address;
            Phone = phone;
        }
    }
}
