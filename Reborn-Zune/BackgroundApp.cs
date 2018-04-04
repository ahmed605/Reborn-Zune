using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml;

namespace Reborn_Zune
{
    sealed partial class App : Application
    {
        bool isInBackgroundMode;

        partial void Construct()
        {
            // During the transition from foreground to background the
            // memory limit allowed for the application changes. The application
            // has a short time to respond by bringing its memory usage
            // under the new limit.
            MemoryManager.AppMemoryUsageLimitChanging += MemoryManager_AppMemoryUsageLimitChanging;

            // After an application is backgrounded it is expected to stay
            // under a memory target to maintain priority to keep running.
            // Subscribe to the event that informs the app of this change.
            MemoryManager.AppMemoryUsageIncreased += MemoryManager_AppMemoryUsageIncreased;

            // Subscribe to key lifecyle events to know when the app
            // transitions to and from foreground and background.
            // Leaving the background is an important transition
            // because the app may need to restore UI.
            EnteredBackground += App_EnteredBackground;
            LeavingBackground += App_LeavingBackground;

            // Subscribe to regular lifecycle events to display a toast notification
            Suspending += App_Suspending;
            Resuming += App_Resuming;
        }

        private void App_Resuming(object sender, object e)
        {
            
        }

        private void App_Suspending(object sender, SuspendingEventArgs e)
        {
            
        }

        private void App_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            isInBackgroundMode = false;
        }

        private void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            isInBackgroundMode = true;
        }

        private void MemoryManager_AppMemoryUsageIncreased(object sender, object e)
        {
            
        }

        private void MemoryManager_AppMemoryUsageLimitChanging(object sender, AppMemoryUsageLimitChangingEventArgs e)
        {
            
        }
    }
}
