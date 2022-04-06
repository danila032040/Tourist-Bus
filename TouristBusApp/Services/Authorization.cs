using TouristBusApp.Models;

namespace TouristBusApp.Services
{
    public class Authorization
    {
        private User _signedInUser;

        public void SignIn(string login, string password)
        {
            
        }

        public void SignOut()
        {
            _signedInUser = null;
        }
    }
}