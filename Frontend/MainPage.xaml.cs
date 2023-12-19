using Frontend.ViewModel;


namespace Frontend {
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel) {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }

}
