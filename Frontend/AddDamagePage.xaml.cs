using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.ViewModel;

namespace Frontend;

public partial class AddDamagePage : ContentPage
{
    public AddDamagePage(AddDamageViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();

        MessagingCenter.Subscribe<AddDamageViewModel, string>(this, "ValidationError", async (sender, arg) => {
            await DisplayAlert("Validation Error", arg, "OK");
        });
    }
}