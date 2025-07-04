using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Database;

namespace ToDoApp.Infrastructure.Database;

public sealed class ToDoDbContext : DbContext, IToDoDbContext
{
}