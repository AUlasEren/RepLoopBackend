using FluentValidation;

namespace StatisticsService.Application.Features.Statistics.Commands.UpdateBodyMeasurement;

public class UpdateBodyMeasurementCommandValidator : AbstractValidator<UpdateBodyMeasurementCommand>
{
    public UpdateBodyMeasurementCommandValidator()
    {
        RuleFor(x => x.MeasuredAt).NotEmpty();
        RuleFor(x => x.WeightKg).GreaterThan(0).When(x => x.WeightKg.HasValue);
        RuleFor(x => x.BodyFatPercentage).InclusiveBetween(1, 100).When(x => x.BodyFatPercentage.HasValue);
        RuleFor(x => x.ChestCm).GreaterThan(0).When(x => x.ChestCm.HasValue);
        RuleFor(x => x.WaistCm).GreaterThan(0).When(x => x.WaistCm.HasValue);
        RuleFor(x => x.HipsCm).GreaterThan(0).When(x => x.HipsCm.HasValue);
        RuleFor(x => x.BicepsCm).GreaterThan(0).When(x => x.BicepsCm.HasValue);
        RuleFor(x => x.ThighCm).GreaterThan(0).When(x => x.ThighCm.HasValue);
        RuleFor(x => x.Notes).MaximumLength(500).When(x => x.Notes != null);
    }
}
