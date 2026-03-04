using System.Reflection;
using System.Text.Json;
using ExerciseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExerciseService.Persistence.Configurations;

public class ExerciseSeedConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceStream = assembly.GetManifestResourceStream(
            "ExerciseService.Persistence.SeedData.exercises.json");

        if (resourceStream is null)
            throw new InvalidOperationException("Embedded resource 'SeedData/exercises.json' not found.");

        using var reader = new StreamReader(resourceStream);
        var json = reader.ReadToEnd();

        var seedDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc);

        var exercises = JsonSerializer.Deserialize<List<ExerciseSeedDto>>(json)
            ?? throw new InvalidOperationException("Failed to deserialize exercises.json");

        builder.HasData(exercises.Select(e => new Exercise
        {
            Id = Guid.Parse(e.Id),
            Name = e.Name,
            MuscleGroup = e.MuscleGroup,
            Equipment = e.Equipment,
            Difficulty = e.Difficulty,
            IsPublic = true,
            CreatedAt = seedDate
        }));
    }

    private sealed class ExerciseSeedDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string MuscleGroup { get; set; } = default!;
        public string Equipment { get; set; } = default!;
        public string Difficulty { get; set; } = default!;
    }
}