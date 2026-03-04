namespace StatisticsService.Application;

public static class ErrorCodes
{
    // STAT-7xxx: İstatistik hataları
    public const string ExerciseLogNotFound     = "STAT-7001";
    public const string MeasurementNotFound     = "STAT-7002";
    public const string InvalidPeriod           = "STAT-7003";
    public const string MeasurementForbidden   = "STAT-7004";
}
