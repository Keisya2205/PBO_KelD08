﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBO_KelD08.JAPRI.Controller;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PBO_KelD08.JAPRI.Model
{
    public class M_PilihanJadwal : AM_Connectdb, IM_Connectdb
    {

        public List<Data_PilihanJadwal> GetPilihanJadwal(DateTime tanggal, int id_ruangan)
        {
            string tanggalStr = tanggal.ToString("yyyy-MM-dd");
            DataTable data = Execute_With_Return($"SELECT pjk.tanggal, pjk.jam_mulai, pjk.jam_selesai, pjk.id_ruangan, r.nama_ruangan " +
                $"FROM pilihan_jadwal_asisten pjk " +
                $"join ruangan r on ( pjk.id_ruangan=r.id_ruangan) " +
                $"WHERE pjk.tanggal = '{tanggalStr}' AND pjk.id_ruangan = {id_ruangan}");

            List<Data_PilihanJadwal> list = new List<Data_PilihanJadwal>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                Data_PilihanJadwal data_pilihanjadwal = new Data_PilihanJadwal
                {
                    tanggal = Convert.ToDateTime(data.Rows[i]["tanggal"]),
                    jam_mulai = (TimeSpan)data.Rows[i]["jam_mulai"],
                    jam_selesai = (TimeSpan)data.Rows[i]["jam_selesai"],
                    id_ruangan = (int)data.Rows[i]["id_ruangan"],
                    nama_ruangan = data.Rows[i]["nama_ruangan"].ToString(),
                };
                list.Add(data_pilihanjadwal);
            }
            return list;
        }

        public DataTable GetPilihanJadwalDefault(DateTime tanggal, int id_ruangan)
        {
            string hari = tanggal.ToString("dddd", new CultureInfo("id-ID")); // ex : "Senin"
            DataTable data = Execute_With_Return($"SELECT jk.jam_mulai, jk.jam_selesai FROM jadwal_kelas jk WHERE jk.hari = '{hari}' AND jk.id_ruangan = {id_ruangan}");
            return data;
        }
        public DataTable GetRuangan()
        {
            DataTable data = Execute_With_Return($"SELECT id_ruangan, nama_ruangan from ruangan");
            return data;
        }
        public List<object> Get()
        {
            return new List<object>();
        }

        public void Insert(object item)
        {
            Data_PilihanJadwal akun = item as Data_PilihanJadwal;
            Execute_No_Return($"INSERT INTO pilihan_jadwal_asisten(id_kelas,id_ruangan,tanggal,status,jam_mulai,jam_selesai) " +
                $"Values ({akun.id_kelas},{akun.id_ruangan},'{akun.tanggal}','Pending','{akun.jam_mulai}','{akun.jam_selesai}')");
        }

        public bool cekexist(Data_PilihanJadwal data) 
        {
            DateTime tanggal = data.tanggal;
            int offsetHari = (int)tanggal.DayOfWeek - (int)DayOfWeek.Monday;
            if (offsetHari < 0) offsetHari += 7;

            DateTime tanggalawal = tanggal.AddDays(-offsetHari);

            string awalMinggu = tanggalawal.ToString("yyyy-MM-dd");// Senin
            string akhirMinggu = tanggalawal.AddDays(6).ToString("yyyy-MM-dd");     // Minggu

            //string tglAwal = awalMinggu.ToString("yyyy-MM-dd");
            //string tglAkhir = akhirMinggu.ToString("yyyy-MM-dd");

            DataTable jadwal= Execute_With_Return($"Select * from pilihan_jadwal_asisten where tanggal between '{awalMinggu}' and '{akhirMinggu}' and id_kelas= {data.id_kelas}");
            if (jadwal.Rows.Count != 0)
            { return false; }
            else { return true; }
        }

        public Data_PilihanJadwal getjadwalpengganti(int id_kelas)
        {
            DateTime tanggal = DateTime.Now;
            int offsetHari = (int)tanggal.DayOfWeek - (int)DayOfWeek.Monday;
            if (offsetHari < 0) offsetHari += 7;

            DateTime tanggalawal = tanggal.AddDays(-offsetHari);

            string awalMinggu = tanggalawal.ToString("yyyy-MM-dd");// Senin
            string akhirMinggu = tanggalawal.AddDays(6).ToString("yyyy-MM-dd");     // Minggu

            DataTable data = Execute_With_Return($"SELECT pjk.tanggal, pjk.jam_mulai, pjk.jam_selesai, pjk.id_ruangan, r.nama_ruangan " +
                $"FROM pilihan_jadwal_asisten pjk " +
                $"join ruangan r on (pjk.id_ruangan=r.id_ruangan) " +
                $"where pjk.tanggal between '{awalMinggu}' and '{akhirMinggu}' and pjk.id_kelas= {id_kelas}");

            Data_PilihanJadwal data_pilihanjadwal = new Data_PilihanJadwal
            {
                tanggal = Convert.ToDateTime(data.Rows[0]["tanggal"]),
                jam_mulai = (TimeSpan)data.Rows[0]["jam_mulai"],
                jam_selesai = (TimeSpan)data.Rows[0]["jam_selesai"],
                id_ruangan = (int)data.Rows[0]["id_ruangan"],
                nama_ruangan = data.Rows[0]["nama_ruangan"].ToString(),
            };
            return data_pilihanjadwal;
        }
        public int getidjadwalpengganti(int id_kelas)
        {
            DateTime tanggal = DateTime.Now;
            int offsetHari = (int)tanggal.DayOfWeek - (int)DayOfWeek.Monday;
            if (offsetHari < 0) offsetHari += 7;

            DateTime tanggalawal = tanggal.AddDays(-offsetHari);

            string awalMinggu = tanggalawal.ToString("yyyy-MM-dd");// Senin
            string akhirMinggu = tanggalawal.AddDays(6).ToString("yyyy-MM-dd");     // Minggu

            DataTable data = Execute_With_Return($"SELECT pjk.id_pilihan, pjk.tanggal, pjk.jam_mulai, pjk.jam_selesai, pjk.id_ruangan, r.nama_ruangan " +
                $"FROM pilihan_jadwal_asisten pjk " +
                $"join ruangan r on (pjk.id_ruangan=r.id_ruangan) " +
                $"where pjk.tanggal between '{awalMinggu}' and '{akhirMinggu}' and pjk.id_kelas= {id_kelas}");

            Data_PilihanJadwal data_pilihanjadwal = new Data_PilihanJadwal
            {
                id_pilihan = (int)data.Rows[0]["id_pilihan"],
                tanggal = Convert.ToDateTime(data.Rows[0]["tanggal"]),
                jam_mulai = (TimeSpan)data.Rows[0]["jam_mulai"],
                jam_selesai = (TimeSpan)data.Rows[0]["jam_selesai"],
                id_ruangan = (int)data.Rows[0]["id_ruangan"],
                nama_ruangan = data.Rows[0]["nama_ruangan"].ToString(),
            };
            return data_pilihanjadwal.id_pilihan;
        }


        public List<Data_PilihanJadwal> ambiljadwal(int id)
        {
            DataTable jadwal = Execute_With_Return($"Select pjk.id_pilihan, pjk.tanggal,pjk.jam_mulai,pjk.jam_selesai,r.nama_ruangan, pjk.note from pilihan_jadwal_asisten pjk " +
                $"join ruangan r on (pjk.id_ruangan=r.id_ruangan) " +
                $"where id_kelas= {id}");
            List<Data_PilihanJadwal> list = new List<Data_PilihanJadwal>();

            for (int i = 0; i < jadwal.Rows.Count; i++)
            {
                Data_PilihanJadwal data_jadwal = new Data_PilihanJadwal

                {
                    id_pilihan = Convert.ToInt32(jadwal.Rows[i]["id_pilihan"]),
                    id_kelas = id,
                    tanggal = Convert.ToDateTime(jadwal.Rows[i]["tanggal"]),
                    jam_mulai = (TimeSpan)jadwal.Rows[i]["jam_mulai"],
                    jam_selesai = (TimeSpan)jadwal.Rows[i]["jam_selesai"],
                    nama_ruangan = jadwal.Rows[i]["nama_ruangan"].ToString(),
                    note = jadwal.Rows[i]["note"].ToString(),
                };
                list.Add(data_jadwal);
            }
            return list;
        }
        public void updatenotes(string note,int id)
        {
            Execute_No_Return($"Update pilihan_jadwal_asisten set note = '{note}' where id_pilihan = {id}");
        }
        public void Delete(int ID)
        {
            Execute_No_Return($"DELETE FROM pilihan_jadwal_asisten WHERE id_pilihan = {ID}");
        }
        public DataTable GetDurasi(int id)
        {
            string query = @"
        SELECT jk.jam_mulai, jk.jam_selesai 
        FROM kelas k
        JOIN jadwal_kelas jk ON k.id_jadwal = jk.id_jadwal
        WHERE k.id_kelas = @id AND k.id_jadwal IS NOT NULL
    ";
            DataTable data = Execute_With_Return(query.Replace("@id", id.ToString()));

            return data;
           
        }
        public void Update(object data, int id)
        {
        }
    }
    }
