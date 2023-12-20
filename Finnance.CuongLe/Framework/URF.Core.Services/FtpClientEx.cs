using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace URF.Core.Services
{
    public class FtpClientEx
    {

        private readonly string _host;
        private readonly string _user;
        private readonly string _pass;
        private FtpWebRequest _request;
        private FtpWebResponse _response;
        private const int bufferSize = 2048;
        private readonly List<string> _ListFile = new List<string>();
        private readonly List<string> _ListDirectory = new List<string>();
        private static string[] _ParseFormats = new string[] {
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)",
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)",
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)",
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)",
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})(\\s+)(?<size>(\\d+))(\\s+)(?<ctbit>(\\w+\\s\\w+))(\\s+)(?<size2>(\\d+))\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{2}:\\d{2})\\s+(?<name>.+)",
            "(?<timestamp>\\d{2}\\-\\d{2}\\-\\d{2}\\s+\\d{2}:\\d{2}[Aa|Pp][mM])\\s+(?<dir>\\<\\w+\\>){0,1}(?<size>\\d+){0,1}\\s+(?<name>.+)" };

        public FtpClientEx(string hostIP, string userName, string password)
        {
            _user = userName;
            _pass = password;
            _host = "ftp://" + hostIP;
        }

        public bool Delete(string deleteFile)
        {
            try
            {
                Uri uriSource = deleteFile.Contains(_host)
                    ? new Uri(deleteFile)
                    : new Uri(_host + "/" + deleteFile);
                /* Create an FTP Request */
                _request = (FtpWebRequest)WebRequest.Create(uriSource);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _request.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _request.UseBinary = true;
                _request.UsePassive = true;
                _request.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _request.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Communication with the FTP Server */
                _response = (FtpWebResponse)_request.GetResponse();
                /* Resource Cleanup */
                _response.Close();
                _request = null;
                return true;
            }
            catch
            {
                return false;
            }
        }
       
        public string DownloadAsString(string remoteFile)
        {
            var result = string.Empty;
            try
            {
                /* Create an FTP Request */
                _request = (FtpWebRequest)WebRequest.Create(_host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _request.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _request.UseBinary = true;
                _request.UsePassive = true;
                _request.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _request.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                _response = (FtpWebResponse)_request.GetResponse();
                /* Get the FTP Server's Response Stream */
                var ftpStream = _response.GetResponseStream();
                /* Open a File Stream to Write the Downloaded File */
                var localFileStream = new MemoryStream();
                /* Buffer for the Downloaded Data */
                var byteBuffer = new byte[bufferSize];
                if (ftpStream != null)
                {
                    var bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                    /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
                    try
                    {
                        while (bytesRead > 0)
                        {
                            localFileStream.Write(byteBuffer, 0, bytesRead);
                            bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                        }
                        result = Encoding.UTF8.GetString(localFileStream.ToArray());
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
                /* Resource Cleanup */
                localFileStream.Close();
                if (ftpStream != null) ftpStream.Close();
                _response.Close();
                _request = null;
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.ToString();
            }
            return result;
        }
        public bool Download(string remoteFile, string localFile)
        {
            try
            {
                /* Create an FTP Request */
                _request = (FtpWebRequest)WebRequest.Create(_host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _request.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _request.UseBinary = true;
                _request.UsePassive = true;
                _request.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _request.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                _response = (FtpWebResponse)_request.GetResponse();
                /* Get the FTP Server's Response Stream */
                var ftpStream = _response.GetResponseStream();
                /* Open a File Stream to Write the Downloaded File */
                var localFileStream = new FileStream(localFile, FileMode.Create);
                /* Buffer for the Downloaded Data */
                var byteBuffer = new byte[bufferSize];
                if (ftpStream != null)
                {
                    var bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                    /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
                    try
                    {
                        while (bytesRead > 0)
                        {
                            localFileStream.Write(byteBuffer, 0, bytesRead);
                            bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
                /* Resource Cleanup */
                localFileStream.Close();
                if (ftpStream != null) ftpStream.Close();
                _response.Close();
                _request = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Upload(string localFile, string remoteFile, bool reUp = false)
        {
            try
            {
                // Check file local
                if (!File.Exists(localFile)) return false;

                // Create path ftp
                var uri = _host.StartsWith("ftp")
                              ? string.Format("{0}/{1}", _host, remoteFile)
                              : string.Format("ftp://{0}/{1}", _host, remoteFile);

                // Check file ftp
                if (!reUp && Exists(uri))
                    return true;

                // Create folder ftp
                var folderFtp = Path.GetDirectoryName(remoteFile);
                if (!string.IsNullOrEmpty(folderFtp))
                    CreateDirectory(folderFtp);

                /* Create an FTP Request */
                _request = (FtpWebRequest)WebRequest.Create(_host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _request.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _request.UseBinary = true;
                _request.KeepAlive = true;
                _request.UsePassive = true;
                /* Specify the Type of FTP Request */
                _request.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                var ftpStream = _request.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                var localFileStream = new FileStream(localFile, FileMode.OpenOrCreate);
                /* Buffer for the Downloaded Data */
                var byteBuffer = new byte[bufferSize];
                var bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex) { throw ex; }
                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                _request = null;
                return true;
            }
            catch (Exception ex) { throw ex; }
        }
        public bool UploadAsStream(string remoteFile, Stream stream, bool reUp = false)
        {
            try
            {
                // Create path ftp
                var uri = _host.StartsWith("ftp")
                              ? string.Format("{0}/{1}", _host, remoteFile)
                              : string.Format("ftp://{0}/{1}", _host, remoteFile);

                // Check file ftp
                if (!reUp && Exists(uri))
                    return true;

                // Create folder ftp
                var folderFtp = Path.GetDirectoryName(remoteFile);
                if (!string.IsNullOrEmpty(folderFtp))
                    CreateDirectory(folderFtp);

                /* Create an FTP Request */
                _request = (FtpWebRequest)WebRequest.Create(_host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _request.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _request.UseBinary = true;
                _request.KeepAlive = true;
                _request.UsePassive = true;
                /* Specify the Type of FTP Request */
                _request.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                var ftpStream = _request.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                var localFileStream = stream;
                /* Buffer for the Downloaded Data */
                var byteBuffer = new byte[bufferSize];
                var bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                _request = null;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UploadAsString(string remoteFile, string textStream, bool reUp = false)
        {
            try
            {
                // Create path ftp
                var uri = _host.StartsWith("ftp")
                              ? string.Format("{0}/{1}", _host, remoteFile)
                              : string.Format("ftp://{0}/{1}", _host, remoteFile);

                // Check file ftp
                if (!reUp && Exists(uri))
                    return true;

                // Create folder ftp
                var folderFtp = Path.GetDirectoryName(remoteFile);
                if (!string.IsNullOrEmpty(folderFtp))
                    CreateDirectory(folderFtp);

                /* Create an FTP Request */
                _request = (FtpWebRequest)WebRequest.Create(_host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _request.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _request.UseBinary = true;
                _request.KeepAlive = true;
                _request.UsePassive = true;
                /* Specify the Type of FTP Request */
                _request.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                var ftpStream = _request.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                var localFileStream = new MemoryStream(Encoding.UTF8.GetBytes(textStream));
                /* Buffer for the Downloaded Data */
                var byteBuffer = new byte[bufferSize];
                var bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                _request = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Exists(string fileFtp)
        {
            // Create path ftp
            var uri = _host.StartsWith("ftp")
                          ? string.Format("{0}/{1}", _host, fileFtp)
                          : string.Format("ftp://{0}/{1}", _host, fileFtp);
            if (_ListFile.Contains(uri)) return true;

            _request = (FtpWebRequest)WebRequest.Create(uri);
            _request.Credentials = new NetworkCredential(_user, _pass);
            _request.Method = WebRequestMethods.Ftp.GetFileSize;
            _request.UsePassive = true;
            _request.UseBinary = true;

            try
            {
                var response = (FtpWebResponse)_request.GetResponse();
                _ListFile.Add(uri);
                response.Close();
                return true;
            }
            catch (WebException ex)
            {
                var response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    return false;
            }
            return false;
        }
        public void CreateDirectory(string newDirectory)
        {
            var directory = string.Empty;
            var arrDirectory = newDirectory.Split(new[] { '/', '\\' });
            foreach (var item in arrDirectory)
            {
                try
                {
                    directory += string.IsNullOrEmpty(directory) ? item : "/" + item;
                    if (_ListDirectory.Contains(directory)) continue;

                    /* Create an FTP Request */
                    _request = (FtpWebRequest)WebRequest.Create(_host + "/" + directory);
                    /* Log in to the FTP Server with the User Name and Password Provided */
                    _request.Credentials = new NetworkCredential(_user, _pass);
                    /* When in doubt, use these options */
                    _request.UseBinary = true;
                    _request.KeepAlive = true;
                    _request.UsePassive = true;
                    /* Specify the Type of FTP Request */
                    _request.Method = WebRequestMethods.Ftp.MakeDirectory;
                    /* Establish Return Communication with the FTP Server */
                    _response = (FtpWebResponse)_request.GetResponse();
                    /* Resource Cleanup */
                    _response.Close();
                    _request = null;
                }
                catch
                {
                    _request = null;
                }
                finally
                {
                    if (!_ListDirectory.Contains(directory))
                        _ListDirectory.Add(directory);
                }
            }
        }
        public bool FtpRename(string source, string destination)
        {
            if (source == destination) return false;
            Uri uriSource = source.Contains(_host)
                ? new Uri(source)
                : new Uri(_host + "/" + source);
            Uri uriDestination = destination.Contains(_host)
                ? new Uri(destination)
                : new Uri(_host + "/" + destination);

            // Do the files exist?
            if (!Exists(uriSource.AbsolutePath)) return false;
            if (Exists(uriDestination.AbsolutePath))
            {
                var resultDelete = Delete(uriDestination.AbsolutePath);
                if (!resultDelete)
                    return false;
            }
            Uri targetUriRelative = uriSource.MakeRelativeUri(uriDestination);


            //perform rename
            _request = (FtpWebRequest)WebRequest.Create(uriSource.AbsoluteUri);
            _request.KeepAlive = true;
            _request.UseBinary = true;
            _request.UsePassive = true;
            _request.Method = WebRequestMethods.Ftp.Rename;
            _request.Credentials = new NetworkCredential(_user, _pass);
            _request.RenameTo = Uri.UnescapeDataString(targetUriRelative.OriginalString);

            try
            {
                var response = (FtpWebResponse)_request.GetResponse();
                response.Close();
                return true;
            }
            catch (WebException ex)
            {
                var response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    return false;
            }
            return false;

        }

        public string GetFileSize(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                _request = (FtpWebRequest)WebRequest.Create(_host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _request.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _request.UseBinary = true;
                _request.UsePassive = true;
                _request.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _request.Method = WebRequestMethods.Ftp.GetFileSize;
                /* Establish Return Communication with the FTP Server */
                _response = (FtpWebResponse)_request.GetResponse();
                /* Establish Return Communication with the FTP Server */
                var ftpStream = _response.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                var ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                try { while (ftpReader.Peek() != -1) { fileInfo = ftpReader.ReadToEnd(); } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                if (ftpStream != null) ftpStream.Close();
                _response.Close();
                _request = null;
                /* Return File Size */
                return fileInfo;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }
        public string GetFileCreatedDateTime(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                _request = (FtpWebRequest)WebRequest.Create(_host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _request.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _request.UseBinary = true;
                _request.UsePassive = true;
                _request.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _request.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                /* Establish Return Communication with the FTP Server */
                _response = (FtpWebResponse)_request.GetResponse();
                /* Establish Return Communication with the FTP Server */
                var ftpStream = _response.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                var ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                try { fileInfo = ftpReader.ReadToEnd(); }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                if (ftpStream != null) ftpStream.Close();
                _response.Close();
                _request = null;
                /* Return File Created Date Time */
                return fileInfo;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }
        public string[] DirectoryListSimple(string directory)
        {
            try
            {
                // folder parent
                var folderParent = directory.Split('/')[directory.Split('/').Length - 1];

                /* Create an FTP Request */
                _request = (FtpWebRequest)WebRequest.Create(_host + "/" + directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _request.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _request.UseBinary = true;
                _request.UsePassive = true;
                _request.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _request.Method = WebRequestMethods.Ftp.ListDirectory;
                /* Establish Return Communication with the FTP Server */
                _response = (FtpWebResponse)_request.GetResponse();
                /* Establish Return Communication with the FTP Server */
                var ftpStream = _response.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                var ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try
                {
                    while (ftpReader.Peek() != -1)
                    {
                        // Get folder
                        var folder = ftpReader.ReadLine();
                        if (folder == null) continue;

                        // Trim
                        folder = folder.Replace(folderParent, string.Empty);
                        folder = folder.Trim(new[] { ' ', '.', '/' });
                        if (string.IsNullOrEmpty(folder)) continue;
                        folder = directory + "/" + folder;
                        directoryRaw += string.IsNullOrEmpty(directoryRaw) ? folder : "|" + folder;
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                if (ftpStream != null) ftpStream.Close();
                _response.Close();
                _request = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try
                {
                    if (directoryRaw != null)
                    {
                        var directoryList = directoryRaw
                            .Split("|".ToCharArray())
                            .Where(c => !string.IsNullOrEmpty(c))
                            .ToArray();
                        return directoryList;
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return null;
        }
        public string[] DirectoryListDetailed(string directory = default)
        {
            try
            {
                /* Create an FTP Request */
                _request = string.IsNullOrEmpty(directory)
                    ? (FtpWebRequest)WebRequest.Create(_host)
                    : (FtpWebRequest)WebRequest.Create(_host + "/" + directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _request.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _request.UseBinary = true;
                _request.UsePassive = true;
                _request.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                /* Establish Return Communication with the FTP Server */
                _response = (FtpWebResponse)_request.GetResponse();
                /* Establish Return Communication with the FTP Server */
                var ftpStream = _response.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                var ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                if (ftpStream != null) ftpStream.Close();
                _response.Close();
                _request = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try
                {
                    if (directoryRaw != null)
                    {
                        var result = new List<string>();
                        foreach (var line in directoryRaw.Split('|'))
                        {
                            if (string.IsNullOrEmpty(line)) continue;
                            for (int i = 0; i <= _ParseFormats.Length - 1; i++)
                            {
                                var rx = new Regex(_ParseFormats[i]);
                                var match = rx.Match(line);
                                if (match.Success)
                                {
                                    var filename = match.Groups["name"].Value;
                                    if (!string.IsNullOrEmpty(directory))
                                        filename = directory + "/" + filename;
                                    result.Add(filename);
                                    break;
                                }
                            }
                        }
                        return result.ToArray();
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return null;
        }
    }
}
