using ErrorOr;

namespace Argus.Constants.Errors
{
    public static class UserErrors
    {
        //The error code will be: "User.Conflict"
        // The title will be: "User already exists"

        public static readonly Error NotFound =
            Error.NotFound(
                code: "User.NotFound",
                description: "User not found.");

        public static readonly Error InvalidAuthentication =
            Error.Unauthorized(
                code: "User.InvalidCredentials",
                description: "Invalid username or password.");
        public static readonly Error WrongCurrentPassword =
            Error.Validation(
                code: "User.WrongCurrentPassword",
                description: "The current password is incorrect.");

        public static readonly Error CannotDeleteSelf =
            Error.Validation(
                code: "User.CannotDeleteSelf",
                description: "Administrators cannot delete their own account.");

        public static readonly Error AlreadyExists =
            Error.Conflict(
                code: "User.AlreadyExists",
                description: "User already exists.");
        public static readonly Error DuplicateEmail =
            Error.Conflict(
                code: "User.DuplicateEmail",
                description: "This email is already in use.");
        public static readonly Error DuplicateUserName =
            Error.Conflict(
                code: "User.DuplicateUserName",
                description: "This username is already taken.");

        public static readonly Error Forbidden = 
            Error.Forbidden(
                code: "User.Forbidden",
                description: "You do not have permission to perform this action.");


    }
}
