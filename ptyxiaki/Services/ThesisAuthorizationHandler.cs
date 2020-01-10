using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ptyxiaki.Common;
using ptyxiaki.Data;
using ptyxiaki.Extensions;
using ptyxiaki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Services
{
  public class ThesisAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Thesis>
  {
    private readonly DepartmentContext departmentContext;

    public ThesisAuthorizationHandler(DepartmentContext departmentContext)
    {
      this.departmentContext = departmentContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   OperationAuthorizationRequirement requirement,
                                                   Thesis resource)
    {
      var user = context.User;
      var userId = user.getUserId();
      var isPostPeriod = departmentContext.dates.isPostPeriod();
      var isDeclarationPeriod = departmentContext.dates.isDeclarationPeriod();
      var isProfessor = user.IsInRole(Globals.PROFESSOR_ROLE);
      var isStudent = user.IsInRole(Globals.STUDENT_ROLE);

      if (user.IsInRole(Globals.ADMINISTRATOR_ROLE) && requirement.Name != nameof(Operations.Declare))
      {
        context.Succeed(requirement);
      }

      switch (requirement.Name)
      {
        case nameof(Operations.Details):
          if (isProfessor || !new[] { Status.Unavailable, Status.Canceled }.Contains(resource.status))
          {
            context.Succeed(requirement);
          }

          break;
        case nameof(Operations.Create):
          if (isProfessor && isPostPeriod)
          {
            context.Succeed(requirement);
          }

          break;
        case nameof(Operations.Copy):
          if (isProfessor && isPostPeriod && userId == resource.professorId && new[] { Status.Unavailable, Status.Canceled }.Contains(resource.status))
          {
            context.Succeed(requirement);
          }

          break;
        case nameof(Operations.Edit):
          if (isProfessor && isPostPeriod && userId == resource.professorId && resource.status < Status.Active)
          {
            context.Succeed(requirement);
          }

          break;
        case nameof(Operations.Delete):
          if (isProfessor && userId == resource.professorId && resource.status < Status.Active)
          {
            context.Succeed(requirement);
          }

          break;
        case nameof(Operations.Cancel):
          if (isProfessor && userId == resource.professorId && resource.status == Status.Active)
          {
            context.Succeed(requirement);
          }

          break;
        case nameof(Operations.Complete):
          if (isProfessor && userId == resource.professorId && resource.status == Status.Active)
          {
            context.Succeed(requirement);
          }

          break;
        case nameof(Operations.TransferToAvailable):
          if (isProfessor && userId == resource.professorId && resource.status == Status.Unavailable)
          {
            context.Succeed(requirement);
          }

          break;
        case nameof(Operations.TransferToUnavailable):
          if (isProfessor && userId == resource.professorId && resource.status == Status.Available)
          {
            context.Succeed(requirement);
          }

          break;

        case nameof(Operations.Declare):
          if (isStudent && isDeclarationPeriod && resource.status == Status.Available &&
              departmentContext.students.meetsRequirements(userId))
          {
            context.Succeed(requirement);
          }

          break;
      }

      return Task.CompletedTask;
    }
  }

  public static class Operations
  {
    public static OperationAuthorizationRequirement Details = new OperationAuthorizationRequirement { Name = nameof(Details) };
    public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = nameof(Create) };
    public static OperationAuthorizationRequirement Copy = new OperationAuthorizationRequirement { Name = nameof(Copy) };
    public static OperationAuthorizationRequirement Edit = new OperationAuthorizationRequirement { Name = nameof(Edit) };
    public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = nameof(Delete) };
    public static OperationAuthorizationRequirement Cancel = new OperationAuthorizationRequirement { Name = nameof(Cancel) };
    public static OperationAuthorizationRequirement Complete = new OperationAuthorizationRequirement { Name = nameof(Complete) };
    public static OperationAuthorizationRequirement TransferToAvailable = new OperationAuthorizationRequirement { Name = nameof(TransferToAvailable) };
    public static OperationAuthorizationRequirement TransferToUnavailable = new OperationAuthorizationRequirement { Name = nameof(TransferToUnavailable) };
    public static OperationAuthorizationRequirement Declare = new OperationAuthorizationRequirement { Name = nameof(Declare) };
  }
}
