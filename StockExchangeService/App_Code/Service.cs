using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;


[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class Service : WebService
{
    private static Random random;
    private static object threadObj = new object();

    public Service()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public int GetStockPrice(string ticker, string user)
    {
        if (IsValidUser(user))
        {
            lock (threadObj)
            {
                if (random == null)
                {
                    random = new Random();
                }

                return random.Next(1, 1001);
            }
        }

        throw new UnauthorizedAccessException();
    }

    private bool IsValidUser(string username)
    {
        using (var db = new ApplicationDbContext())
        {
            return db.Users.Any(u => u.UserName == username);
        }
    }
}