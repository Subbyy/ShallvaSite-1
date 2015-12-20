using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Utils
{
    public class ShallvaUser
    {
        private static ShallvaUser _current;

        private const string CURRENT_USER = "CurrenUser";

        private string _name;
        private int _id;
        
        public string Name
        {
            get { return _name; }
        }

        public int Id
        {
            get { return _id; }
        }

        private ShallvaUser(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public static void SetCurrentUser(int id, string name)
        {
            _current = new ShallvaUser(id, name);
            HttpContext.Current.Session[CURRENT_USER] = _current;
        }

        public static void LogOut()
        {
            HttpContext.Current.Session[CURRENT_USER] = null;
        }

        public static ShallvaUser Current
        {
            get
            {
                if (HttpContext.Current.Session[CURRENT_USER] == null)
                {
                    return DataProvider.GetCurrentUser(HttpContext.Current.User.Identity.Name);
                }
                else
                {
                    return (ShallvaUser)HttpContext.Current.Session[CURRENT_USER];
                }
            }
        }

    }
}