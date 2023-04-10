using Microsoft.EntityFrameworkCore;

namespace FullstackQnA_API.Models
{
    public class AnswersContext : DbContext
    {
        public AnswersContext(DbContextOptions<AnswersContext> options)
            : base(options)
        {
        }

        public DbSet<Answer> Answers { get; set; } = null!;
    }
}
