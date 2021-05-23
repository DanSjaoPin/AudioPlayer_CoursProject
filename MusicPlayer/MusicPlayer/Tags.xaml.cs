using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TagLib;
using Un4seen.Bass.AddOn.Tags;
using System.Text.RegularExpressions;

namespace MusicPlayer
{
    /// <summary>
    /// Логика взаимодействия для Tags.xaml
    /// </summary>
    public partial class Tags : Window
    {
        public string FilePath { get; set; }

        public Tags(int number)
        {
            InitializeComponent();
            GetTags(number);
        }

        // Кнопка "Закрыть окно"
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Перемещение окна приложения мышью
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void GetTags(int number)
        {
            string filePath = Vars.Files[number];

            FilePath = filePath;

            TagLib.File f = TagLib.File.Create(filePath);

            TBNumber.Text = f.Tag.Track.ToString();
            TBSong.Text = f.Tag.Title;
            TBAlbum.Text = f.Tag.Album;
            foreach(string Artist in f.Tag.Performers)
                TBBand.Text = Artist;
            TBYear.Text = f.Tag.Year.ToString();

            TAG_INFO tag_inf = new TAG_INFO();
            tag_inf = BassTags.BASS_TAG_GetFromFile(filePath);

            TBBitrate.Text = tag_inf.bitrate + " KBps";
            TBChannels.Text = tag_inf.channelinfo.ToString();
        }

        //Сохранить изменения
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(FilePath);

                try
                {
                    f.Tag.Track = Convert.ToUInt16(TBNumber.Text);
                }
                catch
                {
                    
                    if (Vars.IsRus == true)
                        MessageBox.Show("Номер трэка может содержать только числа");
                    if (Vars.IsRus != true)
                        MessageBox.Show("The track number can contain only a numbers");
                    return;
                }

                try
                {
                    f.Tag.Title = TBSong.Text;
                    f.Tag.Album = TBAlbum.Text;
                    f.Tag.Performers[0] = TBBand.Text;
                }
                catch 
                {
                    if (Vars.IsRus == true)
                        MessageBox.Show("Некорректные данные в текстовых подях");
                    if (Vars.IsRus != true)
                        MessageBox.Show("Incorrect values it text areas");
                }

                try
                {
                    f.Tag.Year = Convert.ToUInt16(TBYear.Text);
                }
                catch
                {
                    if (Vars.IsRus == true)
                        MessageBox.Show("Год может содержать максимум 4 цифры");
                    if (Vars.IsRus != true)
                        MessageBox.Show("The year can contain a maximum of 4 numbers");
                    return;
                }
                f.Save();

                if (Vars.IsRus == true)
                    MessageBox.Show("Теги успешно изменены\nвыберите трек в плейлист заново чтоб увидеть изменения") ;
                if (Vars.IsRus != true)
                    MessageBox.Show("Tags have been successfully changed\nselect the track in the playlist again to see the changes");
            }
            catch 
            {
                if(Vars.IsRus == true)
                    MessageBox.Show("Остановите воспроизведение трэка");
                if (Vars.IsRus != true)
                    MessageBox.Show("Stop playing a track");
            }
        }
    }
}
