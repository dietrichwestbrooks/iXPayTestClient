using System.Windows.Markup;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Converters
{
    /// <summary>
    /// Represents a case for a switch converter.
    /// </summary>
    [ContentProperty("Then")]
    public class SwitchConverterCase
    {
        // case instances.

        #region Public Properties.
        /// <summary>
        /// Gets or sets the condition of the case.
        /// </summary>
        public string When { get; set; }

        /// <summary>
        /// Gets or sets the results of this case when run through a <see cref="SwitchConverter"/>
        /// </summary>
        public object Then { get; set; }

        #endregion
        #region Construction.
        /// <summary>
        /// Switches the converter.
        /// </summary>
        public SwitchConverterCase()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchConverterCase"/> class.
        /// </summary>
        /// <param name="when">The condition of the case.</param>
        /// <param name="then">The results of this case when run through a <see cref="SwitchConverter"/>.</param>
        public SwitchConverterCase(string when, object then)
        {
            // Hook up the instances.
            Then = then;
            When = when;
        }
        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"When={When}; Then={Then}";
        }
    }
}
