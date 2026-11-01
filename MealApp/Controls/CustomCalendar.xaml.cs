
using MealApp.Models;

namespace MealApp.Controls;

public partial class CustomCalendar : ContentView
{
	public event EventHandler<DateTime>? DateTapped;
    public event EventHandler<DateTime>? DateDoubleTapped;

    public CustomCalendar()
	{
		InitializeComponent();
        currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        buildWeekDayHeader();
        buildCalendar();
    }    

    public Dictionary<DateTime, List<CalendarDot>> Dots
    {
        get => (Dictionary<DateTime, List<CalendarDot>>)GetValue(DotsProperty);
        set => SetValue(DotsProperty, value);
    }

    public DateTime? SelectedDate
    {
        get => (DateTime?)GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public static readonly BindableProperty DotsProperty =
        BindableProperty.Create(nameof(Dots), typeof(Dictionary<DateTime, List<CalendarDot>>),
            typeof(CustomCalendar), new Dictionary<DateTime, List<CalendarDot>>(),
            propertyChanged: OnDotsChanged);

    public static readonly BindableProperty SelectedDateProperty = 
        BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?),
            typeof(CustomCalendar), null,
            propertyChanged: OnSelectedDateChanged);

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        buildDaysGrid();
    }

    private static void OnDotsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomCalendar calendar)
        {
            calendar.buildDaysGrid();
        }
    }

    private void buildDaysGrid()
    {
        DaysGrid.Children.Clear();
        MonthLabel.Text = currentMonth.ToString("MMMM yyyy");

        var firstDayOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);

        int startCol = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
        int row = 0;
        int col = startCol;

        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(currentMonth.Year, currentMonth.Month, day);
            var dayView = creatDayView(date);

            Grid.SetColumn((BindableObject)dayView, col);
            Grid.SetRow((BindableObject)dayView, row);
            DaysGrid.Children.Add((IView)dayView);

            col++;
            if (col > 6)
            {
                col = 0;
                row++;
            }
        }
    }

    private object creatDayView(DateTime date)
    {
        bool isToday = date.Date == DateTime.Today;
        bool isSelected = SelectedDate.HasValue && date.Date == SelectedDate.Value.Date;

        var layout = new VerticalStackLayout
        {
            Padding = new Thickness(2),
            Spacing = 2,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = isSelected ? Color.FromArgb("#6B8E4E") : Colors.Transparent
        };

        var dayLabel = new Label
        {
            Text = date.Day.ToString(),
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = isSelected ? Colors.White : (isToday ? Color.FromArgb("#6B8E4E") : Colors.Black),
            FontAttributes = isToday ? FontAttributes.Bold : FontAttributes.None
        };

        layout.Children.Add(dayLabel);

        // Punkte hinzufügen
        if (Dots != null && Dots.ContainsKey(date.Date))
        {
            var dotContainer = new HorizontalStackLayout
            {
                Spacing = 2,
                HorizontalOptions = LayoutOptions.Center
            };

            foreach (var dot in Dots[date.Date].Take(3))
            {
                dotContainer.Children.Add(new BoxView
                {
                    WidthRequest = 5,
                    HeightRequest = 5,
                    CornerRadius = 3,
                    Color = dot.Color
                });
            }

            layout.Children.Add(dotContainer);
        }

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) =>
        {
            if (SelectedDate.HasValue && SelectedDate.Value.Date == date.Date)
                DateDoubleTapped?.Invoke(this, date);
            else
            {
                SelectedDate = date;
                DateTapped?.Invoke(this, date);
            }
        };
        layout.GestureRecognizers.Add(tapGesture);

        return layout;
    }

    private static void OnSelectedDateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomCalendar calendar)
        {
            calendar.buildDaysGrid();
        }
    }    

    private void OnPreviousMonthClicked(object sender, EventArgs e)
    {
        currentMonth = currentMonth.AddMonths(-1);
        buildDaysGrid();
    }

    private void OnNextMonthClicked(object sender, EventArgs e)
    {
        currentMonth = currentMonth.AddMonths(1);
        buildDaysGrid();
    }

    private void buildCalendar()
    {
        buildWeekDayHeader();
        buildDaysGrid();
    }

    private void buildWeekDayHeader()
    {
        
        var days = new[] { "Mo", "Di", "Mi", "Do", "Fr", "Sa", "So" };
        for (int i = 0; i < days.Length; i++)
        {
            var label = new Label
            {
                Text = days[i],
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 12,
                TextColor = Colors.Gray
            };
            Grid.SetColumn(label, i);
            WeekdayHeader.Children.Add(label);
        }
    }

    private DateTime currentMonth;

    
}