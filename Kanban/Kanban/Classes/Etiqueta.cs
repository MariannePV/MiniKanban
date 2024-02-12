using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban
{
    public class Etiqueta
    {
        //private static int lastId = 0;
        public int id {  get; set; }
        public string nom { get; set; }
        public string color { get; set; }


        public Etiqueta() 
        {           
            id = ++Dades.idEtiquetaHigher;
        }

        public void AfegirEtiquetaBD() //Funcio que crea una etiqueta a la base de dades, amb les dades que té ens el seus atributs
        {
            string insertQuery = "INSERT INTO `ETIQUETA`(`id_etiqueta`, `nom_etiqueta`, `color_etiqueta`) VALUES (@idValor, @titolValor, @colorValor)";

            using (MySqlCommand cmd = new MySqlCommand(insertQuery, Dades.connexioSQL))
            {
                // Asegúrate de asignar valores a los parámetros adecuados
                cmd.Parameters.AddWithValue("@idValor", id);
                cmd.Parameters.AddWithValue("@titolValor", nom);
                cmd.Parameters.AddWithValue("@colorValor", color);
                // ...

                // Ejecutar la consulta
                cmd.ExecuteNonQuery();
            }
            Dades.EditarLastChangeBD();
        }

        public void EliminarEtiquetaBD() //Funció que elimina de la base de dades, una etiqueta amb la mateixa id que aquest.
        {

            string deleteQuery = "DELETE FROM `ETIQUETA` WHERE `id_etiqueta` = @idValor";

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
