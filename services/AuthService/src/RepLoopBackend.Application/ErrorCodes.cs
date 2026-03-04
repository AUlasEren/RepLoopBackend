namespace RepLoopBackend.Application;

public static class ErrorCodes
{
    // AUTH-1xxx: Kimlik doğrulama hataları
    public const string LoginFailed             = "AUTH-1001";
    public const string RegistrationFailed      = "AUTH-1002";
    public const string InvalidGoogleToken      = "AUTH-1003";
    public const string GoogleAuthFailed        = "AUTH-1004";
    public const string InvalidAppleToken       = "AUTH-1005";
    public const string AppleAuthFailed         = "AUTH-1006";
    public const string ChangePasswordFailed    = "AUTH-1007";
    public const string ResetPasswordFailed     = "AUTH-1008";
    public const string InvalidRefreshToken     = "AUTH-1009";
}
