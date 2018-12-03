using Microsoft.EntityFrameworkCore;
using ptyxiaki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Data
{
  public class DepartmentContext : DbContext
  {
    public DepartmentContext(DbContextOptions<DepartmentContext> options) : base(options)
    { }

    public DbSet<Thesis> theses { get; set; }
    public DbSet<Professor> professors { get; set; }
    public DbSet<Student> students { get; set; }
    public DbSet<Announcement> announcements { get; set; }
  }
}
