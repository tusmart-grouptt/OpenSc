# YoutubeExtractor

## Overview
YoutubeExtractor is a reusable library for .NET, written in C#, that allows to download videos from YouTube and/or extract their audio track (currently only for flash videos).

## Credits

### - [YoutubeFisher](http://youtubefisher.codeplex.com/)
Code for resolving the video download links

### - [FlvExtract](http://moitah.net/)
Code for extracting MP3 and AAC audio tracks out of flash files.

## License

YouTubeExtractor is licenced under the [GNU General Public License version 2 (GPLv2)](http://opensource.org/licenses/gpl-2.0)

## Dependencies

- .NET Framework 3.5

## NuGet

YoutubeExtractor is available on [NuGet](http://nuget.org/packages/YoutubeExtractor)!

## Projects that use this library

- [Espera](http://github.com/flagbug/Espera)

## Example code

**Get the download URLs**

```c#

// Our test youtube link
string link = "insert youtube link";

/*
 * Get the available video formats.
 * We'll work with them in the video and audio download examples.
 */
IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);

```

**Download the video**

```c#

/*
 * Select the first .mp4 video with 360p resolution
 */
VideoInfo video = videoInfos
    .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);

/*
 * Create the video downloader.
 * The first argument is the video to download.
 * The second argument is the path to save the video file.
 */
var videoDownloader = new VideoDownloader(video, Path.Combine("D:/Downloads", video.Title + video.VideoExtension));

// Register the ProgressChanged event and print the current progress
videoDownloader.ProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);

/*
 * Execute the video downloader.
 * For GUI applications note, that this method runs synchronously.
 */
videoDownloader.Execute();

```

**Download the audio track**

```c#

/*
 * We want the first extractable video with the highest audio quality.
 */
VideoInfo video = videoInfos
    .Where(info => info.CanExtractAudio)
    .OrderByDescending(info => info.AudioBitrate)
    .First();

/*
 * Create the audio downloader.
 * The first argument is the video where the audio should be extracted from.
 * The second argument is the path to save the audio file.
 */
var audioDownloader = new AudioDownloader(video, Path.Combine("D:/Downloads", video.Title + video.AudioExtension));

// Register the ProgressChanged event and print the current progress
audioDownloader.ProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);

/*
 * Execute the audio downloader.
 * For GUI applications note, that this method runs synchronously.
 */
audioDownloader.Execute();

```