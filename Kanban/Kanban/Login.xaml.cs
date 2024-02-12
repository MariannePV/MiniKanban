using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
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
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        Dades con = new Dades();    //Connexió amb la base de dades (recuperem tota la informació de la base de dades per així treballar amb ella mitjançant les llistes
        private List<Responsable> responsables;

        //Placeholders
        private string pswdPred = "Paraula";
        private string usrPred = "Nom d'usuari";

        #region Login()
        public Login()
        {
            InitializeComponent();

            //Assignem els placeholders als camps de text
            password.Password = pswdPred;
            nomUser.Text = usrPred;

            //Obtenim el llistat d'usuaris
            responsables = Dades.responsables;
        }
        #endregion

        #region Btn entrar click
        private void iniciarSessio(object sender, RoutedEventArgs e)
        {
            string errorMssg = "L'usuari o la contrasenya és incorrecte";

            string nombreUsuario = nomUser.Text;
            //Generem un hash amb la contrasenya inserida per l'usuari
            string contrasenya = Dades.GenerarHashSHA256(password.Password);

            //Comprovem l'existència del nom d'usuari en la BDD amb la contrasenya corresponent
            Responsable usuariAutentificat = responsables.FirstOrDefault(u => u.NomUsuari == nombreUsuario && u.Password == contrasenya);

            //En cas de que en polsar el botó per entrar, no estiguin assignats els valors predefinits
            if (nomUser.Text != usrPred && password.Password != pswdPred)
            {
                //"Entrem" al programa obrint la MainWindow i tancant la finestra de Login
                if (usuariAutentificat != null)
                {
                    Dades.usuariActual = usuariAutentificat;
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    this.Close();
                }
                else
                {
                    //Mostrem error, en cas de que les credencials no tinguin una coincidència en la base de dades
                    lblError.Text = errorMssg;
                }
            }
            else
            {
                //Mostrem error, en cas de que les credencials siguin les mateixes que les predeterminades
                lblError.Text = errorMssg;
            }
        }
        #endregion

        #region "Placeholder" nom usuari i password
        private void FieldGotFocus(object sender, RoutedEventArgs e)
        {
            // Quan el TextBox rep el focus, el seu contingut es canvia a una cadena buida
            if (sender is TextBox nomUser && nomUser.Text == usrPred)
            {
                nomUser.Text = string.Empty;
            }
            if (sender is PasswordBox password && password.Password == pswdPred)
            {
                password.Password = string.Empty;
            }
        }

        private void FieldLostFocus(object sender, RoutedEventArgs e)
        {
            //Si està buit, tornem a mostrar el placeholder
            if (sender is TextBox nomUser && nomUser.Text == "")
            {
                nomUser.Text = usrPred;
            }
            if (sender is PasswordBox password && password.Password == "")
            {
                password.Password = pswdPred;
            }
        }
        #endregion

        #region Detecció tecla enter
        private void keyDownEnter(object sender, KeyEventArgs e)
        {
            //En cas de detectar la tecla enter, simulem el fet de polsar el botó per iniciar sessió
            if (e.Key == Key.Enter)
            {
                iniciarSessio(sender, e);
            }
        }
        #endregion
    }
}
