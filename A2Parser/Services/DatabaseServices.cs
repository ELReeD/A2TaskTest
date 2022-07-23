using A2Parser.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2Parser.Services
{
    public class DatabaseServices
    {
        private string CONNECTION_STRING_CREATEDB = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=SSPI;Initial Catalog=master";
        private string CONNECTION_STRING = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=SSPI;Initial Catalog=WoodDealsDB";
        private string DATABASE_NAME = "WoodDealsDB";
        private string TABLE_NAME = "WoodDeals";
        private string PROCEDURE_NAME = "AddData";
        private bool WorkToDatabase(string connectionString, string sqlQuery)
        {
            bool result = true;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, conn);

            try
            {
                conn.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return result;
        }
        public bool CreateDataBase()
        {
            string sqlQuery = $"IF NOT EXISTS (select * from master.dbo.sysdatabases where name = '{DATABASE_NAME}')" +
                $"Begin " +
                 $"CREATE DATABASE {DATABASE_NAME} " +
                $"End";

            return WorkToDatabase(CONNECTION_STRING_CREATEDB, sqlQuery);
        }
        public bool CreateTable()
        {
            string sqlQuery = $"IF NOT EXISTS (select * from information_schema.TABLES where TABLE_NAME = '{TABLE_NAME}') " +
                $"Begin" +
                    $" Create Table {TABLE_NAME}(WoodDealsId int primary key identity," +
                    "DealNumber nvarchar(300)," +
                    "SellerName nvarchar(300)," +
                    "SellerInn nvarchar(300)," +
                    "BuyerName nvarchar(300)," +
                    "BuyerInn nvarchar(300)," +
                    "DealDate nvarchar(50)," +
                    "WoodVolumeBuyer float," +
                    "WoodVolumeSeller float) " +
                "END";

            return WorkToDatabase(CONNECTION_STRING, sqlQuery);
        }
        public bool CreateProcedure()
        {
            string sqlQuery = $@"if not EXISTS(
	                          SELECT * FROM sys.objects 
                              WHERE object_id = OBJECT_ID(N'{PROCEDURE_NAME}') 
                              AND type in (N'P', N'PC')
                            )

                             Begin
	                            exec ('Create Procedure {PROCEDURE_NAME} @DealNumber nvarchar(100),
                                      @SellerName nvarchar(300),
                                      @SellerInn  nvarchar(300),
                                      @BuyerName  nvarchar(300),
                                      @BuyerInn   nvarchar(300),
                                      @DealDate date ,
                                      @WoodVolumeBuyer float,
                                      @WoodVolumeSeller float
								        AS                                    
                                      Begin 
									    IF NOT EXISTS( Select * From WoodDeals Where DealNumber = @DealNumber)
                                        Insert Into WoodDeals(DealNumber,SellerName,SellerInn,BuyerName,BuyerInn,DealDate,WoodVolumeBuyer,WoodVolumeSeller)
									    Values(@DealNumber,@SellerName,@SellerInn,@BuyerName,@BuyerInn,@DealDate,@WoodVolumeBuyer,@WoodVolumeSeller)
                                      End')
                            End";


            return WorkToDatabase(CONNECTION_STRING, sqlQuery);
        }
        public bool AddData(List<Content> contents)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    foreach (var item in contents)
                    {
                        SqlCommand cmd = new SqlCommand(PROCEDURE_NAME, conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@DealNumber", item.DealNumber == null ? "" : item.DealNumber));
                        cmd.Parameters.Add(new SqlParameter("@SellerName", item.SellerName == null ? "" : item.SellerName ));
                        cmd.Parameters.Add(new SqlParameter("@SellerInn", item.SellerInn   == null ? "" : item.SellerInn ));
                        cmd.Parameters.Add(new SqlParameter("@BuyerName", item.BuyerName   == null ? "" : item.BuyerName ));
                        cmd.Parameters.Add(new SqlParameter("@BuyerInn", item.BuyerInn     == null ? "" : item.BuyerInn));
                        cmd.Parameters.Add(new SqlParameter("@DealDate", item.DealDate     == null ? "" : item.DealDate));
                        cmd.Parameters.Add(new SqlParameter("@WoodVolumeBuyer", item.WoodVolumeBuyer == null ? 0 : item.WoodVolumeBuyer));
                        cmd.Parameters.Add(new SqlParameter("@WoodVolumeSeller", item.WoodVolumeSeller == null ? 0 : item.WoodVolumeSeller));
                        using (SqlDataReader rdr = cmd.ExecuteReader()){}
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
           

            return true;
        }
    }
}
