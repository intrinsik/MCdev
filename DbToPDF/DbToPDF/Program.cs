using System;
using System.Configuration;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.html.simpleparser;

namespace DbToPDF
{
    class Program
    {
        //static int _id = PressRelease.releaseId;
        //static string _timeStamp = PressRelease.releasedTimeStamp;
        static string _title = PressRelease.releaseTitle;
        static string _shorttxt = PressRelease.releaseShortTxt;
        static string _body;
        static string _all;
        static string _rawHtml;

        public static object DsCollected { get; private set; }

        private static void Initialize()
        {
            var pr = new PressRelease();
        }

        static void Main(string[] args)
        {
            Initialize();

            var path = @"c:/temp/PressReleases/";// + filename;
                //
                //var ds = new DataSet();
                //var sqlConn = new SqlConnection();
                //try
                //{
                //    sqlConn = new SqlConnection(strSqlConn);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine("Error: Failed to create a database connection. \n{0}", ex.Message);
                //    throw;
                //}
                //
                //try
                //{
                //    var sqlCommand = new SqlCommand(strSelect, sqlConn);
                //    var sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                //
                //    sqlConn.Open();
                //    sqlDataAdapter.Fill(ds, "PressReleases");
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine("Error: Failed to retrieve the required data from the database. \n{0}", ex.Message);
                //    throw;
                //}
                //finally
                //{
                //    sqlConn.Close();
                //}
                //
                //var dta = ds.Tables;
                //foreach (DataTable dt in dta)
                //{
                //    Console.WriteLine("Found data table {0}", dt.TableName);
                //    Console.WriteLine("There are {0} rows in this set", dt.Rows.Count);
                //    Console.ReadLine();
                //}
                //
                //
                //Console.WriteLine("{0} rows in PressRelease table", ds.Tables["PressReleases"].Rows.Count);
                //Console.WriteLine("{0} columns in PressRelease table", ds.Tables["PressReleases"].Columns.Count);
                //
                //DataColumnCollection drc = ds.Tables["PressReleases"].Columns;
                //int i = 0;
                //foreach (DataColumn dc in drc)
                //{
                //    Console.WriteLine("Column name[{0}] is {1}, of type {2}", i++, dc.ColumnName, dc.DataType);
                //}



            //Loop through each row in table and create PDF files
            var ds = PressRelease.DsCollected.Tables["PressReleases"].Rows;
            var dra = ds;
            foreach (DataRow dr in dra)
            {
                //CreateHtmlBody(dr[5].ToString());
                //SbPressReleaseContents(dr[1].ToString(), dr[2].ToString(), dr[4].ToString(), _body, dr[3].ToString());
                //CreatePdf(_all, path + dr[0] + ".pdf");
                var docFormatted = SbPressReleaseContents(dr[1].ToString(), dr[2].ToString(), dr[4].ToString(), _body, dr[3].ToString());
                try
                {
                    CreateHtmlBody(docFormatted + dr[5], path + dr[0] + ".pdf");
                }
                catch (Exception ex)
                {
                    var docFailedFormatted = SbPressReleaseContentsFailed(dr[1].ToString(), dr[2].ToString(), dr[4].ToString(), _body, dr[3].ToString());
                    try
                    {
                        CreateHtmlBody(docFailedFormatted + dr[5], path + dr[0] + ".pdf");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "\n\n" + dr[0] + ",");
                    }
                    Console.WriteLine("\n\n" + ex.Message);
                }
            }
            //Console.ReadLine();

            //Loop through each row in table and create HTML files
            //var dra = ds.Tables["PressReleases"].Rows;
            //foreach (DataRow dr in dra)
            //{
            //    CreateHtmlContents(dr[5].ToString());
            //    SbPressReleaseContents(dr[1].ToString(), dr[3].ToString(), dr[4].ToString(), _body, dr[2].ToString());
            //    CreateHtml(_all, path + dr[0] + ".html");
            //}


        }

        //pass the contents of the press release body as a parameter using MemoryStream
        private static void CreateHtmlBody(string html, string path)
        {
            //Console.WriteLine(html);
            var sr = new StringReader(html);
            //Console.WriteLine();

            //Console.WriteLine(sr);
            //Console.ReadLine();
            var pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            var htmlparser = new HTMLWorker(pdfDoc);

            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                var writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                htmlparser.Parse(sr);
                pdfDoc.Close();

                bytes = memoryStream.ToArray();
                memoryStream.Close();
            }
            File.WriteAllBytes(path, bytes);

            //var releaseBody = new Paragraph(html);
            //using (var doc = new Document(PageSize.LETTER, 25, 25, 50, 25))
            //{
            //    var htmlparser = new HTMLWorker(doc);

            //    PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            //    doc.Open();
            //    htmlparser.Parse(releaseBody);
            //    doc.Add(releaseBody);
            //    doc.Close();
            //}
        }

        //pass the contents of the press release body as a parameter using MemoryStream
        //private static string CreateHtmlBody(string html)
        //{
        //    string contents;
        //    byte[] byteArray = Encoding.UTF8.GetBytes(html);
        //    var stream = new MemoryStream(byteArray);
        //    using (stream)
        //    {
        //        contents = XmlToTxt.Parse(stream);
        //    }
        //    return _body = contents;
        //}

        //private static string SbPressReleaseContents(string dept, string title, string shortDesc, string body, string posted)
        private static string SbPressReleaseContents(string dept, string title, string shortDesc, string body, string posted)
        {
            var sb = new StringBuilder();
            const string nl = "<br />";
            if (dept != null) sb.Append(dept + nl + posted + nl + title + nl + shortDesc + nl + body);
            return _all = sb.ToString();
        }

        private static string SbPressReleaseContentsFailed(string dept, string title, string shortDesc, string body, string posted)
        {
            var sb = new StringBuilder();
            const string op = "<p>";
            const string cp = "</p>";
            const string nl = "<br />";
            if (dept != null) sb.Append(op+dept+cp + nl + op+posted+cp + nl + op + title + cp + nl + op + shortDesc + cp + nl + body);
            return _all = sb.ToString();
        }


        //pass the contents of the entire press release and return as one object
        //private static string SbPressReleaseContents(string dept, string title, string shortDesc, string body, string posted)
        //{
        //    var sb = new StringBuilder();
        //    const string nl = "\n\n";
        //    if (dept != null) sb.Append(dept + nl + posted + nl + title + nl + shortDesc + nl + body);
        //    return _all = sb.ToString();
        //}

        //create a new file (pdf) in the specified folder
        private static void CreatePdf(string body, string path)
        {
            var releaseBody = new Paragraph(body);
            using (var doc = new Document(PageSize.LETTER, 25, 25, 50, 25))
            {
                PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
                doc.Open();
                doc.Add(releaseBody);
                doc.Close();
            }
        }



        //pass the contents of the press release body as a parameter using MemoryStream
        private static string CreateHtmlContents(string html)
        {
            var sb = new StringBuilder();
            sb.Append(html);
            var contents = (new StringWriter(sb)).ToString();
            return _body = contents;
        }

        //create a new file (html) in the specified folder
        //private void CreateHtml(string body, string path)
        //{
        //    var sbContents = new StringBuilder(body);
        //    var writer = new HtmlTextWriter().Write(body);

        //    using ()
        //    {

        //    }
        //}
    }
}