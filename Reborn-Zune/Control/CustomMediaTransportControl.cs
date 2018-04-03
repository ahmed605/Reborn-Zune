using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Reborn_Zune.Control
{
    public sealed class CustomMediaTransportControl : MediaTransportControls
    {
        public CustomMediaTransportControl()
        {
            DefaultStyleKey = typeof(CustomMediaTransportControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
