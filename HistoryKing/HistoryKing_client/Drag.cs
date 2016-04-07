using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;


namespace HistoryKing_client
{
    class Drag : DependencyObject
    {
        private static Point FirstPoint;
        private static int OldzIndex;
        private static Page page;
        private static FrameworkElement CurrentElement;

        private static readonly DependencyProperty DragProperty =
            DependencyProperty.RegisterAttached("Drag",
            typeof(bool),
            typeof(Drag),
            new UIPropertyMetadata(false, ChangeDragProperty));

        public static void SetWindow(Page target)
        {
            page = target;
        }

        public static void SetDrag(UIElement element, bool flag)
        {
            if (page != null)
            {
                element.SetValue(DragProperty, flag);
            }
        }

        public static bool GetDrag(UIElement element)
        {
            return (bool)element.GetValue(DragProperty);
        }

        private static void ChangeDragProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;

            if (element != null)
            {
                if ((bool)e.NewValue)
                {
                    // 오프셋 변경을 위해 생성합니다.
                    element.RenderTransform = new TranslateTransform(0, 0);

                    element.PreviewMouseDown += element_PreviewMouseDown;
                    element.PreviewMouseUp += element_PreviewMouseUp;
                }
                else
                {
                    element.PreviewMouseDown -= element_PreviewMouseDown;
                    element.PreviewMouseUp -= element_PreviewMouseUp;
                }
            }
        }

        static void window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (CurrentElement != null &&
                e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                Point CurrentPoint = e.GetPosition(CurrentElement);

                TranslateTransform Translate = CurrentElement.RenderTransform as TranslateTransform;

                if (Translate != null)
                {
                    CurrentElement.LayoutTransform = Translate;
                    Translate.X += CurrentPoint.X - FirstPoint.X;
                    Translate.Y += CurrentPoint.Y - FirstPoint.Y;
                }
            }
        }

        static void element_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CurrentElement = sender as FrameworkElement;

            OldzIndex = Canvas.GetZIndex(CurrentElement);
            Canvas.SetZIndex(CurrentElement, 100);

            FirstPoint = e.GetPosition(CurrentElement);

            page.PreviewMouseMove += window_PreviewMouseMove;
        }

        private static void element_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;

            Canvas.SetZIndex(element, OldzIndex);

            CurrentElement = page;
            page.MouseMove -= window_PreviewMouseMove;
        }
    }
}
