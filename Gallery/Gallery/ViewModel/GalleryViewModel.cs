using System;
using System.Windows.Input;
using Xamarin.Forms;
using Gallery.Views;

namespace Gallery.ViewModel 
{
    class GalleryViewModel : SharredViewModel
    {
        public ICommand NavigateCommand {private set; get; }

        public Display displayPage;

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public GalleryViewModel()
        {
            //setup page title, image list and an event that updates the page when changes are made to the image list
            Title = "GalleryApp";

            //setup the display page and the navigate command that sets image displayed by the display page and navigates from Gallery to display
            displayPage = new Display();
            NavigateCommand = new Command<object>(
            async (object id) =>
            {
                Console.WriteLine("test " + (int)id);
                displayPage.setImage((int)id);
                await Application.Current.MainPage.Navigation.PushAsync(displayPage);
            });
        }

        public void OnAppearing()
        {
            Console.WriteLine("Gallery View Re-appeared");
            OnPropertyChanged("ImageList");
        }
    }
}
