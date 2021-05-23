using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass.AddOn.Tags;
using System.Windows.Media;
using System.Drawing;
using Un4seen.Bass;
using TagLib;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace MusicPlayer
{
    public class Song
    {
        public string TrackNumber { get; set; }
        public string Title { get; set; }
        public string AlbumName { get; set; }
        public string ArtistName { get; set; }
        public string Lenght { get; set; }
        public string Year { get; set; }
        public string Inf { get; set; }
        public Image AlbumArt { get; set; }

        public Song(string file)
        {
            TAG_INFO tag_inf= new TAG_INFO();
            tag_inf = BassTags.BASS_TAG_GetFromFile(file);
            if (tag_inf.title == "")
                Title = MainWindow.GetFileName(file);
            else
                Title = tag_inf.title;
            AlbumName = tag_inf.album;
            ArtistName = tag_inf.artist;
            Year = tag_inf.year;
            Inf = tag_inf.bitrate + "KBps, " + tag_inf.channelinfo.chans;

            TagLib.File file_TAG = TagLib.File.Create(file);
            TrackNumber = file_TAG.Tag.Track.ToString();

            int minutes = (int)file_TAG.Properties.Duration.TotalMinutes;
            int seconds = file_TAG.Properties.Duration.Seconds;
            string sec = seconds.ToString();

            if (sec.Length == 1)
                sec = "0" + sec;

            Lenght = minutes.ToString() + ":" + sec;
        }
    }
}
