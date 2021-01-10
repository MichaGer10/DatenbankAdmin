﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;

namespace DatenbankAdmin
{
    public partial class Produkte : Form
    {
        MySqlConnection connection;
        MySqlCommand command;
        MySqlDataAdapter adapter;
        DataTable dtable;

        private string connectionstring = "Server=127.0.0.1;Database=supermarket;Uid=root;Pwd=F4laRl!JDfMZBHr;SslMode=None";
        string insertquery = "INSERT INTO produkte(barcode,produktname,hersteller,preis,produktbild,freigabe,bestandsmenge) VALUES(@barcode,@produktname,@hersteller,@preis,@produktbild,@freigabe,@bestandsmenge)";

        public Produkte()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            searchDataDB("");
        }

        public void searchDataDB(string ssearch)
        {
            string searchquerry = "SELECT * FROM supermarket.produkte WHERE CONCAT(barcode,produktname,hersteller,preis,freigabe,bestandsmenge) LIKE '%"+ssearch+"%'";
            adapter = new MySqlDataAdapter(searchquerry, connectionstring);
            dtable = new DataTable();
            adapter.Fill(dtable);
            dataGridView1.DataSource = dtable;
        }

        private void textBox1_search(object sender, EventArgs e)
        {
            searchDataDB(textBox1.Text);
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            
            string barcodeSelectedRow = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string deletequery = "DELETE FROM supermarket.produkte WHERE barcode=" + barcodeSelectedRow;

            try
            {
                connection = new MySqlConnection(connectionstring);
                connection.Open();

                command = new MySqlCommand(deletequery, connection);

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Daten Erfolgreich gelöscht");
                    searchDataDB("");

                }
                else
                {
                    MessageBox.Show("Daten konnten nicht gelöscht werden");
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
        }
    }
}
