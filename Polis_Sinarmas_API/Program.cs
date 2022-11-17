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
                string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                //LOCAL
                //using (SqlCommand cmd = new SqlCommand(@"SELECT DISTINCT b.LSAGREE
                //                                          FROM [dbo].[SYS_CLIENT] a 
                //                                          INNER JOIN LS_AGREEMENT b ON a.CLIENT=b.LESSEE 
                //                                          WHERE b.LSAGREE COLLATE SQL_Latin1_General_CP1_CI_AS 
                //                                          NOT IN (SELECT a.LSAGREE_ID FROM trxResultSINARMAS a 
                //                                          INNER JOIN LS_AGREEMENT b ON a.LSAGREE_ID = b.LSAGREE COLLATE SQL_Latin1_General_CP1_CI_AS)
                //                                          AND b.LSAGREE IN ('001119121100045','000519220100047')
                //                                          ", conn))

                //SERVER
                using (SqlCommand cmd = new SqlCommand(@"SELECT 
                                                       AG.LSAGREE
                                                          FROM IFINANCING_GOLIVE.dbo.LS_INS_SPPA_D d with(NOLOCK)
                                                          LEFT JOIN IFINANCING_GOLIVE.dbo.LS_INS_SPPA_H H with(NOLOCK) ON D.SPPA_NO = H.SPPA_NO  
                                                          LEFT JOIN IFINANCING_GOLIVE.dbo.LS_AGREEMENT AG with(NOLOCK) ON D.LSAGREE = AG.LSAGREE
                                                          WHERE 
                                                       AG.PRODUCT_FACILITY_CODE IN (112,215)
                                                       AND D.STATUS = 'SENT'
                                                       AND D.INSURERNO = 'ASMSY'
                                                       AND D.POLICY_NO IS NULL
                                                       AND AG.LSAGREE NOT IN (SELECT LSAGREE_ID FROM trxResultSINARMAS with(NOLOCK))", conn))

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
                                updateSPPA();
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


    }
}
