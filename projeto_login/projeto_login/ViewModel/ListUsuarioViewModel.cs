using projeto_login.Model;
using projeto_login.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace projeto_login.ViewModel
{
    public class ListUsuarioViewModel : INotifyPropertyChanged
    {
        public Command CriarCommand { get; set; }
        public Command AtualizarCommand { get; set; }

        Usuario user =  new Usuario();

        private List<Usuario> _usuarios;
        public List<Usuario> Usuarios
        {
            get { return _usuarios; }
            set
            {
                _usuarios = value;
                OnPropertyChanged("Usuarios");
            }
        }

        private Usuario _selectedItemUsuario;
        public Usuario SelectedItemUsuario
        {
            get { return _selectedItemUsuario; }
            set
            {
                _selectedItemUsuario = value;
                OnPropertyChanged("SelectedItemUsuario");
                if (value != null)
                {
                    Cadastro(value);
                }
            }
        }

        public ListUsuarioViewModel()
        {
            Carregar();
            AtualizarCommand = new Command(Carregar);
            CriarCommand = new Command(async () => await Cadastro(user));
            Atualizar();
        }

        private void Atualizar()
        {
            Device.StartTimer(System.TimeSpan.FromSeconds(1), () =>
            {
                Device.BeginInvokeOnMainThread(AtualizarDataAsync);
                return true;
            });
        }

        private async void AtualizarDataAsync()
        {
            Carregar();
        }

        public async void Carregar()
        {
            var db = App.Globais.sqLiteConnection;

            var existValueDB = db.GetTableInfo("Usuario");

            if (existValueDB.Count == 0)
                db.CreateTable<Usuario>();

            var listaUsuario = db.Table<Usuario>().ToList();

            if (listaUsuario != null)
            {
                Usuarios = listaUsuario;
            }
        }

        public async Task Cadastro(Usuario usuario)
        {
            await ((NavigationPage)App.Current.MainPage).Navigation.PushAsync(new Cadastro(usuario));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
