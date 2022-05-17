using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace TSS_APP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Leer_Archivo_btn(object sender, EventArgs e)
        {
            

        }

        private void Cargar_Informacion_btn(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("");

            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
                ofd.FilterIndex = 1;

                ofd.ShowDialog();

                string archivo = ofd.ToString();

                if (archivo.Trim().Length > 0)
                    using (StreamReader sr = new StreamReader(archivo))
                    {

                        DataTable dt = new DataTable();


                        String line = sr.ReadToEnd();
                        dataGridView1.DataSource = dt;
                    }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo a ocurrido un error: " + ex.Message + ex.StackTrace);
            }

        }
    }
}
