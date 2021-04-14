using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mogdachi.Models;
using Microsoft.AspNetCore.Http;

namespace Mogdachi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("Submit")]
        public IActionResult Submit(Mog Moogle)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("MoogleName", Moogle.Name);
                HttpContext.Session.SetInt32("MoogleFullness", 20);
                HttpContext.Session.SetInt32("MoogleHappiness", 20);
                HttpContext.Session.SetInt32("MoogleEnergy", 50);
                HttpContext.Session.SetInt32("MoogleMeals", 3);
                return RedirectToAction("PlayGround");
            }
            return View("Index");
        }
        [HttpGet("PlayGround")]
        public IActionResult PlayGround()
        {
            ViewBag.Name = HttpContext.Session.GetString("MoogleName");
            ViewBag.Fullness = HttpContext.Session.GetInt32("MoogleFullness");
            ViewBag.Happiness = HttpContext.Session.GetInt32("MoogleHappiness");
            ViewBag.Energy = HttpContext.Session.GetInt32("MoogleEnergy");
            ViewBag.Meals = HttpContext.Session.GetInt32("MoogleMeals");
            return View();
        }
        // Feeding your Dojodachi costs 1 meal and gains a random amount of fullness between 5 and 10 (you cannot feed your Dojodachi if you do not have meals)
        [HttpGet("Feed")]
        public IActionResult Feed()
        {
            int newMogoMeals = (int)HttpContext.Session.GetInt32("MoogleMeals");
            Random rand = new Random();
            if (newMogoMeals > 0)
            {
                HttpContext.Session.SetInt32("MoogleMeals", newMogoMeals - 1);
                int newMogoFullness = (int)HttpContext.Session.GetInt32("MoogleFullness") + rand.Next(5, 11);
                int yuck = rand.Next(1, 4);
                if (yuck == 2)
                {
                    Console.WriteLine("Yuck, he didn't like it!");
                }
                else
                {
                    HttpContext.Session.SetInt32("MoogleFullness", newMogoFullness);
                }
            }

            return RedirectToAction("PlayGround");
        }
        // Playing with your Dojodachi costs 5 energy and gains a random amount of happiness between 5 and 10
        [HttpGet("Play")]
        public IActionResult Play()
        {
            int newMoogleEnergy = (int)HttpContext.Session.GetInt32("MoogleEnergy");
            Random rand = new Random();
            if (newMoogleEnergy > 0)
            {
                HttpContext.Session.SetInt32("MoogleEnergy", newMoogleEnergy - 5);
                int newMoogleHappiness = (int)HttpContext.Session.GetInt32("MoogleHappiness") + rand.Next(5, 11);
                int yuck = rand.Next(1, 4);
                if (yuck == 2)
                {
                    Console.WriteLine("He's doesn't want to play with you!!!");
                }
                else
                {
                    HttpContext.Session.SetInt32("MoogleHappiness", newMoogleHappiness);
                }
            }

            return RedirectToAction("PlayGround");
        }
        //Working costs 5 energy and earns between 1 and 3 meals
        [HttpGet("Work")]
        public IActionResult Work()
        {
            int newMoogleEnergy = (int)HttpContext.Session.GetInt32("MoogleEnergy");
            Random rand = new Random();
            if (newMoogleEnergy > 0)
            {
                HttpContext.Session.SetInt32("MoogleEnergy", newMoogleEnergy - 5);
                int newMoogleMeals = (int)HttpContext.Session.GetInt32("MoogleMeals") + rand.Next(1, 4);
                HttpContext.Session.SetInt32("MoogleMeals", newMoogleMeals);
            }

            return RedirectToAction("PlayGround");
        }
        [HttpGet("Sleep")]
        //Sleeping earns 15 energy and decreases fullness and happiness each by 5
        public IActionResult Sleep()
        {
            int newMoogleEnergy = (int)HttpContext.Session.GetInt32("MoogleEnergy");
            int newMoogleHappiness = (int)HttpContext.Session.GetInt32("MoogleHappiness");
            int newMoogleFullness = (int)HttpContext.Session.GetInt32("MoogleFullness");
            HttpContext.Session.SetInt32("MoogleEnergy", newMoogleEnergy + 15);
            HttpContext.Session.SetInt32("MoogleHappiness", newMoogleHappiness - 5);
            HttpContext.Session.SetInt32("MoogleFullness", newMoogleFullness - 5);
            return RedirectToAction("PlayGround");
        }
        [HttpGet("Quit")]
        public IActionResult Quit()
        {
            return RedirectToAction("Index");
        }
        [HttpGet("TryAgain")]
        public IActionResult TryAgain()
        {
            HttpContext.Session.SetInt32("MoogleFullness", 20);
            HttpContext.Session.SetInt32("MoogleHappiness", 20);
            HttpContext.Session.SetInt32("MoogleEnergy", 50);
            HttpContext.Session.SetInt32("MoogleMeals", 3);
            return RedirectToAction("PlayGround");
        }
        public IActionResult Privacy()
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
