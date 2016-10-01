using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class Display : Device
    {
        public Display()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetDisplayFontsCommand", typeof (GetDisplayFontsCommand)),
                    new DeviceCommand("GetStatusCommand", typeof (GetStatusCommand)),
                    new DeviceCommand("GetSupportedLanguagesCommand", typeof (GetSupportedLanguagesCommand)),
                    new DeviceCommand("GetCurrentLanguageCommand", typeof (GetCurrentLanguageCommand)),
                    new DeviceCommand("DisplayPromptCommand", typeof (DisplayPromptCommand)),
                    new DeviceCommand("DisplayPromptImmediateCommand", typeof (DisplayPromptImmediateCommand)),
                    new DeviceCommand("SetCurrentLanguageCommand", typeof (SetCurrentLanguageCommand)),
                    new DeviceCommand("GetMessageWindowPropertiesCommand", typeof (GetMessageWindowPropertiesCommand)),
                    new DeviceCommand("SetMessageWindowPropertiesCommand", typeof (SetMessageWindowPropertiesCommand)),
                    new DeviceCommand("GetDataEntryWindowPropertiesCommand", typeof (GetDataEntryWindowPropertiesCommand)),
                    new DeviceCommand("SetDataEntryWindowPropertiesCommand", typeof (SetDataEntryWindowPropertiesCommand)),
                    new DeviceCommand("GetSoftKeyPropertiesCommand", typeof (GetSoftKeyPropertiesCommand)),
                    new DeviceCommand("SetSoftKeyPropertiesCommand", typeof (SetSoftKeyPropertiesCommand)),
                    new DeviceCommand("GetDisplayTypeCommand", typeof (GetDisplayTypeCommand)),
                    new DeviceCommand("GetImageListCommand", typeof (GetImageListCommand)),
                    new DeviceCommand("GetImageDetailsCommand", typeof (GetImageDetailsCommand)),
                    new DeviceCommand("SetImageDetailsCommand", typeof (SetImageDetailsCommand)),
                    new DeviceCommand("GetAnimationListCommand", typeof (GetAnimationListCommand)),
                    new DeviceCommand("GetAnimationDetailsCommand", typeof (GetAnimationDetailsCommand)),
                    new DeviceCommand("SetAnimationDetailsCommand", typeof (SetAnimationDetailsCommand)),
                    new DeviceCommand("GetPromptListCommand", typeof (GetPromptListCommand)),
                    new DeviceCommand("GetPromptListCommand", typeof (GetPromptListCommand)),
                    new DeviceCommand("SetPromptDetailsCommand", typeof (SetPromptDetailsCommand)),
                    new DeviceCommand("DeletePromptCommand", typeof (DeletePromptCommand)),
                    new DeviceCommand("DeleteAnimationCommand", typeof (DeleteAnimationCommand)),
                    new DeviceCommand("DeleteImageCommand", typeof (DeleteImageCommand)),
                    new DeviceCommand("DeleteAllPromptsCommand", typeof (DeleteAllPromptsCommand)),
                    new DeviceCommand("DeleteAllAnimationsCommand", typeof (DeleteAllAnimationsCommand)),
                    new DeviceCommand("DeleteAllImagesCommand", typeof (DeleteAllImagesCommand)),
                    new DeviceCommand("DeleteAllCommand", typeof (DeleteAllCommand)),
                    new DeviceCommand("RestoreDefaultCommand", typeof (RestoreDefaultCommand)),
                };
        }

        public override string Name { get; } = "Display";

        public override Type CommandType { get; } = typeof(DisplayCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
