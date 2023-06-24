using BanDieuFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDieuFood.Controllers
{
    public class CartController : Controller
    {
        //private readonly BanDieuDBContext _context;

        //public CartController(BanDieuDBContext context)
        //{
        //    _context = context;

        //}
        //// GET: Cart
        //public List<CartItem> GioHang
        //{
        //    get
        //    {
        //        List<CartItem> Cart = (List<CartItem>)Session["giohang"];
        //        if (Cart == default(List<CartItem>))
        //        {
        //            Cart = new List<CartItem>();
        //        }
        //        return Cart;
        //    }
        //}
        //public ActionResult AddToCart(int productID, int? amount)
        //{
        //    List<CartItem> cart = GioHang;

        //    try
        //    {
        //        //Them san pham vao gio hang
        //        CartItem item = cart.SingleOrDefault(p => p.product.ProductID == productID);
        //        if (item != null) // da co => cap nhat so luong
        //        {
        //            item.amount = item.amount + amount.Value;
        //            //luu lai session
        //            List<CartItem> Cart1 = (List<CartItem>)Session["giohang"];
        //        }
        //        else
        //        {
        //            Product hh = _context.Products.SingleOrDefault(p => p.ProductID == productID);
        //            item = new CartItem
        //            {
        //                amount = amount.HasValue ? amount.Value : 1,
        //                product = hh
        //            };
        //            cart.Add(item);
        //        }

        //        //Luu lai Session
        //        List<CartItem> Cart = (List<CartItem>)Session["giohang"];

        //        return Json(new { success = true });
        //    }
        //    catch
        //    {
        //        return Json(new { success = false });
        //    }
        //}
        //public ActionResult UpdateCart(int productID, int? amount)
        //{
        //    //Lay gio hang ra de xu ly
        //    List<CartItem> Cart = (List<CartItem>)Session["giohang"];
        //    try
        //    {
        //        if (Cart != null)
        //        {
        //            CartItem item = Cart.SingleOrDefault(p => p.product.ProductID == productID);
        //            if (item != null && amount.HasValue) // da co -> cap nhat so luong
        //            {
        //                item.amount = amount.Value;
        //            }
        //            List<CartItem> Cart1 = (List<CartItem>)Session["giohang"];

        //        }
        //        return Json(new { success = true });
        //    }
        //    catch
        //    {
        //        return Json(new { success = false });
        //    }
        //}
        //public ActionResult Remove(int productID)
        //{

        //    try
        //    {
        //        List<CartItem> gioHang = GioHang;
        //        CartItem item = gioHang.SingleOrDefault(p => p.product.ProductID == productID);
        //        if (item != null)
        //        {
        //            gioHang.Remove(item);
        //        }
        //        //luu lai session
        //        List<CartItem> Cart = (List<CartItem>)Session["giohang"];
        //        return Json(new { success = true });
        //    }
        //    catch
        //    {
        //        return Json(new { success = false });
        //    }
        //}
        public ActionResult GioHang()
        {
            List<CartItem> dsGiohang = Laygiohang();
            if (dsGiohang.Count == 0)
            {
                return RedirectToAction("Index", "User");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(dsGiohang);
        }
        public List<CartItem> Laygiohang()
        {
            List<CartItem> dsGiohang = Session["Giohang"] as List<CartItem>;
            if (dsGiohang == null)
            {
                dsGiohang = new List<CartItem>();
                Session["Giohang"] = dsGiohang;
            }
            return dsGiohang;
        }
        public ActionResult ThemGiohang(int iMAGIAY, string strURL)
        {
            List<CartItem> dsGiohang = Laygiohang();
            CartItem sanpham = dsGiohang.Find(n => n.IDproducts == iMAGIAY);
            if (sanpham == null)
            {   
                sanpham = new CartItem(iMAGIAY);
                dsGiohang.Add(sanpham);
                return Redirect(strURL);    
            }
            else
            {
                sanpham.Numbers++;
                return Redirect(strURL);
            }
        }
       
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<CartItem> dsGiohang = Session["GioHang"] as List<CartItem>;
            if (dsGiohang != null)
            {
                iTongSoLuong = dsGiohang.Sum(n => n.Numbers);
            }
            return iTongSoLuong;
        }
        //Tinh tong tien
        private double TongTien()
        {
            double iTongTien = 0;
            List<CartItem> dsGiohang = Session["GioHang"] as List<CartItem>;
            if (dsGiohang != null)
            {
                iTongTien = dsGiohang.Sum(n => n.Total);
            }
            return iTongTien;
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGiohang(int iMaSP)
        {
            List<CartItem> dsGiohang = Laygiohang();
            CartItem sanpham = dsGiohang.SingleOrDefault(n => n.IDproducts == iMaSP);
            if (sanpham != null)
            {
                dsGiohang.RemoveAll(n => n.IDproducts == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (dsGiohang.Count == 0)
            {
                return RedirectToAction("index", "User");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatcaGiohang()
        {
            List<CartItem> dsGiohang = Laygiohang();
            dsGiohang.Clear();
            return RedirectToAction("index", "User");
        }
        public ActionResult DatHang()
        {
            //Kiem tra dang nhap
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "User");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "User");
            }

            //Lay gio hang tu Session
            List<CartItem> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Checkout()
        {
            return View();
        }
    }
}