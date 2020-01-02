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
    public DbSet<Category> categories { get; set; }
    public DbSet<Categorization> categorizations { get; set; }

    public DbSet<Course> courses { get; set; }
    public DbSet<Requirement> requirements { get; set; }
    public DbSet<Grade> grades { get; set; }

    public DbSet<Student> students { get; set; }
    public DbSet<Assignment> assignments { get; set; }
    public DbSet<Declaration> declarations { get; set; }

    public DbSet<Professor> professors { get; set; }

    public DbSet<Semester> semesters { get; set; }
    public DbSet<Date> dates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Student>()
        .HasIndex(s => s.registrationNumber)
        .IsUnique();
    }
  }
}
