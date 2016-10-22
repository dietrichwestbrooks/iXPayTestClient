﻿using System;
using System.Windows;
using MahApps.Metro.Controls;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Behaviors
{
    public class WindowWrapper : IWindow
    {
        private readonly MetroWindow _window;

        /// <summary>
        /// Initializes a new instance of <see cref="WindowWrapper"/>.
        /// </summary>
        public WindowWrapper()
        {
            _window = new MetroWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ResizeMode = ResizeMode.NoResize,
                    ShowMinButton = false,
                    ShowMaxRestoreButton = false,
                };
        }

        /// <summary>
        /// Ocurrs when the <see cref="Window"/> is closed.
        /// </summary>
        public event EventHandler Closed
        {
            add { _window.Closed += value; }
            remove { _window.Closed -= value; }
        }

        /// <summary>
        /// Gets or Sets the content for the <see cref="Window"/>.
        /// </summary>
        public object Content
        {
            get { return _window.Content; }
            set { _window.Content = value; }
        }

        /// <summary>
        /// Gets or Sets the <see cref="Window.Owner"/> control of the <see cref="Window"/>.
        /// </summary>
        public object Owner
        {
            get { return _window.Owner; }
            set { _window.Owner = value as Window; }
        }

        /// <summary>
        /// Gets or Sets the <see cref="FrameworkElement.Style"/> to apply to the <see cref="Window"/>.
        /// </summary>
        public Style Style
        {
            get { return _window.Style; }
            set { _window.Style = value; }
        }

        /// <summary>
        /// Opens the <see cref="Window"/>.
        /// </summary>
        public void Show()
        {
            _window.Show();
        }

        /// <summary>
        /// Closes the <see cref="Window"/>.
        /// </summary>
        public void Close()
        {
            _window.Close();
        }
    }
}
