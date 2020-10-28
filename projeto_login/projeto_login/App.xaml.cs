using SQLite;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace projeto_login
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Views.login();
        }

        public static class Globais
        {
            public static SQLiteConnection sqLiteConnection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XamarinSQLite.db3"));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
