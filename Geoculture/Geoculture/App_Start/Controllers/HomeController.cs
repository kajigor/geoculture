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
            using (SqlConnection con = new SqlConnection("Data Source=KIRA\\SQLEXPRESS;Initial Catalog=GeoCultureDB;Integrated Security=True"))
            {
                con.Open();
                List<Models.InstitutionBaseInfo> institutions = new List<Models.InstitutionBaseInfo>();
                string[] filter = new string[] { Request.Form["ch1"], Request.Form["ch2"], Request.Form["ch3"], Request.Form["ch4"], Request.Form["ch5"], Request.Form["ch6"], Request.Form["ch7"], Request.Form["ch8"] };
                for (int i = 0; i < 5; i++)
                {
                    if (filter[i] == "true")
                    {
                        if (filter[5] == "true")
                        {
                            if (filter[6] == "true")
                            {
                                if (filter[7] == "true")
                                {
                                    string select = String.Format("SELECT * FROM InstitutionBaseInfo where TypeID = {0} and PhoneNumber != '' and Email != ''and [Site]!= ''", i + 1);
                                    SqlCommand com = new SqlCommand(select, con);
                                    SqlDataReader reader = com.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        Models.InstitutionBaseInfo info = Models.InstitutionBaseInfo.fromSqlDataReader(reader);
                                        institutions.Add(info);
                                    }
                                    reader.Close();
                                }
                                else
                                {
                                    string select = String.Format("SELECT * FROM InstitutionBaseInfo where TypeID = {0} and  PhoneNumber != '' and [Site] != ''", i + 1);
                                    SqlCommand com = new SqlCommand(select, con);
                                    SqlDataReader reader = com.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        Models.InstitutionBaseInfo info = Models.InstitutionBaseInfo.fromSqlDataReader(reader);
                                        institutions.Add(info);
                                    }
                                    reader.Close();
                                }
                            }
                            else
                            {
                                if (filter[7] == "true")
                                {
                                    string select = String.Format("SELECT * FROM InstitutionBaseInfo where TypeID = {0} and PhoneNumber != '' and Email != ''", i + 1);
                                    SqlCommand com = new SqlCommand(select, con);
                                    SqlDataReader reader = com.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        Models.InstitutionBaseInfo info = Models.InstitutionBaseInfo.fromSqlDataReader(reader);
                                        institutions.Add(info);
                                    }
                                    reader.Close();
                                }
                                else
                                {
                                    string select = String.Format("SELECT * FROM InstitutionBaseInfo where TypeID = {0} and PhoneNumber != ''", i + 1);
                                    SqlCommand com = new SqlCommand(select, con);
                                    SqlDataReader reader = com.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        Models.InstitutionBaseInfo info = Models.InstitutionBaseInfo.fromSqlDataReader(reader);
                                        institutions.Add(info);
                                    }
                                    reader.Close();
                                }
                            }
                        }
                        else
                        {
                            if (filter[6] == "true")
                            {
                                if (filter[7] == "true")
                                {
                                    string select = String.Format("SELECT * FROM InstitutionBaseInfo where TypeID = {0} and Email != ''and [Site]!= ''", i + 1);
                                    SqlCommand com = new SqlCommand(select, con);
                                    SqlDataReader reader = com.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        Models.InstitutionBaseInfo info = Models.InstitutionBaseInfo.fromSqlDataReader(reader);
                                        institutions.Add(info);
                                    }
                                    reader.Close();
                                }
                                else
                                {
                                    string select = String.Format("SELECT * FROM InstitutionBaseInfo where TypeID = {0} and  [Site] != ''", i + 1);
                                    SqlCommand com = new SqlCommand(select, con);
                                    SqlDataReader reader = com.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        Models.InstitutionBaseInfo info = Models.InstitutionBaseInfo.fromSqlDataReader(reader);
                                        institutions.Add(info);
                                    }
                                    reader.Close();
                                }
                            }
                            else
                            {
                                if (filter[7] == "true")
                                {
                                    string select = String.Format("SELECT * FROM InstitutionBaseInfo where TypeID = {0} and Email!= ''", i + 1);
                                    SqlCommand com = new SqlCommand(select, con);
                                    SqlDataReader reader = com.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        Models.InstitutionBaseInfo info = Models.InstitutionBaseInfo.fromSqlDataReader(reader);
                                        institutions.Add(info);
                                    }
                                    reader.Close();
                                }
                                else
                                {
                                    string select = String.Format("SELECT * FROM InstitutionBaseInfo where TypeID = {0}", i + 1);
                                    SqlCommand com = new SqlCommand(select, con);
                                    SqlDataReader reader = com.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        Models.InstitutionBaseInfo info = Models.InstitutionBaseInfo.fromSqlDataReader(reader);
                                        institutions.Add(info);
                                    }
                                    reader.Close();
                                }
                            }
                        }

                    }
                }
                return JsonConvert.SerializeObject(institutions);
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
