using projeto_login.Model;
using projeto_login.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace projeto_login.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cadastro : ContentPage
    {
        public Cadastro(Usuario usuario)
        {
            InitializeComponent();
            BindingContext = new CadastroViewModel(usuario);
            toDate.MinimumDate = DateTime.Now.AddYears(-100);
            toDate.MaximumDate = DateTime.Now.AddYears(-16);
            toDate.Date = DateTime.Now.AddYears(-16);
        }
    }
}