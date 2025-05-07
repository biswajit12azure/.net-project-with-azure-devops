using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Account.Domain.Constants
{
    public static class Constants
    {
        private const string _Authorization = "Authorization";
        private const string _grantType = "grant_type";
        private const string _userName = "UserName";
        private const string _password = "Password";
        private const string _bearer = "Bearer ";
        private const string _mediaType = "application/json";


        private const string _getUserTokenMethod = "api/1/login/auth";


        public static string Authorization = _Authorization;
        public static string grantType = _grantType;
        public static string userName = _userName;
        public static string password = _password;

        public static string getUserTokenMethod = _getUserTokenMethod;
        public static string Bearer = _bearer;
        public static string MediaType = _mediaType;
    }
}
