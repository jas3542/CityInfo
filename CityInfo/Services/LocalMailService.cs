using System.Diagnostics;

namespace CityInfo.Services
{
    public class LocalMailService : IMailService
    {
        private string mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        private string mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

        public void Send(string subjectt, string messagee)
        {
            Debug.WriteLine($"Mail from {mailFrom} to {mailTo}, with LocalMailService");
            Debug.WriteLine($"Subject: {subjectt}");
            Debug.WriteLine($"Message: {messagee}");
        }
    }
}
