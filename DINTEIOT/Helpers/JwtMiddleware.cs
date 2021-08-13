using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Helpers
{
    public class JwtMiddleware
    {
        //public interface IJWTAuthenticationManager
        //{
        //    string Authenticate(string username, string password);
        //}

        //public class JWTAuthenticationManager : IJWTAuthenticationManager
        //{
        //    IDictionary<string, string> users = new Dictionary<string, string>
        //{
        //    { "test1", "password1" },
        //    { "test2", "password2" }
        //};

        //    private readonly string tokenKey;

        //    public JWTAuthenticationManager(string tokenKey)
        //    {
        //        this.tokenKey = tokenKey;
        //    }

        //    //public string Authenticate(string username, string password)
        //    //{
        //    //    if (!users.Any(u => u.Key == username && u.Value == password))
        //    //    {
        //    //        return null;
        //    //    }

        //    //    //return token.ToString();
        //    //}
        //}
    }
}