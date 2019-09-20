using Cosmos.Classes;
using System;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cosmos.Windows
{
    public partial class CBugReport : Window
    {
        private bool database = false;
        //
        private string senderEmail;
        private string senderPassword;
        private string addresseeEmail;

        private static string supportDBPath = "";
        private static OleDbCommand dbCommand;
        //private static OleDbDataAdapter dbDataAdapter;
        private static OleDbConnection conn;
        private string connectionstring = "Provider=Microsoft.JET.OLEDB.4.0;Data Source = ";

        /// <summary>
        /// Only for internal database
        /// </summary>
        public CBugReport(string _supportDBPath, string email, string version, string product)
        {
            InitializeComponent();
            database = true;

            supportDBPath = _supportDBPath;
            connectionstring += supportDBPath;
            conn = new OleDbConnection(connectionstring);


            TB_email.Text = email;
            TB_version.Text = version;
            TB_product.Text = product;

            SelectOS();
        }
        public CBugReport(string _addresseeEmail, string _senderEmail, string _senderPassword, string email, string version, string product)
        {
            InitializeComponent();

            senderEmail = _senderEmail;
            senderPassword = _senderPassword;
            addresseeEmail = _addresseeEmail;

            TB_email.Text = email;
            TB_version.Text = version;
            TB_product.Text = product;

            SelectOS();
        }

        private void SelectOS()
        {
            OperatingSystem os_info = System.Environment.OSVersion;
            string os = GetOsName(os_info);

            for (int i = 0; i < CB_os.Items.Count; i++)
                if ((CB_os.Items[i] as ComboBoxItem).Content.ToString() == os)
                    CB_os.SelectedIndex = i;
        }

        private string GetOsName(OperatingSystem os_info)
        {
            string version =
                os_info.Version.Major.ToString() + "." +
                os_info.Version.Minor.ToString();
            switch (version)
            {
                case "10.0": return "Windows 10";
                case "6.3": return "Windows 8.1";
                case "6.2": return "Windows 8";
                case "6.1": return "Windows 7";
                case "6.0": return "Windows Vista";
                case "5.1": return "Windows XP";
            }
            return "";
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void B_send_Click(object sender, RoutedEventArgs e)
        {
            //add bugeport to database
            if (database)
            {
                //check correctness of input data
                if (TB_description.Text.Replace(" ", string.Empty) == string.Empty ||
                    CB_os.SelectedIndex < 0 ||
                    CB_impact.SelectedIndex < 0 ||
                    (TB_email.Text.Replace(" ", string.Empty) != "" && !CValidation.ValidateEmail(TB_email.Text)))
                {
                    CMessageBox message = new CMessageBox("Bitte füllen sie alle Felder die mit einem * gekennzeichnet sind und überprüfen sie die Daten auf ihre Richtigkeit", "Eingabe Korrigieren", CColor.Theme.DarkGrey, CImage.ImageType.edit_black, CMessageBox.CMessageBoxButton.OK);
                    message.ShowDialog();
                    return;
                }

                string impact = string.Empty;
                if (CB_impact.SelectedIndex >= 0)
                    impact = (CB_impact.SelectedItem as ComboBoxItem).Content.ToString();

                try
                {
                    if (!InsertRequest(TB_email.Text, DateTime.Today, "", false, TB_description.Text, CB_os.Text, CB_priority.Text, TB_version.Text, impact, TB_product.Text))
                        return;
                }
                catch (Exception ex)
                {
                    CMessageBox errormessage = new CMessageBox(ex.InnerException.Message.ToString(), "Datenbankfehler", CColor.Theme.Red, CImage.ImageType.error_outline_black, CMessageBox.CMessageBoxButton.OK);
                    errormessage.ShowDialog();
                    return;
                }

                CMessageBox donemessage = new CMessageBox("Vielen Dank.\nIhre Anfrage wird so schnell wie möglich bearbeitet.", "Hinweis", CColor.Theme.DarkGrey, CImage.ImageType.error_outline_black, CMessageBox.CMessageBoxButton.OK);
                donemessage.ShowDialog();
            }
            //send bugreport via email
            else
            {
                string impact = string.Empty;
                if (CB_impact.SelectedIndex >= 0)
                    impact = (CB_impact.SelectedItem as ComboBoxItem).Content.ToString();

                CEmail.SendEmail(addresseeEmail, senderEmail, senderPassword, "Fehlermeldung", TB_email.Text + "\n" +
                                                                                               DateTime.Today + "\n" +
                                                                                               TB_description.Text + "\n" +
                                                                                               CB_os.Text + "\n" +
                                                                                               CB_priority.Text + "\n" +
                                                                                               TB_version.Text + "\n" +
                                                                                               impact + "\n" +
                                                                                               TB_product.Text);

            }

            Close();
        }

        private bool InsertRequest(string email, DateTime? date, string editor, bool? done, string description, string os, string priority, string version, string impact, string product)
        {
            try
            {
                conn.Open();
            }
            catch
            {
                CMessageBox message = new CMessageBox("Datenbank \"" + supportDBPath + "\" konnte nicht gefunden werden.", "Fehler", CColor.Theme.Red, CImage.ImageType.error_outline_black, CMessageBox.CMessageBoxButton.OK);
                message.ShowDialog();
                return false;
            }

            dbCommand = new OleDbCommand("INSERT INTO Anfragen (Email, Datum, Bearbeiter, Erledigt, Problembeschreibung, Betriebssystem, Prioritaet, Version, Schwere, Produkt) VALUES(@Email, @Datum, @Bearbeiter, @Erledigt, @Problembeschreibung, @Betriebssystem, @Prioritaet, @Version, @Schwere, @Produkt)", conn);

            dbCommand.Parameters.Add("@Email", OleDbType.VarChar).Value = email;
            dbCommand.Parameters.Add("@Datum", OleDbType.Date).Value = date;
            dbCommand.Parameters.Add("@Bearbeiter", OleDbType.VarChar).Value = editor;
            dbCommand.Parameters.Add("@Erledigt", OleDbType.Boolean).Value = done;
            dbCommand.Parameters.Add("@Problembeschreibung", OleDbType.VarChar).Value = description;
            dbCommand.Parameters.Add("@Betriebssystem", OleDbType.VarChar).Value = os;
            dbCommand.Parameters.Add("@Prioritaet", OleDbType.VarChar).Value = priority;
            dbCommand.Parameters.Add("@Version", OleDbType.VarChar).Value = version;
            dbCommand.Parameters.Add("@Schwere", OleDbType.VarChar).Value = impact;
            dbCommand.Parameters.Add("@Produkt", OleDbType.VarChar).Value = product;

            foreach (OleDbParameter _parameter in dbCommand.Parameters)
                if (_parameter.Value == null)
                    _parameter.Value = DBNull.Value;

            //dbDataAdapter = new OleDbDataAdapter(dbCommand);
            dbCommand.ExecuteNonQuery();

            conn.Close();
            return true;
        }
    }
}
