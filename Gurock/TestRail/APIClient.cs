using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Gurock.TestRail
{
    public class APIClient
    {
        private string m_user;
        private string m_password;
        private string m_url;

        public APIClient(string base_url)
        {
            if (!base_url.EndsWith("/"))
            {
                base_url += "/";
            }

            this.m_url = base_url + "index.php?/api/v2/";
        }

        public string User
        {
            get { return this.m_user; }
            set { this.m_user = value; }
        }

        public string Password
        {
            get { return this.m_password; }
            set { this.m_password = value; }
        }

        public object SendGet(string uri, object filepath = null)
        {
            return SendRequest("GET", uri, filepath);
        }

        public object SendPost(string uri, object data)
        {
            return SendRequest("POST", uri, data);
        }

        private object SendRequest(string method, string uri, object data)
        {
            string url = this.m_url + uri;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = method;

            string auth = Convert.ToBase64String(
                Encoding.ASCII.GetBytes(
                    String.Format(
                        "{0}:{1}",
                        this.m_user,
                        this.m_password
                    )
                )
            );

            request.Headers.Add("Authorization", "Basic " + auth);

            if (method == "POST")
            {
                if (uri.StartsWith("add_attachment"))  
                {
                    string boundary = String.Format("{0:N}", Guid.NewGuid());
                    string filePath = (String)data;

                    request.ContentType = "multipart/form-data; boundary=" + boundary;

                    using (MemoryStream postDataStream = new MemoryStream())
                    using (StreamWriter postDataWriter = new StreamWriter(postDataStream))
                    {
                        postDataWriter.Write("\r\n--" + boundary + "\r\n");
                        postDataWriter.Write("Content-Disposition: form-data; name=\"attachment\";"
                                        + "filename=\"{0}\""
                                        + "\r\nContent-Type: {1}\r\n\r\n",
                                        Path.GetFileName(filePath),
                                        Path.GetExtension(filePath));
                        postDataWriter.Flush();

                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead;
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                postDataStream.Write(buffer, 0, bytesRead);
                            }

                            postDataWriter.Write("\r\n--" + boundary + "--\r\n");
                            postDataWriter.Flush();


                            request.ContentLength = postDataStream.Length;

                            using (Stream requestStream = request.GetRequestStream())
                            {
                                postDataStream.WriteTo(requestStream);
                            }
                        }
                    }
                }

                else if (data != null)
                {
                    byte[] block = Encoding.UTF8.GetBytes(
                        JsonConvert.SerializeObject(data)
                    );
                    request.GetRequestStream().Write(block, 0, block.Length);
                }
            }
            
            Exception ex = null;
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    throw;
                }

                response = (HttpWebResponse)e.Response;
                ex = e;
            }

            if ((response != null) 
                && ((int)response.StatusCode == 200) 
                && (uri.StartsWith("get_attachment/")))
            {
                byte[] buffer = new byte[1024];

                try
                {
                    Stream receiveStream = response.GetResponseStream();
                    Stream fileStream = File.Create((String)data);
                    long bytesReceived = 0;
                    int bytesRead;

                    while ((bytesRead = receiveStream.Read(buffer, 0, buffer.Length)) > 0)
                    { 
                        fileStream.Write(buffer, 0, bytesRead);
                        bytesReceived += bytesRead;
                    }
                    fileStream.Flush();
                    receiveStream.Close();
                    fileStream.Close();
                    response.Close();

                    return (String) data;
                }
                catch (Exception)
                {
                    throw new APIException("Unable to save attachment.");
                }
            }

            string text = "";
            if (response != null)
            {
                var reader = new StreamReader(
                    response.GetResponseStream(),
                    Encoding.UTF8
                );

                using (reader)
                {
                    text = reader.ReadToEnd();
                }
            }

            JContainer result;
            if (text != "")
            {
                if (text.StartsWith("["))
                {
                    result = JArray.Parse(text);
                }
                else
                {
                    try
                    {
                        result = JObject.Parse(text);
                    }
                    catch   
                    {
                        throw new APIException(String.Format(
                            "TestRail API returned the following: {0}\n",
                            text));
                    }
                }
            }
            else
            {
                result = new JObject();
            }

            if (ex != null)
            {
                string error = (string)result["error"];
                if (error != null)
                {
                    error = '"' + error + '"';
                }
                else
                {
                    error = "No additional error message received";
                }

                throw new APIException(
                    String.Format(
                        "TestRail API returned HTTP {0} ({1})",
                        (int)response.StatusCode,
                        error
                    )
                );
            }

            return result;
        }
    }
}
