using Android.Content.Res;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Gallery.Views;
using Gallery.ViewModel;
using Gallery.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Gallery.ViewModel
{
    class DisplayViewModel : SharredViewModel
    {
        public GalleryImage CurrentImage { get; set; }

        //command called when an image is favorited/un-favorited
        public ICommand ToggleFavoriteCommand { private set; get; }
        //command called to navigate back to the gallery page
        public ICommand NavigateBackCommand { private set; get; }

        string title = string.Empty;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        public DisplayViewModel()
        {
            ImageList.CollectionChanged += (sender, e) => { Console.WriteLine($"{e.Action}" + "collection changed"); };
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
            OnPropertyChanged("CurrentImage");
        }
    }
}
