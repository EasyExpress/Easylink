using System.Configuration;

namespace EasylinkApp.WPF 
{
    public static class InforCenter
    {

        public static bool IsInTestMode
        {

            get { return bool.Parse(ConfigurationManager.AppSettings["InTestMode"]); }

        }
    }
}
