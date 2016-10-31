clr.AddReference('iXPayTestClient.Business.Messaging')
from Wayne.Payment.Tools.iXPayTestClient.Business.Messaging import TerminalDevice, TerminalDeviceMethod, TerminalDeviceProperty, TerminalDeviceCommand

# define Beep method command
class BeeperBeepMethod(TerminalDeviceMethod[BeepCommand, BeepResponse]):
	def __new__(cls, device):
		return TerminalDeviceMethod.__new__(cls, device, "Beep")
	def __init__(self, device):
		self.InvokeCommand = TerminalDeviceCommand[BeepCommand, BeepResponse](self, self.Name)
		pass

#define Beeper device
class Beeper(TerminalDevice[BeeperCommand, BeeperResponse]):
	def __new__(cls):
		return TerminalDevice[BeeperCommand, BeeperResponse].__new__(cls, "pyBeeper")
	def __init__(self):
		self.Methods.Add(BeeperBeepMethod(self))
		pass
	#def HandleRequest(self, command):
	#	return DeviceObject.HandleRequest(self, command)
	#def HandleResponse(self, response):
	#	return DeviceObject.HandleResponse(self, response)
	#def get_SuccessorType(self):
	#	return DeviceObject.SuccessorType.__get__(self)

def CreateDevice():
	return Beeper()
