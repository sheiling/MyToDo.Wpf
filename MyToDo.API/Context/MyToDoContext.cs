using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyToDo.API.Context
{
    public class MyToDoContext : DbContext
    {
        public MyToDoContext(DbContextOptions<MyToDoContext> options) : base(options)
        {

        }

        public DbSet<ToDo> ToDos { get; set; }

        public DbSet<Memo> Memos { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
