#region Using statement
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
#endregion

namespace Tetris.Controls
{
    public class Overlay : ContentControl
    {
        private const string FadeInName = "FadeInStoryboard";
        private const string FadeOutName = "FadeOutStoryboard";
        private const string LayoutGridName = "LayoutGrid";

        private Storyboard fadeIn;
        private Storyboard fadeOut;
        private Grid layoutGrid;

        public Overlay()
        {
            DefaultStyleKey = typeof(Overlay);
        }

        public object OverlayControl
        {
            get { return (object)GetValue(ProgressControlProperty); }
            set { SetValue(ProgressControlProperty, value); }
        }

        public bool IsAvailable
        {
            get { return (bool) GetValue(IsOverlayAvailable); }
            set
            {
                SetValue(IsOverlayAvailable, value);
                if (value) Show();
                else Hide();
            }
        }

        public static readonly DependencyProperty ProgressControlProperty = DependencyProperty.Register("OverlayControl", typeof(object), typeof(Overlay), new PropertyMetadata(null));
        public static readonly DependencyProperty IsOverlayAvailable = DependencyProperty.Register("IsAvailable", typeof(bool), typeof(Overlay), new PropertyMetadata(null, OnLabelChanged));

        private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Overlay) d;
            var isAvailable = (bool) e.NewValue;
            if(isAvailable)
                control.Show();
            else
                control.Hide();
        }


        public void FadeOutCompleted(object sender, object e)
        {
            layoutGrid.Opacity = 1;
            Visibility = Visibility.Collapsed;
        }

        public void Switch()
        {
            if (Visibility == Visibility.Visible)
                Hide();
            else
                Show();
        }

        public void Show()
        {
            if (fadeIn == null)
                ApplyTemplate();

            Visibility = Visibility.Visible;

            if (fadeOut != null)
                fadeOut.Stop();

            if (fadeIn != null)
                fadeIn.Begin();
        }

        public void Hide()
        {
            if (fadeOut == null)
                ApplyTemplate();

            if (fadeIn != null)
                fadeIn.Stop();

            if (fadeOut != null)
                fadeOut.Begin();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            fadeIn = GetTemplateChild(FadeInName) as Storyboard;
            fadeOut = GetTemplateChild(FadeOutName) as Storyboard;
            layoutGrid = GetTemplateChild(LayoutGridName) as Grid;

            if (fadeOut != null)
                fadeOut.Completed += FadeOutCompleted;
        }
    }
}