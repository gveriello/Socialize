using System.Reflection;

namespace UnifyMe.Core.Managers
{
    public static class AppManager
    {
        public static string GetAppVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static bool VerifyUpdate()
        {
            return false;
        }
    }
}
