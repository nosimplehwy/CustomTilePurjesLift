using Crestron.RAD.Common;
using Crestron.RAD.Common.Attributes.Programming;
using Crestron.RAD.Common.BasicDriver;
using Crestron.RAD.Common.Enums;
using Crestron.RAD.Common.Interfaces;
using Crestron.RAD.Common.Interfaces.ExtensionDevice;
using Crestron.RAD.Common.Logging;
using Crestron.RAD.DeviceTypes.ExtensionDevice;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;


namespace CustomTilePurjesLift
{
    public class CustomTile : AExtensionDevice, ICloudConnected
    {
        #region Constants


        //UI Definition
        private const string MainIconKey = "MainIcon";
        private const string MainPageTitleKey = "MainPageTitle";
        private const string Button1Key = "Button1Text";
        private const string Button2Key = "Button2Text";
        private const string Button3Key = "Button3Text";
        private const string Button4Key = "Button4Text";

        #endregion Constants

        #region Fields

        private string _deviceState;

        private PropertyValue<string> _mainPageTitle;
        private PropertyValue<string> _tileIcon;
        private PropertyValue<string> _button1Text;
        private PropertyValue<string> _button2Text;
        private PropertyValue<string> _button3Text;
        private PropertyValue<string> _button4Text;

        private CustomTileProtocol _protocol;


        #endregion Fields

        #region Constructor
        public CustomTile()
        {
            CreateDeviceDefinition();
        }

        #endregion Constructor

        #region AExtensionDevice Members

        protected override IOperationResult DoCommand(string command, string[] parameters)
        {

            CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "DoCommand", command);

            if (string.IsNullOrEmpty(command))
                return new OperationResult(OperationResultCode.Error, "command string is empty");

            switch (command)
            {
                case "Button1Command":
                    {
                        CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "Switch", command);
                        OnButton1Command();
                        break;
                    }
                case "Button2Command":
                {
                    CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "Switch", command);
                    OnButton2Command();
                    break;
                }
                case "Button3Command":
                {
                    CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "Switch", command);
                    OnButton3Command();
                    break;
                }
                case "Button4Command":
                {
                    CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "Switch", command);
                    OnButton4Command();
                    break;
                }
                default:
                    {
                        CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "Switch", "Unhandled command!");
                        break;
                    }
            }

            Commit();
            return new OperationResult(OperationResultCode.Success);
        }

        protected override IOperationResult SetDriverPropertyValue<T>(string propertyKey, T value)
        {
            CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "SetDriverPropertyValue", propertyKey);
            return new OperationResult(OperationResultCode.Error, "The property with object does not exist.");

        }

        protected override IOperationResult SetDriverPropertyValue<T>(string objectId, string propertyKey, T value)
        {
            CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "SetDriverPropertyValueWithObject", propertyKey);
            return new OperationResult(OperationResultCode.Error, "The property with object does not exist.");
        }

        #endregion AExtensionDevice Members

        #region ICloudConnected Members

        public void Initialize()
        {
            CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "Initialize", "CustomTile");

            var transport = new CustomTileTransport
            {
                EnableLogging = InternalEnableLogging,
                CustomLogger = InternalCustomLogger,
                EnableRxDebug = InternalEnableRxDebug,
                EnableTxDebug = InternalEnableTxDebug
            };
            ConnectionTransport = transport;

            _protocol = new CustomTileProtocol(transport, Id)
            {
                EnableLogging = InternalEnableLogging,
                CustomLogger = InternalCustomLogger
            };

            _protocol.Initialize(DriverData);
            DeviceProtocol = _protocol;

        }


        #endregion ICloudConnected Members

        #region IConnection Members

        public override void Connect()
        {
            CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "Connect", "Connect");

            Connected = true;

        }


        #endregion ICloudConnected Members

        #region Programmable Operations


        #endregion Programmable Operations

        #region Programmable Events


        [ProgrammableEvent("Lift Down")]
        public event EventHandler Button1;

        [ProgrammableEvent("Lift Bed")]
        public event EventHandler Button2;

        [ProgrammableEvent("Lift Window")]
        public event EventHandler Button3;

        [ProgrammableEvent("Lift Sitting")]
        public event EventHandler Button4;


        #endregion Programmable Events

        #region Private Methods
        private void CreateDeviceDefinition()
        {
            CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "CreateDeviceDefinition", "");

            //Tile
            _tileIcon = CreateProperty<string>(new PropertyDefinition(MainIconKey, String.Empty, DevicePropertyType.String));

            //Main Page
            _mainPageTitle = CreateProperty<string>(new PropertyDefinition(MainPageTitleKey, String.Empty, DevicePropertyType.String));
            _button1Text = CreateProperty<string>(new PropertyDefinition(Button1Key, String.Empty, DevicePropertyType.String));
            _button2Text = CreateProperty<string>(new PropertyDefinition(Button2Key, String.Empty, DevicePropertyType.String));
            _button3Text = CreateProperty<string>(new PropertyDefinition(Button3Key, String.Empty, DevicePropertyType.String));
            _button4Text = CreateProperty<string>(new PropertyDefinition(Button4Key, String.Empty, DevicePropertyType.String));


            
            //Initialize property values
            _tileIcon.Value = "icGenericDeviceOn";
            _mainPageTitle.Value = "TV Lift";
            _button1Text.Value = "Down";
            _button2Text.Value = "Bed";
            _button3Text.Value = "Window";
            _button4Text.Value = "Sitting";

            Commit();

        }


        #endregion Private Methods

        #region Helper Methods

        private void OnButton1Command()
        {
            CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "Button1Event", "Button 1 Pressed");
           Button1?.Invoke(this, EventArgs.Empty);
        }

        private void OnButton2Command()
        {
            CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "Button2Event", "Button 2 Pressed");
            Button2?.Invoke(this, EventArgs.Empty);
        }

        private void OnButton3Command()
        {
            CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "Button3Event", "Button 3 Pressed");
            Button3?.Invoke(this, EventArgs.Empty);
        }

        private void OnButton4Command()
        {
            CustomTileLog.Log(EnableLogging, Log, LoggingLevel.Debug, "Button4Event", "Button 4 Pressed");
            Button4?.Invoke(this, EventArgs.Empty);
        }


        #endregion

        #region Events


        #endregion Events
    }
}
