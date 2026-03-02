using AutoMapper;
using RepLoopBackend.Application.Features.Workouts.Queries.GetWorkoutById;
using RepLoopBackend.Domain.Entities;

namespace RepLoopBackend.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Workout, WorkoutDto>();
    }
}
