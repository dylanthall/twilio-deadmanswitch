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
using System.Windows.Threading;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace DeadMansSwitch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer;
        DateTime alarmTime;
        public static bool ButtonPressed = true;
        public static bool StartupArmed = false;
        public static bool SentWarningToDylan = false;
        public MainWindow()
        {
            InitializeComponent();
            EmergencyContactList.InitList();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            alarmTime = DateTime.Today.AddHours(22);

        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan difference = alarmTime.Subtract(DateTime.Now);
            if(!SentWarningToDylan && (difference.TotalSeconds < 1800) && !ButtonPressed && StartupArmed)
            {
                //Warn Dylan that in 30 minutes his DMS will go off.

                try
                {
                    TwilioAccountInfo.Message(EmergencyContactList.DYLAN,
                        "Hey, your dead man's switch will go off in 30 minutes. Just a warning."
                    );
                }
                catch (Exception ex) { };
                SentWarningToDylan=true;

            }
            if(difference.TotalSeconds < 0)
            {
                //Here we are, i've had until this moment to press the button, am I okay?
                if (!ButtonPressed)
                {
                    //Fire Dead Mans Switch. I've likely been dead for hours.
                    for(int x = 0;x < 1; x++)
                    {
                        FireDeadMans();
                    }
                    alarmTime = alarmTime.AddMinutes(30);
                }
                else
                {
                    //Hey we pressed the button, hot damn! A day where we conquered!

                    //Make the new alarm time tomorrow at 22:00 and re-arm the switch.
                    alarmTime = DateTime.Today.AddDays(1).AddHours(22);
                    button.IsEnabled = true;
                    if (StartupArmed)
                    {
                        ButtonPressed = false;
                        armedLabel.Text = "Armed";
                    }
                }
            }
            label.Text = "Time until DMS Fire: " + (int)(difference.Hours) + ":" + (int)(difference.Minutes) + ":" + (int)(difference.Seconds);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Check if we are starting for the first time. Don't want to spoop everyone lol.
            if (!StartupArmed)
            {
                //Arm the dead mans switch. 
                StartupArmed = true;
                ButtonPressed = false;
                armedLabel.Text = "Armed";
                button.Content = "Mark yourself as safe.";

                //Make arm time tomorrow if after 22:00.
                alarmTime = DateTime.Today.AddHours(22);
                TimeSpan difference = alarmTime.Subtract(DateTime.Now);
                if(difference.TotalSeconds < 0) //Arm time is tomorrow. Re-do it.
                {
                    alarmTime = alarmTime.AddHours(24);
                }
            }
            else
            {
                ButtonPressed = true;
                armedLabel.Text = "You marked yourself safe.";
                button.IsEnabled = false;
            }
        }

        private static void FireDeadMans()
        {
            if (!StartupArmed)
                return;

            try
            {


                foreach(String outbound in EmergencyContactList.ContactList) {
                    TwilioAccountInfo.Message(outbound,
                        "This is Dylan Hall's Dead Man Switch. It has been programmed to go off if " +
                        " Dylan becomes unresponsive for several hours. Check on Dylan."
                    );
                }
            }
            catch (Exception ex)
            {
                FireDeadMans();
            }
        }
    }
}
