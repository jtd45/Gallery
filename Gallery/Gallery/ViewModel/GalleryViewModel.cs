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

        public GalleryViewModel()
        {
            //setup page title, image list and an event that updates the page when changes are made to the image list
            this.Title = "GalleryApp";
            imageList = new ObservableCollection<GalleryImage>();
            this.imageList.CollectionChanged += (sender, e) => { Console.WriteLine($"{e.Action}" + "collection changed"); };

            //read from ImageListJson file if the properties dictionary is not setup yet, otherwise pull JSON string from the properties dict
            string text = "";
            if (!Application.Current.Properties.ContainsKey("JSONList"))
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(GalleryViewModel)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("Gallery.ImageListJson.txt");
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
            }
            else
            {
                text = Application.Current.Properties["JSONList"].ToString();
            }
            //deserialize JSON text and add images to image list
            Console.WriteLine("test is here " + text);
            List<GalleryImage> fileImageList= JsonConvert.DeserializeObject<List<GalleryImage>>(text);
            int count = 0;
            foreach(GalleryImage fileImage in fileImageList)
            {
                fileImage.id = count;
                this.imageList.Add(fileImage);
                count++;
            }
            
            //setup the display page and the navigate command that sets image displayed by the display page and navigates from Gallery to display
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

        ///<summary>save the image list as a JSON string</summary>
        public void SaveImageList()
        {
            Application.Current.Properties["JSONList"] = JsonConvert.SerializeObject(imageList);
        }

        //update page if property of a given variable changes
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
