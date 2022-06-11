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
using System.Data.Entity;

namespace ProjektEF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SZKOLAEntities context = new SZKOLAEntities();
        CollectionViewSource custViewSource;
        CollectionViewSource ordViewSource;
        public MainWindow()
        {
            InitializeComponent();
            custViewSource = ((CollectionViewSource)(FindResource("uczniowieViewSource")));
            ordViewSource = ((CollectionViewSource)(FindResource("uczniowieViewSource")));
            DataContext = context;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource uczniowieViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("uczniowieViewSource")));
            System.Windows.Data.CollectionViewSource przedmiotyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("przedmiotyViewSource")));
            System.Windows.Data.CollectionViewSource nauczycieleViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("nauczycieleViewSource")));
            System.Windows.Data.CollectionViewSource wycieczkiViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("wycieczkiViewSource")));
            context.Uczniowies.Load();
            custViewSource.Source = context.Uczniowies.Local;
        }

        private void UczniowieCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            custViewSource = ((CollectionViewSource)(FindResource("uczniowieViewSource")));
            context.Uczniowies.Load();
            custViewSource.Source = context.Uczniowies.Local;
            uczniowieDataGrid.Visibility = Visibility.Visible;
            przedmiotyDataGrid.Visibility = Visibility.Hidden;
            nauczycieleDataGrid.Visibility = Visibility.Hidden;
            wycieczkiDataGrid.Visibility = Visibility.Hidden;
            uczniowieStackPanel.Visibility = Visibility.Visible;
            nauczycieleStackPanel.Visibility = Visibility.Hidden;
            wycieczkiStackPanel.Visibility = Visibility.Hidden;
            przedmiotyStackPanel.Visibility = Visibility.Hidden;
        }

        private void NauczycieleCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            custViewSource = ((CollectionViewSource)(FindResource("nauczycieleViewSource")));
            context.Nauczycieles.Load();
            custViewSource.Source = context.Nauczycieles.Local;
            uczniowieDataGrid.Visibility = Visibility.Hidden;
            przedmiotyDataGrid.Visibility = Visibility.Hidden;
            nauczycieleDataGrid.Visibility = Visibility.Visible;
            wycieczkiDataGrid.Visibility = Visibility.Hidden;
            uczniowieStackPanel.Visibility = Visibility.Hidden;
            nauczycieleStackPanel.Visibility = Visibility.Visible;
            wycieczkiStackPanel.Visibility = Visibility.Hidden;
            przedmiotyStackPanel.Visibility = Visibility.Hidden;
        }

        private void WycieczkiCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            custViewSource = ((CollectionViewSource)(FindResource("wycieczkiViewSource")));
            context.Wycieczkis.Load();
            custViewSource.Source = context.Wycieczkis.Local;
            uczniowieDataGrid.Visibility = Visibility.Hidden;
            przedmiotyDataGrid.Visibility = Visibility.Hidden;
            nauczycieleDataGrid.Visibility = Visibility.Hidden;
            wycieczkiDataGrid.Visibility = Visibility.Visible;
            uczniowieStackPanel.Visibility = Visibility.Hidden;
            nauczycieleStackPanel.Visibility = Visibility.Hidden;
            wycieczkiStackPanel.Visibility = Visibility.Visible;
            przedmiotyStackPanel.Visibility = Visibility.Hidden;
        }

        private void PrzedmiotyCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            custViewSource = ((CollectionViewSource)(FindResource("przedmiotyViewSource")));
            context.Przedmioties.Load();
            custViewSource.Source = context.Przedmioties.Local;
            uczniowieDataGrid.Visibility = Visibility.Hidden;
            przedmiotyDataGrid.Visibility = Visibility.Visible;
            nauczycieleDataGrid.Visibility = Visibility.Hidden;
            wycieczkiDataGrid.Visibility = Visibility.Hidden;
            uczniowieStackPanel.Visibility = Visibility.Hidden;
            nauczycieleStackPanel.Visibility = Visibility.Hidden;
            wycieczkiStackPanel.Visibility = Visibility.Hidden;
            przedmiotyStackPanel.Visibility = Visibility.Visible;
        }

        private void Delete_Uczen(object sender, RoutedEventArgs e)
        {
            int id = (uczniowieDataGrid.SelectedItem as Uczniowie).ID;
            Uczniowie uczen = (from r in context.Uczniowies where r.ID == id select r).SingleOrDefault();

            var cur = custViewSource.View.CurrentItem as Uczniowie;
            var cust = (from c in context.Uczniowies
                        where c.ID == cur.ID
                        select c).FirstOrDefault();

            foreach (var zd in cust.ZadaniaDomowes.ToList())
            {
                context.ZadaniaDomowes.Remove(zd);
            }
            context.Uczniowies.Remove(uczen);
            context.SaveChanges();
            ordViewSource.View.Refresh();
        }

        private void Delete_Nauczyciel(object sender, RoutedEventArgs e)
        {
            int id = (nauczycieleDataGrid.SelectedItem as Nauczyciele).ID;
            Nauczyciele nauczyciel = (from r in context.Nauczycieles where r.ID == id select r).SingleOrDefault();

            var cur = custViewSource.View.CurrentItem as Nauczyciele;
            var cust = (from c in context.Nauczycieles
                        where c.ID == cur.ID
                        select c).FirstOrDefault();

            //usuwanie z kluczów obcych
            foreach (var zd in cust.ZadaniaDomowes.ToList())
            {
                context.ZadaniaDomowes.Remove(zd);
            }
            foreach (var p in cust.Wycieczkis.ToList())
            {
                context.Wycieczkis.Remove(p);
            }

            context.Nauczycieles.Remove(nauczyciel);
            context.SaveChanges();
            ordViewSource.View.Refresh();
        }

        private void Delete_Przedmioty(object sender, RoutedEventArgs e)
        {
            int id = (przedmiotyDataGrid.SelectedItem as Przedmioty).ID;
            Przedmioty przedmiot = (from r in context.Przedmioties where r.ID == id select r).SingleOrDefault();

            context.Przedmioties.Remove(przedmiot);
            context.SaveChanges();
            ordViewSource.View.Refresh();
        }

        private void Delete_Wycieczki(object sender, RoutedEventArgs e)
        {
            int id = (wycieczkiDataGrid.SelectedItem as Wycieczki).ID;
            Wycieczki wycieczka = (from r in context.Wycieczkis where r.ID == id select r).SingleOrDefault();

            context.Wycieczkis.Remove(wycieczka);
            context.SaveChanges();
            ordViewSource.View.Refresh();
        }

        private void Add_Uczen(object sender, RoutedEventArgs e)
        {

            Uczniowie uczen = new Uczniowie()
            {
                Imię = imieBox.Text,
                Nazwisko = nazwiskoTextBox.Text,

                PESEL = pESELTextBox.Text
            };

            context.Uczniowies.Add(uczen);
            context.SaveChanges();
            ordViewSource.View.Refresh();
        }

        private void Add_Przedmiot(object sender, RoutedEventArgs e)
        {
            Przedmioty przedmiot = new Przedmioty()
            {
                Przedmiot = przedmiotTextBox.Text
            };

            context.Przedmioties.Add(przedmiot);
            context.SaveChanges();
            ordViewSource.View.Refresh();
        }

        private void Add_Nauczyciel(object sender, RoutedEventArgs e)
        {

        }

        private void Add_Wycieczka(object sender, RoutedEventArgs e)
        {

        }
    }
}

