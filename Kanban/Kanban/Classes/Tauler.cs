using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kanban
{
    public class Tauler
    {
        
        //Atributs
        
        public int id { get; set; }
        public string titol { get; set; }
        public string color { get; set; }
        public List<Tasca> Tasques { get; set; } = new List<Tasca>();

        //Constructors
        public Tauler()
        {
            this.id = ++Dades.idTaulerHigher;
            titol = "Tauler";
            color = "Magenta";
        }

        public Tauler(int id, string titol, string color)
        {            
            this.id = ++Dades.idTaulerHigher;
            this.titol = titol;
            this.color = color;
        }


        public void AfegirTaulerBD() //Funcio que crea un tauler a la base de dades, amb les dades que té ens el seus atributs
        {
            string insertQuery = "INSERT INTO `TAULER`(`id_tauler`, `nom_tauler`, `color_tauler`) VALUES (@idValor, @titolValor, @colorValor)";

            using (MySqlCommand cmd = new MySqlCommand(insertQuery, Dades.connexioSQL))
            {
                // Asegúrate de asignar valores a los parámetros adecuados
                cmd.Parameters.AddWithValue("@idValor", id);
                cmd.Parameters.AddWithValue("@titolValor", titol);
                cmd.Parameters.AddWithValue("@colorValor", color);
                // ...

                // Ejecutar la consulta
                cmd.ExecuteNonQuery();
            }
            Dades.EditarLastChangeBD(); //Actualitzem també la última modificació
        }

        public void EditarTaulerBD() //Funcio que edita un tauler a la base de dades, amb el mateix id. Editem les dades amb la informació dels atributs
        {
            string updateQuery = "UPDATE `TAULER` SET `nom_tauler` = @titolValor, `color_tauler` = @colorValor WHERE `id_tauler` = @idValor";

            using (MySqlCommand cmd = new MySqlCommand(updateQuery, Dades.connexioSQL))
            {
                // Asegúrate de asignar valores a los parámetros adecuados
                cmd.Parameters.AddWithValue("@titolValor", titol);
                cmd.Parameters.AddWithValue("@colorValor", color);
                cmd.Parameters.AddWithValue("@idValor", id);

                // Ejecutar la consulta
                cmd.ExecuteNonQuery();
            }
            Dades.EditarLastChangeBD();
        }

        public void EliminarTaulerBD() //Funció que elimina de la base de dades, un tauler amb la mateixa id que aquest.
        {
            string deleteQuery = "DELETE FROM `TAULER` WHERE `id_tauler` = @idValor";

            using (MySqlCommand cmd = new MySqlCommand(deleteQuery, Dades.connexioSQL))
            {
                // Asegúrate de asignar el valor al parámetro adecuado
                cmd.Parameters.AddWithValue("@idValor", id);

                // Ejecutar la consulta
                cmd.ExecuteNonQuery();
            }
            Dades.EditarLastChangeBD();
        }
      
    }
}
