using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Kanban
{
    public class BD
    {
        private string connectionBBDD;
        public MySqlConnection connexioSQL {  get; set; }

        public BD() 
        {
            connectionBBDD = ConfigurationManager.ConnectionStrings["MySQLDBConnectionString"].ConnectionString; //Connexió a la base de dades
            connexioSQL = new MySqlConnection(connectionBBDD); //Connexió establerta amb la base de dades MySqlConnection
        }
    }
}
