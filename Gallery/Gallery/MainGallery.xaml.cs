using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Gallery.ViewModel;
using Gallery.Views;
using Gallery.Models;
using System.Reflection;

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
            _viewModel.OnAppearing();
        }

        /*protected override void OnSleep()
        {
            _viewModel.OnSleep();
        }*/
    }
}
