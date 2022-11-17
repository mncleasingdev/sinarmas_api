using Polis_Sinarmas_API.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using static Polis_Sinarmas_API.Models.ModelReqPolisSNM;
using static Polis_Sinarmas_API.Models.ResultReqPolisSNM;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Polis_Sinarmas_API
{
    public class CreatePolisManager
    {

         public async Task<string> CreateApp(DataTable dtreqPOL)
        {
            string secretkey = "Basic U0VSVklDRU1OQzpNbmMlXiY=";
            //dev
            //Uri Polis_SINARMAS = new Uri("https://pegadev.sinarmas.co.id/prweb/PRRestService/ASMSALESAUTOWORK/Data-Portal/ASMRequestServiceAutoAccept".ToString());

            Uri Polis_SINARMAS = new Uri("https://pegaservice.sinarmas.co.id/prweb/PRRestService/ASMSALESAUTOWORK/Data-Portal/ASMRequestServiceAutoAccept".ToString());
            //string Polis_User = "SERVICEMNC";
            //string Polis_Pwd = "Mnc%^";

            CreatePolisManager result = new CreatePolisManager();

            string strResult = "";
            string LSAGREEID = "";

            foreach (DataRow item in dtreqPOL.Rows)
            {
                LSAGREEID = item["LSAGREE"].ToString();

                SinarmasReq mdlReq = new SinarmasReq();
                NbWorkPage mdlNBW = new NbWorkPage();
                Quotation mdlQtn = new Quotation();

                CustomerP mdlCstP = new CustomerP();
                AddressList mdlAdrs = new AddressList();
                AsmTelfax mdlFax = new AsmTelfax();

                PersonList mdlPerson = new PersonList();
                AsmCoverage mdlCov = new AsmCoverage();
                AsmCoverage mdlCov2 = new AsmCoverage();
                AsmHeir mdlHeir = new AsmHeir();
                Policy mdlPol = new Policy();

                var lstTelFax = new List<AsmTelfax>();
                var listCvrg = new List<AsmCoverage>();
                var listHeir = new List<AsmHeir>();
                var listQtn = new List<Quotation>();
                var listCstP = new List<CustomerP>();
                var listpol = new List<Policy>();
                var listaddrs = new List<AddressList>();
                var listprs = new List<PersonList>();

                ResultReqPolisSNM messageResult = new ResultReqPolisSNM();

                foreach (DataRow dr in dtreqPOL.Rows)
                {
                    //Quotation
                    mdlQtn.BusinessCode = item["BusinessCode"].ToString();
                    mdlQtn.BusinessName = item["BusinessName"].ToString();
                    mdlQtn.GroupPanel = item["GroupPanel"].ToString();
                    mdlQtn.AccessCode = item["AccessCode"].ToString();

                    //Customer_P
                    mdlCstP.PyFirstName = item["pyFirstName"].ToString();
                    mdlCstP.PyLastName = item["PyLastName"].ToString();
                    mdlCstP.PyCity = item["PyCity"].ToString();
                    mdlCstP.AsmDateOfBirth = item["AsmDateOfBirth"].ToString();
                    mdlCstP.AsmGender = item["AsmGender"].ToString();
                    mdlCstP.AsmidCard = item["ASMIDCard"].ToString();
                    mdlCstP.PyCompany = item["pyCompany"].ToString();

                    //Policy
                    mdlPol.StartDateTime = item["StartDateTime"].ToString();
                    mdlPol.EndDateTime = item["EndDateTime"].ToString();
                    mdlPol.QqName = item["QQName"].ToString();
                    mdlPol.CustomerType = item["CustomerType"].ToString();
                    mdlPol.TheInsured = item["TheInsured"].ToString();
                    mdlPol.IdTransaction = item["IdTransaction"].ToString();
                    mdlPol.TripType = item["TripType"].ToString();
                    mdlPol.Currency = item["Currency"].ToString();
                    mdlPol.TypeOfPacket = item["TypeOfPacket"].ToString();
                    mdlPol.StatusPenerbitan = item["StatusPenerbitan"].ToString();

                    //AddressList
                    mdlAdrs.AsmAddress = item["ASMAddress"].ToString();
                    mdlAdrs.AsmAddressType = item["AsmAddressType"].ToString();
                    mdlAdrs.AsmZipCode = item["AsmZipCode"].ToString();

                    //Telfax(in AddressList)
                    mdlFax.TelfaxCode = item["TelFaxCode"].ToString();
                    mdlFax.TelfaxNumber = item["TelFaxNumber"].ToString();
                    mdlFax.TelfaxType = item["TelFaxType"].ToString();

                    //mdlAdrs.AsmTelfax.Add(mdlFax);
                    lstTelFax.Add(mdlFax);
                    mdlAdrs.AsmTelfax = lstTelFax;

                    //PersonList
                    mdlPerson.AsmDateOfBirth = item["ASMDateOfBirth_P"].ToString();
                    mdlPerson.AsmHeight = item["ASMHeight"].ToString();
                    mdlPerson.AsmidCard = item["ASMIDCard"].ToString();
                    mdlPerson.AsmJobName = item["ASMJobName"].ToString();
                    mdlPerson.AsmLeftHanded = item["ASMLeftHanded"].ToString();
                    mdlPerson.AsmParticipantStatus = item["ASMParticipantStatus"].ToString();
                    mdlPerson.AsmWeight = item["ASMWeight"].ToString();
                    mdlPerson.StartDateTime = item["StartDateTime_P"].ToString();
                    mdlPerson.EndDateTime = item["EndDateTime_P"].ToString();
                    mdlPerson.PyFullName = item["pyFullName"].ToString();

                    //Coverage-PersonList #1
                    mdlCov.Coverage = item["Coverage"].ToString();
                    mdlCov.CoverageNote = item["CoverageNote"].ToString();
                    mdlCov.DiscountPercentage = item["DiscountPercentage"].ToString();
                    mdlCov.CalculateMethod = item["CalculateMethod"].ToString();
                    mdlCov.Rate = item["Rate"].ToString();
                    mdlCov.Tsi = item["TSI"].ToString();
                    //listCvrg[0] = mdlCov;
                    listCvrg.Add(mdlCov);

                    //Coverage-PersonList #2
                    mdlCov2.Coverage = item["Coverage2"].ToString();
                    mdlCov2.CoverageNote = item["CoverageNote2"].ToString();
                    mdlCov2.DiscountPercentage = item["DiscountPercentage2"].ToString();
                    mdlCov2.CalculateMethod = item["CalculateMethod2"].ToString();
                    mdlCov2.Rate = item["Rate2"].ToString();
                    mdlCov2.Tsi = item["TSI2"].ToString();
                    //listCvrg[1] = mdlCov2;
                    listCvrg.Add(mdlCov2);

                    mdlPerson.AsmCoverage = listCvrg;

                    //ASMHeir-PersonList
                    mdlHeir.AsmDateOfBirth = item["ASMDateOfBirth_H"].ToString();
                    mdlHeir.AsmGender = item["ASMGender_P"].ToString();
                    mdlHeir.AsmHeirPercentage = item["ASMHeirPercentage"].ToString();
                    mdlHeir.AsmRelationName = item["ASMRelationName"].ToString();
                    mdlHeir.PyFullName = item["pyFullName_H"].ToString();

                    listHeir.Add(mdlHeir);
                    mdlPerson.AsmHeir = listHeir;

                    //Final Add To Header
                    mdlNBW.Quotation = mdlQtn;
                    mdlNBW.CustomerP = mdlCstP;
                    mdlNBW.Policy = mdlPol;
                    var lstAdrs = new List<AddressList>();
                    lstAdrs.Add(mdlAdrs);
                    mdlNBW.AddressList = lstAdrs;

                    var lstPrsn = new List<PersonList>();
                    lstPrsn.Add(mdlPerson);
                    mdlNBW.PersonList = lstPrsn;

                    mdlReq.NbWorkPage = mdlNBW;
                }

                string jsonString = JsonConvert.SerializeObject(mdlReq);
                string stringResponse = String.Empty;
                bool isErrorParam = false;

                if (mdlCov.Tsi == null || mdlCov.Tsi == "" || mdlCov2.Tsi == null || mdlCov2.Tsi == "")
                {
                    messageResult.FeedbackMessage = "Parameter Total Sum Insured Tidak Boleh Kosong !";
                    strResult = messageResult.FeedbackMessage.ToString();
                    InsertLogData("https://pegaservice.sinarmas.co.id", "Parameter Required Missing.", 1, jsonString, strResult, "SINARMAS");
                    isErrorParam = true;
                }

                if (mdlPol.StartDateTime == null || mdlPol.StartDateTime == "" || mdlPol.EndDateTime == null || mdlPol.EndDateTime == "")
                {
                    messageResult.FeedbackMessage = "Parameter StartDate Time / EndDate Time Tidak Boleh Kosong !";
                    strResult = messageResult.FeedbackMessage.ToString();
                    InsertLogData("https://pegaservice.sinarmas.co.id", "Parameter Required Missing.", 1, jsonString, strResult, "SINARMAS");
                    isErrorParam = true;
                }

                if (isErrorParam == false)
                {
                    try
                    {
                        var response = new HttpResponseMessage();
                        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                        HttpClientHandler clientHandler = new HttpClientHandler();
                        var client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", secretkey);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                        response = await client.PostAsync(Polis_SINARMAS, httpContent);
                        stringResponse = await response.Content.ReadAsStringAsync();

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                        messageResult = JsonConvert.DeserializeObject<ResultReqPolisSNM>(stringResponse);

                        if (messageResult.HTTPCode == "200")
                        {
                            strResult = messageResult.FeedbackMessage.ToString();
                            InsertResultSINARMAS(messageResult, LSAGREEID);
                            insertDocumentManegement(messageResult, LSAGREEID);
                            InsertLogData("https://pegaservice.sinarmas.co.id", stringResponse.ToString(), 0, jsonString, strResult, "SINARMAS");
                        }

                        else if (messageResult.HTTPCode == "400")
                        {
                            strResult = messageResult.FeedbackMessage.ToString();
                            InsertLogData("https://pegaservice.sinarmas.co.id", stringResponse.ToString(), 1, jsonString, strResult, "SINARMAS");
                            //InsertResultSINARMAS(messageResult, LSAGREEID);
                            SendMailNotif(LSAGREEID, LSAGREEID, strResult);
                        }

                        //else if (messageResult.FeedbackMessage == "Failed to fetch data/ Request server error.")
                        //{
                        //    strResult = messageResult.FeedbackMessage.ToString();
                        //    InsertLogData("https://pegadev.sinarmas.co.id", stringResponse.ToString(), 1, jsonString, strResult, "SINARMAS");
                        //}

                        else
                        {
                            //strResult += "- Error connecting to API";
                            InsertLogData("https://pegaservice.sinarmas.co.id", stringResponse.ToString(), 1, jsonString, strResult, "SINARMAS");
                        }
                    }

                    catch (Exception ex)
                    {
                        strResult = "ERROR: " + messageResult.FeedbackMessage;
                        InsertLogData("https://pegaservice.sinarmas.co.id", ex.Message.ToString(), 1, jsonString, strResult, "SINARMAS");
                    }
                }
            }
            return strResult;
        }

        public static void InsertLogData(string api_url, string result, int is_error, string postval, string retval, string user)
        {
            result = result.Replace("'", "");
            postval = postval.Replace("'", "");

            string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            string ssql = "INSERT INTO [dbo].[API_Log] SELECT '" + api_url + "', '" + result + "', '" + retval + "'," + is_error + ",'" +
                "sysadmin" + "',GETDATE(),'" + postval + "'";
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand(ssql, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public static void InsertResultSINARMAS(ResultReqPolisSNM messageResult, string LSAGREE)
        {
            if (messageResult != null)
            {
                string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
                SqlConnection myconn = new SqlConnection(connString);
                string pdf_en = messageResult.PDF64;
                byte[] imageBytes = Convert.FromBase64String(pdf_en);
                Policy mdlPol = new Policy();
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[trxResultSINARMAS]([ADMINFEE],[SUM_PAYMENT_TOTAL],[POLICY_NO],[STATUS_PENERBITAN],[STAMP_POLICY],[CON_ID],[IDTRANSACTION],[STAMP_RECEIPT],[FEEDBACK_MESSAGE],[PAYMENT_TOTAL],[DISCOUNT],[CASEID],[HTTP_CODE],[PDF_ATTACHMENT],[PDF64],[COMMISION],[PREMIUM],[USER_ID],[CREATED_DATE],[LSAGREE_ID])
                                                        VALUES (@ADMINFEE,@SUM_PAYMENT_TOTAL,@POLICY_NO,@STATUS_PENERBITAN,@STAMP_POLICY,@CON_ID,@IDTRANSACTION,@STAMP_RECEIPT,@FEEDBACK_MESSAGE,@PAYMENT_TOTAL,@DISCOUNT,@CASEID,@HTTP_CODE,@PDF_ATTACHMENT,@PDF64,@COMMISION,@PREMIUM,@USER_ID,@CREATED_DATE,@LSAGREE_ID)");
                sqlCommand.Connection = myconn;
                myconn.Open();
                try
                {
                    if (messageResult.HTTPCode == "200")
                    {
                        SqlParameter sqlParameter1 = sqlCommand.Parameters.Add("@ADMINFEE", SqlDbType.Decimal);
                        sqlParameter1.Value = messageResult.AdminFee;

                        SqlParameter sqlParameter2 = sqlCommand.Parameters.Add("@SUM_PAYMENT_TOTAL", SqlDbType.Decimal);
                        sqlParameter2.Value = messageResult.SumPaymentTotal;

                        SqlParameter sqlParameter3 = sqlCommand.Parameters.Add("@POLICY_NO", SqlDbType.VarChar);
                        sqlParameter3.Value = messageResult.PolicyNo;

                        SqlParameter sqlParameter4 = sqlCommand.Parameters.Add("@STATUS_PENERBITAN", SqlDbType.VarChar);
                        sqlParameter4.Value = messageResult.StatusPenerbitan;

                        SqlParameter sqlParameter5 = sqlCommand.Parameters.Add("@STAMP_POLICY", SqlDbType.VarChar);
                        sqlParameter5.Value = messageResult.StampPolicy;

                        SqlParameter sqlParameter6 = sqlCommand.Parameters.Add("@CON_ID", SqlDbType.VarChar);
                        sqlParameter6.Value = messageResult.ConID;

                        SqlParameter sqlParameter7 = sqlCommand.Parameters.Add("@IDTRANSACTION", SqlDbType.VarChar);
                        sqlParameter7.Value = messageResult.IdTransaction;

                        SqlParameter sqlParameter8 = sqlCommand.Parameters.Add("@STAMP_RECEIPT", SqlDbType.VarChar);
                        sqlParameter8.Value = messageResult.StampReceipts;

                        SqlParameter sqlParameter9 = sqlCommand.Parameters.Add("@FEEDBACK_MESSAGE", SqlDbType.VarChar);
                        sqlParameter9.Value = messageResult.FeedbackMessage;

                        SqlParameter sqlParameter10 = sqlCommand.Parameters.Add("@PAYMENT_TOTAL", SqlDbType.Decimal);
                        sqlParameter10.Value = messageResult.PaymentTotal;

                        SqlParameter sqlParameter11 = sqlCommand.Parameters.Add("@DISCOUNT", SqlDbType.Decimal);
                        sqlParameter11.Value = messageResult.Discount;

                        SqlParameter sqlParameter12 = sqlCommand.Parameters.Add("@CASEID", SqlDbType.VarChar);
                        sqlParameter12.Value = messageResult.CaseID;

                        SqlParameter sqlParameter13 = sqlCommand.Parameters.Add("@HTTP_CODE", SqlDbType.VarChar);
                        sqlParameter13.Value = messageResult.HTTPCode;

                        SqlParameter sqlParameter14 = sqlCommand.Parameters.Add("@PDF_ATTACHMENT", SqlDbType.VarChar);
                        sqlParameter14.Value = DBNull.Value;

                        SqlParameter sqlParameter15 = sqlCommand.Parameters.Add("@PDF64", SqlDbType.Binary);
                        sqlParameter15.Value = DBNull.Value;

                        SqlParameter sqlParameter16 = sqlCommand.Parameters.Add("@COMMISION", SqlDbType.Decimal);
                        sqlParameter16.Value = messageResult.Commision;

                        SqlParameter sqlParameter17 = sqlCommand.Parameters.Add("@PREMIUM", SqlDbType.Decimal);
                        sqlParameter17.Value = messageResult.Premium;

                        SqlParameter sqlParameter18 = sqlCommand.Parameters.Add("@USER_ID", SqlDbType.VarChar);
                        sqlParameter18.Value = "SYSTEM";

                        SqlParameter sqlParameter19 = sqlCommand.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);
                        sqlParameter19.Value = DateTime.Now;

                        SqlParameter sqlParameter20 = sqlCommand.Parameters.Add("@LSAGREE_ID", SqlDbType.VarChar);
                        sqlParameter20.Value = LSAGREE;
                    }

                    else
                    {
                        SqlParameter sqlParameter1 = sqlCommand.Parameters.Add("@ADMINFEE", SqlDbType.Decimal);
                        sqlParameter1.Value = null;

                        SqlParameter sqlParameter2 = sqlCommand.Parameters.Add("@SUM_PAYMENT_TOTAL", SqlDbType.Decimal);
                        sqlParameter2.Value = null;

                        SqlParameter sqlParameter3 = sqlCommand.Parameters.Add("@POLICY_NO", SqlDbType.VarChar);
                        sqlParameter3.Value = "";

                        SqlParameter sqlParameter4 = sqlCommand.Parameters.Add("@STATUS_PENERBITAN", SqlDbType.VarChar);
                        sqlParameter4.Value = "0";

                        SqlParameter sqlParameter5 = sqlCommand.Parameters.Add("@STAMP_POLICY", SqlDbType.VarChar);
                        sqlParameter5.Value = null;

                        SqlParameter sqlParameter6 = sqlCommand.Parameters.Add("@CON_ID", SqlDbType.VarChar);
                        sqlParameter6.Value = "";

                        SqlParameter sqlParameter7 = sqlCommand.Parameters.Add("@IDTRANSACTION", SqlDbType.VarChar);
                        sqlParameter7.Value = "";

                        SqlParameter sqlParameter8 = sqlCommand.Parameters.Add("@STAMP_RECEIPT", SqlDbType.VarChar);
                        sqlParameter8.Value = "";

                        SqlParameter sqlParameter9 = sqlCommand.Parameters.Add("@FEEDBACK_MESSAGE", SqlDbType.VarChar);
                        sqlParameter9.Value = messageResult.FeedbackMessage;

                        SqlParameter sqlParameter10 = sqlCommand.Parameters.Add("@PAYMENT_TOTAL", SqlDbType.Decimal);
                        sqlParameter10.Value = null;

                        SqlParameter sqlParameter11 = sqlCommand.Parameters.Add("@DISCOUNT", SqlDbType.Decimal);
                        sqlParameter11.Value = null;

                        SqlParameter sqlParameter12 = sqlCommand.Parameters.Add("@CASEID", SqlDbType.VarChar);
                        sqlParameter12.Value = "";

                        SqlParameter sqlParameter13 = sqlCommand.Parameters.Add("@HTTP_CODE", SqlDbType.VarChar);
                        sqlParameter13.Value = "400";

                        SqlParameter sqlParameter14 = sqlCommand.Parameters.Add("@PDF_ATTACHMENT", SqlDbType.VarChar);
                        sqlParameter14.Value = DBNull.Value;

                        SqlParameter sqlParameter15 = sqlCommand.Parameters.Add("@PDF64", SqlDbType.Binary);
                        sqlParameter15.Value = DBNull.Value;

                        SqlParameter sqlParameter16 = sqlCommand.Parameters.Add("@COMMISION", SqlDbType.Decimal);
                        sqlParameter16.Value = null;

                        SqlParameter sqlParameter17 = sqlCommand.Parameters.Add("@PREMIUM", SqlDbType.Decimal);
                        sqlParameter17.Value = null;

                        SqlParameter sqlParameter18 = sqlCommand.Parameters.Add("@USER_ID", SqlDbType.VarChar);
                        sqlParameter18.Value = "SYSTEM";

                        SqlParameter sqlParameter19 = sqlCommand.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);
                        sqlParameter19.Value = DateTime.Now;

                        SqlParameter sqlParameter20 = sqlCommand.Parameters.Add("@LSAGREE_ID", SqlDbType.VarChar);
                        sqlParameter20.Value = LSAGREE;
                    }

                    sqlCommand.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }

                finally
                {
                    myconn.Close();
                }
            }
        }

        public static void insertDocumentManegement(ResultReqPolisSNM messageResult, string lsagree)
        {
            if (messageResult != null)
            {
                string applicno = "";
                string name_client = "";
                string connString2 = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString2))
                using (SqlCommand cmd = new SqlCommand(@"SELECT * FROM LS_AGREEMENT WHERE LSAGREE = '" + lsagree + "'", conn))
                {
                    DataTable resDT = new DataTable();

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(resDT);

                    applicno = resDT.Rows[0]["APPLICNO"].ToString();
                    name_client = resDT.Rows[0]["NAME"].ToString();
                }


                string connString = ConfigurationManager.ConnectionStrings["SqlLocalConnectionString"].ConnectionString;
                SqlConnection myconn = new SqlConnection(connString);
                string pdf_en = messageResult.PDF64;
                byte[] imageBytes = Convert.FromBase64String(pdf_en);
                Policy mdlPol = new Policy();

                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[DocumentFile]([Name],[Type],[Ext],[Remarks],[FileDoc],[AppNo],[CreatedBy],[CreatedDateTime],[DebiturName],[AgreeNo],[Module],[MemoNo],[SubType],[Branch],[FileSize])
                                                        VALUES (@Name, @Type, @Ext, @Remarks, @FileDoc, @AppNo, @CreatedBy, @CreatedDateTime, @DebiturName, @AgreeNo, @Module, @MemoNo, @SubType, @Branch, @FileSize)");
                sqlCommand.Connection = myconn;
                myconn.Open();
                try
                {
                    SqlParameter sqlParameter1 = sqlCommand.Parameters.Add("@Name", SqlDbType.NVarChar);
                    sqlParameter1.Value = "DOKUMEN UNTUK DEBITUR";

                    SqlParameter sqlParameter2 = sqlCommand.Parameters.Add("@Type", SqlDbType.NVarChar);
                    sqlParameter2.Value = "DOKUMEN UNTUK DEBITUR";

                    SqlParameter sqlParameter3 = sqlCommand.Parameters.Add("@Ext", SqlDbType.NVarChar);
                    sqlParameter3.Value = ".pdf";

                    SqlParameter sqlParameter4 = sqlCommand.Parameters.Add("@Remarks", SqlDbType.NVarChar);
                    sqlParameter4.Value = "DOKUMEN POLIS ASURANSI";

                    SqlParameter sqlParameter5 = sqlCommand.Parameters.Add("@FileDoc", SqlDbType.Binary);
                    sqlParameter5.Value = imageBytes;

                    SqlParameter sqlParameter6 = sqlCommand.Parameters.Add("@AppNo", SqlDbType.NVarChar);
                    sqlParameter6.Value = applicno;

                    SqlParameter sqlParameter7 = sqlCommand.Parameters.Add("@CreatedBy", SqlDbType.NVarChar);
                    sqlParameter7.Value = "SYSTEM_SCHEDULER";

                    SqlParameter sqlParameter8 = sqlCommand.Parameters.Add("@CreatedDateTime", SqlDbType.DateTime);
                    sqlParameter8.Value = DateTime.Now;

                    SqlParameter sqlParameter9 = sqlCommand.Parameters.Add("@DebiturName", SqlDbType.NVarChar);
                    sqlParameter9.Value = name_client;

                    SqlParameter sqlParameter10 = sqlCommand.Parameters.Add("@AgreeNo", SqlDbType.NVarChar);
                    sqlParameter10.Value = lsagree;

                    SqlParameter sqlParameter11 = sqlCommand.Parameters.Add("@Module", SqlDbType.NVarChar);
                    sqlParameter11.Value = "IMBT";

                    SqlParameter sqlParameter12 = sqlCommand.Parameters.Add("@MemoNo", SqlDbType.NVarChar);
                    sqlParameter12.Value = applicno;

                    SqlParameter sqlParameter13 = sqlCommand.Parameters.Add("@SubType", SqlDbType.NVarChar);
                    sqlParameter13.Value = "DOKUMEN POLIS ASURANSI";

                    SqlParameter sqlParameter14 = sqlCommand.Parameters.Add("@Branch", SqlDbType.NVarChar);
                    sqlParameter14.Value = "JAKARTA (HEAD OFFICE)";

                    SqlParameter sqlParameter15 = sqlCommand.Parameters.Add("@FileSize", SqlDbType.NVarChar);
                    sqlParameter15.Value = "1553 KB";

                    sqlCommand.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }

                finally
                {
                    myconn.Close();
                }
            }
        }

        //private void SendMailNotif(string noKontrak, string value, string resAPI)
        //{
        //    string Name = "";
        //    string LSagree = "";
        //    string DOB = "";
        //    string retAPI = "";
        //    string Age = "";

        //    string ssql = @"select top 10 convert(varchar, cl.INBORNDT, 106) AS DOB,ls.LSAGREE,cl.NAME, CAST(ROUND((DATEDIFF(DAY,cl.INBORNDT,GETDATE())/365.0),0) AS INT) AS AGE, tr.FEEDBACK_MESSAGE 
        //                    from LS_AGREEMENT ls
        //                    left join trxResultSINARMAS tr on ls.LSAGREE COLLATE SQL_Latin1_General_CP1_CI_AS = tr.LSAGREE_ID COLLATE SQL_Latin1_General_CP1_CI_AS
        //                    left join SYS_CLIENT cl on ls.LESSEE = cl.CLIENT
        //                    where ls.LSAGREE = '" + value + "' ";
        //    DataTable resDT = new DataTable();
        //    string connString1 = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
        //    SqlConnection myconn1 = new SqlConnection(connString1);
        //    myconn1.Open();
        //    try
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(ssql);
        //        sqlCommand.CommandType = CommandType.Text;
        //        sqlCommand.Connection = myconn1;

        //        SqlDataReader reader = sqlCommand.ExecuteReader();
        //        resDT.Load(reader);
        //        if (resDT.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr2 in resDT.Rows)
        //            {
        //                LSagree = dr2["LSAGREE"].ToString();
        //                Name = dr2["NAME"].ToString();
        //                DOB = dr2["DOB"].ToString();
        //                retAPI = dr2["FEEDBACK_MESSAGE"].ToString();
        //                Age = dr2["AGE"].ToString();
        //            }
        //        }

        //     }

        //    catch (Exception ex)
        //    {

        //    }

        //    finally
        //    {
        //        myconn1.Close();
        //    }


        //    string bodyTextEmail = "";
        //    string subjectTextEmail = "";
        //    string emailAddress = "";

        //    var sb = new StringBuilder();
        //        sb.AppendLine("Dear Credam & Team,");
        //        sb.AppendLine("");
        //        sb.AppendLine("Berikut Response yang tidak dapat di proses pada Scheduler API Sinarmas Syariah, Mohon Di proses pada Smile dengan menginputkan nomor polis pada nomor kontrak yang terkait");
        //        sb.AppendLine("");
        //        sb.AppendLine("Nomor Kontrak : " + LSagree + "");
        //        sb.AppendLine("Nama : " + Name + "");
        //        sb.AppendLine("Tanggal Lahir : " + DOB + "");
        //        sb.AppendLine("Usia : " + Age + " Tahun");
        //        sb.AppendLine("");
        //        sb.AppendLine("Respon API : " + resAPI + "");
        //        sb.AppendLine("");
        //        sb.AppendLine("Thanks");

        //    var items = sb.ToString();

        //    bodyTextEmail = items;

        //    subjectTextEmail = "Asuransi Sinarmas Syariah - Notification";
        //    emailAddress = "satuni.hasan@mncgroup.com";

        //    string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
        //    SqlConnection myconn = new SqlConnection(connString);
        //    myconn.Open();
        //    try
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(@"sp_SSS_SendMail");
        //        sqlCommand.CommandType = CommandType.StoredProcedure;
        //        sqlCommand.Connection = myconn;

        //        SqlParameter sqlParameter1 = sqlCommand.Parameters.Add("@profile_name", SqlDbType.NVarChar);
        //        sqlParameter1.Value = "SQLMelisa";

        //        SqlParameter sqlParameter2 = sqlCommand.Parameters.Add("@recipients", SqlDbType.NVarChar);
        //        sqlParameter2.Value = emailAddress;

        //        SqlParameter sqlParameter3 = sqlCommand.Parameters.Add("@copy_recipients", SqlDbType.NVarChar);
        //        sqlParameter3.Value = "credam.mncleasing@mncgroup.com";

        //        SqlParameter sqlParameter4 = sqlCommand.Parameters.Add("@body", SqlDbType.NVarChar);
        //        sqlParameter4.Value = bodyTextEmail;

        //        SqlParameter sqlParameter5 = sqlCommand.Parameters.Add("@subject", SqlDbType.NVarChar);
        //        sqlParameter5.Value = subjectTextEmail;

        //        sqlCommand.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException(ex.Message);
        //    }
        //    finally
        //    {
        //        myconn.Close();
        //    }
        //}

        private void SendMailNotif(string noKontrak, string value, string resAPI)
        {
            string Name = "";
            string LSagree = "";
            string DOB = "";
            string retAPI = "";
            string Age = "";

            string ssql = @"select top 10 convert(varchar, cl.INBORNDT, 106) AS DOB,ls.LSAGREE,cl.NAME, CAST(ROUND((DATEDIFF(DAY,cl.INBORNDT,GETDATE())/365.0),0) AS INT) AS AGE, tr.FEEDBACK_MESSAGE 
                            from LS_AGREEMENT ls
                            left join trxResultSINARMAS tr on ls.LSAGREE COLLATE SQL_Latin1_General_CP1_CI_AS = tr.LSAGREE_ID COLLATE SQL_Latin1_General_CP1_CI_AS
                            left join SYS_CLIENT cl on ls.LESSEE = cl.CLIENT
                            where ls.LSAGREE = '" + value + "' ";
            DataTable resDT = new DataTable();
            string connString1 = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            SqlConnection myconn1 = new SqlConnection(connString1);
            myconn1.Open();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(ssql);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = myconn1;

                SqlDataReader reader = sqlCommand.ExecuteReader();
                resDT.Load(reader);
                if (resDT.Rows.Count > 0)
                {
                    foreach (DataRow dr2 in resDT.Rows)
                    {
                        LSagree = dr2["LSAGREE"].ToString();
                        Name = dr2["NAME"].ToString();
                        DOB = dr2["DOB"].ToString();
                        retAPI = dr2["FEEDBACK_MESSAGE"].ToString();
                        Age = dr2["AGE"].ToString();
                    }
                }

            }

            catch (Exception ex)
            {

            }

            finally
            {
                myconn1.Close();
            }

            string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            SqlConnection myconn = new SqlConnection(connString);
            myconn.Open();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"SP_MNCL_Email_SIMAS_SYARIAH");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = myconn;

                SqlParameter sqlParameter1 = sqlCommand.Parameters.Add("@lsagree", SqlDbType.NVarChar);
                sqlParameter1.Value = LSagree;

                SqlParameter sqlParameter2 = sqlCommand.Parameters.Add("@name", SqlDbType.NVarChar);
                sqlParameter2.Value = Name;

                SqlParameter sqlParameter3 = sqlCommand.Parameters.Add("@dob", SqlDbType.NVarChar);
                sqlParameter3.Value = DOB;

                SqlParameter sqlParameter4 = sqlCommand.Parameters.Add("@retapi", SqlDbType.NVarChar);
                sqlParameter4.Value = retAPI;

                SqlParameter sqlParameter5 = sqlCommand.Parameters.Add("@age", SqlDbType.NVarChar);
                sqlParameter5.Value = Age;

                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                myconn.Close();
            }
        }
    }
}
