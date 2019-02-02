using System;
using System.Collections.Generic;
using System.IO;

namespace UtilityLibraries
{
    public class Class1
    {
        public static List<string> CheckOldFilesFromDirectory(string folder, string mask)
        {
            int time = -10;

            if (folder == "" || folder == null)
            {
                throw new ArgumentNullException();
            }
            if (mask == "" || mask == null)
            {
                throw new ArgumentNullException();
            }

            List<string> files = new List<string>(Directory.GetFiles(folder));
            List<string> returnFiles = new List<string>();

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if ((fi.LastAccessTime < DateTime.Now.AddMinutes(time)) && fi.Extension == mask)
                {
                    Console.WriteLine(fi.Extension);
                    returnFiles.Add(file);
                }
            }
            return returnFiles;
        }
    }
}
