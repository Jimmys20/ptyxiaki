using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Common;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Controllers
{
  [Route("[controller]/[action]")]
  public class ApiController : Controller
  {
    private readonly DepartmentContext context;

    public ApiController(DepartmentContext context)
    {
      this.context = context;
    }

    public class DataTablesParameters
    {
      public int? draw { get; set; }
      public int? start { get; set; }
      public int? length { get; set; }
      public Search search { get; set; }
      public Order[] order { get; set; }
      public Column[] columns { get; set; }

      public class Search
      {
        public string value { get; set; }
        public bool regex { get; set; }
      }

      public class Order
      {
        public int column { get; set; }
        public string dir { get; set; }
      }

      public class Column
      {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public Search search { get; set; }
      }
    }

    [HttpPost]
    [Authorize(Policy = Globals.PROFESSOR_POLICY)]
    public JsonResult Students(DataTablesParameters parameters)
    {
      var data = context.students.AsQueryable();

      var recordsTotal = data.Count();

      if (parameters.search != null && !string.IsNullOrEmpty(parameters.search.value))
        data = data.Where(s => s.lastName.ToUpper().Contains(parameters.search.value.ToUpper()) ||
                               s.firstName.ToUpper().Contains(parameters.search.value.ToUpper()) ||
                               s.registrationNumber.ToString().Contains(parameters.search.value));

      var recordsFiltered = data.Count();

      if (parameters.order != null)
      {
        var name = parameters.columns[parameters.order[0].column].data;

        if (parameters.order[0].dir == "asc")
        {
          data = data.OrderBy(s => s.GetType().GetProperty(name).GetValue(s, null));
        }
        else
        {
          data = data.OrderByDescending(s => s.GetType().GetProperty(name).GetValue(s, null));
        }
      }

      if (parameters.start != null)
        data = data.Skip(parameters.start.Value);

      if (parameters.length != null)
        data = data.Take(parameters.length.Value);

      data.ToList();

      return Json(new
      {
        parameters.draw,
        recordsTotal,
        recordsFiltered,
        data
      });
    }
  }
}
