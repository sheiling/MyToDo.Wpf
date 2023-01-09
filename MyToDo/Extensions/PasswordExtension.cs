using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;


namespace MyToDo.Extensions
{
    public class PasswordExtension 
    {
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for PassWord.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordExtension),
                new FrameworkPropertyMetadata(string.Empty, OnProppertyPasswordChanged));

        private static void OnProppertyPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            string password = (string)e.NewValue;
            if (passwordBox != null && passwordBox.Password != password)
            {
                passwordBox.Password = password;
            }
        }
    }

    public class PasswordBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += AssociatedObjectOnPasswordChanged;
        }

        private void AssociatedObjectOnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            string password = PasswordExtension.GetPassword(passwordBox);

            if (passwordBox != null && password != passwordBox.Password)
                PasswordExtension.SetPassword(passwordBox, passwordBox.Password);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= AssociatedObjectOnPasswordChanged;
        }
    }

}
