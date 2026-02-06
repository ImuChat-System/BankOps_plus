namespace BankOpsPlus.Helpers;

/// <summary>
/// Helper class for managing user session data
/// </summary>
public static class SessionHelper
{
    private const string UserIdKey = "UserId";
    private const string UserNameKey = "UserName";
    private const string UserRoleKey = "UserRole";

    public static void SetUserId(this ISession session, int userId)
    {
        session.SetInt32(UserIdKey, userId);
    }

    public static int? GetUserId(this ISession session)
    {
        return session.GetInt32(UserIdKey);
    }

    public static void SetUserName(this ISession session, string userName)
    {
        session.SetString(UserNameKey, userName);
    }

    public static string? GetUserName(this ISession session)
    {
        return session.GetString(UserNameKey);
    }

    public static void SetUserRole(this ISession session, string role)
    {
        session.SetString(UserRoleKey, role);
    }

    public static string? GetUserRole(this ISession session)
    {
        return session.GetString(UserRoleKey);
    }

    public static bool IsAuthenticated(this ISession session)
    {
        return session.GetUserId().HasValue;
    }

    public static void ClearUserSession(this ISession session)
    {
        session.Remove(UserIdKey);
        session.Remove(UserNameKey);
        session.Remove(UserRoleKey);
    }
}
