using ErrorOr;

namespace Argus.Constants.Errors
{
    public static class UserErrors
    {
        //The error code will be: "User.Conflict"
        // The title will be: "User already exists"

        //status 409
        public static readonly Error AlreadyExists =
            Error.Conflict(
                code: "User.AlreadyExists",
                description: "User already exists.");

        //status 404
        public static readonly Error NotFound =
            Error.NotFound(
                code: "User.NotFound",
                description: "User not found.");

        //status 400
        public static readonly Error InvalidPassword =
            Error.Validation(
                code: "User.InvalidPassword",
                description: "Password is invalid.");

        //status 400
        public static readonly Error InvalidAuthentication =
            Error.Unauthorized(
                code: "User.InvalidCredentials",
                description: "Invalid username or password.");

        //status 400
        public static readonly Error WrongCurrentPassword =
            Error.Validation(
                code: "User.WrongCurrentPassword",
                description: "The current password is incorrect.");

        //status 409
        public static readonly Error DuplicateEmail =
            Error.Conflict(
                code: "User.DuplicateEmail",
                description: "This email is already in use.");

        //status 409
        public static readonly Error DuplicateUserName =
            Error.Conflict(
                code: "User.DuplicateUserName",
                description: "This username is already taken.");

        //status 400
        public static readonly Error CannotDeleteSelf =
            Error.Validation(
                code: "User.CannotDeleteSelf",
                description: "Administrators cannot delete their own account.");

        //status 403
        public static readonly Error Forbidden = 
            Error.Forbidden(
                code: "User.Forbidden",
                description: "You do not have permission to perform this action.");
    }
}
