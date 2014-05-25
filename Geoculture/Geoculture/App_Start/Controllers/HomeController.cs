using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Geoculture.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public string RandomNumber()
        {
            try
            {
                int a = int.Parse(Request.Form["a"]);
                int b = int.Parse(Request.Form["b"]);
                int r = new Random().Next(a, b);
                return r.ToString();
            }
            catch (Exception e)
            {
                return "Bad-bad-bad request!";
            }
        }

        public string Institutions()
        {
            // TODO: Make it right
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["geoculture"].ConnectionString))
            {
                SqlCommand com = new SqlCommand("SELECT * FROM InstitutionBaseInfo", con);
                List<Models.InstitutionBaseInfo> institutions = new List<Models.InstitutionBaseInfo>();
                try
                {
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Models.InstitutionBaseInfo info = Models.InstitutionBaseInfo.fromSqlDataReader(reader);
                        institutions.Add(info);
                    }
                    return JsonConvert.SerializeObject(institutions);
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                finally
                {
                    con.Close();
                }
                
            }
        }

        public string Wiki()
        {
            try
            {
                string title = Request.Form["title"];
                string url = "http://ru.wikipedia.org/w/api.php?format=xml&action=parse&page=" + title + "&prop=text&section=0";
                var request = WebRequest.Create(url);
                var response = request.GetResponse();
                XmlReader reader = XmlReader.Create(response.GetResponseStream());
                reader.ReadToFollowing("text");
                string result = reader.ReadElementContentAsString();
                reader.Close();
                return result;
            }
            catch (Exception e)
            {
                return "Ошибка! Что-то пошло не так";
            }
        }
    }
}
