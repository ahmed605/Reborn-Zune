using Reborn_Zune_Common.Services;
using Reborn_Zune_MusicPlayerService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.UI.Core;

namespace Reborn_Zune_MusicPlayerService
{
    public class MusicPlaybackService: IService
    {
        private PlayerViewModel pVM { get; set; }

        public MediaPlayer Player { get; private set; }

        public MusicPlaybackService(CoreDispatcher dispatcher)
        {
            Player = new MediaPlayer();
            Player.AutoPlay = true;
            pVM = new PlayerViewModel(Player, dispatcher);
        }

        public void Run()
        {
        }
    }
}
