using System;
using System.Windows;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Behaviors
{
    public interface IWindow
    {
        /// <summary>
        /// Ocurrs when the <see cref="IWindow"/> is closed.
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// Gets or sets the content for the <see cref="IWindow"/>.
        /// </summary>
        object Content { get; set; }

        /// <summary>
        /// Gets or sets the owner control of the <see cref="IWindow"/>.
        /// </summary>
        object Owner { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Windows.Style"/> to apply to the <see cref="IWindow"/>.
        /// </summary>
        Style Style { get; set; }

        /// <summary>
        /// Opens the <see cref="IWindow"/>.
        /// </summary>
        void Show();

        /// <summary>
        /// Closes the <see cref="IWindow"/>.
        /// </summary>
        void Close();
    }
}
