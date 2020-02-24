using Microsoft.AspNetCore.Mvc;
using Pi.Math;
using Pi.Web.Models;
using System;
using System.Diagnostics;

namespace Pi.Web.Controllers
{
    public class PiController : Controller
    {
        public IActionResult Index(int? dp = 6)
        {
            var stopwatch = Stopwatch.StartNew();

            var pi = MachinFormula.Calculate(dp.Value);

            var model = new PiViewModel
            {
                DecimalPlaces = dp.Value,
                Value = pi.ToString(),
                ComputeMilliseconds = stopwatch.ElapsedMilliseconds,
                ComputeHost = Environment.MachineName
            };

            return View(model);
        }
    }
}