using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para CrearTauler.xaml
    /// </summary>
    public partial class CrearTauler : Window
    {

        public List<Tauler> taulers;
        string colorString;
        ColorControl colorControl = new ColorControl();

        public CrearTauler(Tauler taulerExistent = null)
        {
            InitializeComponent();
            taulers = Dades.taulers;
            //No mostrem el botó d'eliminar
            btnEliminar.Visibility = Visibility.Collapsed;

            //Si hi ha un tauler existent, emplenem els valors amb els corresponents
            if (taulerExistent != null)
            {
                txtTitol.Text = taulerExistent.titol;

                //Actualitzar el color
                if (taulerExistent.color != null && taulerExistent.color != "")
                {
                    ColorRectangle.Fill = colorControl.ConvertirStringAColorBrush(taulerExistent.color);
                }
                
                colorString = taulerExistent.color;

                idTauler.Text = taulerExistent.id.ToString();

                //Fem visible el botó d'eliminar
                btnEliminar.Visibility = Visibility.Visible;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) //Event del propi Windows que és crida quant mentenim el mouse en la pestanya de windows
        {
            this.DragMove(); //Mètode per poder moure la finestra
        }

        //Selecció del color
        private void SeleccionarColor_Click(object sender, RoutedEventArgs e)
        {           
            Color colorSeleccionat = colorControl.SeleccionarColorFinestra(); //Funció que obra una finestra per seleccionar un color
            ColorRectangle.Fill = new SolidColorBrush(colorSeleccionat); //Emplenem el rectangle amb el color seleccionat, a de ser tipus SolidColorBrush 
            colorString = colorControl.ConvertirColorAString(colorSeleccionat); // Transformem el color en format string amb el mètode ConvertirColorAString()
        }

        private void CerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DesarTauler(object sender, RoutedEventArgs e)
        {
            string titol = txtTitol.Text;
            string color = colorString;
            int id = 0;

            //En cas de que el tauler sí existeixi, li assignem la id que toca
            if (idTauler.Text != "")
            {
                id = int.Parse(idTauler.Text);
            }

            //Afegim un nou tauler mitjançant el mètode del mainWindow
            ActualitzarTauler(id, titol, color);

            Close();
        }

        private void EliminarTauler(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(idTauler.Text);

            //Eliminem el tauler mitjançant el mètode del mainWindow
            EliminarTauler(id);

            Close();
        }

        public void ActualitzarTauler(int id, string nouTitol, string color)
        {
            //Busquem un tauler existent amb la mateixa Id
            Tauler taulerExistent = taulers.FirstOrDefault(t => t.id == id);

            if (taulerExistent != null)
            {
                //Actualitzem el tauler existent
                taulerExistent.titol = nouTitol;
                taulerExistent.color = color;
                taulerExistent.EditarTaulerBD(); //Actualitzem el tauler a la base de dades
            }
            else
            {
                if (nouTitol != "" && color != "")
                {
                    Tauler tauler = new Tauler
                    {
                        titol = nouTitol,
                        color = color
                    };
                    //Si no es troba cap tauler amb la Id, en creem un de nou
                    taulers.Add(tauler);
                    tauler.AfegirTaulerBD(); //Afegim el nou tauler a la base de dades
                }
                else
                {
                    taulers.Add(new Tauler());

                }
            }

        }

        public void EliminarTauler(int id)
        {
            if (taulers.Count() == 1)   //Si només hi ha un tauler
            {
                System.Windows.MessageBox.Show("No pots esborrar l'últim tauler", "Error d'esborrat", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //Busquem un tauler existent amb la mateixa Id
                Tauler taulerExistent = taulers.FirstOrDefault(t => t.id == id);

                if (taulerExistent.Tasques.Count != 0)     //Si la llista de tasques del tauler no està buida
                {
                    System.Windows.MessageBox.Show("El tauler té tasques associades. canvia-les de tauler abans d'esborrar-lo.", "Error d'esborrat", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    taulers.RemoveAll(t => t.id == taulerExistent.id);
                    taulerExistent.EliminarTaulerBD(); //Eliminem el tauler a la base de dades

                }
            }
        }        
    }
}
