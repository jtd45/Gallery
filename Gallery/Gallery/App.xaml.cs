using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gallery
{
    public partial class App : Application
    {
        private MainGallery mainGallery;
        public App()
        {
            InitializeComponent();

            mainGallery = new MainGallery();
            MainPage = new NavigationPage(mainGallery);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            mainGallery.OnSleep();
        }

        protected override void OnResume()
        {
        }
    }
}
