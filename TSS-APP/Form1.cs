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
        string selectedFileName = "";
        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
        }

        private void Leer_Archivo_btn(object sender, EventArgs e)
        {

            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                //Filtro de tipo de archivo.
                ofd.Filter = "All Files (*.*)|*.*";
                //Indice establecido del filtro.
                ofd.FilterIndex = 1;
                //Mostrar el dialogo.
                ofd.ShowDialog();

                //Contiene una cadena con la ruta del archivo.
                selectedFileName = ofd.FileName;
                

                if (selectedFileName.Trim().Length > 0)
                {
                    button2.Enabled = true;
                    
                    using (StreamReader sr = new StreamReader(selectedFileName))
                    {
                        //Leer los caracteres del archivo desde la posicion actual hasta el final.
                        String line = sr.ReadToEnd();
                        //Mostrar los caracteres en un textbox.
                        textBox1.Text = line;
                    }
                }
                    

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error: " + ex.Message + ex.StackTrace);
            }
        }

        private void Cargar_Informacion_btn(object sender, EventArgs e)
        {

            if (selectedFileName.Trim().Length > 0)
            {

                try
                {
                    SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=TSSDB;Integrated Security=True");
                    conn.Open(); 

                    using (StreamReader file = new StreamReader(selectedFileName))
                    {
                        string line;
                        string rnc = "";
                        string fecha = "";

                        while ((line = file.ReadLine()) != null)
                        {
                            string[] registro = line.Split('|');

                            string idNomina;
                            string cedula;
                            string nombre;
                            string primerApellido;
                            string segundoApellido;
                            string sexo;
                            string fechaNacimiento;
                            string salario;

                            if (registro[0] == "E")
                            {
                                rnc = registro[1];
                                fecha = registro[2];
                                fecha = fecha.Substring(4) + "-" + fecha.Substring(2, 2) + "-" + fecha.Substring(0, 2);


                                SqlCommand cmd = new SqlCommand("if not exists (SELECT * FROM empresas WHERE rnc = @rnc) begin insert into empresas values (@rnc) end;", conn);
                                cmd.Parameters.AddWithValue("@rnc", rnc);
                                int i = cmd.ExecuteNonQuery(); 

                                if (i == 0) throw new Exception("Hubo un error de SQL"); 
                            }

                            if (registro[0] == "D")
                            {
                                idNomina = registro[1];
                                cedula = registro[2];
                                nombre = registro[3];
                                primerApellido = registro[4];
                                segundoApellido = registro[5];
                                sexo = registro[6];
                                fechaNacimiento = registro[7];
                                salario = registro[8];
                                
                                // Construir  el comando
                                SqlCommand cmd = new SqlCommand("insert into nominas(" +
                                    "id_nomina, " +
                                    "cedula, " +
                                    "nombres, " +
                                    "primer_apellido, " +
                                    "segundo_apellido, " +
                                    "sexo, " +
                                    "fecha_nacimiento, " +
                                    "salario, " +
                                    "rnc_empresa, " +
                                    "fecha_autodeterminacion) " +
                                    "values (" +
                                    "@id_nomina, " +
                                    "@cedula," +
                                    "@nombres," +
                                    "@primer_apellido," +
                                    "@segundo_apellido," +
                                    "@sexo," +
                                    "@fecha_nacimiento," +
                                    "@salario," +
                                    "@rnc_empresa," +
                                    "@fecha_autodeterminacion" +
                                    ")", conn);

                                // cambiar el formato de la fecha a SQL DATE
                                fechaNacimiento = DateTime.Parse(fechaNacimiento).ToString("yyyy-MM-dd");

                               
                                // cargar las variables al comando
                                cmd.Parameters.AddWithValue("@id_nomina", idNomina);
                                cmd.Parameters.AddWithValue("@cedula", cedula);
                                cmd.Parameters.AddWithValue("@nombres", nombre);
                                cmd.Parameters.AddWithValue("@primer_apellido", primerApellido);
                                cmd.Parameters.AddWithValue("@segundo_apellido", segundoApellido);
                                cmd.Parameters.AddWithValue("@sexo", sexo);
                                cmd.Parameters.AddWithValue("@fecha_nacimiento", fechaNacimiento);
                                cmd.Parameters.AddWithValue("@salario", salario);
                                cmd.Parameters.AddWithValue("@rnc_empresa", rnc);
                                cmd.Parameters.AddWithValue("@fecha_autodeterminacion", fecha);

                                int i = cmd.ExecuteNonQuery();
                                if (i == 0) throw new Exception("Hubo un error de SQL");
                            }
                        }
                        conn.Close();
                        MessageBox.Show("Datos guardados satisfactoriamente.");
                    }
                }
                catch (Exception ex)
                { 
                    MessageBox.Show(ex.Message + ex.StackTrace);
                }
            } else
            {
                MessageBox.Show("Por favor, seleccione un archivo antes de cargar a la base de datos.");
            }
                
        }
    }
}
