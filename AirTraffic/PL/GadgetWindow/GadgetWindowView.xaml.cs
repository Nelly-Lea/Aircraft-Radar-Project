using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using System.Media;

namespace PL.GadgetWindow
{
    /// <summary>
    /// Interaction logic for GadgetWindow.xaml
    /// </summary>
    public partial class GadgetWindowView : Window
    {
        //public ObservableCollection<BE.Root> listRoot = null;
        GadgetWindowViewModel gadgetViewModel = new GadgetWindowViewModel();
        BE.Root flight = new BE.Root();
        public string CurrentTime;
        public DispatcherTimer LiveTimeLabel = new DispatcherTimer();
        public TimeSpan timeNow = new TimeSpan();
        public DispatcherTimer timer = new DispatcherTimer();
        public TimeSpan Timer = new TimeSpan();
        public TimeSpan ArrivalTimeSpan = new TimeSpan();
        public SoundPlayer snd = new SoundPlayer("C:/projet AirTraffic last version/AirTraffic/PL/images/alert sound.wav");
        public GadgetWindowView()
        {
            InitializeComponent();
            CurrentTime = DateTime.Now.ToString();

//            LiveTimeLabel.Interval = TimeSpan.FromSeconds(1);
  //          LiveTimeLabel.Tick += timer_Tick;
    //        LiveTimeLabel.Start();

            //listRoot=new ObservableCollection<BE.Root>( gadgetViewModel.GVMGetAllFlightsRoot());
            //cbListFlights.ItemsSource = listRoot;
            //cbListFlights.DisplayMemberPath = "identification.number.@default";
            //cbListFlights.DataContext = listRoot;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            LabelTimerNow.Content = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            snd.Stop();
            timer.Stop();
           // LiveTimeLabel.Stop();

        }

        //private void cbListFlights_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    flight = cbListFlights.SelectedItem as BE.Root;
        //}

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) // faire attention des pas trouve avion alors qu'il existe
        {
           
            if (e.Key != System.Windows.Input.Key.Enter) return;

            // your event handler here
            e.Handled = true;
            flight = gadgetViewModel.GVMgetRootFromFlightCode(tbFlight.Text);
            if (flight != null)
            {
                string arrivalstr = flight.status.text;
                string[] numbers = Regex.Split(arrivalstr, @"\D+");


                ArrivalTimeSpan = new TimeSpan(Int32.Parse(numbers[1]), Int32.Parse(numbers[2]), Int32.Parse("00"));

               // LiveTimeLabel.Stop(); // je ne sais pas si on met ou pas
                DateTime CurrentTime = DateTime.Now;
                timeNow = CurrentTime.TimeOfDay;


                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick1;
                timer.Start();
                TimeSpan t = new TimeSpan(0, 0, Int32.Parse("1"));
                timeNow += t;




                //  TimeSpan interval = new TimeSpan();
                if (ArrivalTimeSpan >= timeNow)
                {
                    snd.Play();
                    Timer = (ArrivalTimeSpan.Subtract(timeNow));
                    txtTimeDue.Text = Timer.ToString();
                    if (Timer.Hours.ToString() == "1" && Timer.Minutes.ToString() == "0" && Timer.Seconds.ToString() == "0") // alarm sound one hour before landing of the plane
                        snd.Play();
                }
                else
                {
                    snd.Play();
                    timer.Stop();
                    MessageBox.Show("the plane has already landed");
                }




            }
            else
            {
                MessageBox.Show("the fligh code is wrong");
            }
        }

        void timer_Tick1(object sender, EventArgs e)
        {
            TimeSpan t = new TimeSpan(0, 0, Int32.Parse("1"));
            timeNow += t;
            if (ArrivalTimeSpan >= timeNow)
            {
               

                Timer = (ArrivalTimeSpan.Subtract(timeNow));
                txtTimeDue.Text = Timer.ToString();
                if (Timer.Hours.ToString() == "1" && Timer.Minutes.ToString() == "0" && Timer.Seconds.ToString() == "0")// alarm sound one hour before landing of the plane
                    snd.Play();
            }
            else
            {
                snd.Play();
                timer.Stop();
                MessageBox.Show("the plane has already landed");
            }





        }



    }
}

