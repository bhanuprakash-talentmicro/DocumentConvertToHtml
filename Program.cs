using C1.C1Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DocumentUpload
{
    public class Program
    {

        // Console.WriteLine("Hello, World!");
        //Console.ReadKey();
        public static void Main(string[] args)
        {
            //public clsDocuments.GetResumeDoc beGetResumeDoc(long lRid, bool isReqResId = false)
            //{
                using (SqlConnection con = HC_DBConnection.GetsqlConnection())
                {

                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "Usp_GetResumeDocBasedonTypebyTool";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@input", "");

                    var vDataout = cmd.Parameters.Add("@output", SqlDbType.NVarChar, -1);
                    vDataout.Direction = ParameterDirection.Output;
                    var vData = cmd.Parameters.Add("@FileData",
                        SqlDbType.NVarChar, -1);
                        vData.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        clsDocuments.GetResumeDoc ResDoc = new clsDocuments.GetResumeDoc();
                        /*
                         * If Document is migrated then converted html text will be not there 
                         * fot that we have created new function(beGetResumeV4Doc) in order to convert v4 documents to v5
                         */
                    //    if (Convert.ToString(vData.Value) == "")
                    //    {
                    //    ResDoc.FileData = "came here";   //Program.beGetResumeV4Doc(0, false);
                    //    string filedata=Program. beGetResumeV4Doc(0, false);
                    //}


                    if (Convert.ToString(vData.Value) == "Data")
                    {
                        ResDoc.FileData = "came here";
                        Program programInstance = new Program();
                        string filedata = programInstance.beGetResumeV4Doc(0, false);
                        //Console.WriteLine(filedata);
                        //Console.WriteLine(HC_DBConnection.ParserUrl);
                        //Console.ReadKey();

                    }
                    else
                        {
                            ResDoc.FileData = Convert.ToString(vData.Value);
                        }

                        //return ResDoc;
                    }
                }
            //}

        }

        //public static void beGetResumeV4Doc(long value, bool someBool)
        //{
            public string beGetResumeV4Doc(long lRid, bool isReqResId = false)
            {
                try
                {
                    string filedata = "";
                    string htmlText = "";
                    using (SqlConnection con = HC_DBConnection.GetsqlConnection())
                    {
                        using (SqlCommand cmd = con.CreateCommand())
                        {
                            cmd.CommandText = "Usp_GetResumeDocV4byTool";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@input", lRid);
                        var vData = cmd.Parameters.Add("@output", SqlDbType.NVarChar, -1);
                        vData.Direction = ParameterDirection.Output;
                      
                            SqlDataAdapter sqlda = new SqlDataAdapter();
                            DataSet ds = new DataSet();
                            sqlda = new SqlDataAdapter(cmd);
                            sqlda.Fill(ds);

                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                //string filedatanew = Convert.ToString(ds.Tables[0].Rows[0]["fileData"]);
                                //byte[] bFileData = Convert.FromBase64String(filedatanew);

                                //string tFileName = Convert.ToString(ds.Tables[0].Rows[0]["FileName"]);
                                //bFileData = DownLoad(tFileName, bFileData);
                                //long rid = Convert.ToInt64(ds.Tables[0].Rows[0]["rid"]);


                                string filedatanew = Convert.ToString(row["fileData"]);
                                byte[] bFileData = Convert.FromBase64String(filedatanew);

                                string tFileName = Convert.ToString(row["FileName"]);
                                bFileData = DownLoad(tFileName, bFileData);
                                long rid = Convert.ToInt64(row["rid"]);
                                string mainfiletype= Convert.ToString(row["Filetype"]);
                                #region zip
                                //byte[] data = null;
                                //try
                                //{
                                //    tFileName = tFileName.Replace(",", "").Replace(" ", "_");

                                //   using (MemoryStream mStream = new MemoryStream())
                                //   {
                                //       if (tFileName.EndsWith(".docx") == true || tFileName.EndsWith(".pdf") == true)
                                //            mStream.Write(bFileData, 0, bFileData.Length - 1);
                                //        else
                                //            mStream.Write(bFileData, 0, bFileData.Length);// write zip byte to memory stream
                                //        ZipFile tzipfile = new ZipFile(mStream);//get zip file 
                                //        ZipEntry tzentry = tzipfile.GetEntry(tFileName);//get file from zip 
                                //        using (Stream objStream = tzipfile.GetInputStream(tzentry))
                                //        {
                                //            long tCoppresedSize = tzentry.CompressedSize;//Compressed size
                                //            long tSize = tzentry.Size;//original size
                                //            data = new byte[tzentry.Size];
                                //            objStream.Read(data, 0, data.Length);//write file in zip to local byte array

                                //            objStream.Close();
                                //        }
                                //        mStream.Close();
                                //    }
                                //}
                                //catch
                                //{
                                //    data = bFileData;//If file byte is not zip then take consider normal file
                                //}

                                //if (tFileName.EndsWith(".docx") == true)
                                //{
                                //    MemoryStream memo = new MemoryStream();
                                //    memo.Write(data, 0, data.Length - 1);
                                //    data = memo.ToArray();
                                //    memo.Close();
                                //}
                                #endregion

                                filedata = Convert.ToBase64String(bFileData);

                                //filedata = Convert.ToBase64String(bFileData);
                                //bFileData = Convert.FromBase64String(filedata);

                                //ImpResume impResume = new ImpResume();
                                JObject jPar = null;
                                string[] file = tFileName.Split('.');
                                string filetype = "";
                                foreach (var res in file)
                                {
                                    filetype = "." + res.ToString();
                                }
                                jPar = beGetconvertedHtmldata(bFileData, tFileName, mainfiletype == "" ? filetype : mainfiletype);
                                if (Convert.ToInt16(jPar.GetValue("Id")) == -23)
                                {
                                    // beErrorLog.opWriteErrorLogToSentry(HCErrorCodes.ServerError, beErrorLog.LogLevel.Error);
                                    return "error";
                                }
                                else if (Convert.ToInt16(jPar.GetValue("Id")) == -22)
                                {
                                    return "error";
                                    //beErrorLog.opWriteErrorLogToSentry(HCErrorCodes.ServerError, beErrorLog.LogLevel.Error);
                                }
                                else
                                {
                                    clsDocuments.CreateResumeDocument oDocument = new clsDocuments.CreateResumeDocument();
                                    oDocument.ResConvertedText = jPar["ConvertedText"].Value<string>();
                                    oDocument.ResHTMLText = jPar["HTMLText"].Value<string>();
                                    htmlText = oDocument.ResHTMLText;
                                    beUpdateResumeDocumentV4(filedata, oDocument.ResHTMLText, oDocument.ResConvertedText, rid, isReqResId);

                                }
                            }

                            }

                        }
                    }
                    return htmlText;
                }
                catch (Exception ex)
                {
                    //beErrorLog.opWriteErrorLogToSentry(HCErrorCodes.ServerError, beErrorLog.LogLevel.Error, ex);
                    return ex.ToString();
                }
            }



            public byte[] DownLoad(string fileName, byte[] fileContent)
            {
                Byte[] buffer = null;
                try
                {

                    string _tempFileName = "", _ext = Path.GetExtension(fileName).ToLower().Replace(".", "");

                    string _tempDir = HttpContext.Current.Server.MapPath("~\\Temp\\");

                    if (Directory.Exists(_tempDir) == false) Directory.CreateDirectory(_tempDir);
                    Int32 i = 0;
                CreateTempFile:
                    _tempFileName = _tempDir + DateTime.Now.ToString("ddMMyyHHmmssfff") + i.ToString() + "." + _ext;
                    if (File.Exists(_tempFileName))
                    {
                        i++;
                        goto CreateTempFile;
                    }

                    #region docx
                    else if (_ext == "docx")
                    {
                        using (FileStream fs = new FileStream(_tempFileName, FileMode.Create))
                        {
                            fs.Write(fileContent, 0, fileContent.Length - 1);
                            fs.Flush();
                            fs.Close();
                        }
                        WebClient client = new WebClient();
                        buffer = client.DownloadData(_tempFileName);
                    }
                    #endregion docx
                    #region Other Documents
                    else
                    {
                        using (FileStream fs = new FileStream(_tempFileName, FileMode.Create))
                        {
                            fs.Write(fileContent, 0, fileContent.Length);
                            fs.Flush();
                            fs.Close();
                        }
                        //if (_ext != "doc")
                        //{
                        if (C1ZipFile.IsZipFile(_tempFileName))
                        {
                            string _zipPath = Path.GetDirectoryName(_tempFileName) + "\\" + Path.GetFileNameWithoutExtension(_tempFileName) + ".zip";
                            File.Copy(_tempFileName, _zipPath, true);
                            File.Delete(_tempFileName);
                            C1ZipFile objZipFile = new C1ZipFile();
                            objZipFile.Open(_zipPath);
                            objZipFile.Entries.Extract(0, _tempFileName);
                            File.Delete(_zipPath);
                        }
                        WebClient client = new WebClient();
                        buffer = client.DownloadData(_tempFileName);
                        //}



                    }
                    #endregion Other Documents
                    return buffer == null ? fileContent : buffer;
                }
                catch (Exception Ex)
                {
                    // beErrorLog.opWriteErrorLog(Ex.StackTrace + Ex.Message, beErrorLog.LogLevel.Error);
                    return buffer = fileContent;
                    //ErrorLog.WriteErrorLog(Ex);
                }
            }



            public void beUpdateResumeDocumentV4(string fileData, string resHTMLText, string resConvertedText, long lRid = 0, bool isReqResID = false)
            {
                try
                {
                    Int64 iResId = 0;
                    Int64 iReqResId = 0;
                    if (isReqResID == false)
                    {
                        iResId = lRid;
                    }
                    else
                    {
                        iReqResId = lRid;
                    }
                iResId = lRid;
                using (SqlConnection con = HC_DBConnection.GetsqlConnection())
                    {
                        using (SqlCommand cmd = con.CreateCommand())
                        {
                            cmd.CommandText = "usp_ConvertDocumentFromV4toV5New";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@RID", iResId);
                          
                            cmd.Parameters.AddWithValue("@FileData", fileData);
                            cmd.Parameters.AddWithValue("@ResHTMLText", resHTMLText == null ? DBNull.Value.ToString() : resHTMLText.Trim());
                            cmd.Parameters.AddWithValue("@ResConvertedText", resConvertedText == null ? DBNull.Value.ToString() : resConvertedText.Trim());

                            var vOpcode = cmd.Parameters.Add("@OPcode", SqlDbType.BigInt);
                            vOpcode.Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    //beErrorLog.opWriteErrorLogToSentry(HCErrorCodes.ServerError, beErrorLog.LogLevel.Error, ex);
                }
            }






            #region [Convert into HTML]
            public JObject beGetconvertedHtmldata(byte[] bFileData, string sFileName, string sFileType)
            {
                JObject beResponse = null;
                byte[] bHtmlDoc = null;
                string tData = "", ConvertedText = "", sResumedoc = "";
                //  bFileData = Convert.FromBase64String(sFileData);
                var webClient = new WebClient();
                string boundary = "------------------------" + DateTime.Now.Ticks.ToString("x");
                webClient.Headers.Add("Content-Type", "multipart/form-data; boundary=" + boundary);
                var fileData = webClient.Encoding.GetString(bFileData);
                var package = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"file\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n{3}\r\n--{0}--\r\n", boundary, sFileName, "application/octet-stream", fileData);
                var nfile = webClient.Encoding.GetBytes(package);
            //Console.WriteLine(HC_DBConnection.ParserUrl);
                byte[] resp = webClient.UploadData(HC_DBConnection.ParserUrl + "convertedtext", "POST", nfile);

                tData = System.Text.Encoding.UTF8.GetString(resp);
                tData = tData.Substring(1, (tData.Length - 2));

                if (tData == "-23")
                {
                    beResponse = new JObject(
                              new JProperty("Id", -23));
                    return beResponse;

                }
                else if (tData == "-22")
                {

                    beResponse = new JObject(
                                  new JProperty("Id", -22));
                    return beResponse;
                }
                else
                {
                    ConvertedText = JsonConvert.SerializeObject(tData, Newtonsoft.Json.Formatting.None);
                }
                //  ImpResume oImpRes = new ImpResume();
                string apiUrl = null, oDoc = null, sData = null;
                clsDocuments.GetHtmlText oHtml = new clsDocuments.GetHtmlText();
                oHtml.FileData = bFileData;
                oHtml.FileName = sFileName;
                oHtml.ResponseType = ".html";
                oDoc = JsonConvert.SerializeObject(oHtml, Newtonsoft.Json.Formatting.None);
                apiUrl = HC_DBConnection.ParserUrl + "htmltext";
                Uri address = new Uri(apiUrl);
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                request.Method = "Post";
                request.ContentType = "application/json";
                var formatData = Encoding.ASCII.GetBytes(oDoc);

                using (var stream = request.GetRequestStream()) //TODO: Need to handle the connection error
                {
                    stream.Write(formatData, 0, formatData.Length);
                    stream.Flush();
                    stream.Close();
                }
                try
                {
                    using (Stream responseStream = request.GetResponse().GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(responseStream, Encoding.Default);
                        sData = reader.ReadToEnd();
                        sData = sData.Substring(1, (sData.Length - 2));
                        bHtmlDoc = Convert.FromBase64String(sData);
                    }

                }
                catch (Exception ex)
                {
                    ex.ToString();
                    // beErrorLog.opWriteErrorLog(HCErrorCodes.DBConnection, ModuleName, ex.Message, ex.StackTrace, beErrorLog.LogLevel.Error);
                }

                sResumedoc = Encoding.Default.GetString(bHtmlDoc);

                beResponse = new JObject(
                                 new JProperty("Id", 1),
                                 new JProperty("ConvertedText", ConvertedText),
                             new JProperty("HTMLText", sResumedoc));
                return beResponse;
            }
            #endregion





    }
}
