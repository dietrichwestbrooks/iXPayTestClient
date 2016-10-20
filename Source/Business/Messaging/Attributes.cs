using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TerminalDeviceAttribute : ExportAttribute
    {
        public TerminalDeviceAttribute()
            : base(typeof(ITerminalDevice))
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TerminalRequestHandlerAttribute : ExportAttribute
    {
        public TerminalRequestHandlerAttribute()
            : base(typeof(ITerminalRequestHandler))
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ValuePropertyAttribute : Attribute
    {
        public string PropertyName { get; }

        public ValuePropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }

    //[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    //public class DefaultParameterValueAttribute : DefaultValueAttribute
    //{
    //    public string PropertyName { get; }

    //    public DefaultParameterValueAttribute(string propertyName)
    //        : base(null)
    //    {
    //        PropertyName = propertyName;
    //    }

    //    public DefaultParameterValueAttribute(string propertyName, Type type, string value)
    //        : base(type, value)
    //    {
    //        PropertyName = propertyName;
    //    }

    //    public DefaultParameterValueAttribute(string propertyName, char value)
    //        : base(value)
    //    {
    //        PropertyName = propertyName;
    //    }

    //    public DefaultParameterValueAttribute(string propertyName, byte value)
    //        : base(value)
    //    {
    //        PropertyName = propertyName;
    //    }

    //    public DefaultParameterValueAttribute(string propertyName, short value)
    //        : base(value)
    //    {
    //        PropertyName = propertyName;
    //    }

    //    public DefaultParameterValueAttribute(string propertyName, int value)
    //        : base(value)
    //    {
    //        PropertyName = propertyName;
    //    }

    //    public DefaultParameterValueAttribute(string propertyName, long value)
    //        : base(value)
    //    {
    //        PropertyName = propertyName;
    //    }

    //    public DefaultParameterValueAttribute(string propertyName, float value)
    //        : base(value)
    //    {
    //        PropertyName = propertyName;
    //    }

    //    public DefaultParameterValueAttribute(string propertyName, double value)
    //        : base(value)
    //    {
    //        PropertyName = propertyName;
    //    }

    //    public DefaultParameterValueAttribute(string propertyName, bool value)
    //        : base(value)
    //    {
    //        PropertyName = propertyName;
    //    }

    //    public DefaultParameterValueAttribute(string propertyName, string value)
    //        : base(value)
    //    {
    //        PropertyName = propertyName;
    //    }

    //    public DefaultParameterValueAttribute(string propertyName, object value)
    //        : base(value)
    //    {
    //        PropertyName = propertyName;
    //    }

    //    public new void SetValue(object value)
    //    {
    //        base.SetValue(value);
    //    }
    //}
}
