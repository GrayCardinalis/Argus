namespace Argus.Constants.Security
{
    public static class AuthConstants
    {
        //ps. In the future, add generation at startup
        public const int WorkFactor = 11;
        public const string DummyHash = "$2a$11$/zHm54QGqKBvusoTbr6CDOxC./O5vZbAKNCwYXwXR66ir51/ndPKG";

        // Password requirements (see DECISIONS.md, section “Passwords and Login”).
        // Minimum — in characters: NIST SP 800-63B-4 requires 15 if the password is the only login factor.
        // Maximum — in UTF-8 bytes: bcrypt uses only the first 72 bytes and silently discards the rest.
        public const int PasswordMinLength = 15;
        public const int PasswordMaxBytes = 72;

    }
}
