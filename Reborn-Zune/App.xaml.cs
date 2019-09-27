﻿using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Reborn_Zune_Common.Services;
using Reborn_Zune_MusicLibraryService;
using System;
using System.Diagnostics;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Reborn_Zune
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            
            this.InitializeComponent();
            this.Construct();
            AppCenter.Start("ffbf2e68-b5cf-44b3-9bd8-88990d3e6307", typeof(Analytics), typeof(Crashes));

            ServiceLocator.SetInstance(new MusicLibraryService());

            this.UnhandledException += App_UnhandledException;
        }

        private void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.Message.ToString());
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                bool loadState = (e.PreviousExecutionState == ApplicationExecutionState.Terminated);
                ExtendSplash extendedSplash = new ExtendSplash(e.SplashScreen, loadState);
                Window.Current.Content = extendedSplash;
            }

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 300));
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            Window.Current.Activate();

            //Frame rootFrame = Window.Current.Content as Frame;

            //// Do not repeat app initialization when the Window already has content,
            //// just ensure that the window is active
            //if (rootFrame == null)
            //{
            //    // Create a Frame to act as the navigation context and navigate to the first page
            //    rootFrame = new Frame();

            //    rootFrame.NavigationFailed += OnNavigationFailed;

            //    if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            //    {
            //        //TODO: Load state from previously suspended application
            //    }

            //    // Place the frame in the current Window
            //    Window.Current.Content = rootFrame;
            //}

            //if (e.PrelaunchActivated == false)
            //{
            //    if (rootFrame.Content == null)
            //    {
            //        // When the navigation stack isn't restored navigate to the first page,
            //        // configuring the new page by passing required information as a navigation
            //        // parameter
            //        rootFrame.Navigate(typeof(SplashScreen), e.Arguments);
            //    }
            //    // Ensure the current window is active

            //    // using Windows.UI.ViewManagement;
            



            //    Window.Current.Activate();
            //}
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        partial void Construct();
    }
}
