using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GerenciadorFinanceiro
{
    public partial class Form1 : Form
    {
        private string connectionString = "server=localhost;database=GerenciadorFinanceiro;userid=root;password=@Divinh@2025;";
        public Form1()
        {
            InitializeComponent();
            CarregarTransacoes();
        }

        private void CarregarTransacoes()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Transacoes";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvTransacoes.DataSource = dt;
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validação de entrada
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                MessageBox.Show("A descrição não pode estar vazia.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtValor.Text, out decimal valor) || valor <= 0)
            {
                MessageBox.Show("O valor deve ser um número positivo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbTipo.SelectedItem == null || (cmbTipo.SelectedItem.ToString() != "Receita" && cmbTipo.SelectedItem.ToString() != "Despesa"))
            {
                MessageBox.Show("Selecione um tipo válido (Receita ou Despesa).", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string descricao = txtDescricao.Text;
            string tipo = cmbTipo.SelectedItem.ToString();
            DateTime data = dtpData.Value;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Transacoes (Descricao, Valor, Tipo, Data) VALUES (@Descricao, @Valor, @Tipo, @Data)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Descricao", descricao);
                cmd.Parameters.AddWithValue("@Valor", valor);
                cmd.Parameters.AddWithValue("@Tipo", tipo);
                cmd.Parameters.AddWithValue("@Data", data);
                cmd.ExecuteNonQuery();
            }

            CarregarTransacoes();
        }

        private void lblDataTransacao_Click(object sender, EventArgs e)
        {

        }
    }
 
}
