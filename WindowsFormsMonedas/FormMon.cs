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

namespace WindowsFormsMonedas
{
    public partial class FormMoneda : Form
    {
        
        public FormMoneda()
        {
            InitializeComponent();
        }

        DetallesMon Datos = new DetallesMon();

        

        private void btnNuevo_Click(object sender, EventArgs e)


        {
            int codigo;

            SqlConnection con = new SqlConnection(@"data source=thinkpad\sqlexpress; initial catalog=Moneda; integrated security=true");

            string query = "Select max (IDMoneda) as Mayor from Monedas ";

            SqlCommand comando = new SqlCommand(query, con);

            con.Open();

            codigo = Convert.ToInt32(comando.ExecuteScalar()) + 1;

            txtID.Text = codigo.ToString();
            con.Close();

            txtMoneda.Focus();

            txtMoneda.Clear();
            txtTasa1.Clear();

        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {


            float Num1 = 0, Num2 = 0, Resultado;

            Num1 = float.Parse(txtCantidad.Text);

            Num2 = float.Parse(txtTasa2.Text);

            Resultado = Num1 * Num2;

            lblResultado.Text = "RD$" + Resultado + "DOP".ToString();

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtTasa2.Clear();
            txtCantidad.Clear();
            lblResultado.Text = "";
        }


        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            DetallesMon.Numeros(e);
        }

        private void txtMoneda_KeyPress(object sender, KeyPressEventArgs e)
        {
            DetallesMon.SoloLetras(e);
        }

        private void txtConsulta_KeyPress(object sender, KeyPressEventArgs e)
        {
            //DetallesMon.SoloLetras(e);
        }

        private void cbxMoneda_KeyPress(object sender, KeyPressEventArgs e)
        {
            DetallesMon.SoloLetras(e);
        }

        private void txtTasa1_KeyPress(object sender, KeyPressEventArgs e)
        {
            DetallesMon.Numeros(e);
        }

        private void txtTasa2_KeyPress(object sender, KeyPressEventArgs e)
        {
            DetallesMon.Numeros(e);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            int ID;
            float tasa;
            string moneda;

            ID = int.Parse(txtID.Text);
            tasa = float.Parse(txtTasa1.Text);
            moneda = txtMoneda.Text;



            if (moneda == "")


            {
                MessageBox.Show("Debe de completar el campo vacio");

                txtMoneda.Focus();
            }



            else if (tasa == 0)

            {
                MessageBox.Show("Debe de completar el campo vacio");

                txtTasa1.Focus();
            }


            else if (ID == 0)

            {
                MessageBox.Show("Debe de completar el campo vacio");

                txtID.Focus();
            }


            else

            {


                SqlConnection con = new SqlConnection(@"Data source=thinkpad\sqlexpress; initial catalog=Moneda; integrated security=true");

                string strsql = "PS_Insertar_Monedas";


                SqlCommand cmd = new SqlCommand(strsql, con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDMoneda", int.Parse(txtID.Text));
                cmd.Parameters.AddWithValue("@NomMoneda", txtMoneda.Text);
                cmd.Parameters.AddWithValue("@TasaMoneda", float.Parse(txtTasa1.Text));


                cmd.ExecuteNonQuery();
                con.Close();

                mostrard();

                MessageBox.Show("Moneda Guardado o Actualiazo Correctamente");

            }


        }

        void Eliminar()

        {
            SqlConnection con = new SqlConnection(@"Data source=thinkpad\sqlexpress; initial catalog=Moneda; integrated security=true");


            DialogResult resul = MessageBox.Show("Seguro que quiere eliminar el Registro?", "Eliminar Registro", MessageBoxButtons.YesNo);

            if (resul == DialogResult.Yes)
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("Delete from Monedas where IDMoneda=" + Convert.ToInt32(this.txtID.Text + ""), con);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Registro Eliminado Correctamente");

                con.Close();


            }

            txtID.Clear();
            txtMoneda.Clear();
            txtTasa1.Clear();
            
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {

            Eliminar();

            mostrard();

        }


        public void mostrard()

        {

            Datos.Examinar("select * from Monedas ", "Monedas");
            dgvMonedas.DataSource = Datos.ds.Tables["Monedas"];

        }

        private void dgvMonedas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            int fila = dgvMonedas.CurrentRow.Index;
            txtID.Text = dgvMonedas.Rows[fila].Cells[0].Value.ToString();
            txtMoneda.Text = dgvMonedas.Rows[fila].Cells[1].Value.ToString();
            txtTasa1.Text = dgvMonedas.Rows[fila].Cells[2].Value.ToString();
            

        }

        private void FormMoneda_Load(object sender, EventArgs e)
        {

            SqlConnection cone = new SqlConnection(@"Data source=thinkpad\sqlexpress; initial catalog=Moneda; integrated security=true");


            SqlCommand cm = new SqlCommand("select * from Monedas", cone);

            cone.Open();

            SqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())

            {
                cbxMoneda.Items.Add(dr.GetString(1));

            }


            cone.Close();

            mostrard();

            DetallesMon Datos = new DetallesMon();

            dgvMonedas.DataSource = Datos.Consulta("Select top 10 IDMoneda as ID, NomMoneda as Moneda, TasaMoneda as Tasa from Monedas");


        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            //DetallesMon Datos = new DetallesMon();

            //dgvMonedas.DataSource = Datos.Consulta("Select top 10 IDMoneda as ID, NomMoneda as Moneda, TasaMoneda as Tasa from Monedas  where NomMoneda like '%" + txtConsulta.Text + "%'");

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea Salir?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void cbxMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data source=thinkpad\sqlexpress; initial catalog=Moneda; integrated security=true");


            SqlCommand cm = new SqlCommand("select * from Monedas where NomMoneda ='" + cbxMoneda.Text + "'", con);

            con.Open();

            SqlDataReader dr = cm.ExecuteReader();

            if (dr.Read() == true)


            {

                txtTasa2.Text = dr["TasaMoneda"].ToString();

            }

            mostrard();

            con.Close();

           

        }

        private void txtConsulta_KeyUp(object sender, KeyEventArgs e)
        {

            SqlConnection con = new SqlConnection(@"data source=thinkpad\sqlexpress; initial catalog=Moneda; integrated security=true");


            string nombre, id;

            nombre = cmbConsulta.Text;
            id =cmbConsulta.Text ;


            con.Open();

            SqlCommand cmd = con.CreateCommand();


            cmd.CommandType = CommandType.Text;


            if (nombre == "Nombre")

            {

                cmd.CommandText = "Select * from Monedas Where NomMoneda  like ('" + txtConsulta.Text + "%')";

            }

            else if (id == "ID")


            {
                cmd.CommandText = "Select * from Monedas Where IDMoneda  like ('" + txtConsulta.Text + "%')";
            }


            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            dgvMonedas.DataSource = dt;

            con.Close();


        }
    }


    }

