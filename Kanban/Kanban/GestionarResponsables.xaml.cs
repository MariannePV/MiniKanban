using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kanban
{
    /// <summary>
    /// Lógica de interacción para GestionarResponsables.xaml
    /// </summary>
    public partial class GestionarResponsables : Window
    {
        private string selectedResponsableId;
        //Declarem un llistat de taulers
        public List<Responsable> responsables = new List<Responsable>();

        public GestionarResponsables()
        {
            InitializeComponent();

            //Assignem el contingut del llistat de responsables i el de la ListBox del xaml
            responsables = Dades.responsables;
            llistaResponsables.ItemsSource = responsables;
        }

        private void CrearResponsables(object sender, RoutedEventArgs e)
        {
            //Obrim la finestra de crear responsables
            CrearResponsable resp = new CrearResponsable();
            resp.ShowDialog();

            //Actualitzem el llistat de responsables tant a la base de dades com a la llista del programa
            llistaResponsables.ItemsSource = null;
            llistaResponsables.ItemsSource = responsables;
        }

        public void ActualitzarResponsable(Responsable resp)
        {
            //Comprovem si existeix un responsable amb la id indicada
            Responsable existingResponsable = responsables.FirstOrDefault(r => r.Id == resp.Id);

            if (existingResponsable != null)
            {
                //Actualitzem el responsable en qüestió
                existingResponsable.Nom = resp.Nom;
                existingResponsable.Cognoms = resp.Cognoms;
                existingResponsable.Email = resp.Email;
                existingResponsable.Dni = resp.Dni;
                existingResponsable.DataNaix = resp.DataNaix;
            }
            else
            {
                //En cas que no existeixi cap responsable amb la id, en creem un de nou
                responsables.Add(resp);
            }
        }
        
        private void EditarResponsable(object sender, MouseButtonEventArgs e)
        {
            //Definim quins usuaris es poden o no editar (el responsable amb Id 1 no es pot actualitzar)
            if (llistaResponsables.SelectedItem is Responsable selectedResponsable)
            {
                if (selectedResponsable.Id == 1)
                {
                    System.Windows.MessageBox.Show("Aquest registre no es pot actualitzar", "Error d'esborrat", MessageBoxButton.OK, MessageBoxImage.Error);
                } 
                else
                {
                    CrearResponsable resp = new CrearResponsable(selectedResponsable);
                    resp.ShowDialog();

                    //Actualitzem la llista
                    llistaResponsables.ItemsSource = null;
                    llistaResponsables.ItemsSource = responsables;
                }
            }
        }

        //Per poder accedir al id associat a la llista quan s'ha carregat
        private void txtID(object sender, RoutedEventArgs e)
        {
            TextBlock txtBlock = sender as TextBlock;

            //Accedim a la data generada
            if (txtBlock.DataContext is Responsable responsable)
            {
                //Guardem la ID del responsable
                selectedResponsableId = responsable.Id.ToString();
            }
        }

        private void EsborrarResponsable(object sender, MouseButtonEventArgs e)
        {
            //Mira si hi ha algun item seleccionat
            if (llistaResponsables.SelectedItem != null)
            {
                //Obtenim el responsable de l'item seleccionat
                Responsable responsableToRemove = llistaResponsables.SelectedItem as Responsable;
                bool responsableEnUs = Dades.taulers.SelectMany(t => t.Tasques).Any(tas => tas.responsable == responsableToRemove);

                //En cas de que el responsable estigui assignat a alguna tasca, no es podrà eliminar
                if (responsableEnUs )
                {
                    System.Windows.MessageBox.Show("No és pot eliminar, ja que aquest usuari està siguent utilitzat en almenys en una tasca", "Error d'esborrat", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (responsableToRemove != null)
                    {
                        if (responsableToRemove.Id == 1)
                        {
                            System.Windows.MessageBox.Show("Aquest registre no es pot esborrar", "Error d'esborrat", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            //Eliminem el responsable
                            responsables.Remove(responsableToRemove);

                            responsableToRemove.EliminarResponsableBD();

                            //Actualitzem la llista
                            llistaResponsables.Items.Refresh();
                        }
                    }
                }               
            }
            else
            {
                System.Windows.MessageBox.Show("No s'ha seleccionat cap ítem", "Error d'esborrat", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void llistaSeleccionada_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Comprovem si hi ha algun ítem seleccionat, que el nivell de l'usuari sigui 2 i que l'ítem seleccionat sigui un responsable
            if (llistaResponsables.SelectedItem != null && Dades.usuariActual.Nivell == 2 && llistaResponsables.SelectedItem is Responsable responsableSeleccionat)
            {
                //En cas de que el responsable seleccionat tingui un nivell de 1 o 2, no el podrà editar ni eliminar
                if(responsableSeleccionat.Nivell < 3)
                {
                    EditarUsuari.Visibility = Visibility.Hidden;
                    EliminarUsuari.Visibility = Visibility.Hidden;
                }
                else
                {
                    EditarUsuari.Visibility = Visibility.Visible;
                    EliminarUsuari.Visibility = Visibility.Visible;
                }
            }
            else
            {
                //Si l'usuari no és de nivell 2, no es motraran els botons d'editar i eliminar
                EditarUsuari.Visibility = Visibility.Visible;
                EliminarUsuari.Visibility = Visibility.Visible;
            }
        }


    }
}
