using System.Linq;
using TouristBusApp.Models;
using TouristBusApp.Services.Repositories;

namespace TouristBusApp.Services
{
    public class Authorization
    {
        private readonly IRepository<User> _rep;

        private User _signedInUser;

        public Authorization(IRepository<User> rep)
        {
            _rep = rep;
        }

        public void SignIn(string login, string password)
        {
            _signedInUser = _rep.Read().FirstOrDefault((u) => u.Login == login && u.Password == password);
        }

        public void SignOut()
        {
            _signedInUser = null;
        }

        public User GetSignedInUser()
        {
            return _signedInUser;
        }
    }
}