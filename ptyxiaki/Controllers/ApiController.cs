﻿using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Common;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Controllers
{
  public class ApiController : Controller
  {
    private readonly DepartmentContext context;
    private readonly IMapper mapper;

    public ApiController(DepartmentContext context, IMapper mapper)
    {
      this.context = context;
      this.mapper = mapper;
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
    public async Task<JsonResult> students(DataTablesParameters parameters)
    {
      var queryable = context.students.AsQueryable();

      var recordsTotal = await queryable.CountAsync();

      if (parameters.search != null && !string.IsNullOrEmpty(parameters.search.value))
      {
        var values = parameters.search.value.Split(' ');

        foreach (var value in values)
        {
          queryable = queryable.Where(s => EF.Functions.ILike(s.lastName, $"%{value}%") ||
                                           EF.Functions.ILike(s.firstName, $"%{value}%") ||
                                           EF.Functions.ILike(s.registrationNumber, $"%{value}%"));
        }
      }

      var recordsFiltered = await queryable.CountAsync();

      if (parameters.order != null)
      {
        var orders = parameters.order.Select(o => $"{parameters.columns[o.column].data} {o.dir}").ToList();
        var ordering = string.Join(", ", orders);
        queryable = queryable.OrderBy(ordering);
      }

      if (parameters.start != null)
        queryable = queryable.Skip(parameters.start.Value);

      if (parameters.length != null)
        queryable = queryable.Take(parameters.length.Value);

      var data = await queryable.ToListAsync();

      return Json(new
      {
        parameters.draw,
        recordsTotal,
        recordsFiltered,
        data
      });
    }

    [HttpPost]
    public async Task<JsonResult> theses(DataTablesParameters parameters)
    {
      var queryable = context.theses.AsQueryable();

      if (!User.IsInRole(Globals.PROFESSOR_ROLE))
      {
        queryable = queryable.Where(t => new[] { Status.Available, Status.Active, Status.Completed }.Contains(t.status));
      }

      foreach (var column in parameters.columns)
      {
        if (column.search != null && !string.IsNullOrEmpty(column.search.value))
        {
          queryable = queryable.Where($"{column.data} == @0", column.search.value);
        }
      }

      var recordsTotal = await queryable.CountAsync();

      if (parameters.search != null && !string.IsNullOrEmpty(parameters.search.value))
      {
        var values = parameters.search.value.Split(' ');

        foreach (var value in values)
        {
          queryable = queryable.Where(s => EF.Functions.ILike(s.title, $"%{value}%") ||
                                           EF.Functions.ILike(s.semester.title, $"%{value}%") ||
                                           EF.Functions.ILike(s.professor.lastName, $"%{value}%"));
        }
      }

      var recordsFiltered = await queryable.CountAsync();

      if (parameters.order != null)
      {
        var orders = parameters.order.Select(o => $"{parameters.columns[o.column].data} {o.dir}").ToList();
        var ordering = string.Join(", ", orders);
        queryable = queryable.OrderBy(ordering);
      }

      if (parameters.start != null)
        queryable = queryable.Skip(parameters.start.Value);

      if (parameters.length != null)
        queryable = queryable.Take(parameters.length.Value);

      var data = await queryable
        .Include(t => t.assignments).ThenInclude(a => a.student)
        .Include(t => t.categorizations).ThenInclude(c => c.category)
        .Include(t => t.requirements).ThenInclude(r => r.course)
        .Include(t => t.professor)
        .Include(t => t.semester)
        .ToListAsync();

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
