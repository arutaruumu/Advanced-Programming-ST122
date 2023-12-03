using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PerpustakaanAppMVC.Model.Entity;
using PerpustakaanAppMVC.Controller;

using System.Data.SQLite;
using PerpustakaanAppMVC.Model.Context;

namespace PerpustakaanAppMVC.View
{
    public partial class FrmMahasiswa : Form
    {
        private List<Mahasiswa> listOfMahasiswa = new List<Mahasiswa>();
        private MahasiswaController controller;

        private SQLiteConnection _conn;

        private SQLiteConnection GetOpenConnection()
        {
            string dbName = @"C:\Users\LENOVO\PEMROG LANJUT\Laporan-Praktikum\Perpustakaan.db";

            string connectionString = string.Format("Data Source={0};FailIfMissing=True", dbName);
            SQLiteConnection conn = new SQLiteConnection(connectionString);


            try
            {
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        public FrmMahasiswa()
        {
            InitializeComponent();
            controller = new MahasiswaController();
            InisialisasiListView();
            LoadDataMahasiswa();
        }

        private void lvwMahasiswa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void InisialisasiListView()
        {
            lvwMahasiswa.View = System.Windows.Forms.View.Details;
            lvwMahasiswa.FullRowSelect = true;
            lvwMahasiswa.GridLines = true;
            lvwMahasiswa.Columns.Add("No.", 35, HorizontalAlignment.Center);
            lvwMahasiswa.Columns.Add("Npm", 91, HorizontalAlignment.Center);
            lvwMahasiswa.Columns.Add("Nama", 350, HorizontalAlignment.Left);
            lvwMahasiswa.Columns.Add("Angkatan", 80, HorizontalAlignment.Center);
        }

        private void LoadDataMahasiswa()
        {
            lvwMahasiswa.Items.Clear();
            listOfMahasiswa = controller.ReadAll();
            foreach (var mhs in listOfMahasiswa)
            {
                var noUrut = lvwMahasiswa.Items.Count + 1;

                var item = new ListViewItem(noUrut.ToString());
                item.SubItems.Add(mhs.Npm);
                item.SubItems.Add(mhs.Nama);
                item.SubItems.Add(mhs.Angkatan);

                lvwMahasiswa.Items.Add(item);
            }
        }

        private void OnCreateEventHandler(Mahasiswa mhs)
        {
            listOfMahasiswa.Add(mhs);
            int noUrut = lvwMahasiswa.Items.Count + 1;
            ListViewItem item = new ListViewItem(noUrut.ToString());
            item.SubItems.Add(mhs.Npm);
            item.SubItems.Add(mhs.Nama);
            item.SubItems.Add(mhs.Angkatan);
            lvwMahasiswa.Items.Add(item);
        }

        private void OnUpdateEventHandler(Mahasiswa mhs)
        {
            int index = lvwMahasiswa.SelectedIndices[0];

            ListViewItem itemRow = lvwMahasiswa.Items[index];
            itemRow.SubItems[1].Text = mhs.Npm;
            itemRow.SubItems[2].Text = mhs.Nama;
            itemRow.SubItems[3].Text = mhs.Angkatan;
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            FrmEntryMahasiswa frmEntry = new FrmEntryMahasiswa("Tambah Data Mahasiswa", controller);

            frmEntry.OnCreate += OnCreateEventHandler;
            frmEntry.ShowDialog();
        }

        private void btnPerbaiki_Click(object sender, EventArgs e)
        {
            if (lvwMahasiswa.SelectedItems.Count > 0)
            {
                Mahasiswa mhs = listOfMahasiswa[lvwMahasiswa.SelectedIndices[0]];
                FrmEntryMahasiswa frmEntry = new FrmEntryMahasiswa("Edit Data Mahasiswa", mhs, controller);

                frmEntry.OnUpdate += OnUpdateEventHandler;
                frmEntry.ShowDialog();
            }
            else
            {
                MessageBox.Show("Data belum dipilih", "Peringatan", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (lvwMahasiswa.SelectedItems.Count > 0)
            {
                var konfirmasi = MessageBox.Show("Apakah data mahasiswa ingin dihapus?", "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (konfirmasi == DialogResult.Yes)
                {
                    Mahasiswa mhs = listOfMahasiswa[lvwMahasiswa.SelectedIndices[0]];
                    var result = controller.Delete(mhs);
                    if (result > 0) LoadDataMahasiswa();
                }
            }
            else
            {
                MessageBox.Show("Data mahasiswa belum dipilih !!!", "Peringatan",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNama.Text))
            {
                MessageBox.Show("Nama harus diisi!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNama.Focus();
                return;
            }

            using (SQLiteConnection conn = GetOpenConnection())
            {
                try
                {
                    string sql = @"select npm, nama, angkatan from mahasiswa where nama like @nama";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@nama", "%" + txtNama.Text + "%");

                        using (SQLiteDataReader dtr = cmd.ExecuteReader())
                        {
                            if (dtr.Read())
                            {
                                string npm = dtr["npm"].ToString();
                                MessageBox.Show("Data mahasiswa ditemukan!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Data mahasiswa tidak ditemukan!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}
