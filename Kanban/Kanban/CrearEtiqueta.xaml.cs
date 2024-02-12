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
    /// Lógica de interacción para CrearEtiqueta.xaml
    /// </summary>
    public partial class CrearEtiqueta : Window
    {      
        string colorString;

        public CrearEtiqueta()
        {
            InitializeComponent();            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) //Event del propi Windows que és crida quant mentenim el mouse en la pestanya de windows
        {
            this.DragMove(); //Mètode per poder moure la finestra
        }

        private void TancarFinestra_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   //Tanquem la finestra
        }

        private void SeleccionarColor_Click(object sender, RoutedEventArgs e)
        {
            ColorControl colorControl = new ColorControl();

            Color colorSeleccionat = colorControl.SeleccionarColorFinestra(); //Funció que obra una finestra per seleccionar un color
            ColorRectangle.Fill = new SolidColorBrush(colorSeleccionat); //Emplenem el rectangle amb el color seleccionat, a de ser tipus SolidColorBrush 
            colorString = colorControl.ConvertirColorAString(colorSeleccionat); // Transformem el color en format string amb el mètode ConvertirColorAString()

        }

        private void DesarEtiqueta_Click(object sender, RoutedEventArgs e)
        {
            //Comprovem si existeix una etiqueta amb el mateix nom
            if (Dades.etiquetes.Any(et => et.nom == txtnom.Text))
            {
                lblError.Text = "Ja exiteix una etiqueta amb aquest nom";
            }
            else
            {
                //Creem un nou objecte Etiqueta amb les dades indicades
                Etiqueta etiqueta = new Etiqueta
                {
                    nom = txtnom.Text,
                    color = colorString
                };

                Dades.etiquetes.Add(etiqueta);  //Afegim l'etiqueta a la llista d'etiquetes
                etiqueta.AfegirEtiquetaBD(); //Afegim la etiqueta a la base de dades
                Close();
            }
        }
    }
}
