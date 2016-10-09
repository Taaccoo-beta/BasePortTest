using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DigitalIO;
using MccDaq;
using ErrorDefs;
using AnalogIO;
using System.Diagnostics;
using PID_WinForm;



namespace NewPortTest
{
    public partial class Form1 : Form
    {

        MccDaq.ErrorInfo ULStat;


        MccDaq.MccBoard DaqBoard1;
        MccDaq.MccBoard DaqBoard2;


        System.UInt16 DataValue;
        System.UInt32 DataValue32;


        MccDaq.DigitalPortType PortNum1,PortNum2;
        MccDaq.Range Range1=Range.Bip10Volts;
        MccDaq.Range Range2=Range.Bip10Volts;

        bool isFirstPor = true;
        int circle;
        bool startPID_1;
        bool startPID_2;
        bool startPID_3;
        bool startPID_4;

        int PID_Count_1;
        int PID_Count_2;
        int PID_Count_3;
        int PID_Count_4;

        bool testPortControl = true;

        int FirstPropotation_1;
        int FirstPropotation_2;
        int FirstPropotation_3;
        int FirstPropotation_4;


        double Timespan_1;
        double Timespan_2;
        double Timespan_3;
        double Timespan_4;

        bool isStartPID_1;
        bool isStartPID_2;
        bool isStartPID_3;
        bool isStartPID_4;
        int count = 0;
        //比例参数

        int propotation_1;
        int propotation_2;
        int propotation_3;
        int propotation_4;


        bool isUp_1;
        bool isUp_2;
        bool isUp_3;
        bool isUp_4;
        bool isDown_1;
        bool isDown_2;
        bool isDown_3;
        bool isDown_4;


        PIDControl PID_1;
        PIDControl PID_2;
        PIDControl PID_3;
        PIDControl PID_4;

        double punishMentTemp;
        double confortableTemp;

        //the port must be AuxPort or FirstPortA for bit output
        //BitPort = MccDaq.DigitalPortType.AuxPort;
      
        private int HighChan, NumAIChans1,NumAIChans2;

        private int ADResolution1;
        private int ADResolution2;

        int n1 = 0;

        int fileLineIndex = 0;

        double result_1;
        double result_2;
        double result_3;
        double result_4;

        bool abc = true;

        bool btnSimInput_1 = true;
        bool btnSimInput_2 = true;
        bool btnSimInput_3 = true;
        bool btnSimInput_4 = true;



        //测试变量
        bool isChangeParam = false;
        double P = 2.0;
        double I = 0.0;
        double D = 0.0;
        int timerCount = 0;
        int beyondNum = 0;
        int downTime;
        bool TimerNotAccept = false;

        bool isRecsiveSingal_1 = false;
        bool isFirstChange_11 = true;
        bool isFirstChange_12 = true;

        bool highBalance = false;
        bool lowBalance = false;

        bool downLine = false;
        bool upLine = false;

        bool longKeep = false;
        List<double> tempCollection = new List<double>();




        private double p1_1;
        private double p2_1;
        private double p1_2;
        private double p2_2;
        private double p1_3;
        private double p2_3;
        private double p1_4;
        private double p2_4;

        System.UInt16 DataValue_1 = 0;
        System.UInt32 DataValue32_1 = 0;

        System.UInt16 DataValue_2 = 0;
        System.UInt32 DataValue32_2 = 0;
        System.UInt16 DataValue_3 = 0;
        System.UInt32 DataValue32_3 = 0;
        System.UInt16 DataValue_4 = 0;
        System.UInt32 DataValue32_4 = 0;

        //获取文本框数值 -------------------------------------------------
        



        string T1;
        string T2;
        string T3;
        string T4;

      
        bool isT1Finish = false;
        bool isT2Finish = false;
        bool isT3Finish = false;
        bool isT4Finish = false;



      

      

           
        int NumPorts1,NumPorts2, NumBits, FirstBit;
        int PortType, ProgAbility;
        int Options = 0;


        AnalogIO.clsAnalogIO AIOProps1 = new AnalogIO.clsAnalogIO();
        AnalogIO.clsAnalogIO AIOProps2 = new AnalogIO.clsAnalogIO();
        DigitalIO.clsDigitalIO DioProps1 = new DigitalIO.clsDigitalIO();

      
        DigitalIO.clsDigitalIO DioProps2 = new DigitalIO.clsDigitalIO();



     


        

       

        // if the temperature up  more than 2times;
       



        // recording the process state;
       



       
        //the port must be AuxPort or FirstPortA for bit output
        //BitPort = MccDaq.DigitalPortType.AuxPort;

        double B1_temperature1;
        double B1_temperature2;
        double B2_temperature1;
        double B2_temperature2;


        int B1_dightOnOFF_1=-1;
        int B1_dightOnOFF_2=-1;
        int B2_dightOnOFF_1=-1;
        int B2_dightOnOFF_2=-1;
       



        public Form1()
        {
            InitializeComponent();
          
        }

        
        private int ConvertAccordToPropotation(int num)
        {
          
            num = num * circle / FirstPropotation_1;
           
            if (num > circle || num < -circle)
            {
                num = circle;
                return num;
            }
            else if (num < 0)
            {
                return -num;
            }
            else return num;
            
        }

       //----------------------------get Analog and convert to a dight siginal;
        private int AnalogConvertDigit(int boardNumber,int portNumber)
        {
            int DataValue32;
            if (boardNumber == 0)
            {
                if (ADResolution1 > 16)
                {
                    ULStat = DaqBoard1.AIn32(portNumber, Range1, out DataValue32, Options);
                    //  Convert raw data to Volts by calling ToEngUnits
                    //  (member function of MccBoard class)
                    if (DataValue32 > 3000)
                        return 1;
                    else if (DataValue32 < 3000)
                        return 0;




                }
                else
                {
                    ULStat = DaqBoard1.AIn(portNumber, Range1, out DataValue);

                    //  Convert raw data to Volts by calling ToEngUnits
                    //  (member function of MccBoard class)
                    if (DataValue > 3000)
                        return 1;
                    else if (DataValue < 3000)
                        return 0;

                    //System.IO.File.AppendAllText("e:\\result.txt", result.ToString("0.00") +"  " +DataValue.ToString() +"  "+ EngUnits.ToString()+ "\r\n");
                }

            }
            else
            {
                if (ADResolution2 > 16)
                {
                    ULStat = DaqBoard2.AIn32(portNumber, Range2, out DataValue32, Options);
                    //  Convert raw data to Volts by calling ToEngUnits
                    //  (member function of MccBoard class)
                    if (DataValue32 > 3000)
                        return 1;
                    else if (DataValue32 < 3000)
                        return 0;




                }
                else
                {
                    ULStat = DaqBoard2.AIn(portNumber, Range2, out DataValue);

                    //  Convert raw data to Volts by calling ToEngUnits
                    //  (member function of MccBoard class)
                    if (DataValue > 3000)
                        return 1;
                    else if (DataValue < 3000)
                        return 0;

                    //System.IO.File.AppendAllText("e:\\result.txt", result.ToString("0.00") +"  " +DataValue.ToString() +"  "+ EngUnits.ToString()+ "\r\n");
                }
            }

            return -1;
            
        }

        //------------------get analog singal and convert to Temperature value;
        private double AnalogConvertTemperature(int boardNumber, int portNumber)
        {

            if (boardNumber == 0)
            {
                if (portNumber == 2)
                {
                    if (ADResolution1 > 16)
                    {
                        ULStat = DaqBoard1.AIn32(portNumber, Range.Bip2Volts, out DataValue32_1, Options);
                        lblVot_1.Text = DataValue32_1.ToString();
                        double result = DataValue32_1 * p1_1 - p2_1;
                        return result;

                    }
                    else
                    {
                        ULStat = DaqBoard1.AIn(portNumber, Range.Bip2Volts, out DataValue_1);

                        lblVot_1.Text = DataValue_1.ToString();
                        double result = DataValue_1 * p1_1 - p2_1;
                        return result;
                    }
                }
                else
                {
                    if (ADResolution1 > 16)
                    {
                        ULStat = DaqBoard1.AIn32(portNumber, Range.Bip2Volts, out DataValue32_2, Options);
                        //  Convert raw data to Volts by calling ToEngUnits
                        //  (member function of MccBoard class)
                        lblVot_2.Text = DataValue32_2.ToString();
                        double result = DataValue32_2 * p1_2 - p2_2;
                        return result;

                    }
                    else
                    {
                        ULStat = DaqBoard1.AIn(portNumber, Range.Bip2Volts, out DataValue_2);

                        //  Convert raw data to Volts by calling ToEngUnits
                        //  (member function of MccBoard class)
                        lblVot_2.Text = DataValue_2.ToString();
                        double result = DataValue_2 * p1_2 - p2_2;
                        return result;

                        //System.IO.File.AppendAllText("e:\\result.txt", result.ToString("0.00") +"  " +DataValue.ToString() +"  "+ EngUnits.ToString()+ "\r\n");
                    }
                }



            }
            else
            {
                if (portNumber == 2)
                {
                    if (ADResolution1 > 16)
                    {
                        ULStat = DaqBoard2.AIn32(portNumber, Range.Bip2Volts, out DataValue32_3, Options);
                        //  Convert raw data to Volts by calling ToEngUnits
                        //  (member function of MccBoard class)
                        lblVot_3.Text = DataValue32_3.ToString();
                        double result = DataValue32_3 * p1_3 - p2_3;
                        return result;

                    }
                    else
                    {
                        ULStat = DaqBoard2.AIn(portNumber, Range.Bip2Volts, out DataValue_3);

                        //  Convert raw data to Volts by calling ToEngUnits
                        //  (member function of MccBoard class)
                        lblVot_3.Text = DataValue_3.ToString();
                        double result = DataValue_3 * p1_3 - p2_3;
                        return result;
                        //System.IO.File.AppendAllText("e:\\result.txt", result.ToString("0.00") +"  " +DataValue.ToString() +"  "+ EngUnits.ToString()+ "\r\n");
                    }
                }
                else
                {
                    if (ADResolution1 > 16)
                    {
                        ULStat = DaqBoard2.AIn32(portNumber, Range.Bip2Volts, out DataValue32_4, Options);
                        //  Convert raw data to Volts by calling ToEngUnits
                        //  (member function of MccBoard class)
                        lblVot_4.Text = DataValue_4.ToString();
                        double result = DataValue32_4 * p1_4 - p2_4;
                        return result;

                    }
                    else
                    {
                        ULStat = DaqBoard2.AIn(portNumber, Range.Bip2Volts, out DataValue_4);

                        //  Convert raw data to Volts by calling ToEngUnits
                        //  (member function of MccBoard class)
                        lblVot_4.Text = DataValue_4.ToString();
                        double result = DataValue_4 * p1_4 - p2_4;
                        return result;
                        //System.IO.File.AppendAllText("e:\\result.txt", result.ToString("0.00") +"  " +DataValue.ToString() +"  "+ EngUnits.ToString()+ "\r\n");
                    }
                }




            }





        }



        private void btnClearAll_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
            timer4.Stop();
            timer1_selfTest.Stop();

            // Flash the LED of the specified DAQ device with FlashLED()
            n1 = 0;
          
            
            //the port must be AuxPort or FirstPortA for bit output
            //BitPort = MccDaq.DigitalPortType.AuxPort;
            tempAllClear(1);
            tempAllClear(2);
            tempAllClear(3);
            tempAllClear(4);
            btnStart1.Text = "Start";
            

          
           
        }



        //private void ReleaseDAQDevices()
        //{
        //    //foreach (mccdaq.mccboard daqboard in cmbboxdiscovereddevs.items)
        //    //{
        //    //    // release resources associated with the specified board number within the universal library with cbreleasedaqdevice()
        //    //    //    parameters:
        //    //    //    	mccboard:			board object

        //    //    mccdaq.daqdevicemanager.releasedaqdevice(daqboard);
        //    //}
        //}


       
       

       

        private void btnStart_Click(object sender, EventArgs e)
        {
            

            if (n1 % 2 == 0)
            {
                this.btnStart1.Text = "Stop";
                

                
                

                System.IO.File.AppendAllText("e:\\result.txt", "==============开始==============" + "\r\n");


            }
            else
            {
                this.btnStart1.Text = "Start";
                 

              

                fileLineIndex = 0;

                System.IO.File.AppendAllText("e:\\result.txt", "==============结束==============" + "\r\n");



            }

            punishMentTemp = double.Parse(tbMaxTemp.Text);
            confortableTemp = double.Parse(tbMinTemp.Text);


           





            n1++;
            

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            tempAllClear(1);
            tempAllClear(2);
            tempAllClear(3);
            tempAllClear(4);

            Application.Exit();

        }

       

     

      

        private void timer2_Tick(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间
            B1_dightOnOFF_2 = AnalogConvertDigit(0, 1);


            // B1_temperature1 = AnalogConvertTemperature(0, 2);
            B1_temperature2 = AnalogConvertTemperature(0, 3);
            T2 = B1_temperature2.ToString("00.0");
            //B2_dightOnOFF_1 = AnalogConvertDigit(1, 0);
            //B2_dightOnOFF_2 = AnalogConvertDigit(1, 1);
            //B2_temperature1 = AnalogConvertTemperature(1, 2);
            //B2_temperature2 = AnalogConvertTemperature(1, 3);

            // label_temValue1.Text = B1_temperature1.ToString("00");
            label_temValue2.Text = B1_temperature2.ToString("00.0");
            //label_temValue3.Text = B2_temperature1.ToString("00");
            //label_temValue4.Text = B2_temperature2.ToString("00");

           

            //System.IO.File.AppendAllText("e:\\result.txt", "Group-2:  " + DateTime.Now.ToString("hh:mm:ss") + ":" + DateTime.Now.Millisecond.ToString() + " " + "temperature: " + B1_temperature2.ToString("0.00") + "\r\n");
            //if (B1_temperature1 > safeTemperature || B1_temperature2 > safeTemperature || B2_temperature1 > safeTemperature || B2_temperature2 > safeTemperature)
            //{
            //    tempAllClear();

            //    timer1.Stop();
            //    MessageBox.Show("The temperature are unsafe");

            //}
            //----initial the parameter;
            //if (B1_temperature2 > 50)
            //{
            //    tempAllClear(2);
            //    timer2.Stop();
            //    lbOnOff_2.Text = "Error";
            //    lbOnOff_2.ForeColor = Color.Red;
            //}
            if (isUp_2 == true)
            {
                if ((punishMentTemp - B1_temperature2) < 5 && isStartPID_2 == true)
                {
                    startPID_2 = true;
                    isStartPID_2 = false;

                    result_2 = PID_2.PIDCalcDirect(B1_temperature2);
                    FirstPropotation_2 = Convert.ToInt32(result_2);
                    propotation_2 = Convert.ToInt32(result_2);
                    propotation_2 = ConvertAccordToPropotation(propotation_2);
                    // lblPropotation.Text = propotation_1.ToString();

                    PID_Count_2 = 0;
                    tempUp(0, 2);

                }





                if (PID_Count_2 == propotation_2 && startPID_2 == true)
                {
                    temp_nature(0, 2);
                }


                if (startPID_2 == true)
                {
                    PID_Count_2++;

                }


                if (startPID_2 == true && PID_Count_2 == circle)
                {
                    result_2 = PID_2.PIDCalcDirect(B1_temperature2);

                    propotation_2 = Convert.ToInt32(result_2);
                    PID_Count_2 = 0;



                    if (result_2 > 0)
                    {

                        tempUp(0, 2);

                    }
                    else
                    {
                        temp_nature(0, 2);
                    }

                    propotation_2 = ConvertAccordToPropotation(propotation_2);

                    //lblPropotation.Text = propotation_1.ToString();
                }
                lblPropotation.Text = propotation_2.ToString();
                System.IO.File.AppendAllText("e:\\result_2.txt", "惩罚部分：" + "output:" + result_1.ToString("000.000") + "  propotation:" + propotation_1.ToString("00") + "温度： " + B1_temperature2.ToString("00.00") + "\r\n");
            }


            if (isDown_2 == true)
            {
                if ((B2_temperature2 - confortableTemp) < 5 && isStartPID_2 == true)
                {
                    startPID_2 = true;
                    isStartPID_2 = false;

                    result_2 = PID_2.PIDCalcDirect(B1_temperature2);
                    FirstPropotation_2 = Convert.ToInt32(result_2);
                    propotation_2 = Convert.ToInt32(result_2);
                    propotation_2 = ConvertAccordToPropotation(propotation_2);
                    PID_Count_2 = 0;
                    tempDown(0, 2);

                }





                if (PID_Count_2 == propotation_2 && startPID_2 == true)
                {
                    temp_nature(0, 2);
                }

                if (startPID_2 == true)
                {
                    PID_Count_2++;

                }

                if (startPID_2 == true && PID_Count_2 == circle)
                {
                    double result = PID_2.PIDCalcDirect(B1_temperature2);
                    propotation_2 = Convert.ToInt32(result);
                    PID_Count_2 = 0;
                    if (result_2 > 0)
                    {
                        temp_nature(0, 2);
                    }
                    else
                    {
                        tempDown(0, 2);
                    }

                    propotation_2 = ConvertAccordToPropotation(propotation_2);


                }
                lblPropotation.Text = propotation_2.ToString();
                System.IO.File.AppendAllText("e:\\result_2.txt", "舒适部分：" + "output:" + result_2.ToString("000.000") + "  propotation:" + propotation_2.ToString() + "温度： " + B1_temperature2.ToString("00.00") + "\r\n");

            }


            stopwatch.Stop(); //  停止监视

            TimeSpan timespan = stopwatch.Elapsed;

            Timespan_2 = timespan.TotalMilliseconds;  //  总毫秒数
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            //B1_dightOnOFF_1 = AnalogConvertDigit(0, 0);
            //B1_dightOnOFF_2 = AnalogConvertDigit(0, 1);
            // B1_temperature1 = AnalogConvertTemperature(0, 2);
            //B1_temperature2 = AnalogConvertTemperature(0, 3);
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间
            B2_dightOnOFF_1 = AnalogConvertDigit(1, 0);

            //B2_dightOnOFF_2 = AnalogConvertDigit(1, 1);
            B2_temperature1 = AnalogConvertTemperature(1, 2);
            //B2_temperature2 = AnalogConvertTemperature(1, 3);
            T3 = B2_temperature1.ToString("00.0");
            // label_temValue1.Text = B1_temperature1.ToString("00");
            //label_temValue2.Text = B1_temperature2.ToString("00");
            label_temValue3.Text = B2_temperature1.ToString("00.0");
            //label_temValue4.Text = B2_temperature2.ToString("00");
            //System.IO.File.AppendAllText("e:\\result.txt", "Group-3:  " + DateTime.Now.ToString("hh:mm:ss") + ":" + DateTime.Now.Millisecond.ToString() + " " +"temperature: "+B2_temperature1.ToString("0.00")+ "\r\n");
            //if (B1_temperature1 > safeTemperature || B1_temperature2 > safeTemperature || B2_temperature1 > safeTemperature || B2_temperature2 > safeTemperature)
            //{
            //    tempAllClear();

            //    timer1.Stop();
            //    MessageBox.Show("The temperature are unsafe");

            //}
            //----initial the parameter;
            //if (B2_temperature1 > 50)
            //{
            //    tempAllClear(3);
            //    timer3.Stop();
            //    lbOnOff_3.Text = "Error";
            //    lbOnOff_3.ForeColor = Color.Red;
            //}
           





            


            if (isUp_3 == true)
            {
                if ((punishMentTemp - B2_temperature1) < 5 && isStartPID_3 == true)
                {
                    startPID_3 = true;
                    isStartPID_3 = false;

                    result_3 = PID_3.PIDCalcDirect(B2_temperature1);
                    FirstPropotation_3 = Convert.ToInt32(result_3);
                    propotation_3 = Convert.ToInt32(result_3);
                    propotation_3 = ConvertAccordToPropotation(propotation_3);
                    // lblPropotation.Text = propotation_3.ToString();

                    PID_Count_3 = 0;
                    tempUp(1, 3);

                }





                if (PID_Count_3 == propotation_3 && startPID_3 == true)
                {
                    temp_nature(1, 3);
                }


                if (startPID_3 == true)
                {
                    PID_Count_3++;

                }


                if (startPID_3 == true && PID_Count_3 == circle)
                {
                    result_3 = PID_3.PIDCalcDirect(B2_temperature1);

                    propotation_3 = Convert.ToInt32(result_3);
                    PID_Count_3 = 0;



                    if (result_3 > 0)
                    {

                        tempUp(1, 3);

                    }
                    else
                    {
                        temp_nature(1, 3);
                    }

                    propotation_3 = ConvertAccordToPropotation(propotation_3);

                    //lblPropotation.Text = propotation_3.ToString();
                }
                lblPropotation.Text = propotation_3.ToString();
                System.IO.File.AppendAllText("e:\\result_3.txt", "惩罚部分：" + "output:" + result_3.ToString("000.000") + "  propotation:" + propotation_3.ToString("00") + "温度： " + B2_temperature1.ToString("00.00") + "\r\n");
            }


            if (isDown_3 == true)
            {
                if ((B2_temperature1 - confortableTemp) < 5 && isStartPID_3 == true)
                {
                    startPID_3 = true;
                    isStartPID_3 = false;

                    result_3 = PID_3.PIDCalcDirect(B2_temperature1);
                    FirstPropotation_3 = Convert.ToInt32(result_3);
                    propotation_3 = Convert.ToInt32(result_3);
                    propotation_3 = ConvertAccordToPropotation(propotation_3);
                    PID_Count_3 = 0;
                    tempDown(1, 3);

                }





                if (PID_Count_3 == propotation_3 && startPID_3 == true)
                {
                    temp_nature(1, 3);
                }

                if (startPID_3 == true)
                {
                    PID_Count_3++;

                }

                if (startPID_3 == true && PID_Count_3 == circle)
                {
                    double result = PID_3.PIDCalcDirect(B2_temperature1);
                    propotation_3 = Convert.ToInt32(result);
                    PID_Count_3 = 0;
                    if (result_3 > 0)
                    {
                        temp_nature(1, 3);
                    }
                    else
                    {
                        tempDown(1, 3);
                    }

                    propotation_3 = ConvertAccordToPropotation(propotation_3);


                }
                lblPropotation.Text = propotation_3.ToString();
                System.IO.File.AppendAllText("e:\\result_3.txt", "舒适部分：" + "output:" + result_3.ToString("000.000") + "  propotation:" + propotation_3.ToString() + "温度： " + B2_temperature1.ToString("00.00") + "\r\n");

            }

            stopwatch.Stop(); //  停止监视

            TimeSpan timespan = stopwatch.Elapsed;
            Timespan_3 = timespan.TotalMilliseconds;  //  总毫秒数
        
    }

        private void timer4_Tick(object sender, EventArgs e)
        {
            //B1_dightOnOFF_1 = AnalogConvertDigit(0, 0);
            //B1_dightOnOFF_2 = AnalogConvertDigit(0, 1);
            //B1_temperature1 = AnalogConvertTemperature(0, 2);
            //B1_temperature2 = AnalogConvertTemperature(0, 3);
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间
                               //B2_dightOnOFF_1 = AnalogConvertDigit(1, 0);
            B2_dightOnOFF_2 = AnalogConvertDigit(1, 1);


            //B2_temperature1 = AnalogConvertTemperature(1, 2);
            B2_temperature2 = AnalogConvertTemperature(1, 3);
            T4 = B2_temperature2.ToString("00.0");
            //label_temValue1.Text = B1_temperature1.ToString("00");
            //label_temValue2.Text = B1_temperature2.ToString("00");
            //label_temValue3.Text = B2_temperature1.ToString("00");
            label_temValue4.Text = B2_temperature2.ToString("00.0");
            //System.IO.File.AppendAllText("e:\\result.txt", "Group-4:  " + DateTime.Now.ToString("hh:mm:ss") + ":" + DateTime.Now.Millisecond.ToString() + " " + "temperature: " + B2_temperature2.ToString("0.00") + "\r\n");
            //if (B1_temperature1 > safeTemperature || B1_temperature2 > safeTemperature || B2_temperature1 > safeTemperature || B2_temperature2 > safeTemperature)
            //{
            //    tempAllClear();

            //    timer1.Stop();
            //    MessageBox.Show("The temperature are unsafe");

            //}
            //----initial the parameter;
            //if (B2_temperature2 > 50)
            //{
            //    tempAllClear(4);
            //    timer4.Stop();
            //    lbOnOff_4.Text = "Error";
            //    lbOnOff_4.ForeColor = Color.Red;
            //}

           





            

            if (isUp_4 == true)
            {
                if ((punishMentTemp - B2_temperature2) < 5 && isStartPID_4 == true)
                {
                    startPID_4 = true;
                    isStartPID_4 = false;

                    result_4 = PID_4.PIDCalcDirect(B2_temperature2);
                    FirstPropotation_4 = Convert.ToInt32(result_4);
                    propotation_4 = Convert.ToInt32(result_4);
                    propotation_4 = ConvertAccordToPropotation(propotation_4);
                    // lblPropotation.Text = propotation_4.ToString();

                    PID_Count_4 = 0;
                    tempUp(1, 4);

                }





                if (PID_Count_4 == propotation_4 && startPID_4 == true)
                {
                    temp_nature(1, 4);
                }


                if (startPID_4 == true)
                {
                    PID_Count_4++;

                }


                if (startPID_4 == true && PID_Count_4 == circle)
                {
                    result_4 = PID_4.PIDCalcDirect(B2_temperature2);

                    propotation_4 = Convert.ToInt32(result_4);
                    PID_Count_4 = 0;



                    if (result_4 > 0)
                    {

                        tempUp(1, 4);

                    }
                    else
                    {
                        temp_nature(1, 4);
                    }

                    propotation_4 = ConvertAccordToPropotation(propotation_4);

                    //lblPropotation.Text = propotation_4.ToString();
                }
                lblPropotation.Text = propotation_4.ToString();
                System.IO.File.AppendAllText("e:\\result_4.txt", "惩罚部分：" + "output:" + result_4.ToString("000.000") + "  propotation:" + propotation_4.ToString("00") + "温度： " + B2_temperature2.ToString("00.00") + "\r\n");
            }


            if (isDown_4 == true)
            {
                if ((B2_temperature2 - confortableTemp) < 5 && isStartPID_4 == true)
                {
                    startPID_4 = true;
                    isStartPID_4 = false;

                    result_4 = PID_4.PIDCalcDirect(B2_temperature2);
                    FirstPropotation_4 = Convert.ToInt32(result_4);
                    propotation_4 = Convert.ToInt32(result_4);
                    propotation_4 = ConvertAccordToPropotation(propotation_4);
                    PID_Count_4 = 0;
                    tempDown(1, 4);

                }





                if (PID_Count_4 == propotation_4 && startPID_4 == true)
                {
                    temp_nature(1, 4);
                }

                if (startPID_4 == true)
                {
                    PID_Count_4++;

                }

                if (startPID_4 == true && PID_Count_4 == circle)
                {
                    double result = PID_4.PIDCalcDirect(B2_temperature2);
                    propotation_4 = Convert.ToInt32(result);
                    PID_Count_4 = 0;
                    if (result_4 > 0)
                    {
                        temp_nature(1, 4);
                    }
                    else
                    {
                        tempDown(1, 4);
                    }

                    propotation_4 = ConvertAccordToPropotation(propotation_4);


                }
                lblPropotation.Text = propotation_4.ToString();
                System.IO.File.AppendAllText("e:\\result_4.txt", "舒适部分：" + "output:" + result_4.ToString("000.000") + "  propotation:" + propotation_4.ToString() + "温度： " + B2_temperature2.ToString("00.00") + "\r\n");

            }




            stopwatch.Stop(); //  停止监视

            TimeSpan timespan = stopwatch.Elapsed;

            Timespan_4 = timespan.TotalMilliseconds;  //  总毫秒数


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            B1_dightOnOFF_1 = AnalogConvertDigit(0, 0);
            lblSingl.Text = B1_dightOnOFF_1.ToString();
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();      //  开始监视代码运行时间

           

             B1_temperature1 = AnalogConvertTemperature(0, 2);
           
             //T1 = B1_temperature1.ToString("00.0");
             label_temValue1.Text = B1_temperature1.ToString("00.0");




            if (isUp_1 == true)
            {
                if ((punishMentTemp - B1_temperature1) < 5 && isStartPID_1 == true)
                {
                    startPID_1 = true;
                    isStartPID_1 = false;

                    result_1 = PID_1.PIDCalcDirect(B1_temperature1);
                    FirstPropotation_1 = Convert.ToInt32(result_1);
                    propotation_1 = Convert.ToInt32(result_1);
                    propotation_1 = ConvertAccordToPropotation(propotation_1);
                    // lblPropotation.Text = propotation_1.ToString();

                    PID_Count_1 = 0;
                    tempUp(0, 1);

                }





                if (PID_Count_1 == propotation_1 && startPID_1 == true)
                {
                    temp_nature(0, 1);
                }


                if (startPID_1 == true)
                {
                    PID_Count_1++;

                }


                if (startPID_1 == true && PID_Count_1 == circle)
                {
                    result_1 = PID_1.PIDCalcDirect(B1_temperature1);

                    propotation_1 = Convert.ToInt32(result_1);
                    PID_Count_1 = 0;



                    if (result_1 > 0)
                    {

                        tempUp(0, 1);

                    }
                    else
                    {
                        temp_nature(0, 1);
                    }

                    propotation_1 = ConvertAccordToPropotation(propotation_1);

                    //lblPropotation.Text = propotation_1.ToString();
                }
                lblPropotation.Text = propotation_1.ToString();
               // System.IO.File.AppendAllText("e:\\result_1.txt", "惩罚部分：" + "output:" + result_1.ToString("000.000") + "  propotation:" + propotation_1.ToString("00") + "温度： " + B1_temperature1.ToString("00.00") + "\r\n");
            }


            if (isDown_1 == true)
            {
                if ((B1_temperature1 - confortableTemp) < 5 && isStartPID_1 == true)
                {
                    startPID_1 = true;
                    isStartPID_1 = false;

                    result_1 = PID_1.PIDCalcDirect(B1_temperature1);
                    FirstPropotation_1 = Convert.ToInt32(result_1);
                    propotation_1 = Convert.ToInt32(result_1);
                    propotation_1 = ConvertAccordToPropotation(propotation_1);
                    PID_Count_1 = 0;
                    tempDown(0, 1);

                }





                if (PID_Count_1 == propotation_1 && startPID_1 == true)
                {
                    temp_nature(0, 1);
                }

                if (startPID_1 == true)
                {
                    PID_Count_1++;

                }

                if (startPID_1 == true && PID_Count_1 == circle)
                {
                    double result_1 = PID_1.PIDCalcDirect(B1_temperature1);
                    propotation_1 = Convert.ToInt32(result_1);
                    PID_Count_1 = 0;
                    if (result_1 > 0)
                    {
                        temp_nature(0, 1);
                    }
                    else
                    {
                        tempDown(0, 1);
                    }

                    propotation_1 = ConvertAccordToPropotation(propotation_1);


                }
                lblPropotation.Text = propotation_1.ToString();
               // System.IO.File.AppendAllText("e:\\result_1.txt", "舒适部分：" + "output:" + result_1.ToString("000.000") + "  propotation:" + propotation_1.ToString() + "温度： " + B1_temperature1.ToString("00.00") + "\r\n");

            }








            stopwatch.Stop(); //  停止监视

            TimeSpan timespan = stopwatch.Elapsed;


            Timespan_1 = timespan.TotalMilliseconds;  //  总毫秒数
           // System.IO.File.AppendAllText("E:\\result.txt", "时间间隔：" + Timespan_1.ToString()+"\r\n");

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void timer5_Tick(object sender, EventArgs e)
        {
           



           
           // lblSingl.Text = B1_dightOnOFF_1.ToString();
            //System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();      //  开始监视代码运行时间



            B1_temperature1 = AnalogConvertTemperature(0, 2);

            //T1 = B1_temperature1.ToString("00.0");
            label_temValue1.Text = B1_temperature1.ToString("00.0");

            if (B1_temperature1 > 60 || B1_temperature1 < 0)
            {
                tempAllClear(1);
                lbOnOff_1.Text = "Error";
                this.timer1_selfTest.Enabled = false;
            }


            if (isRecsiveSingal_1 == true)
            {
                timerCount++;

                if (B1_dightOnOFF_1 == 1 && isFirstChange_11 == true)
                {
                    lbOnOff_1.Text = "On";

                    isUp_1 = true;
                    isDown_1 = false;

                    isStartPID_1 = true;
                    PID_Count_1 = 0;

                    tempUp(0, 1);
                    startPID_1 = false;
                    isFirstChange_11 = false;
                    isFirstChange_12 = true;

                    highBalance = true;

                    circle = int.Parse(tbCircle.Text);
                    PID_1 = new PIDControl(4, 0, 3, punishMentTemp);
                   // System.IO.File.AppendAllText("e:\\result_1.txt", "惩罚：" + "Kp:" + tbKp.Text + "  Ki" + tbKi.Text + "  Kd" + tbKd.Text + "\r\n");
                }


                else if (B1_dightOnOFF_1 == 0 && isFirstChange_12 == true)
                {


                    isFirstChange_11 = true;
                    isFirstChange_12 = false;
                    isDown_1 = true;
                    isStartPID_1 = true;
                    startPID_1 = false;
                    isUp_1 = false;
                    PID_Count_1 = 0;
                    lbOnOff_1.Text = "Off";
                    tempDown(0, 1);
                    circle = int.Parse(tbCircle.Text);
                    PID_1 = new PIDControl(P, I, D, confortableTemp);
                  //  System.IO.File.AppendAllText("e:\\result_1.txt", "舒适：" + "Kp:" + 8 + "  Ki" + 0 + "  Kd" + 3 + "\r\n");
                }
                else
                {
                    ;
                }
                
            }


            if (timerCount == 300 && highBalance == true)
            {
                timerCount = 0;
                B1_dightOnOFF_1 = 0;
                highBalance = false;
                downLine = true;
            }

            if (timerCount == 200 && downLine)
            {
                //System.IO.File.AppendAllText("e:\\result_1.txt", "舒适：" + "Kp:" + 8 + "  Ki" + 0 + "  Kd" + 3 + "\r\n");
                isChangeParam = true;
                downLine = false;
                TimerNotAccept = true;

            }

            if (B1_temperature1 - confortableTemp <= 0.5 && downLine == true)
            {
                beyondNum++;
                if (beyondNum == 3)
                {
                    
                    downLine = false;
                    longKeep = true;
                    beyondNum = 0;
                    downTime = timerCount;
                    timerCount = 0;
                }
            }

            if (longKeep == true)
            {
                tempCollection.Add(B1_temperature1);
                if (timerCount == 300)
                {
                    isChangeParam = true;
                    longKeep = false;
                }
                
            }
           





            if (isChangeParam)
            {
                    
                if (TimerNotAccept)
                {
                    lblTestStatus.Text = (string.Format("P:{0},I:{1},D{2}--时间超出", P, I, D));
                    System.IO.File.AppendAllText("e:\\result_1.txt",P+"   "+I+"   "+D+"   " +"Tout"+ "\r\n");
                    //输出PID 时间超出，舍弃参数
                }
                else
                {
                    resultAnalyse(tempCollection);
                    //执行计算，然后输出
                    tempCollection = new List<double>();
                }
                if (D == 10)
                {
                    timer1_selfTest.Stop();
                }

                B1_dightOnOFF_1 = 1;
                isChangeParam = false;
                       P += 0.1;
                    if (P == 10)
                    {
                        D += 0.1;
                        P = 0.1;
                    }
                 
               
                timerCount = 0;
                highBalance = true;
            }



            //----------升温模块开始---------------------------------
            if (isUp_1 == true)
            {
                if ((punishMentTemp - B1_temperature1) < 5 && isStartPID_1 == true)
                {
                    startPID_1 = true;
                    isStartPID_1 = false;

                    result_1 = PID_1.PIDCalcDirect(B1_temperature1);
                    FirstPropotation_1 = Convert.ToInt32(result_1);
                    propotation_1 = Convert.ToInt32(result_1);
                    propotation_1 = ConvertAccordToPropotation(propotation_1);
                    // lblPropotation.Text = propotation_1.ToString();

                    PID_Count_1 = 0;
                    tempUp(0, 1);

                }





                if (PID_Count_1 == propotation_1 && startPID_1 == true)
                {
                    temp_nature(0, 1);
                }


                if (startPID_1 == true)
                {
                    PID_Count_1++;

                }


                if (startPID_1 == true && PID_Count_1 == circle)
                {
                    result_1 = PID_1.PIDCalcDirect(B1_temperature1);

                    propotation_1 = Convert.ToInt32(result_1);
                    PID_Count_1 = 0;



                    if (result_1 > 0)
                    {

                        tempUp(0, 1);

                    }
                    else
                    {
                        temp_nature(0, 1);
                    }

                    propotation_1 = ConvertAccordToPropotation(propotation_1);

                    //lblPropotation.Text = propotation_1.ToString();
                }
                lblPropotation.Text = propotation_1.ToString();
                //System.IO.File.AppendAllText("e:\\result_1.txt", "惩罚部分：" + "output:" + result_1.ToString("000.000") + "  propotation:" + propotation_1.ToString("00") + "温度： " + B1_temperature1.ToString("00.00") + "\r\n");
            }

            //----------降温模块开始---------------------------------
            if (isDown_1 == true)
            {
                if ((B1_temperature1 - confortableTemp) < 5 && isStartPID_1 == true)
                {
                    startPID_1 = true;
                    isStartPID_1 = false;

                    result_1 = PID_1.PIDCalcDirect(B1_temperature1);
                    FirstPropotation_1 = Convert.ToInt32(result_1);
                    propotation_1 = Convert.ToInt32(result_1);
                    propotation_1 = ConvertAccordToPropotation(propotation_1);
                    PID_Count_1 = 0;
                    tempDown(0, 1);

                }





                if (PID_Count_1 == propotation_1 && startPID_1 == true)
                {
                    temp_nature(0, 1);
                }

                if (startPID_1 == true)
                {
                    PID_Count_1++;

                }

                if (startPID_1 == true && PID_Count_1 == circle)
                {
                    double result_1 = PID_1.PIDCalcDirect(B1_temperature1);
                    propotation_1 = Convert.ToInt32(result_1);
                    PID_Count_1 = 0;
                    if (result_1 > 0)
                    {
                        temp_nature(0, 1);
                    }
                    else
                    {
                        tempDown(0, 1);
                    }

                    propotation_1 = ConvertAccordToPropotation(propotation_1);


                }
                lblPropotation.Text = propotation_1.ToString();
               // System.IO.File.AppendAllText("e:\\result_1.txt", "舒适部分：" + "output:" + result_1.ToString("000.000") + "  propotation:" + propotation_1.ToString() + "温度： " + B1_temperature1.ToString("00.00") + "\r\n");

            }

            //----------升降温模块结束---------------------------------



















        }

        private void resultAnalyse(List<double> list)
        {
            double MaxList = list.Max();
            double MinList = list.Min();
            double SumList = list.Sum();

            double average = list.Average();

            double descret=0;
            foreach(double I in list)
            {
                descret += Math.Pow((I - average),2);
            }

            descret = Math.Sqrt(descret);


            System.IO.File.AppendAllText("e:\\result_1.txt",P+"   "+I+"   "+D+ "   " +downTime+"   "+ MaxList+"   "+
                MinList+"   "+average+"   "+descret+"   "+    "\r\n");
            lblTestStatus.Text = string.Format("P:{0},I:{1},D:{2};正常", P, I, D);
        }
















        private void button2_Click_1(object sender, EventArgs e)
        {

            MccDaq.DigitalLogicState bitValue;
            int portNum = int.Parse(tbTestDnum.Text);
            //MccDaq.DigitalPortDirection ConfigDirection;
            //// ConfigDirection = DigitalPortDirection.DigitalOut;

            //ConfigDirection = DigitalPortDirection.DigitalIn;

           // DaqBoard1.DConfigPort(DigitalPortType.FirstPortA, ConfigDirection);




            MccDaq.DigitalPortType BitPort = MccDaq.DigitalPortType.FirstPortA;


            //MccDaq.DigitalLogicState BitValueHigh = MccDaq.DigitalLogicState.High;
            //MccDaq.DigitalLogicState BitValueLow = MccDaq.DigitalLogicState.Low;



            MccDaq.ErrorInfo ULStat = DaqBoard1.DBitIn(BitPort, portNum, out bitValue);
            lblShowD.Text = Convert.ToInt32(bitValue).ToString("0");

            btnOutDport.Text = "Off";
            testPortControl = false;





            //--------------============================================----------------------------
            //int LowChan;
            //MccDaq.TriggerType DefaultTrig;
            //int ChannelType = clsAnalogIO.ANALOGINPUT;

            //NumAIChans1 = AIOProps1.FindAnalogChansOfType(DaqBoard1, ChannelType,
            //    out ADResolution1, out Range1, out LowChan, out DefaultTrig);
           // timer5.Start();


        }

        private void btnOnoff_2_Click(object sender, EventArgs e)
        {
            
        }

        private void btnOnoff_3_Click(object sender, EventArgs e)
        {
            //timer5.Interval = 100;
            //timer5.Start();



            MccDaq.DigitalLogicState bitValue;
            //  int portNum = int.Parse(tbTestDnum.Text);
            //MccDaq.DigitalPortDirection ConfigDirection;
            //// ConfigDirection = DigitalPortDirection.DigitalOut;

            //ConfigDirection = DigitalPortDirection.DigitalOut;

            //DaqBoard1.DConfigPort(DigitalPortType.FirstPortA, ConfigDirection);

            int portNum = int.Parse(tbTestAnalogPortNum.Text);

            MccDaq.DigitalPortType BitPort = MccDaq.DigitalPortType.FirstPortA;


            MccDaq.DigitalLogicState BitValueHigh = MccDaq.DigitalLogicState.High;
            MccDaq.DigitalLogicState BitValueLow = MccDaq.DigitalLogicState.Low;

            if (abc)

            {
                DaqBoard1.DBitOut(BitPort, portNum, BitValueLow);
                abc = false;
                btnOnoff_3.Text = "Off";
            }      //DaqBoard1.DBitOut(BitPort, 0, BitValueLow);
            else
            {
                DaqBoard1.DBitOut(BitPort, portNum, BitValueHigh);
                abc = true;
                btnOnoff_3.Text = "On";
            }

            //===========================================================================================

            //int LowChan;
            //MccDaq.TriggerType DefaultTrig;
            //int ChannelType = clsAnalogIO.ANALOGOUTPUT;

            //NumAIChans1 = AIOProps1.FindAnalogChansOfType(DaqBoard1, ChannelType,
            //    out ADResolution1, out Range1, out LowChan, out DefaultTrig);


            //DaqBoard1.AOut(int.Parse(tbTestAnalogPortNum.Text), Range.Bip1Volts, short.Parse(tbAnalogOutputValue.Text));



        }
        private void btnOnoff_4_Click(object sender, EventArgs e)
        {
           
        }

        private void btnSelfTest_Click(object sender, EventArgs e)
        {
            timer1_selfTest.Start();
            isRecsiveSingal_1 = true;
            B1_dightOnOFF_1 = 1;
            isFirstChange_12 = false;

            System.IO.File.AppendAllText("e:\\result_3.txt","开始降温自检"+ "\r\n");




        }

        private void timer5_Tick_1(object sender, EventArgs e)
        {

            int portNumber = int.Parse(tbTestAnalogPortNum.Text);
                    if (ADResolution1 > 16)
                    {
                        ULStat = DaqBoard1.AIn32(portNumber, Range.Bip2Volts, out DataValue32_1, Options);
                        lblTestAlalogValue.Text = DataValue32_1.ToString();
                       
                        

                    }
                    else
                    {
                        ULStat = DaqBoard1.AIn(portNumber, Range.Bip2Volts, out DataValue_1);

                        lblTestAlalogValue.Text = DataValue_1.ToString();
                       
                        
                    }
              
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //p1_1 = double.Parse(tbG1_Hand1.Text);
            //p2_1 = double.Parse(tbG1_Hand2.Text);

            p1_1 = 0.02115;
            p2_1 = 41.85;


            p1_2 = double.Parse(tbG2_Hand1.Text);
            p2_2 = double.Parse(tbG2_Hand2.Text);

            p1_3 = double.Parse(tbG3_Hand1.Text);
            p2_3 = double.Parse(tbG3_Hand2.Text);

            p1_4 = double.Parse(tbG4_Hand1.Text);
            p2_4 = double.Parse(tbG4_Hand2.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            


            MccDaq.DaqDeviceManager.IgnoreInstaCal();
            InitUL();

            PortType = clsDigitalIO.PORTOUT;

            Cursor.Current = Cursors.WaitCursor;

            // Discover DAQ devices with GetDaqDeviceInventory()
            //  Parameters:
            //    InterfaceType   :interface type of DAQ devices to be discovered

            MccDaq.DaqDeviceDescriptor[] inventory = MccDaq.DaqDeviceManager.GetDaqDeviceInventory(MccDaq.DaqDeviceInterface.Any);

            int numDevDiscovered = inventory.Length;

         

            lblStatus.Text = numDevDiscovered + " DAQ Device(s) Discovered";

            if (numDevDiscovered > 0)
            {
               
                    try
                    {
                        //    Create a new MccBoard object for Board and assign a board number 
                        //    to the specified DAQ device with CreateDaqDevice()

                        //    Parameters:
                        //        BoardNum			: board number to be assigned to the specified DAQ device
                        //        DeviceDescriptor	: device descriptor of the DAQ device 

                         DaqBoard1 = MccDaq.DaqDeviceManager.CreateDaqDevice(0, inventory[0]);
                    // DaqBoard2 = MccDaq.DaqDeviceManager.CreateDaqDevice(1, inventory[1]);

                    this.lblTestAlalogValue.Text = "Board1_ID:" + DaqBoard1.Descriptor.UniqueID.ToString();

                    // Add the board to combobox
                   
                    }
                    catch (ULException ule)
                    {
                        lblStatus.Text = "Error occured: " + ule.Message;
                    }
                }
            





            Cursor.Current = Cursors.Default;

            p1_1 = double.Parse(tbG1_Hand1.Text);
            p2_1 = double.Parse(tbG1_Hand2.Text);

            p1_2 = double.Parse(tbG2_Hand1.Text);
            p2_2 = double.Parse(tbG2_Hand2.Text);

            p1_3 = double.Parse(tbG3_Hand1.Text);
            p2_3 = double.Parse(tbG3_Hand2.Text);

            p1_4 = double.Parse(tbG4_Hand1.Text);
            p2_4 = double.Parse(tbG4_Hand2.Text);
            int LowChan;
            MccDaq.TriggerType DefaultTrig;
            
            InitUL();




            // determine the number of analog channels and their capabilities
            int ChannelType = clsAnalogIO.ANALOGINPUT;

            NumAIChans1 = AIOProps1.FindAnalogChansOfType(DaqBoard1, ChannelType,
                out ADResolution1, out Range1, out LowChan, out DefaultTrig);

           // int ChannelType = clsAnalogIO.ANALOGOUTPUT;

            NumAIChans1 = AIOProps2.FindAnalogChansOfType(DaqBoard1, clsAnalogIO.ANALOGOUTPUT,
                out ADResolution1, out Range1, out LowChan, out DefaultTrig);

            PortType = clsDigitalIO.PORTOUT;
            NumPorts1 = DioProps1.FindPortsOfType(DaqBoard1, PortType, out ProgAbility,
               out PortNum1, out NumBits, out FirstBit);

            //NumPorts1 = DioProps2.FindPortsOfType(DaqBoard1, clsDigitalIO.PORTIN, out ProgAbility,
            //  out PortNum1, out NumBits, out FirstBit);


            //NumPorts = DioProps.FindPortsOfType(DaqBoard, PortType, out ProgAbility,
            //   out PortNum, out NumBits, out FirstBit);
            // determine the number of analog channels and their capabilities
            //int ChannelType = clsAnalogIO.ANALOGINPUT;
            //NumAIChans = AIOProps.FindAnalogChansOfType(DaqBoard, ChannelType,
            //out ADResolution, out Range, out LowChan, out DefaultTrig);





            startPID_1 = false;
            startPID_2 = false;
            startPID_3 = false;
            startPID_4 = false;

            PID_Count_1 = 0;
            PID_Count_2 = 0;
            PID_Count_3 = 0;
            PID_Count_4 = 0;

            timer1.Interval = 10;
            timer2.Interval = 10;
            timer3.Interval = 10;
            timer4.Interval = 10;
            timer1_selfTest.Interval = 10;

             
            //timer1.Start();
            //timer2.Start();
            //timer3.Start();
            //timer4.Start();
           


        }




        private void tempAllClear(short i)
        {

            if (i == 1)
            {
                temp_nature(0, 1);
            }
            else if (i == 2)
            {
                temp_nature(0, 2);
            }
            else if (i == 3)
            {
                temp_nature(1, 3);
            }
            else
            {
                temp_nature(1, 4);
            }
        }


        

        private void tempDown(int boardNumber,int groupNum)
        {

            MccDaq.DigitalPortType BitPort = MccDaq.DigitalPortType.FirstPortA;


            MccDaq.DigitalLogicState BitValueHigh = MccDaq.DigitalLogicState.High;
            MccDaq.DigitalLogicState BitValueLow = MccDaq.DigitalLogicState.Low;
            if (boardNumber == 0)
            {
                if (groupNum == 1)
                {
                    DaqBoard1.DBitOut(BitPort, 0, BitValueHigh);
                    DaqBoard1.DBitOut(BitPort, 1, BitValueLow);
                   
                   
                }
                else
                {
                    DaqBoard1.DBitOut(BitPort, 6, BitValueLow);
                    DaqBoard1.DBitOut(BitPort, 5, BitValueHigh);
                   
                  
                }
            }
            else
            {
                if (groupNum == 3)
                {
                    DaqBoard2.DBitOut(BitPort, 0, BitValueHigh);
                    DaqBoard2.DBitOut(BitPort, 1, BitValueLow);
                   
                    
                }
                else
                {
                   
                    DaqBoard2.DBitOut(BitPort, 6, BitValueLow);
                    DaqBoard2.DBitOut(BitPort, 5, BitValueHigh);
                   
                }
            }
          
           
        }

        
        private void tempUp(int boardNumber, int groupNum)
        {

            MccDaq.DigitalPortType BitPort = MccDaq.DigitalPortType.FirstPortA;


            //MccDaq.DigitalLogicState BitValueHigh = MccDaq.DigitalLogicState.High;
            MccDaq.DigitalLogicState BitValueLow = MccDaq.DigitalLogicState.Low;
            if (boardNumber == 0)
            {
                if (groupNum == 1)
                {
                    DaqBoard1.DBitOut(BitPort, 1, BitValueLow);
                    DaqBoard1.DBitOut(BitPort, 0, BitValueLow);
                    
                  
                }
                else
                {
                    DaqBoard1.DBitOut(BitPort, 6, BitValueLow);
                    DaqBoard1.DBitOut(BitPort, 5, BitValueLow);
                    
                   
                }
            }
            else
            {
                if (groupNum == 3)
                {
                    DaqBoard2.DBitOut(BitPort, 1, BitValueLow);
                    DaqBoard2.DBitOut(BitPort, 0, BitValueLow);
                   
                  
                }
                else
                {
                    DaqBoard2.DBitOut(BitPort, 6, BitValueLow);
                    DaqBoard2.DBitOut(BitPort, 5, BitValueLow);
                   
                    
                }
            }

        }


        private void temp_nature(int boardNumber, int groupNum)
        {




            MccDaq.DigitalPortType BitPort = MccDaq.DigitalPortType.FirstPortA;


            MccDaq.DigitalLogicState BitValueHigh = MccDaq.DigitalLogicState.High;
           
            if (boardNumber == 0)
            {
                if (groupNum == 1)
                {
                    DaqBoard1.DBitOut(BitPort, 1, BitValueHigh);
                   
                            
                }
                else
                {
                   
                    DaqBoard1.DBitOut(BitPort, 6, BitValueHigh);
                   
                }
            }
            else
            {
                if (groupNum == 3)
                {
                 
                    DaqBoard2.DBitOut(BitPort, 1, BitValueHigh);
                   
                }
                else
                {
                 
                    DaqBoard2.DBitOut(BitPort, 6, BitValueHigh);
                   
                }
            }

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
