using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    class Clear
    {
        public static List<string> DelSong(List<String> List, int index)
        {
            List.RemoveAt(index);
            return List;
        }
    }
}
