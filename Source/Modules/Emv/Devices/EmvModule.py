import clr
from Wayne.Payment.Tools.iXPayTestClient.Business.Messaging import *

# define SetTimeout method command
class SetTimeoutMethod(MethodCommand):
	def __new__(cls, successor):
		instance = MethodCommand.__new__(cls, "SetTimeout")
		return instance
	def __init__(self, successor):
		self.Name = "SetTimeout"
		self.CommandType = clr.GetClrType(SetTimeoutCommand)
		self.ResponseType = clr.GetClrType(SetTimeoutResponse)
		self.CreateCommand = self.InternalCreateCommand
		self.Successor = successor
	def InternalCreateCommand(self, p):
		command = SetTimeoutCommand()
		command.TimeoutValue = p.GetValue("TimeoutValue", 0, 5)
		return command

#define EMVModuleStatus property command
class EmvModuleStatusProperty(PropertyCommand):
	def __new__(cls, successor):
		instance = PropertyCommand.__new__(cls, "Status")
		return instance
	def __init__(self, successor):
		self.Name = "EmvModuleStatus"
		self.GetCommandType = clr.GetClrType(GetEMVModuleStatusCommand)
		self.GetResponseType = clr.GetClrType(GetEMVModuleStatusResponse)
		self.CreateGetCommand = self.InternalCreateGetCommand
		#self.ProcessGetResponse = self.InternalProcessGetResponse
		self.Successor = successor
	def InternalCreateGetCommand(self):
		command = GetEMVModuleStatusCommand()
		return command
	#def override(method):
	#	def inner(*args):
	#		base = super(PropertyCommand, args[0])
	#		base_method = getattr(base.__thisclass__, MethodCommand.__name__)
	#		method(*args)
	#		return base_method(*args)
	#	return inner
	#@override
	def ProcessGetResponse(self, response):
		if not super(EmvModuleStatusProperty, self).ProcessGetResponse(response):
			return false
		Value = response.EMVModuleState
		return true


#define EMVModule device
class Device(TerminalDevice):
	def __new__(cls):
		instance = TerminalDevice.__new__(cls)
		return instance
	def __init__(self):
		self.Name = "EmvModule"
		self.CommandType = clr.GetClrType(EMVModuleCommand)
		self.ResponseType = clr.GetClrType(EMVModuleResponse)
		self.Successor = Terminal.Devices["Terminal"]
		self.Methods.Add(SetTimeoutMethod(self))
		self.Properties.Add(EmvModuleStatusProperty(self))
	#def HandleRequest(self, command):
	#	return DeviceObject.HandleRequest(self, command)
	#def HandleResponse(self, response):
	#	return DeviceObject.HandleResponse(self, response)
	#def get_SuccessorType(self):
	#	return DeviceObject.SuccessorType.__get__(self)
