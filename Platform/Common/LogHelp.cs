using log4net;

namespace Platform.Commonn
{
    public class LogHelp
    {
        public static void WriteLog(string txt)
        {
            ILog log = LogManager.GetLogger("WebLogger");
            log.Error(txt);
        }
    }
    
}