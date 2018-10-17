using Microsoft.EntityFrameworkCore;

namespace Todo.api.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions options) : base(options)
        {
        }
    }
}