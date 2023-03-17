using System;
using System.Windows.Input;
using Xamarin.Forms;
using Gallery.Models;

namespace Gallery.ViewModel
{
    class DisplayViewModel : SharredViewModel
    {
        /// <summary>
        ///     image currently displayed on the display page
        /// </summary>
        public GalleryImage CurrentImage { get; set; }
        /// <summary>
        ///     command used to toggle favoriting an image
        /// </summary>
        public ICommand ToggleFavoriteCommand { private set; get; }
        /// <summary>
        ///     command called to navigate back to the gallery page
        /// </summary>
        public ICommand NavigateBackCommand { private set; get; }

        string title = string.Empty;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        public DisplayViewModel()
        {
            Title = "Display";
            ToggleFavoriteCommand = new Command(
                execute: (object id) =>
                    {
                        Console.WriteLine("toggle Favorite of Image " + id);
                        ToggleImageFave((int)id);
                        ImageList.CollectionChanged += (sender, e) => { Console.WriteLine($"{e.Action}" + "collection changed"); };
                    }
            );
            NavigateBackCommand = new Command(
                async () =>
                {
                    Console.WriteLine("Go Back to Last Page");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            );
        }

        /// <summary>
        ///     Set the display page's current image by image id
        /// </summary>
        /// <param name="index"></param>
        public void setImage(int index)
        {
            CurrentImage = ImageList[index];
            Console.WriteLine("Current image is id " + this.CurrentImage.id);
            Title = CurrentImage.fileName;
            OnPropertyChanged("CurrentImage");
        }
    }
}
