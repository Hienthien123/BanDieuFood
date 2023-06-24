using BanDieuFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace BanDieuFood.Controllers
{
    public class CartItem
    {
        public int IDproducts { set; get; }
        public string NameProducts { set; get; }
        public string Images { set; get; }
        public Double Prices { set; get; }
        public int Numbers { set; get; }
        public Double Total
        {
            get { return Prices * Numbers; }

        }
        public CartItem(int MAGIAY)
        {
        }
    }
}