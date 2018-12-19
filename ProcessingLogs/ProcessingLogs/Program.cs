using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingLogs
{
    class Program
    {
        static void Main(string[] args)
        {
            string webAppOrgLogPathsStr = Properties.Settings.Default.WebAppOrgLogDirPaths;
            string[] webAppOrgLogPaths = webAppOrgLogPathsStr.Split(',');

            string webAppLogPathsStr = Properties.Settings.Default.WebAppLogDirPaths;
            string[] webAppLogPaths = webAppLogPathsStr.Split(',');

            var dateTimeNow = DateTime.Now;
            var dateTimeDateStr = dateTimeNow.ToString("yyyyMMdd");
            for (var i = 0; i < webAppOrgLogPaths.Length; i++)
            {
                if (!Directory.Exists(webAppLogPaths[i]))
                {
                    Directory.CreateDirectory(webAppLogPaths[i]);
                }

                string[] orgFiles = Directory.GetFiles(webAppOrgLogPaths[i], "*", SearchOption.AllDirectories);
                foreach (var orgFile in orgFiles)
                {
                    var fileName = Path.GetFileName(orgFile);
                    if (fileName.Contains(dateTimeDateStr))
                    {
                        string newFilePath = Path.Combine(webAppLogPaths[i], fileName);
                        File.Copy(orgFile, newFilePath, true);
                    }
                }
            }

            var searchWordsStr = Properties.Settings.Default.SearchWords;
            var searchWords = searchWordsStr.Split(',');
            var searchCount = 0;
            for (var i = 0; i < webAppLogPaths.Length; i++)
            {
                string[] files = Directory.GetFiles(webAppLogPaths[i], "*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    string[] contentStrArray = File.ReadAllLines(file, Encoding.UTF8);
                    for (int j = 0; j < contentStrArray.GetLength(0); j++)
                    {
                        foreach (var searchWord in searchWords)
                        {
                            if (contentStrArray[j].Contains(searchWord))
                            {
                                searchCount++;
                            }
                        }
                    }
                }
            }

            Console.WriteLine(searchCount);
        }
    }
}
