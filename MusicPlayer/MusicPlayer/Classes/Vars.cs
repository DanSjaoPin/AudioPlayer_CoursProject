using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    class Vars
    {
        //Список файлов в плейлисте
        public static List<string> Files = new List<string>();

        //Current track number
        public static int CurrentTrackNumber;

        // Путь до файла приложения
        public static string AppPath = AppDomain.CurrentDomain.BaseDirectory;

        //Повтор и перемешивалкаа
        public static bool Repeat = false;
        public static bool Shuffle = false;

        //Выбранный язык
        public static bool IsRus = false;

        //Плейлисты
        public static List<string> Playlists = new List<string>();

    }
}
