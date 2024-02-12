using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kanban
{
    /// <summary>
    /// Lógica de interacción para GestionarEtiquetes.xaml
    /// </summary>
    public partial class GestionarEtiquetes : Window
    {

        public GestionarEtiquetes()
        {
            InitializeComponent();
            etiquetasListBox.ItemsSource = Dades.etiquetes;
        }

        private void CrearEtiqueta_Click(object sender, RoutedEventArgs e)
        {
            //Obrim la finestra de crear etiquetes i llavors actualitzem la llista en qüestió
            CrearEtiqueta crearEtiqueta = new CrearEtiqueta();
            crearEtiqueta.ShowDialog();
            etiquetasListBox.Items.Refresh();
        }

        private void EsborrarEtiqueta(object sender, MouseButtonEventArgs e)
        {
            if(etiquetasListBox.SelectedItem != null)
            {
                //Obtenim l'ítem seleccionat
                Etiqueta etiquetaAEliminar = (Etiqueta)etiquetasListBox.SelectedItem;
                bool etiquetaEnUs = Dades.taulers.SelectMany(t => t.Tasques).Any(tas => tas.etiquetes.Any(et => et.nom == etiquetaAEliminar.nom));

                //Comprovem que l'etiqueta no s'estigui fent servir en cap tasca
                if(etiquetaEnUs)
                {
                    MessageBox.Show("No és pot eliminar, ja que la etiqueta està siguent utilitzada almenys per una tasca", "Error d'esborrat", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (etiquetaAEliminar != null)
                    {
                        //Eliminem l'etiqueta de la BDD i de la llista
                        Dades.etiquetes.Remove(etiquetaAEliminar);
                        etiquetaAEliminar.EliminarEtiquetaBD(); //Eliminem la etiqueta de la base de dades
                        //Actualitzem la llista
                        etiquetasListBox.Items.Refresh();
                    }
                }               
            }
            else
            {
                MessageBox.Show("No s'ha seleccionat cap ítem", "Error d'esborrat", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
