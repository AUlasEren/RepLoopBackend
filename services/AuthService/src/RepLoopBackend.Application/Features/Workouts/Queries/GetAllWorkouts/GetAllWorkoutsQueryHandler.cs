using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Application.Features.Workouts.Queries.GetWorkoutById;

namespace RepLoopBackend.Application.Features.Workouts.Queries.GetAllWorkouts;

public class GetAllWorkoutsQueryHandler : IRequestHandler<GetAllWorkoutsQuery, List<WorkoutDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllWorkoutsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<WorkoutDto>> Handle(GetAllWorkoutsQuery request, CancellationToken ct)
    {
        var workouts = await _context.Workouts.ToListAsync(ct);
        return _mapper.Map<List<WorkoutDto>>(workouts);
    }
}
