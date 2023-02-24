using Gallery.Models;
using Gallery.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gallery.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Display : ContentPage
    {
        DisplayViewModel _viewModel;

        
        public Display(ObservableCollection<GalleryImage> imageList)
        {
            InitializeComponent();
            BindingContext = _viewModel = new DisplayViewModel(imageList);
        }

        public void setImage(int index)
        {
            _viewModel.setImage(index);
        }
    }
}