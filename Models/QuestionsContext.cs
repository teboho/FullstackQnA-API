using Microsoft.EntityFrameworkCore;

namespace FullstackQnA_API.Models
{
    public class QuestionsContext : DbContext
    {
        public QuestionsContext(DbContextOptions<QuestionsContext> options)
            : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; } = null!;
    }
}
