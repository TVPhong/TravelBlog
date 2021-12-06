using BlogDuLich.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDuLich.Controllers
{
    public class TravelController : Controller
    {
        private readonly Service _service;
        public TravelController(Service service)
        {
            _service = service;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _service.Paging(page);
            ViewData["Pages"] = model.pages;
            ViewData["Page"] = model.page;
            return View(model.travels);
        }
        public IActionResult Details(int id)
        {
            var trv = _service.Get(id);
            if (trv == null)
            {
                return NotFound();
            }

            else
            {
                return View(trv);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var trv = _service.Get(id);
            if (trv == null)
            {
                return NotFound();

            }
            else
            {
                return View(trv);
            }
        }

        [HttpPost]
        public IActionResult Delete(Travel travel)
        {
            _service.Delete(travel.Id);
            _service.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var trv = _service.Get(id);
            if (trv == null)
            {
                return NotFound();
            }
            else
            {
                return View(trv);
            }
        }

        [HttpPost]
        public IActionResult Edit(Travel travel, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                _service.Upload(travel, file);
                _service.Update(travel);
                _service.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(travel);
        }

        public IActionResult Create() => View(_service.Create());
        [HttpPost]
        public IActionResult Create(Travel travel, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                _service.Upload(travel, file);
                _service.Add(travel);
                _service.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(travel);
        }

        public IActionResult Read(int id)
        {
            var b = _service.Get(id);
            if (b == null) return NotFound();
            if (!System.IO.File.Exists(_service.GetDataPath(b.DataFile))) return NotFound();

            var (stream, type) = _service.Download(b);
            return File(stream, type, b.DataFile);
        }

        public IActionResult Search(string word)
        {
            return View("Index", _service.Get(word));
        }
      
    }
}
