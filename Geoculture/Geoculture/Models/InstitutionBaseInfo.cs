using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Geoculture.Models
{
    public class InstitutionBaseInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Type { get; set; }
        public string Site { get; set; }
        public string WorkingHours { get; set; }

        public static InstitutionBaseInfo fromSqlDataReader(SqlDataReader reader)
        {
            return new InstitutionBaseInfo
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Address = reader.GetString(2),
                Lat = reader.GetDouble(3),
                Lng = reader.GetDouble(4),
                Email = reader.GetString(5),
                Phone = reader.GetString(6),
                Type = reader.GetInt32(7),
                Site = reader.GetString(8),
                WorkingHours = reader.GetString(9)
            };
            
        }
    }
}