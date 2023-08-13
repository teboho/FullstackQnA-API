namespace FullstackQnA_API.Models
{
    public class Question : BaseEntity
    {
        public int QuestionId { get; set; }
        public string? QuestionText { get; set; }
        public string? Quester { get; set; }

        public bool QuestionAnswered { get; set; } = false;
    }

    public class BaseEntity
    {
        public DateTime CreatedDate { get; set; }
    }
}
