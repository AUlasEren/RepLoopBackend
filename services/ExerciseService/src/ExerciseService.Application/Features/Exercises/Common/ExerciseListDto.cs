namespace ExerciseService.Application.Features.Exercises.Common;

public class ExerciseListDto
{
    public List<ExerciseDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
