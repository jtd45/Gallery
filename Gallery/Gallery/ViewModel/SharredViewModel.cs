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
    class SharredViewModel : INotifyPropertyChanged
    {
        //list of images storred in the JSON file ImageListJson intially, then put into the Application.Current.Properties dictionary
        public ObservableCollection<GalleryImage> ImageList {get; set;}
        //Static list of images sharred between every class that extends sharred view model
        private static ObservableCollection<GalleryImage> _staticImageList { get; set; }

        public delegate void OnImageListUpdate();
        /// <summary>
        ///     Event called when Image list has been updated, used to synch up the local image lists between different classes that extend the sharred view model
        /// </summary>
        public static event OnImageListUpdate onImageListUpdate;

        //Initilize shared view model by filling the image list
        public SharredViewModel()
        {
            ImageList = new ObservableCollection<GalleryImage>();
            
            //ImageList.CollectionChanged += (sender, e) => { Console.WriteLine($"{e.Action}" + "collection changed"); };

            if (_staticImageList == null)
            {
                _staticImageList = new ObservableCollection<GalleryImage>();
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
                List<GalleryImage> fileImageList = JsonConvert.DeserializeObject<List<GalleryImage>>(text);
                int count = 0;
                foreach (GalleryImage fileImage in fileImageList)
                {
                    fileImage.id = count;
                    _staticImageList.Add(fileImage);
                    count++;
                }
            }
            ImageList = _staticImageList;
            onImageListUpdate += SynchImageLists;
        }
        ///<summary>save the image list as a JSON string</summary>
        public void SaveImageList()
        {
            Application.Current.Properties["JSONList"] = JsonConvert.SerializeObject(_staticImageList);
        }

        /// <summary>
        ///     Synch local image list with the sharred static image list
        /// </summary>
        public void SynchImageLists()
        {
            Console.WriteLine("synch image lists");
            ImageList = _staticImageList;
            OnPropertyChanged(nameof(ImageList));
        }

        /// <summary>
        ///     toggle favorite boolean for a given gallery image in the image list
        /// </summary>
        /// <param name="id">image id</param>
        public void ToggleImageFave(int id)
        {
            //ImageList[id].fave = !ImageList[id].fave;
            _staticImageList[id].fave = !_staticImageList[id].fave;
            onImageListUpdate?.Invoke();
            OnPropertyChanged(nameof(ImageList));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null) { return; }

            Console.WriteLine("Property Changed Somewhere should update page" + PropertyChanged.ToString() + " " + propertyName);
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) { return false; }
            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
