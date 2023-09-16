namespace MinimalQuiz.Api.Models;

public class Quiz
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Description { get; set; } = string.Empty;
    public IEnumerable<Answer> Answers { get; set; } = Enumerable.Empty<Answer>();
}
