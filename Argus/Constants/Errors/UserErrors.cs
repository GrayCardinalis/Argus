using ErrorOr;

namespace Argus.Constants.Errors
{
    public static class UserErrors
    {
        //Код ошибки будет: "User.Conflict"
        // Заголовок (Title) будет: "User already exists" (на техническом английском)
        public static readonly Error AlreadyExists =
            Error.Conflict(
                code: "User.AlreadyExists",
                description: "User already exists.");

        public static readonly Error NotFound =
            Error.NotFound(
                code: "User.NotFound",
                description: "User not found.");

        public static readonly Error InvalidPassword =
            Error.Validation(
                code: "User.InvalidPassword.",
                description: "Password is invalid.");

        public static readonly Error WrongCurrentPassword =
            Error.Validation(
                code: "User.WrongCurrentPassword",
                description: "The current password is incorrect.");
    }
}
