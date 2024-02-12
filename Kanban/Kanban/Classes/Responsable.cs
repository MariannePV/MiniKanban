using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban
{
    public class Responsable
    {
        
        //Atributs
        private int id;
        private string nomUsuari;
        private string nom;
        private string cognoms;
        private string email;
        private string dni;
        private string password;
        private int nivell;
        private DateTime dataNaix;

        //Constructor
        public Responsable()
        {
            id = ++Dades.idResponsableHigher;
            nomUsuari = "";
            nom = "";
            cognoms = "";
            email = "";
            dni = "";
            password = "";
            nivell = 3;
            dataNaix = DateTime.Now;
        }

        public Responsable(string nomUsuari ,string nom, string cognoms, string email, string dni, string password, int nivell, DateTime dataNaix)
        {
            id = ++Dades.idResponsableHigher;
            this.nomUsuari = nomUsuari;
            this.nom = nom;
            this.cognoms = cognoms;
            this.email = email;
            this.dni = dni;
            this.password = password;
            this.nivell = nivell;
            this.dataNaix = dataNaix;
        }

        //Funcionalitat dels atributs
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string NomUsuari
        {
            get { return nomUsuari; }
            set { nomUsuari = value; }
        }

        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public string Cognoms
        {
            get { return cognoms; }
            set { cognoms = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Dni
        {
            get { return dni; }
            set { dni = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public int Nivell
        {
            get { return nivell; }
            set { nivell = value; }
        }

        public DateTime DataNaix
        {
            get { return dataNaix; }
            set { dataNaix = value; }
        }


        public void AfegirResponsableBD() //Funcio que crea un responsable a la base de dades, amb les dades que té ens el seus atributs
        {
            string insertQuery = "INSERT INTO `USUARI`(`id_responsable`, `nom_usuari`, `nom`, `cognom`, `email`, `data_naixament`, `DNI`, `password`, `nivell`) " +
                "VALUES (@idValor, @nomUsuariValor, @nomValor, @cognomValor, @emailValor, @dataValor, @DNIValor, @passValor, @nivellValor)";

            using (MySqlCommand cmd = new MySqlCommand(insertQuery, Dades.connexioSQL))
            {
                // Asegúrate de asignar valores a los parámetros adecuados
                cmd.Parameters.AddWithValue("@idValor", Id);
                cmd.Parameters.AddWithValue("@nomUsuariValor", NomUsuari);
                cmd.Parameters.AddWithValue("@nomValor", Nom);
                cmd.Parameters.AddWithValue("@cognomValor", Cognoms);
                cmd.Parameters.AddWithValue("@emailValor", Email);
                cmd.Parameters.AddWithValue("@dataValor", DataNaix);
                cmd.Parameters.AddWithValue("@DNIValor", Dni);
                cmd.Parameters.AddWithValue("@passValor", Password);
                cmd.Parameters.AddWithValue("@nivellValor", nivell);
                // ...
                // Ejecutar la consulta
                cmd.ExecuteNonQuery();
            }
            Dades.EditarLastChangeBD();
        }

        public void EditarResponsableBD() //Funcio que edita un responsable a la base de dades, amb el mateix id. Editem les dades amb la informació dels atributs
        {
            string insertQuery = "UPDATE `USUARI` SET `nom_usuari` = @nomUsuariValor, `nom` = @nomValor, `cognom` = @cognomValor, `email` = @emailValor, `data_naixament` = @dataValor, `DNI` = @DNIValor, `password` = @passValor, `nivell` = @nivellValor  WHERE `id_responsable` = @idValor";

            using (MySqlCommand cmd = new MySqlCommand(insertQuery, Dades.connexioSQL))
            {
                // Asegúrate de asignar valores a los parámetros adecuados              
                cmd.Parameters.AddWithValue("@nomUsuariValor", NomUsuari);
                cmd.Parameters.AddWithValue("@nomValor", Nom);
                cmd.Parameters.AddWithValue("@cognomValor", Cognoms);
                cmd.Parameters.AddWithValue("@emailValor", Email);
                cmd.Parameters.AddWithValue("@dataValor", DataNaix);
                cmd.Parameters.AddWithValue("@DNIValor", Dni);
                cmd.Parameters.AddWithValue("@passValor", Password);
                cmd.Parameters.AddWithValue("@nivellValor", nivell);
                cmd.Parameters.AddWithValue("@idValor", Id);
                // ...
                // Ejecutar la consulta
                cmd.ExecuteNonQuery();
            }
            Dades.EditarLastChangeBD();
        }

        public void EliminarResponsableBD() //Funció que elimina de la base de dades, un responsable amb la mateixa id que aquest.
        {
            string deleteQuery = "DELETE FROM `USUARI` WHERE `id_responsable` = @idValor";

            using (MySqlCommand cmd = new MySqlCommand(deleteQuery, Dades.connexioSQL))
            {
                // Asegúrate de asignar el valor al parámetro adecuado
                cmd.Parameters.AddWithValue("@idValor", Id);

                // Ejecutar la consulta
                cmd.ExecuteNonQuery();
            }
            Dades.EditarLastChangeBD();
        }

    }
}
