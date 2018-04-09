//Shopping List Class for Start Finance
//Date last modified 06/04/2018
//@Author: Leon Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace StartFinance.Models
{
    public class ShoppingList
    {
        [PrimaryKey,AutoIncrement]
        public int ShoppingItemID { get; set; }

        public string ShopName { get; set; }

        [NotNull]
        public string NameOfItem { get; set; }

        public string ShoppingDate { get; set; }

        [NotNull]
        public double PriceQuoted { get; set; }
    }
}
