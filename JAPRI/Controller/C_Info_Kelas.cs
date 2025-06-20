﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Windows.Forms;
using PBO_KelD08.JAPRI.Model;
using PBO_KelD08.JAPRI.View;
using System.Data;

namespace PBO_KelD08.JAPRI.Controller
{
    public class C_Info_Kelas
    {
        private C_Main_Menu mainMenu;
        public V_Info_Kelas v_info_kelas;
        M_Peserta m_peserta= new M_Peserta();
        M_PilihanJadwal m_pilihanjadwal = new M_PilihanJadwal();
        M_Jadwal m_jadwal = new M_Jadwal();
        V_Info_Kelas_Ketua_Page v_info_kelasketua;
        public C_Info_Kelas(C_Main_Menu mainMenu)
        {
            this.mainMenu = mainMenu;
            v_info_kelas = new V_Info_Kelas(this);
            v_info_kelasketua = new V_Info_Kelas_Ketua_Page(this);
        }


        public List<Data_Peserta_Kelas> ambilpeserta()
        {
            int id = mainMenu.ProfileController.getidkelas();
            var dataObjek = m_peserta.GetPeserta(id);

            List<Data_Peserta_Kelas> list = new List<Data_Peserta_Kelas>();
            foreach (var item in dataObjek)
            {
                if (item is Data_Peserta_Kelas peserta)
                {
                    list.Add(peserta);
                }
            }
            return list;
        }

        public void setkelas()
        {
            int id = mainMenu.ProfileController.getidkelas();
            DataTable data = m_peserta.Carikelas(id);

            string full = data.Rows[0]["nama_kelas"].ToString().Trim();
            int lastSpaceIndex = full.LastIndexOf(' ');

            if (lastSpaceIndex > 0)
            {
                string matkul = full.Substring(0, lastSpaceIndex);
                string kelas = full.Substring(lastSpaceIndex + 1);

                v_info_kelas.matkul.Text = matkul;
                v_info_kelas.kelass.Text = kelas;
            }
            else
            {
                // Fallback jika format tidak sesuai
                v_info_kelas.matkul.Text = full;
                v_info_kelas.kelass.Text = "-";
            }

        }
        public Data_Jadwal datakelasuser()
        {
            int id_kelas = mainMenu.ProfileController.getidkelas();
            Data_Jadwal jadwal = m_jadwal.GetInfoKelasUser(id_kelas);
            return jadwal;
        }

        public void updatenote(string note)
        {
            int id_kelas = mainMenu.ProfileController.getidkelas();
            int id_jadwal = m_pilihanjadwal.getidjadwalpengganti(id_kelas);

            DialogResult result = MessageBox.Show("Yakin ingin menyimpan catatan?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                m_pilihanjadwal.updatenotes(note, id_jadwal);

                MessageBox.Show("Catatan berhasil diperbarui.", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public Data_PilihanJadwal GetlistJadwalpengganti ()
        {
            int id_kelas = mainMenu.ProfileController.getidkelas();
            Data_PilihanJadwal data = m_pilihanjadwal.getjadwalpengganti(id_kelas);
            return data;
        }

        public Data_Akun GetDatakelas()
        {
            Data_Akun data = mainMenu.ProfileController.GetData();
            return data;
        }
        public void SwitchToProfile()
        {
            mainMenu.SwitchForm(v_info_kelas, mainMenu.ProfileController.GetView());
        }
        public void SwitchToInfoProfileKetua()
        {
            mainMenu.SwitchForm(v_info_kelasketua, mainMenu.ProfileController.GetViewKetua());
        }

        public void SwitchToInfoJadwal()
        {
            mainMenu.SwitchForm(v_info_kelas, mainMenu.JadwalController.GetView());
        }

        public void SwitchToRuangKelas()
        {
            mainMenu.SwitchForm(v_info_kelas, mainMenu.RuangKelasController.GetView());
        }

        public void SwitchToGantiJadwal()
        {
            mainMenu.SwitchForm(v_info_kelas, mainMenu.GantiJadwalController.GetView());
        }
        public Form GetView()
        {
            return v_info_kelas;

        }
        public Form GetViewKetua() 
        {
            return v_info_kelasketua;
        }
    }
}
