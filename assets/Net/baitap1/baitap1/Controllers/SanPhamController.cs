using baitap1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace baitap1.Controllers
{
    public class SanPhamController : Controller
    {
        static List<SanPham> data = new List<SanPham>()
        {
            new SanPham{ Id=1, Ten="Samsung Galaxy Tab 10.1", Gia=241.99m, Hinh="/Content/img/1.png", SoldOut=true, Rating=4, Type=ProductType.Latest },
            new SanPham{ Id=2, Ten="iPod Classic", Gia=122, Hinh="/Content/img/2.png", Rating=5, Type=ProductType.Latest },
            new SanPham{ Id=3, Ten="HP LP3065", Gia=122, Hinh="/Content/img/3.png", Rating=4, Type=ProductType.Latest },
            new SanPham{ Id=4, Ten="Sony VAIO", Gia=1200, Hinh="/Content/img/4.png", Rating=4, Type=ProductType.Latest },

            new SanPham{ Id=5, Ten="MacBook Pro", Gia=2000, GiaCu=1950, Sale=true, Hinh="/Content/img/5.png", Rating=5, Type=ProductType.Featured },
            new SanPham{ Id=6, Ten="MacBook Air", Gia=120, GiaCu=104, Sale=true, Hinh="/Content/img/6.png", Rating=4, Type=ProductType.Featured },
            new SanPham{ Id=7, Ten="Samsung Galaxy Tab 10.1", Gia=241.99m, Hinh="/Content/img/1.png", SoldOut=true, Rating=4, Type=ProductType.Latest },
            new SanPham{ Id=8, Ten="iPod Classic", Gia=122, Hinh="/Content/img/2.png", Rating=5, Type=ProductType.Latest },
            new SanPham{ Id=9, Ten="HP LP3065", Gia=122, Hinh="/Content/img/3.png", Rating=4, Type=ProductType.Latest },
            new SanPham{ Id=10, Ten="Sony VAIO", Gia=1200, Hinh="/Content/img/4.png", Rating=4, Type=ProductType.Latest },
            new SanPham{ Id=11, Ten="Samsung Galaxy Tab 10.1", Gia=241.99m, Hinh="/Content/img/1.png", SoldOut=true, Rating=4, Type=ProductType.Latest },
            new SanPham{ Id=12, Ten="iPod Classic", Gia=122, Hinh="/Content/img/2.png", Rating=5, Type=ProductType.Latest },
            new SanPham{ Id=13, Ten="HP LP3065", Gia=122, Hinh="/Content/img/3.png", Rating=4, Type=ProductType.Latest },
            new SanPham{ Id=14, Ten="Sony VAIO", Gia=1200, Hinh="/Content/img/4.png", Rating=4, Type=ProductType.Latest },
            new SanPham{ Id=15, Ten="Samsung Galaxy Tab 10.1", Gia=241.99m, Hinh="/Content/img/1.png", SoldOut=true, Rating=4, Type=ProductType.Latest },
            new SanPham{ Id=16, Ten="iPod Classic", Gia=122, Hinh="/Content/img/2.png", Rating=5, Type=ProductType.Latest },
            new SanPham{ Id=17, Ten="HP LP3065", Gia=122, Hinh="/Content/img/3.png", Rating=4, Type=ProductType.Latest },
            new SanPham{ Id=18, Ten="Sony VAIO", Gia=1200, Hinh="/Content/img/4.png", Rating=4, Type=ProductType.Latest },
        };

        public ActionResult Index(string tab = "Latest")
        {
            ViewBag.Tab = tab;
            var list = data.Where(x => x.Type.ToString() == tab).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(SanPham sp)
        {
            sp.Id = data.Count + 1;
            data.Add(sp);
            return RedirectToAction("Index");
        }
    }
}
