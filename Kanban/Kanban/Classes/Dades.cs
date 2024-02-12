using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Media;
using System.Security.Cryptography;
using System.Windows;
using System.Data.SqlClient;
using System.Drawing;

namespace Kanban
{
    public class Dades
    {
        BD con = new BD(); //Crida a la connexió de la base de dades
        public static MySqlConnection connexioSQL;

        public static List<Tauler> taulers { get; set; }
        public static  List<Etiqueta> etiquetes { get; set; }
        public static  List<Responsable> responsables { get; set; }   //Totes les llistes on guardem els nostres objectes de les classes per tenir la informació 
        public static List<Tasca> tasques { get; set; }
        public static List<int[]> tasquesEtiquetes { get; set; } //En aquest cas no necessitem una classe, amb un array de 2 nombres per simular clau valor en serveix
        public static DateTime lastChange { get; set; } //Data de la darrera modifiació feta

        public static Responsable usuariActual; //Responsable actual. Es a dir l'usuari conectat

        public static int idTaulerHigher = 0;
        public static int idEtiquetaHigher = 0; //Guardaràn la id més alta de cada taula de la base de dades
        public static int idResponsableHigher = 0;
        public static int idTascaHigher = 0;

        public Dades() //En el constructor, emplenem les llistes amb tota la informació de la base de dades
        {
            connexioSQL = con.connexioSQL;
         
            connexioSQL.Open();

            taulers = ObtenirTaulers();

            etiquetes = ObtenirEtiquetes();

            responsables = ObtenirResponsables();
           
            tasquesEtiquetes = ObtenirTasquesEtiquetes();

            tasques = ObtenirTasques();

            lastChange = ObtenirLastChange();

        }


        public static List<Tauler> ObtenirTaulers() //Obtenim la informació dels taulers de la base de dades
        {
            List<Tauler> llistaTaulers = new List<Tauler>();
             
             // Reemplaza con tu cadena de conexión
            string query = "SELECT * FROM TAULER";

            using (MySqlCommand command = new MySqlCommand(query, connexioSQL))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tauler tauler = new Tauler
                        {
                            id = Convert.ToInt32(reader["id_tauler"]),
                            titol = reader["nom_tauler"].ToString(),
                            color = reader["color_tauler"].ToString()
                        };

                        llistaTaulers.Add(tauler);

                        if (tauler.id > idTaulerHigher)
                        {
                            idTaulerHigher = tauler.id;
                        }
                    }
                }
            }
            return llistaTaulers;
        }

        public static List<Etiqueta> ObtenirEtiquetes() //Obtenim la informació de les etiquetes de la base de dades
        {
            List<Etiqueta> llistaEtiquetes = new List<Etiqueta>();
            string query = "SELECT * FROM ETIQUETA";
            
            using (MySqlCommand command = new MySqlCommand(query, connexioSQL))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Etiqueta etiqueta = new Etiqueta
                        {
                            id = Convert.ToInt32(reader["id_etiqueta"]),
                            nom = reader["nom_etiqueta"].ToString(),
                            color = reader["color_etiqueta"].ToString()
                        };

                        llistaEtiquetes.Add(etiqueta);

                        if (etiqueta.id > idEtiquetaHigher)
                        {
                            idEtiquetaHigher = etiqueta.id;
                        }
                    }
                }
            }
            return llistaEtiquetes;

        }

        public static List<Responsable> ObtenirResponsables() //Obtenim la informació dels responsables de la base de dades
        {
            List<Responsable> llistaResponsables = new List<Responsable>();

            // Reemplaza con tu cadena de conexión
            string query = "SELECT * FROM USUARI";

            using (MySqlCommand command = new MySqlCommand(query, connexioSQL))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Responsable responsable = new Responsable
                        {
                            Id = Convert.ToInt32(reader["id_responsable"]),
                            NomUsuari = reader["nom_usuari"].ToString(),
                            Nom = reader["nom"].ToString(),
                            Cognoms = reader["cognom"].ToString(),
                            Email = reader["email"].ToString(),
                            DataNaix = Convert.ToDateTime(reader["data_naixament"]),
                            Dni = reader["DNI"].ToString(),
                            Nivell = Convert.ToInt32(reader["nivell"]),
                            Password = reader["password"].ToString()
                            
                        };
                        llistaResponsables.Add(responsable);

                        if (responsable.Id > idResponsableHigher)
                        {
                            idResponsableHigher = responsable.Id;
                        }
                    }
                }
            }



            return llistaResponsables;
        }

        public static List<int[]> ObtenirTasquesEtiquetes() //Obtenim la informació de la referencia entre les etiquetes i tasques de la base de dades
        {
            List<int[]> llistatasquesEtiquetes = new List<int[]>();

            // Reemplaza con tu cadena de conexión
            string query = "SELECT * FROM TASCA_ETIQUETA";
            using (MySqlCommand command = new MySqlCommand(query, connexioSQL))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int[] tasqueEtiqueta = new int[2];
                        tasqueEtiqueta[0] = Convert.ToInt32(reader["id_tasca"]);
                        tasqueEtiqueta[1] = Convert.ToInt32(reader["id_etiqueta"]);

                        llistatasquesEtiquetes.Add(tasqueEtiqueta);
                    }
                }
            }
            return llistatasquesEtiquetes;
        }


        public static List<Tasca> ObtenirTasques() //Obtenim la informació de les tasques de la base de dades
        {
            List<Tasca> llistaTasques = new List<Tasca>();

           // Reemplaza con tu cadena de conexión
            string query = "SELECT * FROM TASCA";

            using (MySqlCommand command = new MySqlCommand(query, connexioSQL))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tasca tasca = new Tasca
                        {
                            id = Convert.ToInt32(reader["id_tasca"]),
                            title = reader["title"].ToString(),
                            description = reader["descripcio"].ToString(),
                            dataCreacio = reader["data_creacio"] is DBNull ? default(DateTime) : Convert.ToDateTime(reader["data_creacio"]),
                            dataFinalitzacio = reader["data_finalitzacio"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["data_finalitzacio"]),
                            ColorFons = reader["color_tasca"] is DBNull ? null : reader["color_tasca"].ToString(),
                            TaulerId = reader["id_tauler"] is DBNull ? 0 : Convert.ToInt32(reader["id_tauler"]),
                            prioritat = reader["prioritat"] is DBNull ? 0 : Convert.ToInt32(reader["prioritat"]),

                        };

                        int id_responsable = Convert.ToInt32(reader["id_responsable"]);
                        tasca.responsable = AfegirResponsable(tasca, id_responsable);                       
                        tasca.etiquetes = AfegirEtiquetaTasca(tasca);
                        llistaTasques.Add(tasca);

                        if (tasca.id > idTascaHigher)
                        {
                            idTascaHigher = tasca.id;
                        }
                    }
                }
            }
            return llistaTasques;
        }

        private static Responsable AfegirResponsable(Tasca tasca, int idResponsable)
        {
            Responsable responsableTasca = responsables.FirstOrDefault(res => res.Id == idResponsable);
            return responsableTasca;
        }
      
        private static List<Etiqueta> AfegirEtiquetaTasca(Tasca tasca) //Funció per afegir les etiquetes en la tasca, que està associada en la taula TASCA_ETIQETA. Retornem una llista de les etiquetes associades a una tasca pasada per parametres
        {
            List<Etiqueta> llistaEtiquetes = new List<Etiqueta>();

            foreach (var tascaEtiqueta in tasquesEtiquetes) 
            {
                if (tascaEtiqueta[0] == tasca.id) //Si hi ha una tasca amb la mateixa id que la tascaEtiqueta, implica que aquesta tasca te associada aquella etiqueta
                {
                    Etiqueta etiqueta = etiquetes.FirstOrDefault(et => et.id == tascaEtiqueta[1]); //Mirem quina etiqueta te
                    llistaEtiquetes.Add(etiqueta);
                }
            }
            return llistaEtiquetes;
        }

        public static DateTime ObtenirLastChange() //Obtenim la informació de la darrera modificació de la base de dades
        {
            //if (connexioSQL.State == System.Data.ConnectionState.Closed)
            //{
            //    connexioSQL.Open();
            //}

            DateTime ultimCanvi = DateTime.Now;

            // Reemplaza con tu cadena de conexión
            string query = "SELECT * FROM LAST_CHANGE";

            using (MySqlCommand command = new MySqlCommand(query, connexioSQL))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ultimCanvi = Convert.ToDateTime(reader["last_change"]);                                        
                    }
                }
                
            }
            return ultimCanvi;

        }

        public static void EditarLastChangeBD() //Editem la base de dades i la variable amb la data actual
        {
            DateTime dataActualMili = DateTime.Now;
            DateTime dataActual = new DateTime(dataActualMili.Year, dataActualMili.Month, dataActualMili.Day, dataActualMili.Hour, dataActualMili.Minute, dataActualMili.Second);
            string updateQuery = "UPDATE `LAST_CHANGE` SET `last_change`= @dataActualValor";

            using (MySqlCommand cmd = new MySqlCommand(updateQuery, Dades.connexioSQL))
            {
                // Asegúrate de asignar valores a los parámetros adecuados
                cmd.Parameters.AddWithValue("@dataActualValor", dataActual);

                // Ejecutar la consulta
                cmd.ExecuteNonQuery();
            }
            lastChange = dataActual;
        }

        public static void ActualitarDades() //Funció per tornar a carregar tota la informació de la base de dades en el programa, i aixì actualitzar-lo
        {
            taulers = ObtenirTaulers();

            etiquetes = ObtenirEtiquetes();

            responsables = ObtenirResponsables();

            tasquesEtiquetes = ObtenirTasquesEtiquetes();

            tasques = ObtenirTasques();

            lastChange = ObtenirLastChange();

        }

        public static string GenerarHashSHA256(string text) //Funció que l'hi pases per paràmetres un string i retorna un hash 
        {
            using (SHA256 sha256Hash = SHA256.Create()) //Crearem un hash amb l'algoritme SHA256
            {
                // Convertim la cadena de entrada a un array de bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                // Construïm la cadena en format string partint amb l'array de bytes
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringBuilder.Append(bytes[i].ToString("x2")); 
                }

                return stringBuilder.ToString();
            }
        }

    }
}
