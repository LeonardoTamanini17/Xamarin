using projeto_login.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace projeto_login.ViewModel
{
    public class CadastroViewModel : INotifyPropertyChanged
    {
        private string _nome;
        private long _telefone;
        private string _email;
        private string _senha;
        private DateTime _aniversario;

        Usuario userUpdate = new Usuario();

        public Command SalvarCommand { get; set; }

        public string Email { get { return _email; } set { _email = value; PropertyChanged(this, new PropertyChangedEventArgs("Email")); } }
        public string Senha { get { return _senha; } set { _senha = value; PropertyChanged(this, new PropertyChangedEventArgs("Senha")); } }
        public long Telefone { get { return _telefone; } set { _telefone = value; PropertyChanged(this, new PropertyChangedEventArgs("Telefone")); } }
        public DateTime Aniversario { get { return _aniversario; } set { _aniversario = value; PropertyChanged(this, new PropertyChangedEventArgs("Aniversario")); } }
        public string Nome { get { return _nome; } set { _nome = value; PropertyChanged(this, new PropertyChangedEventArgs("Nome")); } }

        public CadastroViewModel(Usuario usuario)
        {
            if (usuario != null)
            {
                userUpdate = usuario;
                Carregar(usuario);
            }
           
            SalvarCommand = new Command(async () => await Salvar(userUpdate));
        }

        private async void Carregar(Usuario usuario)
        {
            _email = usuario.Email;
            _nome = usuario.Nome;
            _telefone = usuario.Telefone;
            _aniversario = usuario.DataAniversario;
            _senha = usuario.Senha;
        }

        private async Task Salvar(Usuario usuarioUpdate)
        {
            var result = await Application.Current.MainPage.DisplayAlert("Atenção", "Deseja realmente salvar?", "Sim", "Não");
            if (result)
            {
                var db = App.Globais.sqLiteConnection;
                var existTable = db.GetTableInfo("Usuario");

                if (existTable.Count == 0)
                    db.CreateTable<Usuario>();

                if (Nome == null || Email == null || Senha == null || Telefone == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta!", "Por favor preencha todos os campos", "OK");
                    return;
                }
                if (Nome.Length <= 5)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta!", "Preencha o nome completo", "OK");
                    return;
                }
                if (!Email.Contains("@"))
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta!", "Verifique o email", "OK");
                    return;
                }
                if (Telefone.ToString().Length < 11)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta!", "Verifique o número de telefone", "OK");
                    return;
                }
                if (Senha.Length < 8)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta!", "Senha deve ter ao menos oito caracteres", "OK");
                    return;
                }

                if (usuarioUpdate != null && usuarioUpdate.Id > 0)
                {
                    await EditarUSuario(db, usuarioUpdate);
                }
                else
                {
                    await SalvarUSuario(db); 
                }

                await Application.Current.MainPage.Navigation.PopAsync(true);
            }
        }

        private async Task SalvarUSuario(SQLite.SQLiteConnection conexao)
        {
            Usuario usuario = new Usuario();
            {
                usuario.Nome = Nome;
                usuario.Email = Email;
                usuario.Senha = Senha;
                usuario.Telefone = Telefone;
                usuario.DataAniversario = Aniversario;

                conexao.Insert(usuario);
                await Application.Current.MainPage.DisplayAlert("Alerta!", "Dados salvos com sucesso", "OK");
            }
        }

        private async Task EditarUSuario(SQLite.SQLiteConnection conexao, Usuario usuarioUpdate)
        {
            Usuario usuario = new Usuario();
            {
                usuario.Id = usuarioUpdate.Id;
                usuario.Nome = Nome;
                usuario.Email = Email;
                usuario.Senha = Senha;
                usuario.Telefone = Telefone;
                usuario.DataAniversario = Aniversario;

                conexao.Update(usuario);
                await Application.Current.MainPage.DisplayAlert("Alerta!", "Dados salvos com sucesso", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
