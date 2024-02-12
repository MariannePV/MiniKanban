using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
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
    /// Lógica de interacción para CrearResponsable.xaml
    /// </summary>
    public partial class CrearResponsable : Window
    {
        private Responsable responsableExistent;

        public CrearResponsable(Responsable responsableExistent = null)
        {
            InitializeComponent();
            this.responsableExistent = responsableExistent;

            //En cas de que el responsable existeixi, emplenem directament les dades amb el contingut corresponent
            if (responsableExistent != null)
            {
                txtNomUsuari.Text = responsableExistent.NomUsuari;
                txtNom.Text = responsableExistent.Nom;
                txtCognoms.Text = responsableExistent.Cognoms;
                txtEmail.Text = responsableExistent.Email;
                txtDni.Text = responsableExistent.Dni;
                selectedDate.SelectedDate = responsableExistent.DataNaix;               
                cmbNivell.SelectedIndex = responsableExistent.Nivell - 1;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) //Event del propi Windows que és crida quant mentenim el mouse en la pestanya de windows
        {
            this.DragMove(); //Mètode per poder moure la finestra
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            lblError.Text = "";

            if (string.IsNullOrWhiteSpace(txtNomUsuari.Text) && string.IsNullOrWhiteSpace(pwPassword.Password))
            {
                //En cas de detectar que el nom de l'usuari o el camp de la contrasenya estàs buits, mostrem un error
                lblError.Text = "Has de posar un Usuari i una contrassenya";
            }
            else
            {
                ComprovarValorsNull();

                //En cas de que el responsable existeixi, actualitzem les seves dades
                if (responsableExistent != null)
                {
                    //Comprovem que l'usuari tingui el nivell per actualitzar-ne les dades
                    if(Dades.usuariActual.Nivell > 1 && cmbNivell.SelectedIndex + 1 > 2 || Dades.usuariActual.Nivell == 1)
                    {
                        responsableExistent.NomUsuari = txtNomUsuari.Text;
                        responsableExistent.Nom = txtNom.Text;
                        responsableExistent.Cognoms = txtCognoms.Text;
                        responsableExistent.Email = txtEmail.Text;
                        responsableExistent.Dni = txtDni.Text;
                        responsableExistent.DataNaix = (DateTime)selectedDate.SelectedDate;
                        if (pwPassword.Password != string.Empty)
                        {
                            //Generem un hash de la nova contrasenya
                            responsableExistent.Password = Dades.GenerarHashSHA256(pwPassword.Password);
                        }
                        responsableExistent.Nivell = cmbNivell.SelectedIndex + 1;

                        //Editem el responsable
                        responsableExistent.EditarResponsableBD();
                        this.Close();                      
                    }
                    else 
                    {
                        lblError.Text = "No tens suficients permissos per crear un SuperUser o un Admin";
                    }

                }
                else
                {
                    //Comprovem que el nom d'usuari sigui únic
                    if (Dades.responsables.Any(res => res.NomUsuari == txtNomUsuari.Text))
                    {
                        lblError.Text = "Ja exiteix aquest nom d'usuari";
                    }
                    else
                    {
                        //Comprovem permissos
                        if (Dades.usuariActual.Nivell > 1 && cmbNivell.SelectedIndex + 1 > 2 || Dades.usuariActual.Nivell == 1)
                        {
                            //Creem un nou responsable
                            Responsable responsable = new Responsable
                            {
                                NomUsuari = txtNomUsuari.Text,
                                Nom = txtNom.Text,
                                Cognoms = txtCognoms.Text,
                                Email = txtEmail.Text,
                                Dni = txtDni.Text,
                                DataNaix = (DateTime)selectedDate.SelectedDate,
                                Password = Dades.GenerarHashSHA256(pwPassword.Password),
                                Nivell = cmbNivell.SelectedIndex + 1

                            };

                            Dades.responsables.Add(responsable);
                            responsable.AfegirResponsableBD();
                            this.Close();

                        }
                        else
                        {
                            lblError.Text = "No tens suficients permissos per crear un SuperUser o un Admin";
                        }
                    }
                }               
            }           
        }

        private void CerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            //Tanquem la finestra
            this.Close();
        }

        //Comprovem que els valors no sigui null
        private void ComprovarValorsNull()
        {
            if (txtNom.Text == null)
            {
                txtNom.Text = string.Empty;
            }
            if (txtCognoms.Text == null)
            {
                txtCognoms.Text = string.Empty;
            }
            if (txtEmail.Text == null)
            {
                txtEmail.Text = string.Empty;
            }
            if (txtDni.Text == null)
            {
                txtDni.Text = string.Empty;
            }
            if (selectedDate.SelectedDate == null)
            {
                selectedDate.SelectedDate = new DateTime();
            }            
        }
    }
}
