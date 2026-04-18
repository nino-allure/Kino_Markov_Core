using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kino_Markov.Classes;

namespace Kino_Markov.Pages.Users
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Page
    {
        public ClubsContext AllClub = new ClubsContext();
        /// <summary> Данные пользователя для изменения
        public Models.Users User;
        /// <summary> Страница Main
        public Main Main;

        public Add(Main Main, Models.Users User = null)
        {
            InitializeComponent();
            // Запоминаем страницу
            this.Main = Main;
            // Перебираем клубы
            foreach (Models.Clubs Club in AllClub.Clubs)
                // добавляем в выпадающий список
                Clubs.Items.Add(Club.Name);
            // Добавляем элемент
            Clubs.Items.Add("Выберите ...");
            // Если пользователь для изменения
            if (User != null)
            {
                // Запоминаем пользователя
                this.User = User;
                // Указываем данные пользователя
                this.FIO.Text = User.FIO;
                this.RentStart.Text = User.RentStart.ToString("yyyy-MM-dd");
                this.RentTime.Text = User.RentStart.ToString("HH:mm");
                this.Duration.Text = User.Duration.ToString();
                Clubs.SelectedItem = AllClub.Clubs.Where(x => x.Id == User.IdClub).First().Name;
                // Изменяем текст кнопки
                BtnAdd.Content = "Изменить";
            }
        }

        /// <summary> Метод добавления или изменения
        private void AddUser(object sender, System.Windows.RoutedEventArgs e)
        {
            // Создаём дату аренды
            DateTime DTRentStart = new DateTime();
            // Конвертируем дату
            DateTime.TryParse(this.RentStart.Text, out DTRentStart);
            // Добавляем время к полю
            DTRentStart = DTRentStart.Add(TimeSpan.Parse(this.RentTime.Text));
            // Если пользователь для добавления
            if (this.User == null)
            {
                // Создаём новый объект
                User = new Models.Users();
                // Указываем данные
                User.FIO = this.FIO.Text;
                User.RentStart = DTRentStart;
                User.Duration = Convert.ToInt32(this.Duration.Text);
                User.IdClub = AllClub.Clubs.Where(x => x.Name == Clubs.SelectedItem).First().Id;
                // Добавляем пользователя в контекст
                this.Main.AllUsers.Users.Add(this.User);
            }
            else
            {
                // Изменяем данные объекта
                User.FIO = this.FIO.Text;
                User.RentStart = DTRentStart;
                User.Duration = Convert.ToInt32(this.Duration.Text);
                User.IdClub = AllClub.Clubs.Where(x => x.Name == Clubs.SelectedItem).First().Id;
            }
            // Сохраняем все изменения
            this.Main.AllUsers.SaveChanges();
            // Открываем страницу с пользователями
            MainWindow.init.OpenPages(new Pages.Users.Main());
        }
    }
}
