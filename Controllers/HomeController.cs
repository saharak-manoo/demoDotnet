using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using demoDotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace demoDotnet.Controllers {
  public class HomeController : Controller {
    private readonly DemoDotnetContext _context;

    public HomeController (DemoDotnetContext _context) {
      this._context = _context;
    }

    [HttpGet]
    public IActionResult Index () {
      // create classrooms
      if (_context.Classrooms.Count () == 0) {
        _context.Classrooms.Add (new Classroom () { Name = "CS-01" });
        _context.Classrooms.Add (new Classroom () { Name = "CS-02" });
        _context.Classrooms.Add (new Classroom () { Name = "CS-03" });
        _context.Classrooms.Add (new Classroom () { Name = "CS-04" });
        _context.Classrooms.Add (new Classroom () { Name = "CS-05" });
        _context.SaveChanges ();
      }

      // create genders
      if (_context.Genders.Count () == 0) {
        _context.Genders.Add (new Gender () { Name = "Man" });
        _context.Genders.Add (new Gender () { Name = "Woman" });
        _context.SaveChanges ();
      }

      // ข้อที่ 2
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
      // จบข้อที่ 2

      // ข้อที่ 3 แบบ joins table เอง
      var datas = _context.Students
        .Where (s => s.Status.Equals ("กำลังศึกษา"))
        .Join (_context.Classrooms,
          s => s.Classroom.Id,
          c => c.Id,
          (s, c) => new { Student = s, Classroom = c })
        .Join (_context.Genders,
          s => s.Student.Gender.Id,
          g => g.Id,
          (s, g) => new { Student = s, Gender = g })
        .Select (select => new {
          Id = select.Student.Student.Id,
            Name = select.Student.Student.Name,
            GenderName = select.Gender.Name,
            ClassroomName = select.Student.Classroom.Name,
            Status = select.Student.Student.Status
        }).ToList ();

      foreach (var data in datas) {
        Console.WriteLine ("query => แบบ Joins เอง");
        Console.WriteLine ("Id => {0}", data.Id);
        Console.WriteLine ("Name => {0}", data.Name);
        Console.WriteLine ("GenderName => {0}", data.GenderName);
        Console.WriteLine ("ClassroomName => {0}", data.ClassroomName);
        Console.WriteLine ("Status => {0}", data.Status);
      }
      // จบ

      // ข้อที่ 3 แบบใช้ความสามารถที่ join กันใน models
      var listStudent = _context.Students
        .Where (s => s.Status.Equals ("กำลังศึกษา"))
        .Select (s => new {
          Id = s.Id,
            Name = s.Name,
            GenderName = s.Gender.Name,
            ClassroomName = s.Classroom.Name,
            Status = s.Status
        }).ToList ();

      foreach (var student in listStudent) {
        Console.WriteLine ("query => แบบใช้ความสามารถที่ join กันใน models");
        Console.WriteLine ("Id => {0}", student.Id);
        Console.WriteLine ("Name => {0}", student.Name);
        Console.WriteLine ("GenderName => {0}", student.GenderName);
        Console.WriteLine ("ClassroomName => {0}", student.ClassroomName);
        Console.WriteLine ("Status => {0}", student.Status);
      }

      // จบ

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
      var studentData = new StudentData ();
      studentData.Classrooms = _context.Classrooms.ToList ();
      studentData.Genders = _context.Genders.ToList ();
      return View (studentData);
    }

    [HttpPost]
    public IActionResult Create (Student studentData) {
      var student = new Student ();
      student.Name = "Test3";
      student.Status = "กำลังศึกษา";
      student.Brithday = DateTime.Now;
      student.Classroom = _context.Classrooms.Find (1);
      student.Gender = _context.Genders.Find (1);
      _context.Students.Add (student);
      _context.SaveChanges ();
      // if (ModelState.IsValid) {
      //   var student = studentData.Student;
      //   student.Brithday = DateTime.Now;
      //   student.Classroom = _context.Classrooms.Find (studentData.ClassroomId);
      //   student.Gender = _context.Genders.Find (studentData.GenderId);
      //   _context.Students.Add (student);
      //   _context.SaveChanges ();
      // }

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