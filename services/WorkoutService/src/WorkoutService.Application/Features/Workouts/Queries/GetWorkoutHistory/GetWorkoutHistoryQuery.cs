using MediatR;
using WorkoutService.Application.Features.Workouts.Common;

namespace WorkoutService.Application.Features.Workouts.Queries.GetWorkoutHistory;

public record GetWorkoutHistoryQuery(int Page = 1, int PageSize = 10) : IRequest<WorkoutHistoryDto>;

public class WorkoutHistoryDto
{
    public List<WorkoutDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
