using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SubtitleMatcher
{
    public class MatchSubs
    {
        private static string _videoExt = ".mkv;.avi;.mp4;.wmv";
        private static string _logFile = @"E:\Downloads\complete\SABFileMoverLog.txt";

        public string getSubFilePath(string videoFileName, List<string> subFiles)
        {

            string[] titleSplitX = null;
            string patternX = @"^(.+)\.E([0-9]+).*$";
            string episodeNumber = null;
            string subFile = null;

             Match titleMatchX = Regex.Match(videoFileName, patternX);
             if (titleMatchX.Success)
             {
                 titleSplitX = Regex.Split(videoFileName, patternX);
                 episodeNumber = titleSplitX[2].TrimEnd('.', ' ', '-', '_');
                 
                 string patternE = @"^(.+)\.E" + episodeNumber + ".*$";
                 
                 foreach (string s in subFiles)
                 {
                     Match episodeMatchE = Regex.Match(s, patternE);

                     if (episodeMatchE.Success)
                     {
                         subFile = s;
                         break;
                     }

                 }
             
             }
             return subFile;
           
        }

        public void renameSubfile(string folderPath)
        {

            List<string> subFiles = new List<string>();
            MatchSubs m = new MatchSubs();

            var subFilesOnDisk = Directory.GetFiles(folderPath, "*.srt");
            foreach (var file in subFilesOnDisk)
            {
                subFiles.Add(Path.GetFileName(file));
            }

            string[] videoExt = _videoExt.ToString().Split(new char[] { ';' });

            foreach (string ext in videoExt)
            {
                var matchingFiles = Directory.GetFiles(folderPath, "*" + ext);

                foreach (var file in matchingFiles)
                {
                    string filename = Path.GetFileName(file);
                    string targetSubFileName = filename.Replace(ext, ".srt");

                    if (subFiles.Contains(targetSubFileName))
                    {
                        //m.Log("INFO " + DateTime.Now.ToString() + " - Subtitle: " + filename + " subfile already exists");
                    }
                    else
                    {
                        string subFile =  m.getSubFilePath(filename, subFiles);

                        if (subFile != null)
                        {
                            
                            m.Log("INFO " + DateTime.Now.ToString() + " - Subtitle Video: " + filename + " subfile matched: " + subFile);
                            File.Move(Path.Combine(folderPath, subFile), Path.Combine(folderPath, targetSubFileName));
                            subFile = null;
                        }
                    }
                }
            }
        }

        public void Log(string message)
        {
        
            Console.WriteLine(message);
            try
            {
                using (StreamWriter sw = File.AppendText(_logFile))
                {
                    sw.WriteLine(message);
                }
            }
            catch { }
        }





    }
}
