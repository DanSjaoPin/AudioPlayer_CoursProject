using System;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using System.IO;
using System.Collections.Generic;
using System.Windows.Threading;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace MusicPlayer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BassPlayer.InitBass(BassPlayer.Hz);
            GetPlaylists();
        }

        // Кнопка "Закрыть окно"
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application app = Application.Current;
            app.Shutdown();
        }

        // Кнопка Свернуть
        private void Turn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        // Перемещение окна приложения мышью
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //Языки
        private void Rus_Click(object sender, RoutedEventArgs e)
        {
            this.Resources.Clear();
            this.Resources = new ResourceDictionary() { Source = new Uri("pack://application:,,,/Dictionary_ru.xaml") };
            Vars.IsRus = true;
        }

        private void Eng_Click(object sender, RoutedEventArgs e)
        {
            this.Resources.Clear();
            this.Resources = new ResourceDictionary() { Source = new Uri("pack://application:,,,/Dictionary_en.xaml") };
            Vars.IsRus = false;
        }

        public static string GetFileName(string file)
        {
            string[] tmp = file.Split('\\');
            return tmp[tmp.Length - 1];
        }

        //Открытие файлов
        private void BrowsFiles(object sender, EventArgs e)
        {
            OpenFileDialog openfiledlg = new OpenFileDialog();
            openfiledlg.Multiselect = true;
            openfiledlg.Filter = "*All|*.mp3; *.wav; *.aac; *.m4a; *.mp4; *.ogg; *.opus; *.ac3; *.ape; *.mpc; *.flac; *.wma; *.tta; *.alac; *.wv"
                                +"|(.mp3)|*.mp3"
                                +"|(.wav)|*.wav"
                                +"|(.aac)|*.aac"
                                +"|(.mp4/m4a)|*.m4a; *.mp4;"
                                +"|(.ogg)|*.ogg"
                                +"|(.opus)|*.opus"
                                +"|(.ac3)|*.ape"
                                +"|(.flac)|*.flac"
                                +"|(.wma)|*.wma"
                                +"|(.tta)|*.tta"
                                +"|(.alac)|*.alac"
                                +"|(.wv)|*.wv";
            openfiledlg.ShowDialog();
                
            foreach (string file in openfiledlg.FileNames)
            {
                Vars.Files.Add(file);
                string insertsong = file;
                Song InsertSong = new Song(insertsong);
                PlayList.Items.Add(InsertSong);
            }
        }

        //Воспроизведение 
        private void butPlay_Click(object sender, EventArgs e)
        {
            if ((PlayList.Items.Count != 0) && (PlayList.SelectedIndex != -1))
            {
                string current = Vars.Files[PlayList.SelectedIndex];
                Vars.CurrentTrackNumber = PlayList.SelectedIndex;
                BassPlayer.Play(current, BassPlayer.Volume);
                Song SongNow = new Song(current);
                SongName.Text = SongNow.Title;
                Album.Text = SongNow.AlbumName;
                Band.Text = SongNow.ArtistName;
                Year.Text = SongNow.Year;

                try
                {
                    TagLib.File file_TAG = TagLib.File.Create(current);
                    var bin = (byte[])(file_TAG.Tag.Pictures[0].Data.Data);

                    var img = GetArt.ImgFromBytes(bin);
                    Art.Source = img;
                }
                catch 
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.UriSource = new Uri("vinil.png", UriKind.Relative);
                    Art.Source = bi3;
                }

                SongTimeNow.Text = TimeSpan.FromSeconds(BassPlayer.GetPos(BassPlayer.Stream)).ToString();
                SongLenght.Text = TimeSpan.FromSeconds(BassPlayer.GetSongLenght(BassPlayer.Stream)).ToString();
                TimeSlider.Maximum = BassPlayer.GetSongLenght(BassPlayer.Stream);
                TimeSlider.Value = BassPlayer.GetPos(BassPlayer.Stream);
                timerStart();
            }
        }

        //Таймер
        public DispatcherTimer timer = null;
        public void timerStart()
        {
            timer = new DispatcherTimer();  // если надо, то в скобках указываем приоритет, например DispatcherPriority.Render
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            SongTimeNow.Text = TimeSpan.FromSeconds(BassPlayer.GetPos(BassPlayer.Stream)).ToString();
            TimeSlider.Value = BassPlayer.GetPos(BassPlayer.Stream);

            if (BassPlayer.ToNextTrack())
            {
                TimeSlider.Value = 0;
                if (Vars.Shuffle == true)
                {

                    Random rand = new Random();
                    int r = rand.Next(0, Vars.Files.Count);

                    string current = Vars.Files[r];
                    Vars.CurrentTrackNumber = r;
                    PlayList.SelectedIndex = r;

                    BassPlayer.Play(current, BassPlayer.Volume);
                    Song SongNow = new Song(current);
                    SongName.Text = SongNow.Title;
                    Album.Text = SongNow.AlbumName;
                    Band.Text = SongNow.ArtistName;
                    Year.Text = SongNow.Year;

                    try
                    {
                        TagLib.File file_TAG = TagLib.File.Create(current);
                        var bin = (byte[])(file_TAG.Tag.Pictures[0].Data.Data);

                        var img = GetArt.ImgFromBytes(bin);
                        Art.Source = img;
                    }
                    catch
                    {
                        BitmapImage bi3 = new BitmapImage();
                        bi3.UriSource = new Uri("vinil.png", UriKind.Relative);
                        Art.Source = bi3;
                    }

                    SongTimeNow.Text = TimeSpan.FromSeconds(BassPlayer.GetPos(BassPlayer.Stream)).ToString();
                    SongLenght.Text = TimeSpan.FromSeconds(BassPlayer.GetSongLenght(BassPlayer.Stream)).ToString();
                    TimeSlider.Maximum = BassPlayer.GetSongLenght(BassPlayer.Stream);
                    TimeSlider.Value = BassPlayer.GetPos(BassPlayer.Stream);
                }
                else
                {
                    string current = Vars.Files[PlayList.SelectedIndex + 1];
                    PlayList.SelectedIndex = Vars.CurrentTrackNumber;

                    Song SongNow = new Song(current);
                    SongName.Text = SongNow.Title;
                    Album.Text = SongNow.AlbumName;
                    Band.Text = SongNow.ArtistName;
                    Year.Text = SongNow.Year;

                    try
                    {
                        TagLib.File file_TAG = TagLib.File.Create(current);
                        var bin = (byte[])(file_TAG.Tag.Pictures[0].Data.Data);

                        var img = GetArt.ImgFromBytes(bin);
                        Art.Source = img;
                    }
                    catch
                    {
                        BitmapImage bi3 = new BitmapImage();
                        bi3.UriSource = new Uri("vinil.png", UriKind.Relative);
                        Art.Source = bi3;
                    }

                    SongTimeNow.Text = TimeSpan.FromSeconds(BassPlayer.GetPos(BassPlayer.Stream)).ToString();
                    SongLenght.Text = TimeSpan.FromSeconds(BassPlayer.GetSongLenght(BassPlayer.Stream)).ToString();
                    TimeSlider.Maximum = BassPlayer.GetSongLenght(BassPlayer.Stream);
                    TimeSlider.Value = BassPlayer.GetPos(BassPlayer.Stream);
                }
            }

            if (BassPlayer.EndPlayList)
            {
                butStop_Click(this, new EventArgs());
                PlayList.SelectedItem = Vars.CurrentTrackNumber = 0;
                BassPlayer.EndPlayList = false;
            }
        }

        //Стоп 
        private void butStop_Click(object sender, EventArgs e)
        {
            try
            {   if (timer != null)
                {
                    BassPlayer.Stop();
                    timer.Stop();
                    TimeSlider.Value = 0;
                    SongTimeNow.Text = "00:00:00";
                }
            }
            catch { }
        }

        //Пауза
        private void butPause_Click(object sender, EventArgs e)
        {
            BassPlayer.Pause();
        }

        //Предыдущий трэк
        private void butPrevious_Click(object sender, EventArgs e)
        {

            if (BassPlayer.GetPos(BassPlayer.Stream) > 5)
            {
                BassPlayer.PosScroll(BassPlayer.Stream, 0);
            }
            else if (Vars.Shuffle == true)
            {

                Random rand = new Random();
                int r = rand.Next(0, Vars.Files.Count);

                try
                {
                    string current = Vars.Files[r];

                    Vars.CurrentTrackNumber = r;
                    PlayList.SelectedIndex = r;

                    BassPlayer.Play(current, BassPlayer.Volume);
                    Song SongNow = new Song(current);
                    SongName.Text = SongNow.Title;
                    Album.Text = SongNow.AlbumName;
                    Band.Text = SongNow.ArtistName;
                    Year.Text = SongNow.Year;
                }
                catch { }

                try
                {
                    string current = Vars.Files[r];
                    TagLib.File file_TAG = TagLib.File.Create(current);
                    var bin = (byte[])(file_TAG.Tag.Pictures[0].Data.Data);

                    var img = GetArt.ImgFromBytes(bin);
                    Art.Source = img;
                }
                catch
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.UriSource = new Uri("vinil.png", UriKind.Relative);
                    Art.Source = bi3;
                }

                SongTimeNow.Text = TimeSpan.FromSeconds(BassPlayer.GetPos(BassPlayer.Stream)).ToString();
                SongLenght.Text = TimeSpan.FromSeconds(BassPlayer.GetSongLenght(BassPlayer.Stream)).ToString();
                TimeSlider.Maximum = BassPlayer.GetSongLenght(BassPlayer.Stream);
                TimeSlider.Value = BassPlayer.GetPos(BassPlayer.Stream);
                }
            else if (Vars.Repeat == true)
            {
                try
                {
                    BassPlayer.Play(Vars.Files[Vars.CurrentTrackNumber], BassPlayer.Volume);
                }
                catch { }
            }
            else if ((PlayList.Items.Count != 0) && (PlayList.SelectedIndex != -1))
            {
                string current = "";

                if (Vars.CurrentTrackNumber > 0)
                {
                    current = Vars.Files[PlayList.SelectedIndex - 1];
                    Vars.CurrentTrackNumber--;
                    PlayList.SelectedIndex = PlayList.SelectedIndex - 1;
                }

                else 
                { 
                    current = Vars.Files[Vars.Files.Count - 1];
                    Vars.CurrentTrackNumber = Vars.Files.Count - 1;
                    PlayList.SelectedIndex = Vars.Files.Count - 1;
                }

                BassPlayer.Play(current, BassPlayer.Volume);
                Song SongNow = new Song(current);
                SongName.Text = SongNow.Title;
                Album.Text = SongNow.AlbumName;
                Band.Text = SongNow.ArtistName;
                Year.Text = SongNow.Year;

                try
                {
                    TagLib.File file_TAG = TagLib.File.Create(current);
                    var bin = (byte[])(file_TAG.Tag.Pictures[0].Data.Data);

                    var img = GetArt.ImgFromBytes(bin);
                    Art.Source = img;
                }
                catch
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.UriSource = new Uri("vinil.png", UriKind.Relative);
                    Art.Source = bi3;
                }

                SongTimeNow.Text = TimeSpan.FromSeconds(BassPlayer.GetPos(BassPlayer.Stream)).ToString();
                SongLenght.Text = TimeSpan.FromSeconds(BassPlayer.GetSongLenght(BassPlayer.Stream)).ToString();
                TimeSlider.Maximum = BassPlayer.GetSongLenght(BassPlayer.Stream);
                TimeSlider.Value = BassPlayer.GetPos(BassPlayer.Stream);
                timerStart();
            }
        }

        //Следующий трэк
        private void butForvard_Click(object sender, EventArgs e)
        {
            string current = "";

            if ((PlayList.Items.Count != 0) && (PlayList.SelectedIndex != -1))
            {
                if (Vars.Shuffle == true)
                {
                    Random rand = new Random();
                    int r = rand.Next(0, Vars.Files.Count);

                    current = Vars.Files[r];
                    Vars.CurrentTrackNumber = r;
                    PlayList.SelectedIndex = r;

                    BassPlayer.Play(current, BassPlayer.Volume);
                    Song SongNow = new Song(current);
                    SongName.Text = SongNow.Title;
                    Album.Text = SongNow.AlbumName;
                    Band.Text = SongNow.ArtistName;
                    Year.Text = SongNow.Year;

                    try
                    {
                        TagLib.File file_TAG = TagLib.File.Create(current);
                        var bin = (byte[])(file_TAG.Tag.Pictures[0].Data.Data);

                        var img = GetArt.ImgFromBytes(bin);
                        Art.Source = img;
                    }
                    catch
                    {
                        BitmapImage bi3 = new BitmapImage();
                        bi3.UriSource = new Uri("vinil.png", UriKind.Relative);
                        Art.Source = bi3;
                    }

                    SongTimeNow.Text = TimeSpan.FromSeconds(BassPlayer.GetPos(BassPlayer.Stream)).ToString();
                    SongLenght.Text = TimeSpan.FromSeconds(BassPlayer.GetSongLenght(BassPlayer.Stream)).ToString();
                    TimeSlider.Maximum = BassPlayer.GetSongLenght(BassPlayer.Stream);
                    TimeSlider.Value = BassPlayer.GetPos(BassPlayer.Stream);
                }
                else if (Vars.Repeat == true)
                {
                    BassPlayer.Play(Vars.Files[Vars.CurrentTrackNumber], BassPlayer.Volume);
                }
                else
                {
                    if (Vars.Files.Count > Vars.CurrentTrackNumber + 1)
                    {
                        current = Vars.Files[PlayList.SelectedIndex + 1];
                        Vars.CurrentTrackNumber++;
                        PlayList.SelectedIndex = PlayList.SelectedIndex + 1;
                    }

                    else
                    {
                        current = Vars.Files[0];
                        Vars.CurrentTrackNumber = 0;
                        PlayList.SelectedIndex = 0;
                    }

                    BassPlayer.Play(current, BassPlayer.Volume);
                    Song SongNow = new Song(current);
                    SongName.Text = SongNow.Title;
                    Album.Text = SongNow.AlbumName;
                    Band.Text = SongNow.ArtistName;
                    Year.Text = SongNow.Year;

                    try
                    {
                        TagLib.File file_TAG = TagLib.File.Create(current);
                        var bin = (byte[])(file_TAG.Tag.Pictures[0].Data.Data);

                        var img = GetArt.ImgFromBytes(bin);
                        Art.Source = img;
                    }
                    catch
                    {
                        BitmapImage bi3 = new BitmapImage();
                        bi3.UriSource = new Uri("vinil.png", UriKind.Relative);
                        Art.Source = bi3;
                    }

                    SongTimeNow.Text = TimeSpan.FromSeconds(BassPlayer.GetPos(BassPlayer.Stream)).ToString();
                    SongLenght.Text = TimeSpan.FromSeconds(BassPlayer.GetSongLenght(BassPlayer.Stream)).ToString();
                    TimeSlider.Maximum = BassPlayer.GetSongLenght(BassPlayer.Stream);
                    TimeSlider.Value = BassPlayer.GetPos(BassPlayer.Stream);
                    timerStart();
                }
            }
        }

        //Перемотка
        private void TimeSlider_Scroll(object sender, EventArgs Handlere)
        {
            Slider slider = sender as Slider;
            //Классный костыль, т.к. метод смены времени запускался тогда, когда слайдер сам двигался, 
            //то теперь он будет двигаться, если разница во времени больше секунды или отрицательная
            if (slider.Value - BassPlayer.GetPos(BassPlayer.Stream) > 1 || slider.Value - BassPlayer.GetPos(BassPlayer.Stream) < 0)
                BassPlayer.PosScroll(BassPlayer.Stream, slider.Value);
        }

        //Изменение громкости
        private void VolumeSlider_Scroll(object sender, EventArgs e)
        {
            Slider slider = sender as Slider;
            BassPlayer.SetVolume(BassPlayer.Stream, (int)slider.Value);
        }

        //Треки вперемешку
        private void Shuffle_click(object sender, EventArgs e)
        {
            Vars.Shuffle = !Vars.Shuffle;
        }

        //Репит
        private void Repeat_click(object sender, EventArgs e)
        {
            Vars.Repeat = !Vars.Repeat;
        }

        //Удаление трэка
        private void Delit_click(object sender, EventArgs e)
        {
            if (PlayList.Items.Count != 0) 
            {
                Vars.Files = Clear.DelSong(Vars.Files, PlayList.SelectedIndex);
                PlayList.Items.Clear();
                foreach (String file in Vars.Files)
                {
                    string insertsong = file;
                    Song InsertSong = new Song(insertsong);
                    PlayList.Items.Add(InsertSong);
                }

                Vars.CurrentTrackNumber = Vars.CurrentTrackNumber - 1;
            }
        }

        //Очистка плейлиста
        private void Clear_click(object sender, EventArgs e)
        {
            Vars.Files.Clear();
            PlayList.Items.Clear();
        }

        //Окно с тегами
        private void Tags_click(object sender, EventArgs e)
        {
            int si = PlayList.SelectedIndex;
            if (PlayList.SelectedIndex >= 0)
                new Tags(PlayList.SelectedIndex).ShowDialog();
            else
                if(Vars.IsRus)
                    MessageBox.Show("Выберите трэк");
                else if (!Vars.IsRus)
                    MessageBox.Show("Choose track");
        }

        //Сохранение плейлиста
        private void PlayListSave_click(object sender, EventArgs e)
        {
            using (MusicPlayerEntities db = new MusicPlayerEntities())
            {
                if (PlayListName.Text.Length <= 55 && PlayListName.Text.Length >= 1)
                {
                    int ii = 0;
                    var isname = db.DBPlayList.Where(p => p.Name == PlayListName.Text);

                    foreach (var c in isname)
                        ii++;

                    if (ii != 0)
                    {
                        if (Vars.IsRus)
                            MessageBox.Show("Плейлист с таким именем уже есть");
                        else if (!Vars.IsRus)
                            MessageBox.Show("There is already a playlist with the same name");
                    }
                    else
                    {
                        DBPlayList dbplaylist = new DBPlayList();

                        dbplaylist.Name = PlayListName.Text;

                        db.DBPlayList.Add(dbplaylist);
                        db.SaveChanges();

                        int i = 0;
                        foreach (string files in Vars.Files)
                        {
                            DBFiles dbfiles = new DBFiles();
                            i++;
                            var peremennaya = db.DBPlayList.Where(p => p.Name == PlayListName.Text);
                            foreach (var a in peremennaya)
                            {
                                dbfiles.PLId = a.Id;
                            }
                            dbfiles.Path = files;
                            dbfiles.Number = i;
                            db.DBFiles.Add(dbfiles);
                        }
                        db.SaveChanges();

                        if (Vars.IsRus)
                            MessageBox.Show("Плейлист успешно сохранен");
                        else if (!Vars.IsRus)
                            MessageBox.Show("Playlist saved successfully");
                    }
                }
                else
                {
                    if (Vars.IsRus)
                        MessageBox.Show("Название плейлиста должно быть от 1 до 55 символов");
                    else if (!Vars.IsRus)
                        MessageBox.Show("Playlist name must be between 1 and 55 characters");
                }
            }
            GetPlaylists();
        }

        //Выбор плейлиста
        private void PlayListOpen_click(object sender, EventArgs e)
        {
            try
            {
                using (MusicPlayerEntities db = new MusicPlayerEntities())
                {
                    Vars.Files.Clear();
                    PlayList.Items.Clear();

                    int plid = 0;

                    var id = db.DBPlayList.Where(p => p.Name == PlayListList.SelectedItem.ToString());

                    foreach (var a in id)
                        plid = a.Id;

                    var bdfiles = db.DBFiles.Where(p => p.PLId == plid);

                    foreach (var file in bdfiles)
                    {
                        Vars.Files.Add(file.Path.ToString());
                        string insertsong = file.Path.ToString();
                        Song InsertSong = new Song(insertsong);
                        PlayList.Items.Add(InsertSong);
                    }

                }
            }
            catch { }
        }

        //Удаление плейлиста
        private void PlayListDelete_click(object sender, EventArgs e)
        {
            try
            {
                using (MusicPlayerEntities db = new MusicPlayerEntities())
                {
                    //Поиск элемента с именем, как выбранное в списке
                    int plid = 0;
                    var id = db.DBPlayList.Where(p => p.Name == PlayListList.SelectedItem.ToString());

                    //получение его id
                    foreach (var a in id)
                        plid = a.Id;

                    var count = db.DBFiles.Where(p => p.PLId == plid);

                    DBFiles dbfiles;

                    foreach (var f in count)
                    {
                        dbfiles = db.DBFiles.Where(p => p.PLId == plid).FirstOrDefault();
                        db.DBFiles.Remove(dbfiles);
                    }

                    DBPlayList dbpl = db.DBPlayList.Where(p => p.Name == PlayListList.SelectedItem.ToString()).FirstOrDefault();
                    db.DBPlayList.Remove(dbpl);

                    db.SaveChanges();

                    if (Vars.IsRus)
                        MessageBox.Show("Плейлист успешно удален");
                    else if (!Vars.IsRus)
                        MessageBox.Show("Playlist deleted successfully");
                }
            
            }
            catch { }
            GetPlaylists();
        }

        //Получение плейлистов
        private void GetPlaylists()
        {
            Vars.Playlists.Clear();
            PlayListList.Items.Clear();
            using (MusicPlayerEntities db = new MusicPlayerEntities())
            {
                DBPlayList dbplaylist = new DBPlayList();
                var songs = db.DBPlayList.Where(p => p.Name != null);

                foreach (var a in songs)
                    Vars.Playlists.Add(a.Name);
            }
            foreach (var a in Vars.Playlists)
            {
                PlayListList.Items.Add(a);
            }
        }

        //About
        private void About_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }
    }
}
