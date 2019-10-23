using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TVP.DocumentClient.Helpers
{
    public class EventToMethod
    {
        public static readonly DependencyProperty ActionProperty =
        DependencyProperty.RegisterAttached(
            "Action",
            typeof(string),
            typeof(EventToMethod),
            new FrameworkPropertyMetadata(ActionChanged));

        private static void ActionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)d;
            element.DataContextChanged += Element_DataContextChanged;
        }

        private static void Element_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;
            if (!(sender is FrameworkElement element))
                return;
            var sections = GetAction(element).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var section in sections)
            {
                var names = section.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (names.Length != 2)
                    throw new ApplicationException("Invalid number of parameters");
                var source = element.GetType().GetEvent(names[0].Trim());
                if (source == null)
                    throw new ApplicationException($"Can't find source event \"{names[0]}\"");
                var target = element.DataContext.GetType().GetMethod(names[1].Trim());
                if (target == null)
                    throw new ApplicationException($"Can't find target method \"{names[1]}\"");
                var d = target.CreateDelegate(source.EventHandlerType, element.DataContext);
                source.AddEventHandler(element, d);
            }
        }

        public static string GetAction(DependencyObject obj)
        {
            return (string)obj.GetValue(ActionProperty);
        }
        public static void SetAction(DependencyObject obj, string value)
        {
            obj.SetValue(ActionProperty, value);
        }


    }
}
