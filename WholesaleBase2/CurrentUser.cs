using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleBase2
{
    internal class CurrentUser
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static string FullName { get; set; }
        public static string Role { get; set; }

        public static bool IsAuthenticated => UserID > 0;
        public static bool IsAdmin => Role == "Администратор";
        public static bool IsManager => Role == "Менеджер" || IsAdmin;
        public static bool IsWarehouse => Role == "Кладовщик" || IsManager;

        public static void Clear()
        {
            UserID = 0;
            Username = "";
            FullName = "";
            Role = "";
        }
    }
}

