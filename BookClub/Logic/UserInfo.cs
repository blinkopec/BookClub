using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookClub.Logic
{
    /// <summary>
    /// Класс нужный для сохранения данных пользователя
    /// </summary>
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
