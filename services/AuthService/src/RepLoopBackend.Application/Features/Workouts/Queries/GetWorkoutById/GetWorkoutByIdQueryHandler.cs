using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RepLoopBackend.Application.Common.Interfaces;

namespace RepLoopBackend.Application.Features.Workouts.Queries.GetWorkoutById;

public class GetWorkoutByIdQueryHandler : IRequestHandler<GetWorkoutByIdQuery, WorkoutDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWorkoutByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkoutDto?> Handle(GetWorkoutByIdQuery request, CancellationToken ct)
    {
        var workout = await _context.Workouts
            .FirstOrDefaultAsync(w => w.Id == request.Id, ct);

        return workout is null ? null : _mapper.Map<WorkoutDto>(workout);
    }
}
