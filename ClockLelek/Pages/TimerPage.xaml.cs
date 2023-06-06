
using Microsoft.Maui.Controls;

namespace ClockLelek.Pages;

public partial class TimerPage : ContentPage
{
	int[] seconds = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 };
    int[] minutes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59};
    int[] hours = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
    IDispatcherTimer timer;
    int cas = 0;
    bool isRunning = false;
    double totalnicas; // uchování celkového počtu sekund uplynulých od začátku
    bool firststart = true;
    string format;

    public TimerPage()
	{
		InitializeComponent();
        HoursPicker.ItemsSource = hours; //nastavení zdroje dat pro picker
        MinutesPicker.ItemsSource = minutes;
        SecondsPicker.ItemsSource = seconds;
        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        if (cas !=0)
        {
            cas--;
            TimeSpan time = TimeSpan.FromSeconds(cas);
            casLabel.Text = time.ToString(format);
        }
        else
        {
            timerEnd();
            DisplayAlert("Časovaè", "Čas vypršel", "Ok");
        }
    }

    private void animation()
    {
        strankas.ScaleYTo(1, (uint)cas * 1000);
    }
    private void StartStopBt_Clicked(object sender, EventArgs e)
    {
        if (isRunning)
        {
            timer.Stop();
            StartStopBt.Text = "Start";
            isRunning = false;
            strankas.CancelAnimations();
            StartStopBt.TextColor = Color.FromArgb("#90EE90");
            StartStopBt.BackgroundColor = Color.FromArgb("#013220");
        }
        else
        {
            if (firststart)
            {
                timer.Tick += Timer_Tick;
                CancelBt.IsEnabled = true;
                StartStopBt.TextColor = Color.FromArgb("#FF0000");
                StartStopBt.BackgroundColor = Color.FromArgb("#8B0000");
                StartStopBt.Text = "Stop";
                HoursPicker.IsVisible = false; MinutesPicker.IsVisible = false; SecondsPicker.IsVisible = false;
                casLabel.IsVisible = true;
                int hours = (int)HoursPicker.SelectedIndex;
                if (hours == -1)
                {
                    hours = 0;
                }
                int minutes = (int)MinutesPicker.SelectedIndex;
                if (minutes == -1)
                {
                    minutes = 0;
                }
                int seconds = (int)SecondsPicker.SelectedIndex;
                if (seconds == -1)
                {
                    seconds = 0;
                }
                cas = (hours * 3600) + (minutes * 60) + (seconds);
                totalnicas = cas;
                TimeSpan time = TimeSpan.FromSeconds(cas);
                format = (time.TotalHours >= 1) ? @"hh\:mm\:ss" : (time.TotalMinutes >= 1) ? @"mm\:ss" : @"ss";


                casLabel.Text = time.ToString(format);

                isRunning = true;
                firststart = false;
                animation();
                timer.Start();


            }
            else
            {
                timer.Start();
                isRunning = true;
                StartStopBt.Text = "Stop";
                animation();
                StartStopBt.TextColor = Color.FromArgb("#FF0000");
                StartStopBt.BackgroundColor = Color.FromArgb("#8B0000");
            }
        }
    }

    private void CancelBt_Clicked(object sender, EventArgs e)
    {
        timerEnd();
      
    }

    private void timerEnd()
    {
        StartStopBt.IsEnabled = false;
        strankas.CancelAnimations();
        strankas.ScaleY = 0;
        CancelBt.IsEnabled = false;
        StartStopBt.Text = "Start";
        firststart = true;
        isRunning = false;
        casLabel.IsVisible = false;
        HoursPicker.IsVisible = true; MinutesPicker.IsVisible = true; SecondsPicker.IsVisible = true;
        StartStopBt.IsEnabled = true;
        timer.Tick -= Timer_Tick;
        
        StartStopBt.TextColor = Color.FromArgb("#90EE90");
        StartStopBt.BackgroundColor = Color.FromArgb("#013220");

    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        StartStopBt.IsEnabled = true;
    }
}
