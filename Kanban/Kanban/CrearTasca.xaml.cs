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
    /// Lógica de interacción para CrearTasca.xaml
    /// </summary>
    public partial class CrearTasca : Window //Finestra de Windows que s'obra quant volem crear una nova tasca
    {
        private Tasca tascaAEditar;
        ColorControl colorControl = new ColorControl();
        private List<Etiqueta> etiquetesTasca = new List<Etiqueta>(); //Seràn les etiquetes que tindrà aquesta tasca
        public CrearTasca() //Primer constructor de la classeTasca, el fem servir si creem una tasca
        {
            InitializeComponent();
            //Informació que és carrega al obrir la pestanya de crearTasca
            txtDataCreacio.Text = DateTime.Now.ToString("yyyy-MM-dd");
            cmbTauler.ItemsSource = Dades.taulers;
            cmbResponsables.ItemsSource = Dades.responsables;
            cmbEtiquetes.ItemsSource = Dades.etiquetes;
        }
        public CrearTasca(Tasca tascaAEditarEnviada) //Segon constructor només l'utilitzem al editar la tasca
        {
            InitializeComponent();
            // Posem els valors de la tasca en la finestra
            if (tascaAEditarEnviada != null)
            {
                txtTitle.Text = tascaAEditarEnviada.title;
                txtDescription.Text = tascaAEditarEnviada.description;
                cmbResponsables.ItemsSource = Dades.responsables;
                txtDataCreacio.Text = tascaAEditarEnviada.dataCreacio.ToString("yyyy-MM-dd");
                selectedDate.SelectedDate = tascaAEditarEnviada.dataFinalitzacio;

                //Pasem de string a poder mostrar el color al rectangle
                ColorRectangle.Fill = colorControl.ConvertirStringAColorBrush(tascaAEditarEnviada.ColorFons);

                // Emplenem els combobox dels taulers i els responsables amb les dades, i mostrem per defecte el que te assignat la tascaAEditarEnviada
                cmbTauler.ItemsSource = Dades.taulers;
                cmbTauler.SelectedItem = Dades.taulers.FirstOrDefault(t => t.id == tascaAEditarEnviada.TaulerId);

                cmbResponsables.ItemsSource = Dades.responsables;
                cmbResponsables.SelectedItem = Dades.responsables.FirstOrDefault(t => t == tascaAEditarEnviada.responsable);

                //Mostrem les etiquetes disponibles
                cmbEtiquetes.ItemsSource = Dades.etiquetes;
                afegirEtiquetesListView(tascaAEditarEnviada.etiquetes); //afegim les etiquetes que te assignades en el listbox, per visualitzar-les

                //Gestio de la prioritat
                //ComboBoxItem itemSeleccionado = cmbPrioritat.Items.OfType<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == tascaAEditarEnviada.prioritat);
                cmbPrioritat.SelectedIndex = tascaAEditarEnviada.prioritat - 1;

                //La tascaAEditar, es igual a tascaAEditarEnviada. Això ho fem perquè aixì al donar a guardar entri dintre del apartar de editar
                tascaAEditar = tascaAEditarEnviada;
            }

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) //Event del propi Windows que és crida quant mentenim el mouse en la pestanya de windows
        {
            this.DragMove(); //Mètode per poder moure la finestra
        }

        private void SeleccionarColor_Click(object sender, RoutedEventArgs e)
        {
            ColorControl colorControl = new ColorControl();

            Color colorSeleccionat = colorControl.SeleccionarColorFinestra(); //Funció que obra una finestra per seleccionar un color
            ColorRectangle.Fill = new SolidColorBrush(colorSeleccionat); //Emplenem el rectangle amb el color seleccionat, a de ser tipus SolidColorBrush 
            
        }
        private void Guardar_Click(object sender, RoutedEventArgs e) //Funció que és crida al clicar guardar.
        {
            lblError.Text = "";

            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                // Missatge d'error
                lblError.Text = "Has d'emplenar el títol i la descripció";
            }
            else
            {

                if (tascaAEditar != null) //Això implica que hem cridat aquesta pestanya desde el botó de editar, ja que tascaAEditar se li posa un valor només al segon constructor
                {
                    //Comprovem els permisos de l'usuari
                    if(Dades.usuariActual.Nivell > 2 && ((Responsable)cmbResponsables.SelectedItem).Nom != Dades.usuariActual.Nom)
                    {
                        lblError.Text = "No tens permissos per canviar de responsable aquesta tasca";
                    }
                    else
                    {
                        // Els valors que té la pestanya l'hi posem dintre de la classe
                        tascaAEditar.title = txtTitle.Text;
                        tascaAEditar.description = txtDescription.Text;
                        tascaAEditar.responsable = (Responsable)cmbResponsables.SelectedItem;
                        tascaAEditar.dataFinalitzacio = selectedDate.SelectedDate;
                        tascaAEditar.etiquetes = etiquetesTasca;
                        tascaAEditar.ColorFons = colorControl.ConvertirColorBrushAString((SolidColorBrush)ColorRectangle.Fill);
                        tascaAEditar.prioritat = cmbPrioritat.SelectedIndex + 1;

                        // Gestió del tauler, per poder canviar la tasca, i poder afegir-lo en un altre tauler
                        ComprovarCanviarTauler();
                        tascaAEditar.EditarTascaBD();
                        this.Close();
                    }
                   
                }
                else
                {
                    // Si tascaAeditar és null, implica que hem cridat aquesta pestanya per crear una nova tasca
                    Tasca tasca = new Tasca
                    {
                        title = txtTitle.Text,
                        description = txtDescription.Text,
                        responsable = (Responsable)cmbResponsables.SelectedItem,
                        dataFinalitzacio = selectedDate.SelectedDate,
                        dataCreacio = DateTime.Now,
                        etiquetes = etiquetesTasca,
                        ColorFons = colorControl.ConvertirColorBrushAString((SolidColorBrush)ColorRectangle.Fill),
                        prioritat = cmbPrioritat.SelectedIndex + 1
                    };

                    // Referenciem el tauler amb el combobox
                    Tauler taulerSeleccionat = (Tauler)cmbTauler.SelectedItem;

                    if (taulerSeleccionat != null)
                    {
                        //Afegim la tasca dintre del tauler
                        taulerSeleccionat.Tasques.Add(tasca);
                        tasca.TaulerId = taulerSeleccionat.id; //L'hi posem la id del tauler
                        tasca.AfegirTascaBD(); //Afegim la tasca a la base de dades
                    }
                    this.Close();
                }
                
            }
        }
      
        private void CerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   //Tanquem la finestra
        }

        private void ComprovarCanviarTauler()
        {
            // Obtenim el tauler del combobox
            Tauler taulerSeleccionat = (Tauler)cmbTauler.SelectedItem;

            if (taulerSeleccionat != null && tascaAEditar.TaulerId != taulerSeleccionat.id)
            {
                // Li canviem la id 
                tascaAEditar.TaulerId = taulerSeleccionat.id;

                // Elimina la tasca del tauler anterior
                Tauler taulerAnterior = null;

                foreach (Tauler tauler in Dades.taulers)
                {
                    if (tauler.Tasques.Contains(tascaAEditar))
                    {
                        taulerAnterior = tauler;
                        break;  // sortim del bucle
                    }
                }

                if (taulerAnterior != null)
                {
                    taulerAnterior.Tasques.Remove(tascaAEditar);
                }

                // Afegim la tasca al nou tauler
                taulerSeleccionat.Tasques.Add(tascaAEditar);
            }
        }


        //--------------------------------------------------------------------
        //Gestió de etiquetes
        //--------------------------------------------------------------------
        private void AfegirEtiqueta_Click(object sender, RoutedEventArgs e) //Event del botó de afegir etiqueta, que mira l'element seleccionat
        {
            //Obtenim l'etiqueta seleccionada del combobox
            Etiqueta etiquetaSeleccionada = (Etiqueta)cmbEtiquetes.SelectedItem;

            bool existeix = false;
            
            //Comprovem que l'etiqueta sí existeix
            foreach (var item in listEtiquetes.Items)
            {
                if(item is TextBlock textblock && textblock.Text == etiquetaSeleccionada.nom)
                {
                    existeix = true; break;
                }
            }

            //En cas de que no existeixi, la afegim
            if (!existeix)
            {
                TextBlock afegirEtiqueta = new TextBlock();
                afegirEtiqueta.PreviewMouseLeftButtonDown += etiqueta_PreviewMouseLeftButtonDown; //Li donem al textbloc que conte el nom de la etiqueta, el event de poder clicar a sobre d'ell
                afegirEtiqueta.Text = etiquetaSeleccionada.nom;

                if (etiquetaSeleccionada.color != null && etiquetaSeleccionada.color != "")
                {
                    Color colorEtiqueta = (Color)ColorConverter.ConvertFromString(etiquetaSeleccionada.color);
                    afegirEtiqueta.Foreground = new SolidColorBrush(colorEtiqueta);
                }

                listEtiquetes.Items.Add(afegirEtiqueta);
               
                etiquetesTasca.Add(etiquetaSeleccionada);
            }                      
        }

        //Si seleccionem una etiqueta dins el llistat de etiquetes assignades, l'eliminem
        private void etiqueta_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            listEtiquetes.Items.Remove(textBlock);
            
            Etiqueta etiquetaEliminar = etiquetesTasca.Find(et => et.nom == textBlock.Text);

            etiquetesTasca.Remove(etiquetaEliminar);

        }

        private void afegirEtiquetesListView(List<Etiqueta> etiquetes)
        {
            foreach(var etiqueta in etiquetes)
            {
                TextBlock afegirEtiqueta = new TextBlock();
                afegirEtiqueta.PreviewMouseLeftButtonDown += etiqueta_PreviewMouseLeftButtonDown; //Li donem al textbloc que conte el nom de la etiqueta, el event de poder clicar a sobre d'ell
                afegirEtiqueta.Text = etiqueta.nom;

                if (etiqueta.color != null && etiqueta.color != "")
                {
                    Color colorEtiqueta = (Color)ColorConverter.ConvertFromString(etiqueta.color);

                    afegirEtiqueta.Foreground = new SolidColorBrush(colorEtiqueta);
                }


                listEtiquetes.Items.Add(afegirEtiqueta);

                etiquetesTasca.Add(etiqueta);
            }
        }

        //Obrim la finestra per a crear una nova etiqueta
        private void CrearEtiqueta_Click(object sender, RoutedEventArgs e)
        {
            CrearEtiqueta crearEtiqueta = new CrearEtiqueta();
            crearEtiqueta.ShowDialog();
            cmbEtiquetes.Items.Refresh();
        }
    }
}
