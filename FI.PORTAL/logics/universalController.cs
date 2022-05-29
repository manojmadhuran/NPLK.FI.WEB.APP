using FI.PORTAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;

namespace FI.PORTAL.logics
{
    public class universalController
    {
        public SqlConnection conn = null;
        public SqlConnection TransConn = null;
        public SqlCommand com = null;
        public SqlDataAdapter da = null;
        public SqlTransaction trans = null;

        CollectionModal collectionModal = new CollectionModal();

        //string ConnectionString = @"Data Source=INTERNSHIP-IT\SQLEXPRESS;Initial Catalog=PracticeDB;User ID=sa;Password=123@nippon";

        //database connection
        string ConnectionString = ConfigurationManager.ConnectionStrings["Universal"].ConnectionString;
        string FiNANCE_GEN_SYS = ConfigurationManager.ConnectionStrings["FinanceGenSys"].ConnectionString;

        // function to create new customerts for customer outstandiung details page
        public int CreateCustomer(String cusCode, String cusName, String crdLimit, String crdPeriod)
        {

            int returnValue = 0;

            try
            {

                if (conn == null)
                {
                    ConnOpen();
                }

                String query = "INSERT INTO CustomerOutstandingDetails (CusCode, CusName, CreditPeriod, CreditLimit, TotOutstanding, CurrentValue, [1-30Days], [31-60Days], [61-90Days], [91-120Days], [121-180Days], [181-360Days], Over361Days, InvoiceNo, InvoiceDate, DocNo, RefNo, UploadedDate) " +
                    "VALUES ('" + cusCode + "', '" + cusName + "',  '" + crdPeriod + "', '" + crdLimit + "', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '2099-01-01 00:00:00.000')";

                Boolean isSucess = InsertData(query);

                if (isSucess)
                {

                    returnValue = 1;
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                returnValue = 0;
            }

            return returnValue;

        }



        // function to read the cheque details for collection report - To read data from database a different method has used.
        public List<CollectionModal> GetChequeDetailsForReport(String CollectionID)
        {

            List<CollectionModal> datalist = new List<CollectionModal> { };
            SqlConnection connection;
            SqlCommand command;

            connection = new SqlConnection();
            connection.ConnectionString = FiNANCE_GEN_SYS;
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;


            try
            {
                command.CommandText = "select * from viewCollectionReportData where collection_no = @collection_no";
                command.Parameters.Add("collection_no", SqlDbType.VarChar).Value = CollectionID;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                         CollectionModal collectionModal = new CollectionModal(reader.GetString(5), reader.GetString(6), reader.GetString(8), reader.GetString(9), reader.GetString(15), reader.GetString(16), reader.GetString(13), Double.Parse(reader["payment_amount"].ToString()), Double.Parse(reader["unallocated_amount"].ToString()), DateTime.Parse(reader["banking_date"].ToString()), reader.GetString(2), reader.GetString(17));                   
                        datalist.Add(collectionModal);
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Diagnostics.Debug.WriteLine("Error In Sesson ");
                System.Diagnostics.Debug.WriteLine("Error In Sesson " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Error In Sesson " + ex.StackTrace);
            }
            finally
            {
                connection.Close();
            }

            return datalist;
        }

        // function to read the payed invoices details for collection report - To read data from database a different method has used.
        public List<CollectionModal> GetPayedInvoicesForReport(String CollectionID)
        {

            List<CollectionModal> datalist = new List<CollectionModal> { };
            SqlConnection connection;
            SqlCommand command;

            connection = new SqlConnection();
            connection.ConnectionString = FiNANCE_GEN_SYS;
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;


            try
            {
                command.CommandText = "SELECT * from PAYED_INVOICES WHERE collection_no = @collection_no ORDER BY invoice_date ASC";
                command.Parameters.Add("collection_no", SqlDbType.VarChar).Value = CollectionID;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CollectionModal collectionModal = new CollectionModal(DateTime.Parse(reader["invoice_date"].ToString()), reader.GetString(3), reader.GetString(8), reader.GetString(12), Boolean.Parse(reader["acknowledge"].ToString()), Double.Parse(reader["invoice_amount"].ToString()), Double.Parse(reader["os_balance"].ToString()), Double.Parse(reader["invoice_allocated"].ToString()), Int32.Parse(reader["nod"].ToString()));
                        datalist.Add(collectionModal);
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ToString();

            }
            finally
            {
                connection.Close();
            }

            return datalist;
        }



        public bool ConnOpen()
        {
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                return true;
            }

            catch (Exception e1)
            {
                return false;
            }
        }

        public bool ConnClose()
        {
            try
            {
                conn.Close();
                return true;
            }

            catch (Exception e2)
            {
                return false;
            }
        }

        public bool InsertData(string Query)
        {
            try
            {
                ConnOpen();
                com = new SqlCommand(Query, conn);
                com.ExecuteNonQuery();

                ConnClose();
                return true;
            }

            catch (Exception e1)
            {
                ConnClose();
                return false;
            }
        }
    }
}
