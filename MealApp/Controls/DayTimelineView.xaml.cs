using MealApp.Models;
using Microsoft.Maui.Controls.Shapes;
using Plugin.Maui.CalendarStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealApp.Controls
{

    public partial class DayTimelineView : ContentView
    {
        public event EventHandler<CalendarEntryTappedEventArgs>? EntryTapped;
        public DayTimelineView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty EntriesProperty =
            BindableProperty.Create(nameof(Entries), typeof(IEnumerable<CalendarEntry>),
                typeof(DayTimelineView), null, propertyChanged: OnEntriesChanged);

        public static readonly BindableProperty SystemEventsProperty =
        BindableProperty.Create(nameof(SystemEvents), typeof(IEnumerable<CalendarEvent>),
        typeof(DayTimelineView), null, propertyChanged: OnSystemEventsChanged);

        public IEnumerable<CalendarEvent> SystemEvents
        {
            get => (IEnumerable<CalendarEvent>)GetValue(SystemEventsProperty);
            set => SetValue(SystemEventsProperty, value);
        }        

        public IEnumerable<CalendarEntry> Entries
        {
            get => (IEnumerable<CalendarEntry>)GetValue(EntriesProperty);
            set => SetValue(EntriesProperty, value);
        }

        private static void OnSystemEventsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DayTimelineView view)
            {
                view.buildTimeline();
            }
        }

        private static void OnEntriesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DayTimelineView view)
                view.buildTimeline();
        }

        private void buildTimeline()
        {
            TimelineGrid.RowDefinitions.Clear();
            TimelineGrid.Children.Clear();

            int totalHours = endHour - startHour;

            for (int i = 0; i <= totalHours; i++)
            {
                TimelineGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(hourHeight) });

            }

            for (int hour = startHour; hour <= endHour; hour++)
            {
                int row = hour - startHour;

                TimelineGrid.Add(new Label
                {
                    Text = $"{hour:00}:00",
                    FontSize = 12,
                    TextColor = Colors.Gray,
                    VerticalOptions = LayoutOptions.Start,
                }, 0, row);

                TimelineGrid.Add(new BoxView
                {
                    HeightRequest = 1,
                    BackgroundColor = Colors.LightGray,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Start,
                }, 1, row);
            }

            if (Entries == null) return;

            foreach (var entry in Entries)
            {
                var startTime = entry.StartCookingTime.TimeOfDay;
                var endTime = entry.ServingTime;

                if (startTime.Hours < startHour || startTime.Hours > endHour) continue;

                double topOffset = (startTime.Hours - startHour + startTime.Minutes / 60.0) * hourHeight;
                double height = (endTime - startTime).TotalHours * hourHeight;

                if (height < 30) height = 30;

                var color = GetMealTimeColor(entry.MealTime);

                var block = new Border
                {
                    BackgroundColor = color.WithAlpha(0.8f),
                    Stroke = color,
                    StrokeShape = new RoundRectangle { CornerRadius = 8 },
                    Padding = new Thickness(8, 4),
                    Margin = new Thickness(4, topOffset, 4, 0),
                    HeightRequest = height,
                    VerticalOptions = LayoutOptions.Start,
                    Content = new VerticalStackLayout
                    {
                        Children =
                        {
                            new Label
                            {
                                Text = entry.Meal?.Name ?? "",
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 13,
                                TextColor = Colors.White
                            },
                            new Label
                            {
                                Text = $"Fertig: {entry.ServingTime:hh\\:mm}",
                                FontSize = 11,
                                TextColor = Colors.White
                            }
                        }
                    }
                };

                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (s, e) =>
                {
                    EntryTapped?.Invoke(this, new CalendarEntryTappedEventArgs { Entry = entry });
                };
                block.GestureRecognizers.Add(tapGesture);

                Grid.SetColumn(block, 1);
                Grid.SetRow(block, 0);
                Grid.SetRowSpan(block, totalHours);
                TimelineGrid.Children.Add(block);

            }

            if (SystemEvents != null)
            {
                foreach (var ev in SystemEvents)
                {
                    var startTime = ev.StartDate.LocalDateTime.TimeOfDay;
                    var endTime = ev.EndDate.LocalDateTime.TimeOfDay;

                    if (startTime.Hours < startHour || startTime.Hours > endHour) continue;

                    double topOffset = (startTime.Hours - startHour + startTime.Minutes / 60.0) * hourHeight;
                    double height = (endTime - startTime).TotalHours * hourHeight;
                    if (height < 30) height = 30;

                    var block = new Border
                    {
                        BackgroundColor = Color.FromArgb("#888888"),
                        Stroke = Color.FromArgb("#666666"),
                        StrokeShape = new RoundRectangle { CornerRadius = 8 },
                        Padding = new Thickness(8, 4),
                        Margin = new Thickness(4, topOffset, 4, 0),
                        HeightRequest = height,
                        VerticalOptions = LayoutOptions.Start,
                        Opacity = 0.7,
                        Content = new VerticalStackLayout
                        {
                            Children =
                            {
                                 new Label
                                {
                                    Text = $"?? {ev.Title}",
                                    FontAttributes = FontAttributes.Bold,
                                    FontSize = 12,
                                    TextColor = Colors.White
                                },
                                new Label
                                {
                                    Text = $"{startTime:hh\\:mm} - {endTime:hh\\:mm}",
                                    FontSize = 10,
                                    TextColor = Colors.White
                                }
                            }
                        }
                    };

                    Grid.SetColumn(block, 1);
                    Grid.SetRow(block, 0);
                    Grid.SetRowSpan(block, totalHours);
                    TimelineGrid.Children.Add(block);
                }
            }
        }

        private static Color GetMealTimeColor(MealTime mealTime) => mealTime switch
        {
            MealTime.Frühstück => Color.FromArgb("#FFC107"),
            MealTime.Mittagessen => Color.FromArgb("#4CAF50"),
            MealTime.Abendessen => Color.FromArgb("#F44336"),
            _ => Colors.Gray
        };


        private const int startHour = 0;
        private const int endHour = 24;
        private const int hourHeight = 60;
    }
}