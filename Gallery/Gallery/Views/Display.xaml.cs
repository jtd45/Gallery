using Gallery.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gallery.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Display : ContentPage
    {
        DisplayViewModel _viewModel;

        public Display()
        {
            InitializeComponent();
            BindingContext = _viewModel = new DisplayViewModel();
        }

        /// <summary>
        ///     set current image by id
        /// </summary>
        /// <param name="index"></param>
        public void setImage(int index)
        {
            ImageCarousel.Position = index;
            _viewModel.setImage(index);
        }
    }
}