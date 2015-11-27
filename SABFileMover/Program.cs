using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SubtitleMatcher;
using TweetSharp;
using System.Configuration;

namespace SABFileMover
{


    class Program
    {


       


        private static string _videoExt = ".srt;.mkv;.avi;.mp4;.wmv";
        private static string _kdramaFolder = ConfigurationManager.AppSettings["targetFolder"];

        static void Main(string[] args)
        {

           // string category = "K-Drama"; //args[4];

           string folderPath = args[0]; //Get showPath from first CMD Line Argument
           string postprocessStatus = args[6];
           MatchSubs m = new MatchSubs();

           if (postprocessStatus != "0")
           {
               Console.WriteLine("Unpacking failed: Post Processing aborted");
               return;
           }

           string[] videoExt = _videoExt.ToString().Split(new char[] {';'});

            string showName = null;
            string[] titleSplitX = null;
            string patternX = @"^(.+)\.E([0-9]+).*$";

            int filesProcessed = 0;

            foreach (string ext in videoExt)
            {
                var matchingFiles = Directory.GetFiles(folderPath, "*" + ext);

                               
                foreach (var file in matchingFiles)
                {
                    string filename = Path.GetFileName(file);
                    string sourceFilePath = Path.GetFullPath(file);

                    Match titleMatchX = Regex.Match(filename, patternX);
                    if (titleMatchX.Success)
                    {
                        titleSplitX = Regex.Split(filename, patternX);
                        showName = titleSplitX[1].TrimEnd('.', ' ', '-', '_');
                        showName = showName.Replace('.', ' ').Trim();
                        string targetFolder =  Path.Combine(_kdramaFolder, showName);
                        

                        if (!Directory.Exists(targetFolder))
                        {
                            Directory.CreateDirectory(targetFolder);

                        }

                        if (File.Exists(Path.Combine(targetFolder,filename)))
                        {
                             m.Log("ERROR " + DateTime.Now.ToString()  + " - Target file exists " + filename + " in " + targetFolder);
                        }
                        else
                        {
                             File.Move(sourceFilePath, Path.Combine(targetFolder,filename));
                             m.Log("INFO " + DateTime.Now.ToString() + " - Moved " + filename + " to " + targetFolder);
                            

                             if (ext != ".srt")
                             {
                                 m.renameSubfile(targetFolder);
                                 Tweet("Drama: " + filename);
                             }
                            }
                    }
                    else
                    {
                        m.Log("ERROR " + DateTime.Now.ToString()   + " - Unable to parse filename: " + filename);

                    }

                    filesProcessed += 1;      
                }

            }

            if (filesProcessed == 0)
            {
                m.Log("ERROR " + DateTime.Now.ToString() + " - No Files Processed: " + folderPath);

            }
        }

        public static void Tweet(string message)
        {

            string consumerKey = ConfigurationManager.AppSettings["twiiterConsumerKey"];
            string consumerSecret = ConfigurationManager.AppSettings["twitterConsumerSecret"];
            string accessToken = ConfigurationManager.AppSettings["twitterAccessToken"];
            string accessTokenSecret = ConfigurationManager.AppSettings["twitterAccessTokenSecret"];


            var service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(accessToken, accessTokenSecret);
            service.SendTweet(new SendTweetOptions { Status = message});
        }

    }
}
