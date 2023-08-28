using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace CloudDemo
{
    public class EnrollData
    {
        string mDataPath;
        public static EnrollData DataModule;

        private OleDbConnection GetConnection()
        {
            return (new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + mDataPath + "datEnrollDat.mdb"));
			//return (new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + mDataPath + "datEnrollDat.mdb"));
        }

        public DataSet GetEnrollDatas()
        {
            return this.GetEnrollDatas("EnrollNumber");
        }

        public DataSet GetEnrollDatas(string sortfield)
        {
            OleDbConnection conn = GetConnection();
            try
            {
                DataSet ds = new DataSet();
                string sql = "select * from tblEnroll order by " + sortfield;
                OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);
                try
                {
                    da.Fill(ds, "tblEnroll");
                }
                finally
                {
                    da.Dispose();
                }
                return ds;
            }
            finally
            {            	
                conn.Close();
                conn.Dispose();
            }
        }

        public DataSet DeleteDB()
        {
            OleDbConnection conn = GetConnection();

            try
            {
                string sql = "Select * from tblEnroll";
                OleDbDataAdapter da  = new OleDbDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                try
                {
                    da.Fill(ds, "tblEnroll");

                    foreach (DataRow dbRow in ds.Tables[0].Rows)
                    {
                        dbRow.Delete();
                    }
                }
                finally
                {
                    da.Dispose();
                }
                return ds;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public void SaveEnrolls(DataSet ds)
        {
            OleDbConnection conn = GetConnection();

            try
            {
                string sql = "select * from tblEnroll";
                OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);

                try
                {
                    da.InsertCommand = cb.GetInsertCommand(true);
                    if (ds.HasChanges())
                    {
                        da.Update(ds, "tblEnroll");
                        ds.AcceptChanges();
                    }
                }
                finally
                {
                    cb.Dispose();
                    da.Dispose();
                }
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

        }

        public void New(string sDatapath)
        {
            this.mDataPath = sDatapath;
            EnrollData.DataModule = this;
        }
    }
}
