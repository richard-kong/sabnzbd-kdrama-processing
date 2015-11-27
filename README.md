## Introduction
SABnzbd post processing script for korean drama video files. Processing is done by file name where a very specific file name is expected.
As long as the name contains "E##" for episode number, this application should work.	

* Looks for video files in the source folder
* Parses the file name to get the title of the series
* Moves the file to the target folder
* If subtitles already exist in the target file, it will attempt to rename the subtitle to match the video file
* Sends a tweet to notify the file that was downloaded

## Usage
Use as a post processing script in SABnzbd or via the command line where the first argument is the source path and the 7th is a value other than 0. 