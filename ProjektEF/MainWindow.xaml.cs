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
        CollectionViewSource viewSource;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewSource = ((CollectionViewSource)(FindResource("uczniowieViewSource")));
            context.Uczniowies.Load();
            viewSource.Source = context.Uczniowies.Local;
            uczniowieDataGrid.CanUserAddRows = false;
        }

        private void UczniowieCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            viewSource = ((CollectionViewSource)(FindResource("uczniowieViewSource")));
            context.Uczniowies.Load();
            viewSource.Source = context.Uczniowies.Local;
            uczniowieDataGrid.Visibility = Visibility.Visible;
            przedmiotyDataGrid.Visibility = Visibility.Hidden;
            nauczycieleDataGrid.Visibility = Visibility.Hidden;
            wycieczkiDataGrid.Visibility = Visibility.Hidden;
            grid1.Visibility = Visibility.Visible;
            grid2.Visibility = Visibility.Hidden;
            grid3.Visibility = Visibility.Hidden;
            grid4.Visibility = Visibility.Hidden;
            uczniowieDataGrid.CanUserAddRows = false;
        }

        private void NauczycieleCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            viewSource = ((CollectionViewSource)(FindResource("nauczycieleViewSource")));
            context.Nauczycieles.Load();
            viewSource.Source = context.Nauczycieles.Local;
            uczniowieDataGrid.Visibility = Visibility.Hidden;
            przedmiotyDataGrid.Visibility = Visibility.Hidden;
            nauczycieleDataGrid.Visibility = Visibility.Visible;
            wycieczkiDataGrid.Visibility = Visibility.Hidden;
            grid1.Visibility = Visibility.Hidden;
            grid2.Visibility = Visibility.Hidden;
            grid3.Visibility = Visibility.Visible;
            grid4.Visibility = Visibility.Hidden;
            nauczycieleDataGrid.CanUserAddRows = false;
        }

        private void WycieczkiCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            viewSource = ((CollectionViewSource)(FindResource("wycieczkiViewSource")));
            context.Wycieczkis.Load();
            viewSource.Source = context.Wycieczkis.Local;
            uczniowieDataGrid.Visibility = Visibility.Hidden;
            przedmiotyDataGrid.Visibility = Visibility.Hidden;
            nauczycieleDataGrid.Visibility = Visibility.Hidden;
            wycieczkiDataGrid.Visibility = Visibility.Visible;
            grid1.Visibility = Visibility.Hidden;
            grid3.Visibility = Visibility.Hidden;
            grid4.Visibility = Visibility.Visible;
            grid2.Visibility = Visibility.Hidden;
            wycieczkiDataGrid.CanUserAddRows = false;
        }

        private void PrzedmiotyCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            viewSource = ((CollectionViewSource)(FindResource("przedmiotyViewSource")));
            context.Przedmioties.Load();
            viewSource.Source = context.Przedmioties.Local;
            uczniowieDataGrid.Visibility = Visibility.Hidden;
            przedmiotyDataGrid.Visibility = Visibility.Visible;
            nauczycieleDataGrid.Visibility = Visibility.Hidden;
            wycieczkiDataGrid.Visibility = Visibility.Hidden;
            grid1.Visibility = Visibility.Hidden;
            grid3.Visibility = Visibility.Hidden;
            grid4.Visibility = Visibility.Hidden;
            grid2.Visibility = Visibility.Visible;
            przedmiotyDataGrid.CanUserAddRows = false;
        }

        private void Delete_Uczen(object sender, RoutedEventArgs e)
        {
            int id = (uczniowieDataGrid.SelectedItem as Uczniowie).ID;
            Uczniowie uczen = (from r in context.Uczniowies where r.ID == id select r).SingleOrDefault();

            var cur = viewSource.View.CurrentItem as Uczniowie;
            var cust = (from c in context.Uczniowies
                        where c.ID == cur.ID
                        select c).FirstOrDefault();

            foreach (var zd in cust.ZadaniaDomowes.ToList())
            {
                context.ZadaniaDomowes.Remove(zd);
            }
            context.Uczniowies.Remove(uczen);
            context.SaveChanges();
            viewSource.View.Refresh();
        }

        private void Delete_Nauczyciel(object sender, RoutedEventArgs e)
        {
            int id = (nauczycieleDataGrid.SelectedItem as Nauczyciele).ID;
            Nauczyciele nauczyciel = (from r in context.Nauczycieles where r.ID == id select r).SingleOrDefault();

            var cur = viewSource.View.CurrentItem as Nauczyciele;
            var cust = (from c in context.Nauczycieles
                        where c.ID == cur.ID
                        select c).FirstOrDefault();

            
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
            viewSource.View.Refresh();
        }

        private void Delete_Przedmioty(object sender, RoutedEventArgs e)
        {
            int id = (przedmiotyDataGrid.SelectedItem as Przedmioty).ID;
            Przedmioty przedmiot = (from r in context.Przedmioties where r.ID == id select r).SingleOrDefault();

            context.Przedmioties.Remove(przedmiot);
            context.SaveChanges();
            viewSource.View.Refresh();
        }

        private void Delete_Wycieczki(object sender, RoutedEventArgs e)
        {
            int id = (wycieczkiDataGrid.SelectedItem as Wycieczki).ID;
            Wycieczki wycieczka = (from r in context.Wycieczkis where r.ID == id select r).SingleOrDefault();

            context.Wycieczkis.Remove(wycieczka);
            context.SaveChanges();
            viewSource.View.Refresh();
        }

        private void Add_Uczen(object sender, RoutedEventArgs e)
        {

            Uczniowie uczen = new Uczniowie()
            {
                Imię = imięTextBox.Text,
                Nazwisko = nazwiskoTextBox.Text,
                KlasaID = int.Parse(klasaIDTextBox.Text),
                PESEL = pESELTextBox.Text
            };

            context.Uczniowies.Add(uczen);
            context.SaveChanges();
            viewSource.View.Refresh();
        }

        private void Add_Przedmiot(object sender, RoutedEventArgs e)
        {

            Przedmioty przedmiot = new Przedmioty()
            {
                Przedmiot = przedmiotTextBox.Text
            };

            context.Przedmioties.Add(przedmiot);
            context.SaveChanges();
            viewSource.View.Refresh();
        }

        private void Add_Nauczyciel(object sender, RoutedEventArgs e)
        {
            Nauczyciele nauczyciel = new Nauczyciele()
            {
                Imię = imieNau.Text,
                Nazwisko = nazwiskoNau.Text,
                PrzedmiotID = int.Parse(przedmiotIDNau.Text)
            };

            context.Nauczycieles.Add(nauczyciel);
            context.SaveChanges();
            viewSource.View.Refresh();
        }

        private void Add_Wycieczka(object sender, RoutedEventArgs e)
        {
            Wycieczki wycieczka = new Wycieczki()
            {
                Miasto = miastoTextBox.Text,
                Atrakcje = atrakcjeTextBox.Text,
                NauczycielID = int.Parse(nauczycielIDTextBox1.Text),
                KlasaID = int.Parse(klasaIDTextBox1.Text),
                Od = odDatePicker.SelectedDate,
                Do = doDatePicker.SelectedDate
            };

            context.Wycieczkis.Add(wycieczka);
            context.SaveChanges();
            viewSource.View.Refresh();
        }

        private void Update_Uczen(object sender, RoutedEventArgs e)
        {
            int uczenID = int.Parse(idUczen.Text);

            Uczniowie u = context.Uczniowies.First(i => i.ID == uczenID);
            u.Imię = imięTextBox.Text;
            u.Nazwisko = nazwiskoTextBox.Text;
            u.PESEL = pESELTextBox.Text;
            u.KlasaID = int.Parse(klasaIDTextBox.Text);

            context.SaveChanges();
            viewSource.View.Refresh();
        }

        private void Update_Nauczyciel(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(nauID.Text);
            Nauczyciele nau = context.Nauczycieles.First(i => i.ID == id);
            nau.Imię = imieNau.Text;
            nau.Nazwisko = nazwiskoNau.Text;
            nau.PrzedmiotID = int.Parse(przedmiotIDNau.Text);

            context.SaveChanges();
            viewSource.View.Refresh();
        }

        private void Update_Przedmiot(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(przedmiotID.Text);
            Przedmioty p = context.Przedmioties.First(i => i.ID == id);
            p.Przedmiot = przedmiotTextBox.Text;

            context.SaveChanges();
            viewSource.View.Refresh();
        }

        private void Update_Wycieczka(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(wycID.Text);
            Wycieczki w = context.Wycieczkis.First(i => i.ID == id);
            w.Miasto = miastoTextBox.Text;
            w.Atrakcje = atrakcjeTextBox.Text;
            w.KlasaID = int.Parse(klasaIDTextBox1.Text);
            w.NauczycielID = int.Parse(nauczycielIDTextBox1.Text);
            w.Od = odDatePicker.SelectedDate;
            w.Do = doDatePicker.SelectedDate;

            context.SaveChanges();
            viewSource.View.Refresh();
        }
    }
}

