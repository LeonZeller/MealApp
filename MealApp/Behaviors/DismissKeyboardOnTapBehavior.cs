namespace MealApp.Behaviors;

public class DismissKeyboardOnTapBehavior : Behavior<ContentPage>
{
    protected override void OnAttachedTo(ContentPage page)
    {
        base.OnAttachedTo(page);
        page.Loaded += onPageLoaded;
    }

    protected override void OnDetachingFrom(ContentPage page)
    {
        base.OnDetachingFrom(page);
        page.Loaded -= onPageLoaded;
    }

    private void onPageLoaded(object? sender, EventArgs e)
    {
        if (sender is not ContentPage page)
        {
            return;
        }
        if (page.Content == null)
        {
            return;
        }

        attachTapRecognizer(page.Content);
    }

    private void attachTapRecognizer(View content)
    {
        if (content is Layout layout)
        {
            var tap = new TapGestureRecognizer();
            tap.Tapped += onBackGroundTapped;
            layout.GestureRecognizers.Add(tap);
        }
    }

    private void onBackGroundTapped(object? sender, TappedEventArgs e)
    {
        var focused = findFocusedElement(Application.Current?.Windows[0]?.Page);
        focused?.Unfocus();
    }

    private VisualElement? findFocusedElement(Element? root)
    {
        if (root == null)
        {
            return null;
        }

        if (root is InputView iv && iv.IsFocused)
        {
            return iv;
        }

        if (root is IVisualTreeElement vte)
        {
            foreach (var child in vte.GetVisualChildren())
            {
                if (child is Element childElement)
                {
                    var found = findFocusedElement(childElement);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }
        }
        return null;
    }
}