namespace SessionService.Application;

public static class ErrorCodes
{
    // SES-6xxx: Antrenman oturumu hataları
    public const string SessionNotFound          = "SES-6001";
    public const string SetLogNotAllowed         = "SES-6002";
    public const string PauseNotAllowed          = "SES-6003";
    public const string ResumeNotAllowed         = "SES-6004";
    public const string SessionAlreadyCompleted  = "SES-6005";
}
