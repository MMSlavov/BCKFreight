namespace BCKFreightTMS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    public class WIAScanner
    {
        private const string WiaFormatJPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";

        private class WIA_DPS_DOCUMENT_HANDLING_SELECT
        {
            public const uint FEEDER = 0x00000001;
            public const uint FLATBED = 0x00000002;
        }

        private class WIA_DPS_DOCUMENT_HANDLING_STATUS
        {
            public const uint FEEDREADY = 0x00000001;
        }

        private class WIA_PROPERTIES
        {
            public const uint WIARESERVEDFORNEWPROPS = 1024;
            public const uint WIADIPFIRST = 2;
            public const uint WIADPAFIRST = WIADIPFIRST + WIARESERVEDFORNEWPROPS;
            public const uint WIADPCFIRST = WIADPAFIRST + WIARESERVEDFORNEWPROPS;
            public const uint WIADPSFIRST = WIADPCFIRST + WIARESERVEDFORNEWPROPS;
            public const uint WIADPSDOCUMENTHANDLINGSTATUS = WIADPSFIRST + 13;
            public const uint WIADPSDOCUMENTHANDLINGSELECT = WIADPSFIRST + 14;
        }

        /// <summary>
        /// Use scanner to scan an image (with user selecting the scanner from a dialog).
        /// </summary>
        /// <returns>Scanned images.</returns>
        public static List<Image> Scan()
        {
            WIA.ICommonDialog dialog = new WIA.CommonDialog();
            WIA.Device device = dialog.ShowSelectDevice(
                WIA.WiaDeviceType.UnspecifiedDeviceType, true, false);
            if (device != null)
            {
                return Scan(device.DeviceID);
            }
            else
            {
                throw new Exception("You must select a device for scanning.");
            }
        }

        /// <summary>
        /// Use scanner to scan an image (scanner is selected by its unique id).
        /// </summary>
        /// <param name="scannerName"></param>
        /// <returns>Scanned images.</returns>
        public static List<Image> Scan(string scannerId)
        {
            List<Image> images = new List<Image>();
            bool hasMorePages = true;
            while (hasMorePages)
            {
                // select the correct scanner using the provided scannerId parameter
                WIA.DeviceManager manager = new WIA.DeviceManager();
                WIA.Device device = null;
                foreach (WIA.DeviceInfo info in manager.DeviceInfos)
                {
                    if (info.DeviceID == scannerId)
                    {
                        // connect to scanner
                        device = info.Connect();
                        break;
                    }
                }

                // device was not found
                if (device == null)
                {
                    // enumerate available devices
                    string availableDevices = string.Empty;
                    foreach (WIA.DeviceInfo info in manager.DeviceInfos)
                    {
                        availableDevices += info.DeviceID + "\n";
                    }

                    // show error with available devices
                    throw new Exception("The device with provided ID could not be found. Available Devices:\n" + availableDevices);
                }

                WIA.Item item = device.Items[1] as WIA.Item;
                try
                {
                    // scan image
                    WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
                    WIA.ImageFile image = (WIA.ImageFile)wiaCommonDialog.ShowAcquireImage();

                    // save to temp file
                    string fileName = Path.GetTempFileName();
                    File.Delete(fileName);
                    image.SaveFile(fileName);
                    image = null;

                    // add file to output list
                    images.Add(Image.FromFile(fileName));
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(ex.Message);
                }
                finally
                {
                    item = null;

                    // determine if there are any more pages waiting
                    WIA.Property documentHandlingSelect = null;
                    WIA.Property documentHandlingStatus = null;
                    foreach (WIA.Property prop in device.Properties)
                    {
                        if (prop.PropertyID == WIA_PROPERTIES.WIADPSDOCUMENTHANDLINGSELECT)
                        {
                            documentHandlingSelect = prop;
                        }

                        if (prop.PropertyID == WIA_PROPERTIES.WIADPSDOCUMENTHANDLINGSTATUS)
                        {
                            documentHandlingStatus = prop;
                        }
                    }

                    // assume there are no more pages
                    hasMorePages = false;

                    // may not exist on flatbed scanner but required for feeder
                    if (documentHandlingSelect != null)
                    {
                        // check for document feeder
                        if ((Convert.ToUInt32(documentHandlingSelect.get_Value()) &
                        WIA_DPS_DOCUMENT_HANDLING_SELECT.FEEDER) != 0)
                        {
                            hasMorePages = (Convert.ToUInt32(documentHandlingStatus.get_Value()) &
                            WIA_DPS_DOCUMENT_HANDLING_STATUS.FEEDREADY) != 0;
                        }
                    }
                }
            }

            return images;
        }

        /// <summary>
        /// Gets the list of available WIA devices.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDevices()
        {
            List<string> devices = new List<string>();
            WIA.DeviceManager manager = new WIA.DeviceManager();
            foreach (WIA.DeviceInfo info in manager.DeviceInfos)
            {
                devices.Add(info.DeviceID);
            }

            return devices;
        }
    }
}
