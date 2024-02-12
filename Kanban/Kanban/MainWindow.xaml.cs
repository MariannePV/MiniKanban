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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;
using MySqlX.XDevAPI.Relational;

namespace Kanban
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Declarem un llistat de taulers
       
        DispatcherTimer timer;
        int segonsRestants = 10;

        ListBox dragSource = null;
        private Point startPoint;

        public MainWindow()
        {
            InitializeComponent();            

            //Agreguem als taulers les tasques que els pertoquen
            if (Dades.tasques != null)
            {
                foreach (Tauler tauler in Dades.taulers)
                {
                    tauler.Tasques = Dades.tasques.Where(t => t.TaulerId == tauler.id).ToList();
                }
            }

            llistaTaulers.ItemsSource = Dades.taulers;

            //Identifiquem a l'usuari
            UsuariAutentificat.Text = "Hola " + Dades.usuariActual.NomUsuari;
            PermisosUsuariBotonsLaterals();
            contadorComprovarlastChange();
        }

        #region Tasques
        //-----------------------------------------------
        //----------Gestió de Tasques--------------------
        //-----------------------------------------------
        private void CrearTasca(object sender, RoutedEventArgs e)
        {
            //Obrim la finestra per gestionar la creació de tasques
            CrearTasca tasca = new CrearTasca();
            tasca.ShowDialog();

            //Actualitzem la llista de taulers
            llistaTaulers.ItemsSource = null;
            llistaTaulers.ItemsSource = Dades.taulers;
        }

        private void EditarTasca_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender; //Sender és el menuItem que se ha disparat aquest event. El guardem en una variable

            //Amb DataContext, el que fem es agafar el objete de dades associat aquest element (en aquest cas agafem del menuItem l'objecte de les dades, qué es la classe tasca)
            Tasca tascaSeleccionada = (Tasca)menuItem.DataContext;

            //Obrim la mateixa finestra que crear la tasca, fem servi el 2n constructor de la classe CrearTasca
            CrearTasca editarTasca = new CrearTasca(tascaSeleccionada);
            editarTasca.ShowDialog();
        
            llistaTaulers.ItemsSource = null;
            llistaTaulers.ItemsSource = Dades.taulers;
           
        }

        private void EliminarTasca_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender; // al clicar el boto de eliminar

            // tasca associada al menú contextual
            Tasca tascaSeleccionada = (Tasca)menuItem.DataContext;

            // Troba el tauler que conté aquesta tasca 
            Tauler taulerContenedor = Dades.taulers.FirstOrDefault(tauler => tauler.Tasques.Contains(tascaSeleccionada));

            if (taulerContenedor != null)
            {
                // Elimina la tasca del tauler
                taulerContenedor.Tasques.Remove(tascaSeleccionada);
                tascaSeleccionada.EliminarTascaBD();

                // Actualiza la llista de taulers
                llistaTaulers.ItemsSource = null;
                llistaTaulers.ItemsSource = Dades.taulers;              

            }
        }
        #endregion

        #region Taulers
        //-----------------------------------------------
        //----------Gestió de Taulers--------------------
        //-----------------------------------------------

        private void CrearTauler(object sender, RoutedEventArgs e)
        {
            //Obrim la finestra per a la creació de taulers
            CrearTauler tauler = new CrearTauler();
            tauler.ShowDialog();
            llistaTaulers.ItemsSource = null;
            llistaTaulers.ItemsSource = Dades.taulers;
        }

        private void EditarTauler(object sender, RoutedEventArgs e)
        {
            MenuItem boton = (MenuItem)sender;  //Obtenim des de quin botó s'ha accionat l'event

            Tauler taulerSeleccionat = (Tauler)boton.DataContext;   //Busquem el tauler seleccionat segons el botó que ha accionat l'event

            //Obrim la finestra per a l'edició de taulers
            CrearTauler tauler = new CrearTauler(taulerSeleccionat);
            tauler.ShowDialog();
            llistaTaulers.ItemsSource = null;
            llistaTaulers.ItemsSource = Dades.taulers;
        }

        private void OrdenarTauler(object sender, RoutedEventArgs e)
        {
            //Segons el menú item seleccionat, busquem a quin tauler pertany
            MenuItem clickedMenuItem = sender as MenuItem;
            Tauler taulerSeleccionat = (Tauler)clickedMenuItem.DataContext;

            if (clickedMenuItem != null)
            {
                //Obtenim el títol de l'opció d'ordre seleccionada
                string header = clickedMenuItem.Header.ToString();

                //Segons aquest títol duem a terme unes accions o unes altres
                switch (header)
                {
                    case "Prioritat":
                        //Ordenem les tasques per prioritats (Alta, Mitjana, Baixa)
                        OrderTasquesByPriority(taulerSeleccionat);
                        break;
                    case "Ordre alfabètic descendent":                        
                        //Ordenem les tasques alfabèicament segons el seu títol
                        OrderTasquesAlphabetically(taulerSeleccionat);
                        break;
                    case "Data de finalització":
                        //Ordenem les tasques segons la seva data de finalització (de més a menys recent)
                        OrderTasquesByFinishDate(taulerSeleccionat);
                        break;
                    default:
                        break;
                }
            }
        }

        private void OrderTasquesByPriority(Tauler tauler)
        {
            //Definim quines són les prioritats
            List<Prioritat> customPriorityOrder = new List<Prioritat> { Prioritat.Alta, Prioritat.Mitjana, Prioritat.Baixa };

            //Busquem pel tauler les tasques amb les prioritats definides i les ordenem
            tauler.Tasques = tauler.Tasques.OrderBy(tasca => customPriorityOrder.IndexOf((Prioritat)tasca.prioritat)).ToList();

            //Actualitzem el llistat
            llistaTaulers.Items.Refresh();
        }

        private void OrderTasquesAlphabetically(Tauler tauler)
        {
            //Busquem pel tauler les tasques amb les prioritats definides i les ordenem alfabeticament
            tauler.Tasques = tauler.Tasques.OrderBy(tasca => tasca.title).ToList();

            //Actualitzem el llistat
            llistaTaulers.Items.Refresh();
        }

        private void OrderTasquesByFinishDate(Tauler tauler)
        {
            ////Busquem pel tauler les tasques amb les prioritats definides i les ordenem segons la data de finalització
            tauler.Tasques = tauler.Tasques.OrderBy(tasca => tasca.dataFinalitzacio).ToList();

            //Actualitzem el llistat
            llistaTaulers.Items.Refresh();
        }
        #endregion

        #region Responsables
        //-----------------------------------------------
        //----------Gestió de Responsables---------------
        //-----------------------------------------------

        private void GestionarResponsables(object sender, MouseButtonEventArgs e)
        {
            //Mostrem la finestra de la gestió de responsables
            GestionarResponsables resp = new GestionarResponsables();
            resp.ShowDialog();
        }
        #endregion

        #region Etiquetes
        //-----------------------------------------------
        //----------Gestió de Etiquetes------------------
        //-----------------------------------------------

        private void GestionarEtiquetes_Mouse(object sender, EventArgs e)
        {
            //Mostrem la finestra de la gestió d'etiquetes
            GestionarEtiquetes gestionarEtiquetes = new GestionarEtiquetes();
            gestionarEtiquetes.ShowDialog();
        }
        #endregion

        #region UsuariActual
        private void LogOut(object sender, MouseButtonEventArgs e)
        {
            //En cas de que l'usuari faci un log out, tornem a mostrar la finestra de login i tanquem el MainWindow
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void PermisosUsuariBotonsLaterals()
        {
            //Controlem, segons els permisos que té cada usuari, quins botons laterals pot visualitzar
            switch (Dades.usuariActual.Nivell)
            {
                case 4:
                    MenuCrear.Visibility = Visibility.Collapsed;
                    ButtonResponsable.Visibility = Visibility.Collapsed;
                    ButtonEtiqueta.Visibility = Visibility.Collapsed;
                    break;

                case 3:
                    MenuCrear.Visibility = Visibility.Collapsed;
                    ButtonResponsable.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }

        private void MenuTauler_Loaded(object sender, RoutedEventArgs e)
        {
            //Segons el nivell de l'usuari, mostrem els botons per editar els taulers
            Menu menuTauler = sender as Menu;
            if (menuTauler != null && (Dades.usuariActual.Nivell == 4 || Dades.usuariActual.Nivell == 3))
            {
                menuTauler.Visibility = Visibility.Collapsed;
            }
        }

        private void MenuTasca_Loaded(object sender, RoutedEventArgs e)
        {
            //Segons el nivell de cada usuari, mostrarem o no el menú assciat a cada tasca
            Menu menuTasca = sender as Menu;
            bool NoEsVisible = false;

            switch (Dades.usuariActual.Nivell)
            {
                case 4:
                    NoEsVisible = true;
                    break;
                case 3:
                    //Aquí comprovem si la tasca està o no associada a l'usuari en qüestió
                    Tasca tasca = menuTasca.DataContext as Tasca;
                    if (tasca.responsable.Nom == Dades.usuariActual.Nom)
                    {
                        NoEsVisible = false;
                    }
                    else
                    {
                        NoEsVisible = true;
                    }
                    break;
                default:
                    NoEsVisible = false;
                    break;

            }

            //Segons els resultats anteriors, modifiquem o no la visibilitat del menú per interactuar amb les tasques
            if (menuTasca != null && NoEsVisible)
            {
                menuTasca.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region Comptador
        //-----------------------------------------------
        //----------Gestió del contador------------------
        //-----------------------------------------------

        private void contadorComprovarlastChange()
        {
            //Iniciem un contador que comprova canvis en el servidor cada 1000 milisegons
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += OnTimedEvent; //Associem l'event onTimed al temporitzador
            timer.Start();

        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            //Redueix els segons restants
            segonsRestants--;

            //En cas de que els segons restants siguin menors a 0, el tornem a possar a 10 segons
            if (segonsRestants <= 0)
            {
                segonsRestants = 10;

                //Obtenim la última modificació de la base de dades
                DateTime lastChangeDB = Dades.ObtenirLastChange();

                //En cas de que aquesta data de última modificació sigui diferent a la nostra, mostrem un missatge
                if (Dades.lastChange != lastChangeDB)
                {
                    //Para el temporitzador
                    timer.Stop();
                    Dades.lastChange = lastChangeDB;
                    MostrarCustomMessageBox();
                }
            }
        }

        private void ActualitzarKanban(object sender, RoutedEventArgs e)
        {
            //Actualitzem les dades
            Dades.ActualitarDades();
            timer.Start();  //reiniciem el temporitzador (per buscar nous canvis)

            if (Dades.tasques != null)
            {
                foreach (Tauler tauler in Dades.taulers)
                {
                    tauler.Tasques = Dades.tasques.Where(t => t.TaulerId == tauler.id).ToList();
                }
            }

            //Actualitzem la llista de taulers
            llistaTaulers.ItemsSource = null;
            llistaTaulers.ItemsSource = Dades.taulers;
            
            //Deixem de mostrar el botó per actualitzar
            update.Visibility = Visibility.Collapsed;
        }

        private void MostrarCustomMessageBox()
        {
            //Notifiquem del canvi detectat
            MessageBoxResult result = MessageBox.Show("Un altre usuari ha fet una modificació, vols actualitar el teu Kanban, o seguir treballant sense la actualització", "Actualització pendent", MessageBoxButton.YesNo, MessageBoxImage.Question);

            //Si l'usuari vol actualitzar les dades
            if (result == MessageBoxResult.Yes)
            {
                //Reiniciem el temporitzador i actualitzem les dades
                timer.Start();
                Dades.ActualitarDades();

                if (Dades.tasques != null)
                {
                    foreach (Tauler tauler in Dades.taulers)
                    {
                        tauler.Tasques = Dades.tasques.Where(t => t.TaulerId == tauler.id).ToList();
                    }
                }
                llistaTaulers.ItemsSource = null;
                llistaTaulers.ItemsSource = Dades.taulers;

            }
            else if (result == MessageBoxResult.No)
            {
                //Necessari perque si no salta error, perque el boto està siguent utilitzat per un altre process,fem servir Dispacher per dir-li que es del fil principal
                //Mostrem el botó per actualitzar a petició de l'usuari
                update.Visibility = Visibility.Visible;

            }
        }
        #endregion

        #region Drag & Drop
        private void selectedItemLlistaTaulers(object sender, MouseButtonEventArgs e)
        {
            //Verifica que el nivell de l'usuari no sigui 4
            if (Dades.usuariActual.Nivell != 4)
            {
                ListBox parent = (ListBox)sender;
                startPoint = e.GetPosition(parent); //Obtenim la posició inicial del click
                dragSource = parent;    //Establim el llistat d'origen
            }
        }

        private void ListBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (Dades.usuariActual.Nivell != 4)
            {
                ListBox parent = (ListBox)sender;

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point mousePos = e.GetPosition(parent);
                    Vector diff = startPoint - mousePos;

                    //Comprovem que s'hagi detectat la distància mínima perquè es consideri un Drag & Drop
                    if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                        Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                    {
                        object data = GetDataFromListBox(parent, mousePos);

                        //Comprovem si s'ha obtingut un element i si es tracta d'una tasca
                        if (data != null && data is Tasca draggedTasca)
                        {
                            //Si l'usuari és de nivell 1 o 2, pot moure lliurement qualsevol tasca
                            if (Dades.usuariActual.Nivell == 1 || Dades.usuariActual.Nivell == 2)
                            {
                                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
                            }
                            //En cas de que l'usuari tingui un altre nivell, només podrà moure les que tingui assignades
                            else if (draggedTasca.responsable.Nom == Dades.usuariActual.Nom)
                            {
                                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
                            }
                        }
                    }
                }
            }
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            if (Dades.usuariActual.Nivell != 4)
            {
                //La listBox on es fà el drop
                ListBox parent = (ListBox)sender;
                object data = e.Data.GetData(typeof(Tasca)); //Obté les dades de la tasca amb la que fem el drag

                //Verifiquem que hi hagi una font de drag, que aquesta no sigui la mateixa on es fa el drop i que les dades corresponents a la tasca no siguin nulls
                if (dragSource != null && dragSource != parent && data != null)
                {
                    //Accedim al tauler associat a la listBox
                    if (parent.DataContext is Tauler droppedTauler)
                    {
                        Tasca droppedTasca = (Tasca)data;

                        //Eliminem l'element de la llista antiga (dragSource)
                        if (dragSource.ItemsSource is IList<Tasca> oldList)
                        {
                            oldList.Remove(droppedTasca);
                        }

                        //Canviem l'id associat al tauler de la tasca
                        droppedTasca.TaulerId = droppedTauler.id;
                        droppedTasca.EditarTascaBD();
                    }

                    //Afegim la tasca a la ItemsSource de la llista actual
                    if (parent.ItemsSource is IList<Tasca> targetList)
                    {
                        Tasca droppedTasca = (Tasca)data;
                        targetList.Add(droppedTasca);
                    }

                    //Actualitzem
                    llistaTaulers.Items.Refresh();

                    //Restablim la llista d'origen
                    dragSource = null;
                }
            }

        }

        //Obtenim l'element corresponent de la ListBox
        private static object GetDataFromListBox(ListBox source, Point point)
        {
            if (Dades.usuariActual.Nivell != 4)
            {
                //Obtenim l'element de la llista associat a la informació proporcionada (la posició del ratolí)
                UIElement element = source.InputHitTest(point) as UIElement;
                if (element != null)
                {
                    object data = DependencyProperty.UnsetValue;

                    //Busquem l'element de dades associat a l'element visual i en cas de no trobar-lo retornem null
                    while (data == DependencyProperty.UnsetValue)
                    {
                        data = source.ItemContainerGenerator.ItemFromContainer(element);

                        if (data == DependencyProperty.UnsetValue)
                        {
                            element = VisualTreeHelper.GetParent(element) as UIElement;
                        }

                        if (element == source)
                        {
                            return null;
                        }
                    }

                    //En cas de trobar-lo, retornem les dades associades a l'element triat
                    if (data != DependencyProperty.UnsetValue)
                    {
                        return data;
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
