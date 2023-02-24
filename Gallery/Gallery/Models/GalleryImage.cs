using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Gallery.Models
{
    public class GalleryImage : INotifyPropertyChanged
    {
        public int id { get; set; }
        public String fileName { get; set; }

        private Boolean _fave;
        public Boolean fave { get => _fave; set => SetProperty(ref _fave, value); }

        public GalleryImage(int id,string fileName,Boolean fave)
        {
            this.id = id;
            this.fileName = fileName;
            this.fave = fave;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (property == null || !property.Equals(value))
            {
                property = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
}
