using Xamarin.Forms;
using Gallery.ViewModel;

namespace Gallery
{
    public partial class MainGallery : ContentPage
    {
        GalleryViewModel _viewModel;
        public MainGallery()
        {
            InitializeComponent();
            BindingContext = _viewModel = new GalleryViewModel();
        }

        protected override void OnAppearing()
        {
            //base.OnAppearing();
            _viewModel.OnAppearing();
        }
        public void OnSleep()
        {
            _viewModel.SaveImageList();
        }
    }
}
