using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using System.Windows;
using System.Windows.Threading;

namespace MusicPlayer
{
    public static class BassPlayer
    {
        //Частота дискретизации
        public static int Hz = 44100;

        //Состояние инициализации аудио
        public static bool InitDevice;

        //Поток плеера
        public static int Stream;

        //Громкость
        public static int Volume = 100;

        //Канал остановлен вручную
        private static bool IsStopped = true;

        //Плэйлист все
        public static bool EndPlayList;

        // Список плагинов
        private static readonly List<int> BassPluginsHandles = new List<int>(); 

        //Инициализация библиотеки Bass.dll
        public static bool InitBass(int hz)
        {
            if (!InitDevice)
            {
                InitDevice = Bass.BASS_Init(-1/*Звуковая карта по умолчанию*/, hz, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
                if (InitDevice)
                {
                    BassPluginsHandles.Add(Bass.BASS_PluginLoad(Vars.AppPath + @"Plugins\bass_aac.dll"));
                    BassPluginsHandles.Add(Bass.BASS_PluginLoad(Vars.AppPath + @"Plugins\bass_ac3.dll"));
                    BassPluginsHandles.Add(Bass.BASS_PluginLoad(Vars.AppPath + @"Plugins\bass_ape.dll"));
                    BassPluginsHandles.Add(Bass.BASS_PluginLoad(Vars.AppPath + @"Plugins\bass_mpc.dll"));
                    BassPluginsHandles.Add(Bass.BASS_PluginLoad(Vars.AppPath + @"Plugins\bass_tta.dll"));
                    BassPluginsHandles.Add(Bass.BASS_PluginLoad(Vars.AppPath + @"Plugins\bassalac.dll"));
                    BassPluginsHandles.Add(Bass.BASS_PluginLoad(Vars.AppPath + @"Plugins\bassflac.dll"));
                    BassPluginsHandles.Add(Bass.BASS_PluginLoad(Vars.AppPath + @"Plugins\bassopus.dll"));
                    BassPluginsHandles.Add(Bass.BASS_PluginLoad(Vars.AppPath + @"Plugins\basswebm.dll"));
                    BassPluginsHandles.Add(Bass.BASS_PluginLoad(Vars.AppPath + @"Plugins\basswma.dll"));
                    BassPluginsHandles.Add(Bass.BASS_PluginLoad(Vars.AppPath + @"Plugins\basswv.dll"));
                }

                int ErrorCount = 0;
                for (int i = 0; i < BassPluginsHandles.Count; i++)
                {
                    if (BassPluginsHandles[i] == 0)
                        ErrorCount++;
                }
                if (ErrorCount != 0)
                {
                    MessageBox.Show(ErrorCount + " plugins is not loaded", "Error", MessageBoxButton.OK);
                }
            }
            return InitDevice;
        }

        //Плай
        public static void Play(string songfile, int volume)
        {
            if (Bass.BASS_ChannelIsActive(Stream) != BASSActive.BASS_ACTIVE_PAUSED)
            {
                Stop();
                if (InitBass(Hz))
                {
                    Stream = Bass.BASS_StreamCreateFile(songfile, 0/*откуда воспроизводим*/, 0/*до куда воспроизводим*/, BASSFlag.BASS_DEFAULT);
                    if (Stream != 0)
                    {
                        Volume = volume;
                        Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);
                        Bass.BASS_ChannelPlay(Stream, false);
                    }
                }
            }
            else
                Bass.BASS_ChannelPlay(Stream, false);
            IsStopped = false;
        }

        //Стоп
        public static void Stop()
        {
            Bass.BASS_ChannelStop(Stream);
            Bass.BASS_StreamFree(Stream);
            IsStopped = true;
        }

        //Пауза
        public static void Pause()
        {
            if (Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_PLAYING)
                Bass.BASS_ChannelPause(Stream);
        }

        //Громкость
        public static void SetVolume(int stream, int volume)
        {
            Volume = volume;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);
        }

        //Получение длительности в секундах
        public static int GetSongLenght(int stream)
        {
            long SongLenghtBytes = Bass.BASS_ChannelGetLength(stream);
            //Т.к. длительность мы получаем в байтах
            double SongLenght = Bass.BASS_ChannelBytes2Seconds(stream, SongLenghtBytes);
            return (int)SongLenght;
        }

        //Получение текущей позиции
        public static int GetPos(int stream)
        {
            long Pos = Bass.BASS_ChannelGetPosition(stream);
            int posSec = (int)Bass.BASS_ChannelBytes2Seconds(stream, Pos);
            return posSec;
        }

        //Установка позиции
        public static void PosScroll(int stream, double pos)
        {
            Bass.BASS_ChannelSetPosition(stream, /*(double)*/pos);
        }

        //Переключение трэков
        public static bool ToNextTrack()
        {
            if ((Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_STOPPED) && (!IsStopped))
            {
                if (Vars.Repeat == true)
                {
                    Play(Vars.Files[Vars.CurrentTrackNumber], Volume);
                }
                else if (Vars.Files.Count > Vars.CurrentTrackNumber + 1)
                {
                    Vars.CurrentTrackNumber++;
                    Play(Vars.Files[Vars.CurrentTrackNumber], Volume);
                    EndPlayList = false;
                    return true;
                }
                else if (Vars.Files.Count <= Vars.CurrentTrackNumber + 1)
                    EndPlayList = true;
            }
            return false;
        }
    }
}
