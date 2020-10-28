using Acr.UserDialogs;
using projeto_login.Model;
using projeto_login.Services;
using projeto_login.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace projeto_login.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _senha;

        public string Email { get { return _email; } set { _email = value; PropertyChanged(this, new PropertyChangedEventArgs("Email")); } }
        public string Senha { get { return _senha; } set { _senha = value; PropertyChanged(this, new PropertyChangedEventArgs("Senha")); } }

        public Command LoginCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(Acessar);
        }

        private async void Acessar()
        {
            await Task.Run(async () =>
            {
                Usuario user = new Usuario
                {
                    Email = Email,
                    Senha = Senha
                };

            });

            var db = App.Globais.sqLiteConnection;

            var existTable = db.GetTableInfo("Usuario");

            if (existTable.Count == 0)
                db.CreateTable<Usuario>();

            var existeUsuario = db.Query<Usuario>("SELECT * FROM Usuario").ToList();

            if (existeUsuario.Count == 0)
            {
                if (Email == null || Senha == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta!", "Por favor preencher todos os dados", "OK");
                    return;
                }

                if (!Email.Contains("@"))
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta!", "Verifique o email", "OK");
                    return;
                }
                if (Senha.Length < 8)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta!", "Senha deve ter ao menos oito caracteres", "OK");
                    return;
                }

                var result = await Application.Current.MainPage.DisplayAlert("Atenção", "Nenhum usuário cadastrado, deseja cadastrar o usuário digitado como administrador?", "Sim", "Não");
                if (result)
                {
                    await SalvarUSuarioADM(db, Email, Senha);
                }
                else
                {
                    return;
                }
            }

            var sessaoUsuario = db.Query<Usuario>("SELECT * FROM Usuario Where Email=?", Email).FirstOrDefault();

            if (sessaoUsuario != null)
            {
                if (sessaoUsuario.Senha == Senha)
                {
                    var token = await DataService.GetToken();

                    if (token != null)
                    {
                        App.Current.Properties["TOKEN"] = token;
                        App.Current.MainPage = new NavigationPage(new ListUsuarios());
                    }

                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Alerta!", "Problemas aos buscar o token", "OK");
                        return;
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta!", "Dados incorretos", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alerta!", "Usuário não encontrado", "OK");
            }
        }

        private async Task SalvarUSuarioADM(SQLite.SQLiteConnection conexao, string email, string senha)
        {
            Usuario usuario = new Usuario();
            {
                usuario.Nome = "ADM";
                usuario.Email = email;
                usuario.Senha = senha;
                usuario.Telefone = 0;
                usuario.DataAniversario = DateTime.Now;

                conexao.Insert(usuario);
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
