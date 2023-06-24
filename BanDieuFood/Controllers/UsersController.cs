using BanDieuFood.Models;
using System.Linq;
using System.Web.Mvc;

namespace BanDieuFood.Controllers
{
    public class UsersController : Controller
    {
        private readonly BanDieuDBContext _context = new BanDieuDBContext();
        // Đăng Ký
        [HttpGet]
        public ActionResult dangky()
        {
            return View();
        }
        public bool kiemtratendn(string tendn)
        {
            return _context.Users.Count(x => x.UserName == tendn) > 0;
        }
        public bool kiemtraemail(string email)
        {
            return _context.Users.Count(x => x.Email == email) > 0;
        }
        [HttpPost]

        public ActionResult dangky(User model)
        {
            if (ModelState.IsValid)
            {
                if (kiemtratendn(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (kiemtraemail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var kh = new User();
                    kh.UserName = model.UserName;
                    kh.Address = model.Address;
                    kh.Password = model.Password;
                    kh.Email = model.Email;
                    kh.Phone = model.Phone;
                    kh.FullName = model.FullName;
                    _context.Users.Add(kh);
                    _context.SaveChanges();
                    return RedirectToAction("dangnhap");
                }
            }
            return View(model);
        }


        // Đăng Nhập
        [HttpGet]
        public ActionResult dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult dangnhap(User model)
        {
            if (ModelState.IsValid)
            {
                User kh = _context.Users.SingleOrDefault(n => n.UserName == model.UserName && n.Password == model.Password);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("index", "User");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            else
            {

            }
            return View(model);
        }
        public ActionResult dangxuat()
        {
            Session.Clear();
            return RedirectToAction("index", "User");
        }
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

    }
}