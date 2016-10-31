﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Prism.Logging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Services;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Services
{
    [Export(typeof(IScriptService))]
    public class ScriptService : ServiceBase, IScriptService
    {
        private ScriptEngine _py;
        private dynamic _pyScope;
        private ScriptHost _scriptHost = new ScriptHost();
        private readonly Dictionary<string, object> _globalVariables = new Dictionary<string, object>();

        public ScriptService()
        {
            _py = Python.CreateEngine();

            var ms = new MemoryStream();

            var outputWriter = new ScriptOuputWriter(ms);

            outputWriter.Capture += (sender, args) =>
            {
                _scriptHost.Write($"{args.Text}");
            };

            _py.Runtime.IO.SetOutput(ms, outputWriter);
            _py.Runtime.IO.SetErrorOutput(ms, outputWriter);

            CreateScope();
        }

        public void ExecuteScript(string code)
        {
            try
            {
                ScriptSource source = _py.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
                Task.Factory.StartNew(() => OnExecuteScript(source)).GetAwaiter().OnCompleted(OnScriptCompleted);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Category.Exception, Priority.Medium);
            }
        }

        public void ExecuteScriptFile(string path)
        {
            try
            {
                ScriptSource source = _py.CreateScriptSourceFromFile(path);
                source.Execute(_pyScope);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Category.Exception, Priority.Medium);
            }
            finally
            {
                CreateScope();
            }
        }

        public ITerminalDevice CreateDeviceFromFile(string path)
        {
            ITerminalDevice device = null;

            try
            {
                ScriptSource source = _py.CreateScriptSourceFromFile(path);
                source.Execute(_pyScope);
                device = _pyScope.CreateDevice();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Category.Exception, Priority.Medium);
            }
            finally
            {
                CreateScope();
            }

            return device;
        }

        public TFunc GetFunction<TFunc>(object function)
            where TFunc : class
        {
            if (!typeof(TFunc).IsSubclassOf(typeof(Delegate)))
                throw new ArgumentException(typeof(TFunc).Name + " is not a delegate type");

            var pyFunction = function as PythonFunction;

            if (pyFunction == null)
                return default(TFunc);

            return _pyScope.GetVariable(pyFunction.func_name);
        }

        public void SetVariable(string name, object value, bool global = false)
        {
            _pyScope.SetVariable(name, value);

            if (global)
                _globalVariables.Add(name, value);
        }

        public T GetVariable<T>(string name)
        {
            return _pyScope.GetVariable<T>(name);
        }

        public void RemoveVariable(string name)
        {
            _pyScope.RemoveVariable(name);
            _globalVariables.Remove(name);
        }

        public void Write(string message)
        {
            _scriptHost.Write(message);
        }

        public void WriteIf(bool condition, string message)
        {
            _scriptHost.WriteIf(condition, message);
        }

        public void WriteLine(string message)
        {
            _scriptHost.WriteLine(message);
        }

        public void WriteLineIf(bool condition, string message)
        {
            _scriptHost.WriteLineIf(condition, message);
        }

        private void CreateScope()
        {
            try
            {
                _pyScope = _py.CreateScope();

                string references =
    @"
import clr
clr.AddReference('IronPython')
clr.AddReference('IronPython.Modules')
clr.AddReference('iXPayTestClient.Business.TerminalCommands')
clr.AddReference('Microsoft.Scripting.Metadata')
clr.AddReference('Microsoft.Scripting')
clr.AddReference('Microsoft.Dynamic')
clr.AddReference('mscorlib')
clr.AddReference('System')
clr.AddReference('System.Data')
from Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands import *
";

                ScriptSource source = _py.CreateScriptSourceFromString(references, SourceCodeKind.Statements);
                source.Execute(_pyScope);

                RegisterVariables();

                EventAggregator.GetEvent<SetupScriptEvent>().Publish(this);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Category.Exception, Priority.Medium);
            }
        }

        private void RegisterVariables()
        {
            SetVariable("_h", _scriptHost);
            SetVariable("ScriptHost", _scriptHost);

            foreach (var variable in _globalVariables)
            {
                SetVariable(variable.Key, variable.Value);
            }
        }

        private void OnExecuteScript(ScriptSource source)
        {
            try
            {
                source.Execute(_pyScope);
            }
            catch (Exception ex)
            {
                EventAggregator.GetEvent<ScriptOutputTextEvent>().Publish($"{ex.Message}\n");
                Logger.Log(ex.Message, Category.Exception, Priority.Medium);
            }
        }

        private void OnScriptCompleted()
        {
            EventAggregator.GetEvent<TeardownScriptEvent>().Publish(this);

            CreateScope();
        }
    }
}
