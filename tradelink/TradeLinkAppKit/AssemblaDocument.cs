﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;
using TradeLink.API;
using TradeLink.Common;
using System.IO;

namespace TradeLink.AppKit
{
    /// <summary>
    /// this class is for working with assembla documents.
    /// see : https://www.assembla.com/wiki/show/breakoutdocs/Document_REST_API
    /// </summary>
    public class AssemblaDocument
    {
        public static string GetDocumentsUrl(string space)
        {
            return "http://www.assembla.com/spaces/" + space + "/documents";
        }
        public static bool Create(string space, string user, string password, string filepath) { return Create(space, user, password, filepath, 0); }
        public static bool Create(string space, string user, string password, string filename, int ticketid) { return Create(space, user, password, filename, 0, true); }
        public static bool Create(string space, string user, string password, string filename, int ticketid, bool prependdatetime)
        {
            string url = GetDocumentsUrl(space);
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();

                // Generate post objects
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                string unique = prependdatetime ? Util.ToTLDate(DateTime.Now).ToString() +Util.DT2FT(DateTime.Now)+ Path.GetFileName(filename) : Path.GetFileName(filename);
                postParameters.Add("document[name]", unique);
                postParameters.Add("document[file]", data);
                if (ticketid!=0)
                    postParameters.Add("document[ticket_id]",ticketid);

                // Create request and receive response
                string postURL = url;
                try
                {
                    HttpWebResponse webResponse = WebHelpers.MultipartFormDataPost(postURL, user,password, postParameters,contenttype(filename),filename);


                    // Process response
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    string fullResponse = responseReader.ReadToEnd();
                    webResponse.Close();
                }
                catch (WebException ex)
                {
                    // workaround for http://www.assembla.com/spaces/AssemblaSupport/support/tickets/122--500-internal-server-error--received-when-trying-to-create-documents
                    if (ex.Message.Contains("500")) return true;
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                if (SendDebug != null)
                    SendDebug(DebugImpl.Create("exception: " + ex.Message + ex.StackTrace));

                return false;
            }

        }

        public static bool DownloadDocument(AssemblaDoc doc, string user, string password) { return DownloadDocument(doc, Environment.CurrentDirectory, user, password); }
        public static bool DownloadDocument(AssemblaDoc doc, string path, string user, string password)
        {
            if (!doc.isValid) return false;
            string url = doc.Url;
            HttpWebRequest hr = WebRequest.Create(url) as HttpWebRequest;
            hr.Credentials = new System.Net.NetworkCredential(user, password);
            hr.PreAuthenticate = true;
            hr.Method = "GET";
            hr.ContentType = "application/xml";
            HttpWebResponse wr = (HttpWebResponse)hr.GetResponse();
            Stream stream = wr.GetResponseStream();
            byte[] buff = new byte[(int)wr.ContentLength];
            int n = stream.Read(buff, 0, (int)wr.ContentLength);
            if (n == 0) return false;
            try
            {
                stream.Close();
                wr.Close();
                FileStream fs = new FileStream(path + "//" + doc.Name, FileMode.Create);
                fs.Write(buff, 0, (int)buff.Length);
                fs.Close();
                return true;
            }
            catch { }
            return false;
        }

        public static List<AssemblaDoc> GetDocuments(string space, string user, string password)
        {
            string url = GetDocumentsUrl(space);
            HttpWebRequest hr = WebRequest.Create(url) as HttpWebRequest;
            hr.Credentials = new System.Net.NetworkCredential(user, password);
            hr.PreAuthenticate = true;
            hr.Method = "GET";
            hr.ContentType = "application/xml";
            HttpWebResponse wr = (HttpWebResponse)hr.GetResponse();
            StreamReader sr = new StreamReader(wr.GetResponseStream());
            XmlDocument xd = new XmlDocument();
            string result = sr.ReadToEnd();
            xd.LoadXml(result);
            List<AssemblaDoc> docs = new List<AssemblaDoc>();
            XmlNodeList xnl = xd.GetElementsByTagName("document");
            foreach (XmlNode xn in xnl)
            {
                AssemblaDoc doc = new AssemblaDoc();
                doc.Space = space;
                foreach (XmlNode dc in xn.ChildNodes)
                {
                    string m = dc.InnerText;
                    if (dc.Name == "id")
                        doc.Id = m;
                    else if (dc.Name == "filesize")
                        doc.Size = Convert.ToInt32(m);
                    else if (dc.Name == "description")
                        doc.Desc = m;
                    else if (dc.Name == "name")
                        doc.Name = m;
                }
                if (doc.isValid)
                    docs.Add(doc);
            }
            return docs;

        }

        static string contenttype(string filename)
        {
            string ext = Path.GetExtension(filename);
            ext = ext.ToLower();
            if (ext == ".jpg")
                return @"image/jpeg";
            else if (ext == ".txt")
                return @"text/plain";
            else if (ext == ".zip")
                return @"application/zip";
            return "application/octet-stream";
        }

        public static bool Delete(string space, string user, string password, string documentid)
        {
            string url = GetDocumentsUrl(space) + documentid;
            HttpWebRequest hr = WebRequest.Create(url) as HttpWebRequest;
            hr.Credentials = new System.Net.NetworkCredential(user, password);
            hr.Method = "DELETE";
            hr.ContentType = "application/xml";
            try
            {
                // write it
                //System.IO.Stream post = hr.GetRequestStream();
                //post.Write(bytes, 0, 0);
                // get response
                System.IO.StreamReader response = new System.IO.StreamReader(hr.GetResponse().GetResponseStream());
                // display it
                if (SendDebug != null)
                    SendDebug(DebugImpl.Create(response.ReadToEnd()));
            }
            catch (Exception ex)
            {
                if (SendDebug != null)
                    SendDebug(DebugImpl.Create("exception: " + ex.Message + ex.StackTrace));
                return false;
            }
            return true;
        }

        public static event DebugFullDelegate SendDebug;
    }

    public static class WebHelpers
    {
        public static Encoding encoding = Encoding.UTF8;

        /// <summary>
        /// Post the data as a multipart form
        /// postParameters with a value of type byte[] will be passed in the form as a file, and value of type string will be
        /// passed as a name/value pair.
        /// </summary>
        public static HttpWebResponse MultipartFormDataPost(string postUrl, string user, string pw, Dictionary<string, object> postParameters, string contenttype, string fn)
        {
            string formDataBoundary = "-----------------------------28947758029299";
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = WebHelpers.GetMultipartFormData(postParameters, formDataBoundary,contenttype,fn);

            return WebHelpers.PostForm(postUrl,  user,pw,contentType, formData);
        }

        /// <summary>
        /// Post a form
        /// </summary>
        private static HttpWebResponse PostForm(string postUrl, string user, string pw, string contentType, byte[] formData)

        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;
            request.Credentials = new System.Net.NetworkCredential(user, pw);
            request.PreAuthenticate = true;
            request.Method = "POST";
            request.ContentType = contentType;
            SetBasicAuthHeader(request, user, pw);


            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            // Add these, as we're doing a POST

            //request.UserAgent = userAgent;

            // We need to count how many bytes we're sending. 
            request.ContentLength = formData.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                // Push it out there
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }



            return request.GetResponse() as HttpWebResponse;
        }

        public static void SetBasicAuthHeader(WebRequest req, String userName, String userPassword)
        {
            string authInfo = userName + ":" + userPassword;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            req.Headers["Authorization"] = "Basic " + authInfo;
        }

        /// <summary>
        /// Turn the key and value pairs into a multipart form.
        /// See http://www.ietf.org/rfc/rfc2388.txt for issues about file uploads
        /// </summary>
        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary, string contenttype, string fn)
        {
            Stream formDataStream = new System.IO.MemoryStream();

            foreach (var param in postParameters)
            {
                if (param.Value is byte[])
                {
                    byte[] fileData = param.Value as byte[];

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n", boundary, param.Key, fn,contenttype);
                    formDataStream.Write(encoding.GetBytes(header), 0, header.Length);

                    // Write the file data directly to the Stream, rather than serializing it to a string.  This 
                    formDataStream.Write(fileData, 0, fileData.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n", boundary, param.Key, param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, postData.Length);
                }
            }

            // Add the end of the request
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, footer.Length);

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }
    }


    public struct AssemblaDoc
    {
        public bool isValid { get { return (Space != null) && (Id != null); } }
        public string Url { get { return AssemblaDocument.GetDocumentsUrl(Space) + "/" + Id + "/download"; } }
        public string Desc;
        public string Space;
        public string Id;
        public int Size;
        public string Name;
        public override string ToString()
        {
            return !isValid ? "<empty>" : Name;
        }
    }
}
