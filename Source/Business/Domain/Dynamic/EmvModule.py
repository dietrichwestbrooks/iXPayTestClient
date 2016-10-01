from IronPython import *
from IronPython.Modules import *
from Wayne.Payment.Tools.iXPayTestClient.Business.Domain import *

# define SetTimeout method command
class SetTimeout(MethodCommand):
	def __new__(cls, st):
		instance = MethodCommand.__new__(cls)
		return instance
	def __init__(self, st):
		self.Name = "SetTimeout"
		self.CommandType = clr.GetClrType(SetTimeoutCommand)
		self.ResponseType = clr.GetClrType(SetTimeoutResponse)
		self.CreateCommand = self.InternalCreateCommand
		self.SuccessorType = st
	def InternalCreateCommand(self, p):
		command = SetTimeoutCommand()
		command.TimeoutValue = p.GetValue("TimeoutValue", 0, 5)
		return command

#define EMVModuleStatus property command
class EmvModuleStatus(PropertyCommand):
	def __new__(cls, st):
		instance = PropertyCommand.__new__(cls)
		return instance
	def __init__(self, st):
		self.Name = "EmvModuleStatus"
		self.GetCommandType = clr.GetClrType(GetEMVModuleStatusCommand)
		self.GetResponseType = clr.GetClrType(GetEMVModuleStatusResponse)
		self.CreateGetCommand = self.InternalCreateGetCommand
		self.ProcessGetResponse = self.InternalProcessGetResponse
		self.SuccessorType = st
	def InternalCreateGetCommand(self):
		command = GetEMVModuleStatusCommand()
		return command
	def InternalProcessGetResponse(self, response):
		return response.EMVModuleState

#define EMVModule device
class EmvModule(DeviceObject, ITerminalRequestHandler):
	def __new__(cls):
		instance = DeviceObject.__new__(cls)
		return instance
	def __init__(self):
		self.Name = "EmvModule"
		self.CommandType = clr.GetClrType(EMVModuleCommand)
		self.ResponseType = clr.GetClrType(EMVModuleResponse)
		self.SuccessorType = Terminal.Devices["Terminal"].GetType()
		self.Methods.Add(SetTimeout(type(self)))
		self.Properties.Add(EmvModuleStatus(type(self)))
	def HandleRequest(self, command):
		return DeviceObject.HandleRequest(self, command)
	def HandleResponse(self, response):
		return DeviceObject.HandleResponse(self, response)
	def get_SuccessorType(self):
		return DeviceObject.SuccessorType.__get__(self)

def CreateDevice():
	return EmvModule()
