using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Reborn_Zune.Model
{
    public interface ILocalListModel
    {
        bool isEditable { get; }

        Visibility isVisible { get; }

        ObservableCollection<LocalMusicModel> Musics { get; set; }
    }
}
