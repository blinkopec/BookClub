using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookClub
{
    public enum AuthorizationType 
    {
        autorization,
        unauthorized,
    }
    public static class UserInfo
    {
        public static AuthorizationType authationType;
        public static int idUser;
    }
}
