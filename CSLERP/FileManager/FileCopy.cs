using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLERP.FileManager
{

    class FileCopy
    {
        public static Boolean CopyFile( string sourcePath, string targetPath, string fileName)
        {
            Boolean status = false;
            try
            {
                string destFile = System.IO.Path.Combine(targetPath, fileName);
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }
                System.IO.File.Copy(sourcePath, destFile, true);
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
    }
}
