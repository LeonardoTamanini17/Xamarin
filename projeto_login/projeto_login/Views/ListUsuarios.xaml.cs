using projeto_login.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace projeto_login.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListUsuarios : ContentPage
    {
        public ListUsuarios()
        {
            InitializeComponent();
            BindingContext = new ListUsuarioViewModel();
        }
    }
}