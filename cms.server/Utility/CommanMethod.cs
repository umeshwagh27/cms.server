namespace cms.server.Utility
{
    public static class CommonMethod
    {
        public static async Task<string> WriteFile(string rootpath, string folderName, string moduleName, IFormFile file)
        {
            string fileName = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = moduleName + "_" + DateTime.Now.Ticks + extension;

                var pathBuilt = "";
                if (extension == ".mp4")
                {
                    pathBuilt = Path.Combine(rootpath + "/", folderName);
                }
                else if (extension == ".pdf")
                {
                    pathBuilt = Path.Combine(rootpath + "/", folderName);

                }
                else
                {
                    //pathBuilt = Path.Combine(ImageConstant.returnImages+"/", folderName);
                    pathBuilt = Path.Combine(rootpath + "/", folderName);
                }

                if (!Directory.Exists(pathBuilt))
                    Directory.CreateDirectory(pathBuilt);
                var path = "";
                if (extension == ".mp4")
                {
                    //path = Path.Combine(ImageConstant.returnVideos, folderName, fileName);
                    path = Path.Combine(rootpath + "/", folderName + "/", fileName);
                }
                else if (extension == ".pdf")
                {
                    path = Path.Combine(rootpath + "/", folderName + "/", fileName);

                }
                else
                {
                    // path = Path.Combine(ImageConstant.returnOthers+"/",folderName+"/",fileName);
                    path = Path.Combine(rootpath + "/", folderName + "/", fileName);
                }


                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

            }
            catch (Exception)
            {
                //log error
            }

            return fileName;
        }

        public static bool DeleteFile(string rootpath, string folderName, string fileName)
        {
            bool isFileDeleted = false;
            try
            {
                var extension = Path.GetExtension(fileName);
                var path = Path.Combine(rootpath + "/", folderName + "/", fileName);


                if (File.Exists(path))
                {
                    File.Delete(path);
                    isFileDeleted = true;
                }

            }
            catch (Exception)
            {
                //log error
            }

            return isFileDeleted;
        }

    }
}