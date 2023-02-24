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
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Gallery.ViewModel 
{
    class GalleryViewModel : INotifyPropertyChanged
    {
        public ICommand NavigateCommand {private set; get; }
        public ObservableCollection<GalleryImage> imageList { get; }

        public Display displayPage;

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public bool isBusy = true;

        public GalleryViewModel()
        {
            imageList = new ObservableCollection<GalleryImage>();
            this.imageList.CollectionChanged += (sender, e) => { Console.WriteLine($"{e.Action}" + "collection changed"); };
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(GalleryViewModel)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("Gallery.ImageListJson.txt");
            string text = "";
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            Console.WriteLine("test is here " + text);
            List<GalleryImage> fileImageList= JsonConvert.DeserializeObject<List<GalleryImage>>(text);
            int count = 0;
            foreach(GalleryImage fileImage in fileImageList)
            {
                fileImage.id = count;
                this.imageList.Add(fileImage);
                count++;
            }

            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp.txt");

            isBusy = false;
            displayPage = new Display(imageList);
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
            OnPropertyChanged("imageList");
        }

        public void OnSleep()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
