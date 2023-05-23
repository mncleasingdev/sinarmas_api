using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using NLog;
using System.Threading.Tasks;

namespace Polis_Sinarmas_API
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            try
            {
                string sppah = "";
                string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))

                //BEFORE UPDATE
                //using (SqlCommand cmd = new SqlCommand(@"SELECT 
                //                                       AG.LSAGREE
                //                                          FROM IFINANCING_GOLIVE.dbo.LS_INS_SPPA_D d with(NOLOCK)
                //                                          LEFT JOIN IFINANCING_GOLIVE.dbo.LS_INS_SPPA_H H with(NOLOCK) ON D.SPPA_NO = H.SPPA_NO  
                //                                          LEFT JOIN IFINANCING_GOLIVE.dbo.LS_AGREEMENT AG with(NOLOCK) ON D.LSAGREE = AG.LSAGREE
                //                                          WHERE 
                //                                       AG.PRODUCT_FACILITY_CODE IN (112,215)
                //                                       AND D.STATUS = 'SENT'
                //                                       AND D.INSURERNO = 'ASMSY'
                //                                       AND D.POLICY_NO IS NULL
                //                                       AND AG.LSAGREE NOT IN (SELECT LSAGREE_ID FROM trxResultSINARMAS with(NOLOCK))", conn))

                using (SqlCommand cmd = new SqlCommand(@"SELECT LSAGREE FROM LS_AGREEMENT
                                                        WHERE LSAGREE NOT IN (SELECT LSAGREE FROM LS_INS_SPPA_D WHERE LSAGREE IS NOT NULL)
                                                        AND CONTRACT_STATUS = 'GOLIVE'
                                                        AND DISBURSEDT IS NOT NULL
                                                        AND PRODUCT_FACILITY_CODE IN ('112','215')", conn))

                {
                    
                    string lsagree = "";
                    DataTable resDT = new DataTable();

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(resDT);

                    if (resDT.Rows.Count > 0)
                    {
                        

                        foreach (DataRow dr2 in resDT.Rows)
                            {
                                //BIKIN SPPA H
                                CREATE_SPPA_H();

                                //SELECT SPPA
                                sppah = SPPAHDESC();

                                lsagree = dr2["LSAGREE"].ToString();

                                var tmpDtTable = new DataTable();
                                var dtFind = from row in resDT.AsEnumerable()
                                             select row;
                                var dtreqPOL = dtFind.CopyToDataTable();

                                conn.Close();

                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine(string.Format("Polis Asuransi Sinarmas Syariah Dengan Nomor Kontrak : {0} Execute", lsagree));
                                Console.ForegroundColor = ConsoleColor.White;

                                CreatePolisManager APIClass = new CreatePolisManager();

                                tmpDtTable = new DataTable();
                                tmpDtTable = GetTempData(lsagree);
                            
                                var reqPol = await APIClass.CreateApp(tmpDtTable);

                                if (reqPol.Substring(0, 6) == "Sukses")
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine(string.Format("Polis dengan Nomor Kontrak : {0} Created", lsagree));
                                    Console.ForegroundColor = ConsoleColor.White;

                                    //INSERT SPPA_D
                                    CREATE_SPPA_D(lsagree, sppah);

                                    //updateSPPA();
                                }

                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(string.Format("Polis dengan Nomor Kontrak : {0} Failed, Required Parameter Missing / Request server error", lsagree));
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                        }
                    }

                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("There's no Polis Ready to Create");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                POST_SPPA(sppah);
                updateSPPA();
            }
            catch (Exception ex)

            {
                ex.Message.ToString();
            }
            
            return;
        }

        //Inquiry Data
        static DataTable GetTempData(string value)
        {
            string ssql = "exec [dbo].[spMNCL_getClient_SINARMAS] '" + value + "'";
            DataTable resDT = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            SqlConnection myconn = new SqlConnection(connString);
            myconn.Open();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(ssql);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = myconn;

                SqlDataReader reader = sqlCommand.ExecuteReader();
                resDT.Load(reader);
            }

            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            finally
            {
                myconn.Close();
            }

            return resDT;
        }
        

        static DataTable updateSPPA()
        {
            string ssql = "exec [dbo].[SP_SIMASSYARIAH]";
            DataTable resDT = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            SqlConnection myconn = new SqlConnection(connString);
            myconn.Open();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(ssql);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = myconn;

                SqlDataReader reader = sqlCommand.ExecuteReader();
                resDT.Load(reader);
            }

            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            finally
            {
                myconn.Close();
            }

            return resDT;
        }

        static DataTable CREATE_SPPA_H()
        {
            string ssql = @"declare @p1 int, @datenow datetime, @datenowzero datetime
                            set @p1 = (select max(id + 1)from LS_INS_SPPA_H)
                            set @datenow = GETDATE()
                            set @datenowzero = (select CAST(CAST(GETDATE() AS DATE) AS DATETIME))
                            exec sp_ls_ins_sppa_h_insert @p_id = @p1 output,@p_sppa_no = N'-Auto Generate-',@p_sppa_date = @datenowzero,@p_c_code = N'0000',@p_insurance_code = N'ASMSY',@p_notes = N'SPPA DANA HAJI',@p_type = N'A',@p_cre_date = @datenowzero,@p_cre_by = N'1101116',@p_cre_ip_address = N'202.147.201.66',@p_mod_date = @datenow,@p_mod_by = N'1101116',@p_mod_ip_address = N'202.147.201.66',@p_policy_stat = N'N'";
            DataTable resDT = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            SqlConnection myconn = new SqlConnection(connString);
            myconn.Open();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(ssql);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = myconn;

                SqlDataReader reader = sqlCommand.ExecuteReader();
                resDT.Load(reader);
            }

            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            finally
            {
                myconn.Close();
            }

            return resDT;
        }

        static DataTable CREATE_SPPA_D(string contract, string sppah)
        {
            string ssql = @"declare @p1 int, @datenow datetime set @p1 = (select id from LS_ASSETVEHICLE_INS_TEMP where LSAGREE = '"+ contract +"') set @datenow = GETDATE()exec sp_ls_ins_sppa_d_insert @p_temp_id=@p1,@p_sppa_no='"+ sppah +"',@p_cre_date=@datenow,@p_cre_by=N'1101116',@p_cre_ip_address=N'202.147.201.66',@p_mod_date=@datenow,@p_mod_by=N'1101116',@p_mod_ip_address=N'202.147.201.66'";
            DataTable resDT = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            SqlConnection myconn = new SqlConnection(connString);
            myconn.Open();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(ssql);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = myconn;

                SqlDataReader reader = sqlCommand.ExecuteReader();
                resDT.Load(reader);
            }

            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            finally
            {
                myconn.Close();
            }

            return resDT;
        }

        static DataTable POST_SPPA(string sppa)
        {
            string ssql = @"declare @p1 int, @datenow datetime set @p1 = (select id from LS_INS_SPPA_H where SPPA_NO = '"+ sppa + "') set @datenow = GETDATE() exec sp_ls_ins_sppa_h_post @p_id=@p1,@p_status=N'SENT',@p_mod_date=@datenow,@p_mod_by=N'1101116',@p_mod_ip_address=N'202.147.201.66'";
            DataTable resDT = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            SqlConnection myconn = new SqlConnection(connString);
            myconn.Open();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(ssql);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = myconn;

                SqlDataReader reader = sqlCommand.ExecuteReader();
                resDT.Load(reader);
            }

            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            finally
            {
                myconn.Close();
            }

            return resDT;
        }

        static string SPPAHDESC()
        {
            string ssql = @"select top 1 * from LS_INS_SPPA_H where NOTES = 'SPPA DANA HAJI' order by id desc";
            DataTable resDT = new DataTable();
            string sppano = "";
            string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            SqlConnection myconn = new SqlConnection(connString);
            myconn.Open();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(ssql);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = myconn;

                SqlDataReader reader = sqlCommand.ExecuteReader();
                resDT.Load(reader);
                foreach (DataRow dr in resDT.Rows)
                {
                    sppano = dr["SPPA_NO"].ToString();
                }
            }

            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            finally
            {
                myconn.Close();
            }

            return sppano;
        }
    }
}
