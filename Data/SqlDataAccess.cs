﻿using Microsoft.AspNetCore.Identity;
using System.CodeDom;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Reflection.Emit;
using Zencareservice.Models;

namespace Zencareservice.Data
{
    public class SqlDataAccess
    {
        string connectionString = @"Data Source=GOPI\SQLEXPRESS;Initial Catalog = zencareservice; User Id = sa; Password=Devops@22;";

        public string DbConnectionString { get; set; }

		public IConfiguration _configuration;

	
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        public SqlDataAccess()
        {
               

        DbConnectionString = _configuration.GetConnectionString("DefaultConnection");
      

    }
        public DataSet GetDataWithStoredprocedure(string StrSpName)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {


                SqlConnection con = new SqlConnection(connectionString);
                SqlDataAdapter da = new SqlDataAdapter(StrSpName, con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();

            }
            return ds;
        }
        public DbSet<Signup> Roles { get; set; }

    
        public DataSet GetDataWithParamStoredprocedure(string StrSpName, SqlParameter[] Param)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {

                SqlConnection con = new SqlConnection(connectionString);
                SqlDataAdapter da = new SqlDataAdapter();
                //da.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd = new SqlCommand(StrSpName, con);
                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                for (int i = 0; i < Param.Length; i++)
                {
                    da.SelectCommand.Parameters.Add(Param[i]);
                }
     

                da.Fill(ds);


            }
            catch (Exception ex)
            {

                throw ex;
            }


            return ds;
        }

    }
}
