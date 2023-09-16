using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using MinimalQuiz.Api.Models;
using TResults = Microsoft.AspNetCore.Http.TypedResults;

namespace MinimalQuiz.Api.UseCases;

public class QuizEndpoints : ICarterModule
{
    private static readonly ICollection<Quiz> _quizzes = new List<Quiz>
    {
        new Quiz
        {
            Description = "Quiz 1",
            Answers = new List<Answer>
            {
                new Answer
                {
                    Content = "Answer 1",
                },
                new Answer
                {
                    Content = "Answer 2",
                    IsCorrect = true,
                },
                new Answer
                {
                    Content = "Answer 3",
                },
                new Answer
                {
                    Content = "Answer 4",
                }
            },
        },
        new Quiz
        {
            Description = "Quiz 2",
            Answers = new List<Answer>
            {
                new Answer
                {
                    Content = "Answer 1",
                },
                new Answer
                {
                    Content = "Answer 2",
                    IsCorrect = true,
                },
                new Answer
                {
                    Content = "Answer 3",
                },
                new Answer
                {
                    Content = "Answer 4",
                }
            },
        },
        new Quiz
        {
            Description = "Quiz 3",
            Answers = new List<Answer>
            {
                new Answer
                {
                    Content = "Answer 1",
                },
                new Answer
                {
                    Content = "Answer 2",
                    IsCorrect = true,
                },
                new Answer
                {
                    Content = "Answer 3",
                },
                new Answer
                {
                    Content = "Answer 4",
                }
            },
        },
        new Quiz
        {
            Description = "Quiz 4",
            Answers = new List<Answer>
            {
                new Answer
                {
                    Content = "Answer 1",
                },
                new Answer
                {
                    Content = "Answer 2",
                    IsCorrect = true,
                },
                new Answer
                {
                    Content = "Answer 3",
                },
                new Answer
                {
                    Content = "Answer 4",
                }
            },
        }
    };
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("api/quizzes")
            .WithTags("Quizzes");

        var id = "{id}";

        group.MapGet(string.Empty, Get)
            .WithOpenApi(x =>
            {
                x.Summary = "Get Quizzes";
                return x;
            });
        group.MapGet(id, GetById)
            .WithOpenApi(x =>
            {
                x.Summary = "Get Quiz by Id";
                return x;
            });
        group.MapPost(string.Empty, Post)
            .WithOpenApi(x =>
            {
                x.Summary = "Create Quiz";
                return x;
            });
        group.MapPut(id, Put)
            .WithOpenApi(x =>
            {
                x.Summary = "Update Quiz";
                return x;
            });
        group.MapPatch(id, Patch)
            .WithOpenApi(x =>
            {
                x.Summary = "Update a part of Quiz";
                return x;
            });
        group.MapDelete(id, Delete)
            .WithOpenApi(x =>
            {
                x.Summary = "Delete Quiz";
                return x;
            }); ;

        group.MapGet("random", GetRandom)
            .WithOpenApi(x =>
            {
                x.Summary = "Get random list of Quizzes";
                return x;
            });
    }
    public static Ok<IEnumerable<Quiz>> GetRandom() => TResults.Ok(GetQuizzes());
    private static IEnumerable<Quiz> GetQuizzes()
    {
        // Shuffle the quizzes using Fisher-Yates algorithm
        List<Quiz> shuffledQuizzes = _quizzes
            .OrderBy(x => new Random().Next())
            .ToList();

        foreach (var quiz in shuffledQuizzes)
        {
            yield return quiz;
        }
    }
    public static Results<Ok<ICollection<Quiz>>, BadRequest> Get()
        => !_quizzes.Any() switch
        {
            true => TResults.BadRequest(),
            false => TResults.Ok(_quizzes)
        };
    public static Results<Ok<Quiz>, NotFound> GetById(Guid id)
        => _quizzes
            .SingleOrDefault(x => x.Id == id) switch
        {
            null => TResults.NotFound(),
            var quiz => TResults.Ok(quiz)
        };
    public static Results<Ok<Quiz>, BadRequest> Post(Quiz quiz)
    {
        if (_quizzes.Any(x => x.Id == quiz.Id))
            return TResults.BadRequest();
        _quizzes.Add(quiz);
        return TResults.Ok(quiz);
    }
    public static Results<NoContent, NotFound> Put(Guid id, Quiz quiz)
    {
        var currentQuiz = _quizzes
            .SingleOrDefault(x => x.Id == id);
        if (currentQuiz == null)
            return TResults.NotFound();
        currentQuiz.Description = quiz.Description ?? currentQuiz.Description;
        return TResults.NoContent();
    }
    public static Results<NoContent, NotFound, BadRequest> Patch(Guid id, JsonPatchDocument<Quiz> patchDoc)
    {
        if (patchDoc == null)
            return TResults.BadRequest();
        var quiz = _quizzes.SingleOrDefault(x => x.Id == id);
        if (quiz == null)
            return TResults.NotFound();
        patchDoc.ApplyTo(quiz);
        return TResults.NoContent();
    }
    public static Results<Ok<Quiz>, NotFound> Delete(Guid id)
    {
        var quiz = _quizzes.SingleOrDefault(x => x.Id == id);
        if (quiz == null)
            return TResults.NotFound();
        _quizzes.Remove(quiz);
        return TResults.Ok(quiz);
    }
}
