using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DatenbankAdmin
{


    public partial class Form2 : Form
    {

        Image Bild;

        MySqlConnection connection;
        MySqlCommand command;

        MemoryStream ms = new MemoryStream();

        string connectionstring = "Server=127.0.0.1;Database=supermarket;Uid=root;Pwd=F4laRl!JDfMZBHr;SslMode=None";
        string insertQuery = "INSERT INTO produkte(barcode,produktname,hersteller,preis,produktbild,freigabe,bestandsmenge) VALUES(@barcode,@produktname,@hersteller,@preis,@produktbild,@freigabe,@bestandsmenge)";


        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void AddPctrBTN_Click(object sender, EventArgs e)
        {
            //Add Picture
            OpenFileDialog opfd = new OpenFileDialog();
            opfd.Filter = "Datei | *.jpg; *.png; *.gif";
            if (opfd.ShowDialog() == DialogResult.OK)
            {
                Bild = Image.FromFile(opfd.FileName);
                pictureBox1.Image = Bild;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //Write Data in Database

            pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
            byte[] bildbyte = ms.ToArray();
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            command = new MySqlCommand(insertQuery, connection);
            command.Parameters.Add("@barcode", MySqlDbType.Int32);
            command.Parameters.Add("@produktname", MySqlDbType.VarChar);
            command.Parameters.Add("@hersteller", MySqlDbType.VarChar);
            command.Parameters.Add("@preis", MySqlDbType.Decimal);
            command.Parameters.Add("@produktbild", MySqlDbType.Blob);
            command.Parameters.Add("@freigabe", MySqlDbType.Int32);
            command.Parameters.Add("@bestandsmenge", MySqlDbType.Int32);


            command.Parameters["@barcode"].Value = barcodeTB.Text;
            command.Parameters["@produktname"].Value = productnameTB.Text;
            command.Parameters["@hersteller"].Value = manfTB.Text;
            command.Parameters["@preis"].Value = priceTB.Text;
            command.Parameters["@produktbild"].Value = bildbyte;
            command.Parameters["@freigabe"].Value = fgTB.Text;
            command.Parameters["@bestandsmenge"].Value = ammountTB.Text;

            try
            {
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Daten wurden erfolgreich eingefügt!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Es ist ein Fehler beim einfügen aufgetreten!");
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
