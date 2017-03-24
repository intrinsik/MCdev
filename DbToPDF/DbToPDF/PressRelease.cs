using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbToPDF
{
    class PressRelease
    {
        #region fields
        public static int releaseId;               //(int) ReleaseID: <ReleaseID>
        public static string releasedTimeStamp;    //(string) = (datetime) Release Date / (datetime)TimeStamp: <ReleaseDate> ('Timestamp: '<DateTimeStamp>)
        public static string releaseTitle;         //(string) Release Title: <ReleaseTitle>
        public static string releaseShortTxt;      //(string) Release Short Description: <releaseShortDescrip>
        public static string releaseBody;          //Release Text: <releaseText>

        public static string allDetails;

        public static DataSet DsCollected;

        private static readonly string strSqlConn = ConfigurationManager.ConnectionStrings["Events"].ToString();
        private const string strSelect = "SELECT [releaseID],dept.DeptName,(CONVERT(nvarchar,[releaseDate], 110) + ' (Timestamp: ' + CONVERT(nvarchar(30), [DateTimeStamp], 120) + ')') AS 'Release Date',[releaseTitle],[releaseShortDescrip],[releaseText] FROM[Events].[dbo].[tblPressRelease] pr INNER JOIN[Events].[dbo].tblAdvertising_Org adOrg ON pr.Ad_Org_ID = adOrg.Ad_Org_ID INNER JOIN[Events].[dbo].tblDepartment dept ON adOrg.DeptID = dept.DeptID WHERE pr.Ad_Org_ID = 104 ORDER BY releaseID DESC";

        public PressRelease()
        {
                ConnectToDb(strSqlConn,strSelect);
        }
        #endregion

        public static DataSet ConnectToDb(string connString, string sqlCmd)
        {
            var sqlConn = new SqlConnection();
            var ds = new DataSet();

            try
            {
                sqlConn = new SqlConnection(connString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Failed to create a database connection. \n{0}", ex.Message);
            }

            try
            {
                var sqlCommand = new SqlCommand(sqlCmd, sqlConn);
                var sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlConn.Open();
                sqlDataAdapter.Fill(ds, "PressReleases");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Failed to retrieve the required data from the database. \n{0}", ex.Message);
            }
            finally
            {
                sqlConn.Close();
            }
            return DsCollected = ds;
        }
    }
}
