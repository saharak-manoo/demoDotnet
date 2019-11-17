using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using demoDotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace demoDotnet.Controllers {
  public class HomeController : Controller {
    private readonly DemoDotnetContext _context;

    public HomeController (DemoDotnetContext _context) {
      this._context = _context;
    }

    [HttpGet]
    public IActionResult Index () {
      string[] originalString = new string[1];
      List<string[]> testString = new List<string[]> ();

      originalString[0] = "first";
      testString.Add (originalString);
      originalString[0] = "second";
      testString.Add (originalString);

      foreach (var t in testString) {
        Console.WriteLine (">>>>> {0}", t[0]);
      }

      Console.WriteLine ("testString[0][0] = " + testString[0][0]);
      Console.WriteLine ("testString[1][0] = " + testString[1][0]);

      var students = _context.Students;
      return View (students);
    }

    [HttpPost]
    public IActionResult LoadStudent (String tableName, String keyword, int limit, int offset, int[] studentIds, String sort, String order, String count) {
      var students = _context.Students.Where (t =>
        t.Name.Contains (keyword) ||
        t.Status.Contains (keyword));

      var Students = keyword == null ? _context.Students : students;

      if (studentIds.Length != 0 && tableName != "StudentTableModal") {
        Students = Students.Where (t => studentIds.Contains (t.Id));
      }

      if (order == "asc") {
        if (sort == "Id") { Students = Students.OrderBy (x => x.Id); } else if (sort == "Name") { Students = Students.OrderBy (x => x.Name); } else if (sort == "Status") { Students = Students.OrderBy (x => x.Status); } else if (sort == "Brithday") { Students = Students.OrderBy (x => x.Brithday); }
      } else if (order == "desc") {
        if (sort == "Id") { Students = Students.OrderByDescending (x => x.Id); } else if (sort == "Name") { Students = Students.OrderByDescending (x => x.Name); } else if (sort == "Status") { Students = Students.OrderByDescending (x => x.Status); } else if (sort == "Brithday") { Students = Students.OrderByDescending (x => x.Brithday); }
      }

      if (count == null) {
        return PartialView (tableName, Students.Skip (offset).Take (limit).ToList ());
      } else {
        return Json (Students.Count ());
      }
    }

    [HttpGet]
    public IActionResult New () {
      return View ();
    }

    [HttpPost]
    public IActionResult Create (Student student) {
      if (ModelState.IsValid) {
        student.Brithday = DateTime.Now;
        _context.Students.Add (student);
        _context.SaveChanges ();
      }

      return RedirectToAction ("Index");
    }

    [HttpDelete]
    public IActionResult Delete (int id) {
      Student student = _context.Students.Find (id);
      _context.Students.Remove (student);
      _context.SaveChanges ();
      return Json (true);
    }

    public IActionResult Privacy () {
      return View ();
    }

    [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error () {
      return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}