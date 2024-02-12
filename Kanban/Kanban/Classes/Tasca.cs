using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Kanban
{
    public enum Prioritat
    {
        Baixa,
        Mitjana,
        Alta
    }
    public class Tasca
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Responsable responsable { get; set; }
        public DateTime dataCreacio { get; set; }
        public DateTime? dataFinalitzacio { get; set; } //Pot ser null ja que potser no saps la data de finalització
        public List<Etiqueta> etiquetes { get; set; } = new List<Etiqueta>();
        //public string etiqueta { get; set; }
        public int prioritat { get; set; }
        public string ColorFons { get; set; }
        public int TaulerId { get; set; }

        // Constructor que posa la variable dataCreacio amb la data actual
        public Tasca()
        {
            id = ++Dades.idTascaHigher;
            dataCreacio = DateTime.Now;

        }

        public void AfegirTascaBD() //Funcio que crea una tasca a la base de dades, amb les dades que té ens el seus atributs
        {

            string insertQuery = "INSERT INTO `TASCA`(`id_tasca`, `title`, `descripcio`, `data_creacio`, `data_finalitzacio`, `color_tasca`, `id_tauler`, `id_responsable`, `prioritat`) " +
                "VALUES (@idValor, @titleValor, @descripcioValor, @dataCreacioValor, @dataFiValor, @colorValor, @idTaulerValor, @idResponsableValor, @prioritatValor)";

            using (MySqlCommand cmd = new MySqlCommand(insertQuery, Dades.connexioSQL))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@idValor", id);

                }
                catch
                {
                    cmd.Parameters.AddWithValue("@idValor", id + 1);
                    Dades.idTascaHigher++;
                }
                // Asegúrate de asignar valores a los parámetros adecuados
                cmd.Parameters.AddWithValue("@titleValor", title);
                cmd.Parameters.AddWithValue("@descripcioValor", description);
                cmd.Parameters.AddWithValue("@dataCreacioValor", dataCreacio);
                cmd.Parameters.AddWithValue("@dataFiValor", dataFinalitzacio);
                cmd.Parameters.AddWithValue("@colorValor", ColorFons);
                cmd.Parameters.AddWithValue("@idTaulerValor", TaulerId);
                cmd.Parameters.AddWithValue("@idResponsableValor", responsable.Id);
                cmd.Parameters.AddWithValue("@prioritatValor", prioritat);
                // ...
                // Ejecutar la consulta               
                cmd.ExecuteNonQuery();
            }
            AfegirEtiquetesATasca(); //Afegim en la taula N-M de Tasca i Etiquetes les necessaries.
            Dades.EditarLastChangeBD();//Actualitzem també la última modificació
        }

        private void AfegirEtiquetesATasca() 
        {
            foreach (var etiqueta in etiquetes) 
            {
                string insertQuery = "INSERT INTO `TASCA_ETIQUETA`(`id_tasca`, `id_etiqueta`) VALUES (@idTascaValor, @idEtiquetaValor)";

                using (MySqlCommand cmd = new MySqlCommand(insertQuery, Dades.connexioSQL))
                {
                    // Asegúrate de asignar valores a los parámetros adecuados
                    cmd.Parameters.AddWithValue("@idTascaValor", id);
                    cmd.Parameters.AddWithValue("@idEtiquetaValor", etiqueta.id);
                    // ...
                    // Ejecutar la consulta
                    cmd.ExecuteNonQuery();
                }
            }         
        }

        public void EditarTascaBD() //Funcio que edita una tasca a la base de dades, amb el mateix id. Editem les dades amb la informació dels atributs
        {
            string insertQuery = "UPDATE `TASCA` SET `title` = @titleValor, `descripcio` = @descripcioValor, `data_creacio` = @dataCreacioValor, `data_finalitzacio` = @dataFiValor, `color_tasca` = @colorValor, `id_tauler` = @idTaulerValor, `id_responsable` = @idResponsableValor, `prioritat` = @prioritatValor  WHERE `id_tasca` = @idValor";

            using (MySqlCommand cmd = new MySqlCommand(insertQuery, Dades.connexioSQL))
            {
                // Asegúrate de asignar valores a los parámetros adecuados              
               
                cmd.Parameters.AddWithValue("@titleValor", title);
                cmd.Parameters.AddWithValue("@descripcioValor", description);
                cmd.Parameters.AddWithValue("@dataCreacioValor", dataCreacio);
                cmd.Parameters.AddWithValue("@dataFiValor", dataFinalitzacio);
                cmd.Parameters.AddWithValue("@colorValor", ColorFons);
                cmd.Parameters.AddWithValue("@idTaulerValor", TaulerId);
                cmd.Parameters.AddWithValue("@idResponsableValor", responsable.Id);
                cmd.Parameters.AddWithValue("@prioritatValor", prioritat);
                cmd.Parameters.AddWithValue("@idValor", id);
                // ...
                // Ejecutar la consulta
                cmd.ExecuteNonQuery();
            }
            EliminarEtiquetaBD();
            AfegirEtiquetesATasca();
            Dades.EditarLastChangeBD();
        }

        public void EliminarEtiquetaBD()
        {
            string deleteQuery = "DELETE FROM `TASCA_ETIQUETA` WHERE `id_tasca` = @idValor";

            using (MySqlCommand cmd = new MySqlCommand(deleteQuery, Dades.connexioSQL))
            {
                // Asegúrate de asignar el valor al parámetro adecuado
                cmd.Parameters.AddWithValue("@idValor", id);

                // Ejecutar la consulta
                cmd.ExecuteNonQuery();
            }

        }

        public void EliminarTascaBD() //Funció que elimina de la base de dades, una tasca amb la mateixa id que aquest.
        {
            if (Dades.connexioSQL.State == System.Data.ConnectionState.Closed)
            {
                Dades.connexioSQL.Open();
            }

            EliminarEtiquetaBD();
            string deleteQuery = "DELETE FROM `TASCA` WHERE `id_tasca` = @idValor";

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
