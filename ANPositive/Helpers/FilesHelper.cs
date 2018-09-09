using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;

namespace ANPositive
{
    public class FilesHelper
    {
        string DeleteURL = null;
        string DeleteType = null;
        string StorageRoot = null;
        string UrlBase = null;
        string tempPath = null;
        string serverMapPath = null;

        public FilesHelper(string DeleteURL, string DeleteType, string StorageRoot, string UrlBase, string tempPath, string serverMapPath)
        {
            this.DeleteURL = DeleteURL;
            this.DeleteType = DeleteType;
            this.StorageRoot = StorageRoot;
            this.UrlBase = UrlBase;
            this.tempPath = tempPath;
            this.serverMapPath = serverMapPath;
        }

        public void DeleteFiles(string pathToDelete)
        {
            string path = HostingEnvironment.MapPath(pathToDelete);

            System.Diagnostics.Debug.WriteLine(path);
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo fi in di.GetFiles())
                {
                    File.Delete(fi.FullName);
                    System.Diagnostics.Debug.WriteLine(fi.Name);
                }

                di.Delete(true);
            }
        }

        public string DeleteFile(string file, string subFolder)
        {
            System.Diagnostics.Debug.WriteLine("DeleteFile");
            System.Diagnostics.Debug.WriteLine(file);
            string fullPath = Path.Combine(StorageRoot + subFolder + "\\", file);
            System.Diagnostics.Debug.WriteLine(fullPath);
            System.Diagnostics.Debug.WriteLine(File.Exists(fullPath));
            string thumbPath = "/" + file + "_thumbs.jpg";
            string partThumb1 = Path.Combine(StorageRoot + subFolder + "\\", "thumbs");
            string partThumb2 = Path.Combine(partThumb1, file + "_thumbs.jpg");

            System.Diagnostics.Debug.WriteLine(partThumb2);
            System.Diagnostics.Debug.WriteLine(File.Exists(partThumb2));
            if (File.Exists(fullPath))
            {
                if (File.Exists(partThumb2))
                {
                    File.Delete(partThumb2);
                }
                File.Delete(fullPath);
                string succesMessage = "Ok";
                return succesMessage;
            }
            string failMessage = "Error Delete";
            return failMessage;
        }

        public JsonFiles GetFileList(string subFolder)
        {
            var r = new List<ViewDataUploadFilesResult>();

            string fullPath = Path.Combine(StorageRoot);
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo dir = new DirectoryInfo(fullPath);
                foreach (FileInfo file in dir.GetFiles())
                {
                    int SizeInt = unchecked((int)file.Length);
                    r.Add(UploadResult(file.Name,SizeInt,file.FullName, subFolder));
                }

            }
            JsonFiles files = new JsonFiles(r);

            return files;
        }

        public void UploadAndShowResults(HttpContextBase ContentBase, List<ViewDataUploadFilesResult> resultList, string subFolder)
        {
            var httpRequest = ContentBase.Request;
            System.Diagnostics.Debug.WriteLine(Directory.Exists(tempPath));

            string fullPath = Path.Combine(StorageRoot + subFolder + "\\");
            Directory.CreateDirectory(fullPath);
            Directory.CreateDirectory(fullPath + "/thumbs/");

            foreach (string inputTagName in httpRequest.Files)
            {

                var headers = httpRequest.Headers;

                var file = httpRequest.Files[inputTagName];
                System.Diagnostics.Debug.WriteLine(file.FileName);

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {

                    UploadWholeFile(ContentBase, resultList, subFolder);
                }
                else
                {

                    UploadPartialFile(headers["X-File-Name"], ContentBase, resultList, subFolder);
                }
            }
        }

        private void UploadWholeFile(HttpContextBase requestContext, List<ViewDataUploadFilesResult> statuses, string subFolder)
        {
            var request = requestContext.Request;
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];
                string uploadedFileExt = Path.GetExtension(file.FileName);
                string newFileName = DateTime.Now.ToString("yyyyMMddTHHmmssfffffff") + uploadedFileExt;
                string pathOnServer = Path.Combine(StorageRoot + subFolder + "\\");
                string fullPath = Path.Combine(pathOnServer, Path.GetFileName(newFileName));
                file.SaveAs(fullPath);

                string[] imageArray = file.FileName.Split('.');
                if (imageArray.Length != 0)
                {
                    string extansion = imageArray[imageArray.Length - 1].ToLower();
                    if (extansion != "jpg" && extansion != "png" && extansion != "jpeg")
                    {
                        
                    }
                    else
                    {
                        var ThumbfullPath = Path.Combine(pathOnServer, "thumbs");
                        string fileThumb = Path.GetFileNameWithoutExtension(newFileName) + "_thumbs.jpg";
                        var ThumbfullPath2 = Path.Combine(ThumbfullPath, fileThumb);
                        using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(fullPath)))
                        {
                            var thumbnail = new WebImage(stream).Resize(320, 320);
                            thumbnail.Save(ThumbfullPath2, "jpg");
                        }
                    }
                }
                statuses.Add(UploadResult(newFileName, file.ContentLength, newFileName, subFolder));
            }
        }

        private void UploadPartialFile(string fileName, HttpContextBase requestContext, List<ViewDataUploadFilesResult> statuses, string subFolder)
        {
            var request = requestContext.Request;
            if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];
            string uploadedFileExt = Path.GetExtension(file.FileName);
            string newFileName = DateTime.Now.ToString("yyyyMMddTHHmmssfffffff") + uploadedFileExt;
            var inputStream = file.InputStream;
            string patchOnServer = Path.Combine(StorageRoot + subFolder + "\\");
            var fullName = Path.Combine(patchOnServer, Path.GetFileName(newFileName));
            var ThumbfullPath = Path.Combine(fullName, Path.GetFileName(newFileName + "_thumbs.jpg"));
            var handler = new ImageHandler();

            var ImageBit = ImageHandler.LoadImage(fullName);
            handler.Save(ImageBit, 80, 80, 10, ThumbfullPath);
            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            statuses.Add(UploadResult(newFileName, file.ContentLength, newFileName, subFolder));
        }

        public ViewDataUploadFilesResult UploadResult(string FileName, int fileSize, string FileFullPath, string subFolder)
        {
            string getType = MimeMapping.GetMimeMapping(FileFullPath);
            var result = new ViewDataUploadFilesResult()
            {
                name = FileName,
                size = fileSize,
                type = getType,
                url = UrlBase + subFolder + "/" + FileName,
                deleteUrl = DeleteURL + FileName + "&subFolder=" + subFolder,
                thumbnailUrl = CheckThumb(getType, FileName, subFolder),
                deleteType = DeleteType,
            };
            return result;
        }

        public string CheckThumb(string type, string FileName, string subFolder)
        {
            var splited = type.Split('/');
            if (splited.Length == 2)
            {
                string extansion = splited[1].ToLower();
                if(extansion.Equals("jpeg") || extansion.Equals("jpg") || extansion.Equals("png") || extansion.Equals("gif"))
                {
                    string thumbnailUrl = UrlBase + subFolder + "/thumbs/" + Path.GetFileNameWithoutExtension(FileName) + "_thumbs.jpg";
                    return thumbnailUrl;
                }
                else
                {
                    if (extansion.Equals("octet-stream")) //Fix for exe files
                    {
                        return "/Content/Free-file-icons/48px/exe.png";

                    }
                    if (extansion.Contains("zip")) //Fix for exe files
                    {
                        return "/Content/Free-file-icons/48px/zip.png";
                    }
                    string thumbnailUrl = "/Content/Free-file-icons/48px/"+ extansion +".png";
                    return thumbnailUrl;
                }
            }
            else
            {
                return UrlBase + "/thumbs/" + Path.GetFileNameWithoutExtension(FileName) + "_thumbs.jpg";
            }
           
        }

        public List<string> FilesList()
        {
            List<string> Filess = new List<string>();
            string path = HostingEnvironment.MapPath(serverMapPath);
            System.Diagnostics.Debug.WriteLine(path);
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo fi in di.GetFiles())
                {
                    Filess.Add(fi.Name);
                    System.Diagnostics.Debug.WriteLine(fi.Name);
                }
            }
            return Filess;
        }
    }

    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string deleteUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string deleteType { get; set; }
    }

    public class JsonFiles
    {
        public ViewDataUploadFilesResult[] files;
        public string TempFolder { get; set; }
        public JsonFiles(List<ViewDataUploadFilesResult> filesList)
        {
            files = new ViewDataUploadFilesResult[filesList.Count];
            for (int i = 0; i < filesList.Count; i++)
            {
                files[i] = filesList.ElementAt(i);
            }

        }
    }
}