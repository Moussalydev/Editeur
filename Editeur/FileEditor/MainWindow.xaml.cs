using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace EditeurFichier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Nom du fichier à utiliser
        /// </summary>
        private string nomFichier = String.Empty;
        private string CheminCompletNomFichier = String.Empty;
        private string ExtensionFichier = String.Empty;

        public MainWindow()
        {
            InitializeComponent();
            if (editeur.Text.Equals(""))
            {
                boutonEnregistrer.IsEnabled = false;
                bouttonEnregistrerSous.IsEnabled = false;
                menuSousEnregistrer.IsEnabled = false;
                menuEnregistrer.IsEnabled = false;
            }
        }

        /// <summary>
        /// Ouvrir le fichier après la sélection par l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoutonOuvrir_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                //InitialDirectory = "c:\\",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,
                FileName = "Nom Fichier",
                DefaultExt = ".txt"
            };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    nomFichier = LireNomFichier(dlg.FileName);
                    if (!nomFichier.Equals(""))
                    {
                        Stream myStream = dlg.OpenFile();
                        if (myStream != null)
                        {
                            using (myStream)
                            {
                                using (StreamReader reader = new StreamReader(myStream, Encoding.UTF8))
                                {
                                    editeur.Text = reader.ReadToEnd();
                                    ExtensionFichier = LireExtensionFichier(dlg.FileName);
                                    this.Title = "Editeur Fichier : " + dlg.FileName + " | " + ExtensionFichier;
                                    CheminCompletNomFichier = dlg.FileName;
                                    this.FontSize = 14;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Le fichier n'existe pas.");
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Une erreur est survenue lors de l'ouverture du fichier : '" + ee.Message + "' ");
                }
            }
        }

        // TODO - Implémenter une méthode pour lire le nom du fichier 
        // Ajouter une méthode LireNomFichier 


        // Enregistrer les données à nouveau dans le fichier
        private void BoutonEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            if(!CheminCompletNomFichier.Equals(""))
            {
                try
                {
                    File.WriteAllText(CheminCompletNomFichier, editeur.Text, Encoding.UTF8);
                    this.Title = "Editeur Fichier : " + CheminCompletNomFichier + "  ( " + ExtensionFichier + " )";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                EnregistrerNouveauFichier();
            }
        }
        public bool EnregistrerNouveauFichier()
        {
            SaveFileDialog saveFileDia = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt",
                Title = "Enregistrer votre fichier"
            };
            try
            {
                if (saveFileDia.ShowDialog() == true)
                {
                    if (File.Exists(saveFileDia.FileName))
                    {
                        MessageBox.Show("Ce nom de fichier exsite déjà.");
                    }
                    else
                    {
                        using (FileStream fs = File.Create(saveFileDia.FileName))
                        {
                            Byte[] texte = new UTF8Encoding(true).GetBytes(editeur.Text);
                            // Add some information to the file.
                            fs.Write(texte, 0, texte.Length);
                        }
                        CheminCompletNomFichier = saveFileDia.FileName;
                        ExtensionFichier = LireExtensionFichier(saveFileDia.FileName);
                        this.Title = "Editeur Fichier : " + CheminCompletNomFichier + " | " + ExtensionFichier;
                        this.FontSize = 14;
                        return true;
                    }
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show("Erreur lors de l'enregistrement du fichier : '" + eee.Message + "'");
            }
            return false;
        }
        public string LireNomFichier(string cheminFichier)
        {
            if (cheminFichier.Equals(""))
                return "";
            return System.IO.Path.GetFileName(cheminFichier);
        }
        public string LireExtensionFichier(string cheminFichier)
        {
            if (cheminFichier.Equals(""))
                return "";
            return System.IO.Path.GetExtension(cheminFichier);
        }

        private void editeur_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!CheminCompletNomFichier.Equals(""))
            {
                BoutonEnregistrer_Click(sender, e);
                this.Title = "Editeur Fichier : " + CheminCompletNomFichier + " * ";
                labelEnregistrement.Content = "Enregistré automatiquement";
            }
            else
                this.Title = "Editeur Fichier : Nouveau Document * ";
            boutonEnregistrer.IsEnabled = true;
            bouttonEnregistrerSous.IsEnabled = true;
            menuSousEnregistrer.IsEnabled = true;
            menuEnregistrer.IsEnabled = true;
        }
        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            EnregistrerNouveauFichier();
        }

        private void buttonNouveauFichier_Click(object sender, RoutedEventArgs e)
        {
            EnregistrerNouveauFichier();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            BoutonOuvrir_Click(sender, e);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            BoutonEnregistrer_Click(sender, e);
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            editeur.FontSize += 2;
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            editeur.FontSize -= 2;
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            editeur.FontWeight = FontWeights.Bold;
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            editeur.FontWeight = FontWeights.Normal;
            editeur.FontStyle = FontStyles.Normal;
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            editeur.FontStyle = FontStyles.Italic;
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            bouttonEnregistrerSous_Click(sender, e);
        }

        private void bouttonEnregistrerSous_Click(object sender, RoutedEventArgs e)
        {
            if (CheminCompletNomFichier.Equals(""))
            {
                EnregistrerNouveauFichier();
            }
            else
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    Filter = "txt files (*.txt)|*.txt",
                    Title = "Enregistrer votre fichier"
                };
                try
                {
                    if (saveFileDialog1.ShowDialog() == true)
                    {
                        File.Copy(CheminCompletNomFichier, saveFileDialog1.FileName);
                        //Ouvrir le nouveau fichier créer
                        try
                        {
                            nomFichier = LireNomFichier(saveFileDialog1.FileName);
                            if (!nomFichier.Equals(""))
                            {
                                Microsoft.Win32.OpenFileDialog d = new Microsoft.Win32.OpenFileDialog();
                                d.FileName = saveFileDialog1.FileName;
                                Stream myStream = d.OpenFile();
                                if (myStream != null)
                                {
                                    using (myStream)
                                    {
                                        using (StreamReader reader = new StreamReader(myStream, Encoding.UTF8))
                                        {
                                            editeur.Text = reader.ReadToEnd();
                                            ExtensionFichier = LireExtensionFichier(d.FileName);
                                            this.Title = "Editeur Fichier : " + d.FileName + " | " + ExtensionFichier;
                                            CheminCompletNomFichier = d.FileName;
                                            this.FontSize = 14;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Le fichier n'existe pas.");
                            }
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show("Une erreur est survenue lors de l'ouverture du fichier : '" + ee.Message + "' ");
                        }
                    }
                }
                catch (Exception eee)
                {
                    MessageBox.Show("Erreur lors de l'enregistrement du fichier : '" + eee.Message + "'");
                }
            }
        }

        
    }
}
