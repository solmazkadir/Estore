﻿using Estore.Core.Entities;
using Estore.MVCUI.Models;
using Estore.MVCUI.Utils;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Estore.MVCUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<Slider> _serviceSlider;
        private readonly IService<Product> _serviceProduct;
        private readonly IService<Contact> _serviceContact;
        private readonly IService<News> _serviceNews;
        private readonly IService<Brand> _serviceBrand;
        private readonly IService<AppLog> _serviceLog;
        private readonly IService<Setting> _serviceSetting;

        public HomeController(IService<Slider> serviceSlider, IService<Product> serviceProduct, IService<Contact> serviceContact, IService<News> serviceNews, IService<Brand> serviceBrand, IService<AppLog> serviceLog, IService<Setting> serviceSetting)
        {
            _serviceSlider = serviceSlider;
            _serviceProduct = serviceProduct;
            _serviceContact = serviceContact;
            _serviceNews = serviceNews;
            _serviceBrand = serviceBrand;
            _serviceLog = serviceLog;
            _serviceSetting = serviceSetting;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomePageViewModel()
            {
                Sliders = await _serviceSlider.GetAllAsync(),
                Products = await _serviceProduct.GetAllAsync(p => p.IsActive && p.IsHome),
                Brands = await _serviceBrand.GetAllAsync(b => b.IsActive),
                News = await _serviceNews.GetAllAsync(n => n.IsActive)
            };
            return View(model);
        }

        public async Task<IActionResult> Privacy()
        {
           
            return View();
        }
        [Route("iletisim")]
        public async Task<IActionResult> ContactUs()
        {
            //var model = await _serviceSetting.GetAllAsync();
            //if (model !=null)
            //{
            //    return View(model.FirstOrDefault());
            //}
            return View();
        }
        [HttpPost]
        [Route("iletisim")]
        public async Task<IActionResult> ContactUsAsync(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceContact.AddAsync(contact);
                    var sonuc = await _serviceContact.SaveAsync();
                    if (sonuc > 0)
                    {
                        //await MailHelper.SendMailAsync(contact); //gelen mesajı mail gönder
                        TempData["Message"] = "<div class='alert alert-success'>Mesajınız Gönderildi!</div>";
                        return RedirectToAction("ContactUs");
                    }
                }
                catch (Exception hata)
                {
                    await _serviceLog.AddAsync(new AppLog
                    {
                        Title = "İletişim Formu Gönderilirken Hata Oluştu",
                        Description = hata.Message
                    });
                    await _serviceLog.SaveAsync();
                    //await MailHelper.SendMailAsync(contact); //oluşan hatayı yazılımcıyı  mail gönder
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(contact);
        }
        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}