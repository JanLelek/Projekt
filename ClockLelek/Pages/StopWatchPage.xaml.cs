using System.Collections.ObjectModel;

namespace ClockLelek.Pages;

public partial class StopWatchPage : ContentPage
{
    IDispatcherTimer timer;
    int cas = 0;
    bool isRunning = false;
    int index = 0;
    TimeSpan lastLap = TimeSpan.Zero;
    bool isFirst = true;
    public class Lap
    {
        public string lap {  get; set; }//získání a nastavení hodnoty vlastnosti.
        public string fromLast { get; set; }
        public string lapTime { get; set; }
    }

    public ObservableCollection<Lap> lapList = new(); //umožňuje sledování změn a aktualizaci dat

    public StopWatchPage()
	{
        InitializeComponent();
        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(1); //interval časovače na 1 milisekundu.
        timer.Tick += Timer_Tick; //Timer_Tick se bude volat při každém tiknutí časovače.
        LapListView.ItemsSource = lapList; //LapListView bude zobrazovat data z kolekce lapList
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        cas++;
        TimeSpan time = TimeSpan.FromMilliseconds(cas * 10);
        string format = (time.TotalMinutes >= 1) ? @"mm\:ss\:ff" : @"ss\:ff"; //formát pro zobrazení času
        CasLabel.Text = time.ToString(format); //zobrazení čaasu v labelu
    }

    private void Reset_Clicked(object sender, EventArgs e)
    {
        timer.Stop();
        StartPauseBt.Text = "Start";

        cas = 0;
        ResetBt.IsVisible = false;
        LapBt.IsVisible = false;
        CasLabel.Text = "00:00";
        lapList.Clear();
        isRunning = false;
        index = 0;
        isFirst = true;
    }

    private void StartPause_Clicked(object sender, EventArgs e)
    {
        if (isRunning) //pause
        {
            StartPauseBt.Text = "Start";
            timer.Stop();
            LapBt.IsVisible = true;
            isRunning = false;
            LapBt.IsVisible = false;
            StartPauseBt.TextColor = Color.FromArgb("#90EE90");
            StartPauseBt.BackgroundColor = Color.FromArgb("#013220");

        }
        else //start
        {
            StartPauseBt.Text = "Stop";

            isRunning = true;
            timer.Start(); 
            ResetBt.IsVisible = true;
            LapBt.IsVisible = true;
            StartPauseBt.TextColor = Color.FromArgb("#FF0000");
            StartPauseBt.BackgroundColor = Color.FromArgb("#8B0000");
        }
    }

    private void Lap_Clicked(object sender, EventArgs e)
    {
        if (isFirst)
        {
            lapList.Add(new Lap() { lap = (index + 1).ToString("D2"), fromLast = TimeSpan.FromMilliseconds(cas * 10).ToString(@"mm\:ss\:ff"), lapTime = TimeSpan.FromMilliseconds(cas * 10).ToString(@"mm\:ss\:ff") });
            isFirst = false;
        }
        else
        {
            lapList.Add(new Lap() { lap = (index + 1).ToString("D2"), fromLast = (TimeSpan.FromMilliseconds(cas * 10) - lastLap).ToString(@"mm\:ss\:ff"), lapTime = TimeSpan.FromMilliseconds(cas * 10).ToString(@"mm\:ss\:ff") });

        }
        lastLap = TimeSpan.FromMilliseconds(cas * 10);
        index++;
    }
}
