using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EntityFramework_Slider.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            //bu yazdiqlarimiz linkue queryler adlanir.bu kodlar vasitesile lazim  olan iwleri gorursen
            List<Slider> sliders = await _context.Sliders.ToListAsync();

            //IQueryable<Slider> slide = _context.Sliders.AsQueryable();
            //List<Slider> query = slide.Where(m=> m.Id > 5).ToList();
            //Iqueryable ile query yaradir ramda saxlayiriq, hele data getirmirik,
            //sonra werte uygun datani getiririk.
            //ToList() yazdiqda request gedir data bazaya. yazmadiqda getmir.

            SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync();


            //burada where deyib qeyd edirikki haradaki softdelete true olmayanlari goster. veye softdelete == beraberdirse false goster
            IEnumerable<Blog> blogs = await _context.Blogs.Where(m=>m.SoftDelete == false).ToListAsync();

            //Listin ferqli metodlari var.Add,Remove ve s. Listi controllerde hazirlayob view a gonderirik.
            //Listin ichinde olan elave metodlarda iwleyir.
            //IEnumerable da ise hech bir metod yoxdur, ancaq dovre sala bilirsen.View da datani Inumerable kimi qebul etmek daha yaxwidir.
            //Viewda datani sadece foreach edib gosteririk deye, Inumerable iwletmek yaxwidir.LIstin elave metodlari iwleyib sistemi agirlawdirmasin deye.
            IEnumerable<Category> categories = await _context.Categories.Where(m =>m.SoftDelete == false).ToListAsync();


            //burada wekil null gelir Ona gore firstOrDefault ede bilmir.Ona burada Whereden evvel include edirik.
            //Burada relationlu tablelerin icherisinde, ichinde olan o biri tablenide (meselen productun ichinde yazdigimiz  productImage)yeni, productImageni productnan birlikde getirmek isteyirikse,
            //Products.Include edirik
            IEnumerable<Product> products = await _context.Products.Include(m=>m.Images).Where(m => !m.SoftDelete).ToListAsync();

            About abouts = await _context.Abouts.Include(m => m.Adventages).FirstOrDefaultAsync();

            IEnumerable<Experts>  experts = await _context.Experts.Where(m=>m.SoftDelete == false).ToListAsync();

            ExpertsHeader expertsHeader = await _context.ExpertsHeaders.FirstOrDefaultAsync();

            Subscribe subscribe = await _context.Subscribes.FirstOrDefaultAsync();

            OurBlog ourBlog = await _context.OurBlogs.FirstOrDefaultAsync();

            IEnumerable<Say> says = await _context.Says.Where(m => m.SoftDelete == false).ToListAsync();

            IEnumerable<Instagram> instagrams = await _context.Instagrams.Where(m => m.SoftDelete == false).ToListAsync();





            HomeVM model = new()
            {
                Sliders = sliders,
                SliderInfo = sliderInfo,
                Blogs = blogs,
                Categories = categories,
                Products = products,
                Abouts = abouts,
                Experts = experts,
                ExpertsHeader = expertsHeader,
                Subscribe = subscribe,
                OurBlog = ourBlog,
                Says = says,
                Instagrams = instagrams,
            };

            return View(model);
        }
    }
}