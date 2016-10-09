/*----------------------------------------------------------------
//  File:                         MyMccDaq.cs

//  Author:                       HuPenbo
//  Date:                         9-21 2016
//----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalIO;
using MccDaq;
using ErrorDefs;
using AnalogIO;
using System.Diagnostics;
using PID_WinForm;
using System.Windows.Forms;

namespace NewPortTest
{
    class MyMccDaq
    {
        private MccDaq.MccBoard[] DaqBoard;

        /// <summary>
        /// 自定义Board个数，并初始化
        /// </summary>
        /// <param name="NumOfBoard">初始化的个数</param>
        public MyMccDaq(int NumOfBoard)
        {
            for (int i = 0; i != NumOfBoard; i++)
            {
                DaqBoard[i] = new MccDaq.MccBoard();
            }
        }

        /// <summary>
        /// 检测并初始化board
        /// </summary>
        public MyMccDaq()
        {
            MccDaq.DaqDeviceManager.IgnoreInstaCal();
            InitUL();

            

    

            // Discover DAQ devices with GetDaqDeviceInventory()
            //  Parameters:
            //    InterfaceType   :interface type of DAQ devices to be discovered

            MccDaq.DaqDeviceDescriptor[] inventory = MccDaq.DaqDeviceManager.GetDaqDeviceInventory(MccDaq.DaqDeviceInterface.Any);

            int numDevDiscovered = inventory.Length;


            //检测到的个数
            //lblStatus.Text = numDevDiscovered + " DAQ Device(s) Discovered";

            if (numDevDiscovered > 0)
            {

                try
                {
                    //    Create a new MccBoard object for Board and assign a board number 
                    //    to the specified DAQ device with CreateDaqDevice()

                    //    Parameters:
                    //        BoardNum			: board number to be assigned to the specified DAQ device
                    //        DeviceDescriptor	: device descriptor of the DAQ device 
                    for (int i = 0; i != numDevDiscovered; i++)
                    {
                        DaqBoard[i] = MccDaq.DaqDeviceManager.CreateDaqDevice(i, inventory[i]);
                    }
                 
                    //this.BoardDec.Text = "Board1_ID:" + DaqBoard1.Descriptor.UniqueID.ToString() + "   Board2_ID:" + DaqBoard2.Descriptor.UniqueID.ToString();

                    // Add the board to combobox

                }
                catch (ULException ule)
                {
                    //lblStatus.Text = "Error occured: " + ule.Message;
                    MessageBox.Show(ule.Message);
                }
            }






            
        }



        /// <summary>
        /// 设备初始化
        /// </summary>
        public void InitialMccDaq()
        {
            InitUL();
        }







        private void InitUL()
        {

            MccDaq.ErrorInfo ULStat;

            //  Initiate error handling
            //   activating error handling will trap errors like
            //   bad channel numbers and non-configured conditions.
            //   Parameters:
            //     MccDaq.ErrorReporting.PrintAll :all warnings and errors encountered will be printed
            //     MccDaq.ErrorHandling.StopAll   :if an error is encountered, the program will stop

            clsErrorDefs.ReportError = MccDaq.ErrorReporting.PrintAll;
            clsErrorDefs.HandleError = MccDaq.ErrorHandling.StopAll;
            ULStat = MccDaq.MccService.ErrHandling
                (ErrorReporting.PrintAll, ErrorHandling.StopAll);

        }



    }
}
