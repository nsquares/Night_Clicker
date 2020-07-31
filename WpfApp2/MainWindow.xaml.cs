//N^2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Utilities utilities = new Utilities();

        TextBox[,] allTBInput;
        int[,] allXandYint;

        DispatcherTimer Timer = new DispatcherTimer();

        //private delegate void NightRunDelegate();

        public MainWindow()
        {
            InitializeComponent();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();

            AddLineMain("(Origin (0,0) is top-left of the monitor)");
            //Task.Factory.StartNew(() =>
            //{
            //  AddLineMain();
            //});

            allTBInput = new TextBox[,] 
            {
                { x_pixel1, y_pixel1 },
                { x_pixel2, y_pixel2 }
            };

            allXandYint = new int[,] 
            { //  x,y
                { 0,0 },
                { 0,0 }
            };

            AddLineMain($"Current Mouse Position: {Utilities.GetCursorPosition().ToString()}");
        }

        public void AddLineMain(string text)
        {
            outputBox.AppendText(text);
            outputBox.AppendText("\u2028"); // Linebreak, not paragraph break
            outputBox.ScrollToEnd();    
        }

        private void Timer_Tick(object sender, EventArgs e)   
        {
            timeLabel.Content = DateTime.Now.ToLongTimeString();
            utcLabel.Content = DateTime.UtcNow.AddHours(-7).ToLongTimeString();
        }
        

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            AddLineMain("lol");
        }

        //maybe a daily ak run as well later on? too many variables 
        //requests: wants to change the time of delay on cookie clicker [done]
        //maybe could also change duration of delays on the night run [done]

        //be able to press keyboard buttons automatically as well [working on it...]

        //can I create a clock that runs on application so that I can see the average time through full san clear or/and daily run? [done]
        //with the clock, can I display how long the app has been running [done]
        //maybe an interactive timer as well [nah]



        //how does one get off the UI thread and create another thread to run on?



        //also need to close the nox application before shuting down
        //another if statement for when sanity has ran out and refill checkbox is checked so I can refill san automatically
        //what is the screen when there is no san and no more refills as well and mission is sill executed?

        //can I control the volume
        //change color of night run to look for something else [nah, not important and it will be the same concept as logging new delays in the night run but with string instead of float]

        //id8 infinity button that will rename bin files and copy and populate to the backup folder (backup whenever you change names, each bin file is 4KB so I do not need to delete old versions) rename backups to be -y1,-y2,-y3,-y4 no leading zero
        //also first check if the three files in the folder currently have been changed/selected/renamed
        //how about this, analyze the folder for documents, display card bin files present and take input on what to select/rename to use






        //for some reason, these variables cannot update after going through the method modifyRun() -is it because it is a float? maybe try it as an int and deal with multiplying by 1000?
        //ogDelay works perfectly fine but this does not (only difference is that Im not using a method to modify ogDelay and I put the algorithm in the parent)
        //SOLVED: you have to return because I am creating a new instance because I made the parameter 'colorDelay' in the modifyRun function so it is not directly doing math on the targeted variable



        private void Log_Click(object sender, RoutedEventArgs e)  //this will turn into a log button
        {
            /*
            if (nightRunDelaysCB.IsChecked == true)
            {
                nightWin.blueDelay = modifyDelayRun(nightWin.blueDelay, blueTB, "Blue");
                nightWin.redDelay = modifyDelayRun(nightWin.redDelay, redTB, "Red");
                nightWin.whiteDelay = modifyDelayRun(nightWin.whiteDelay, whiteTB, "White");
            }
            */

            AddLineMain("im commented out");
        }       
        /*
        private float modifyDelayRun(float colorDelay, TextBox inputDelay, string ID)   //okay so this method has to be a return and  blueDelay = modifyRun(); has to happen for blueDelay to update successfully when used in another completely different method
        {
            //trying to go from float in seconds to int in miliseconds
            if (!string.IsNullOrWhiteSpace(inputDelay.Text))
            {
                try
                {
                    colorDelay = float.Parse(inputDelay.Text);
                    colorDelay = colorDelay * 1000;
                }
                catch { AddLineMain($"I caught something wrong for {ID} delay"); }
                AddLineMain($"{ID} Delay: {colorDelay} milliseconds");
            }
            return colorDelay;
        }
        */


        static DateTime startTime; //this has to be a global, dont move this
        private void Night_Click(object sender, RoutedEventArgs e)
        {
            void ThreadProc()          //this is ran by the nightThread
            {
                nightWin nightWinInstance = new nightWin();
                //nightWinInstance.Owner = this;              //this calls both threads so cannot be in dispatcher as well

                Dispatcher.BeginInvoke(new Action(() =>
                {                    
                    nightWinInstance.numberOfRuns = numOfRunsTB.Text;
                    /*
                    if (anniCB.IsChecked == true)
                    {
                        nightWinInstance.anniRuns = true;
                    }
                    */

                    if (shutDownCB.IsChecked == true)                //TODO: expand upon this more to (dont place the code here tho), shut down the android app and nox, probably dont turn off the computer nor kill this application because unsafe personally but do it on a throwaway PC
                    {
                        nightWinInstance.shutDown = true;
                    }
                    
                }));
                
                nightWinInstance.Show();
                System.Windows.Threading.Dispatcher.Run();
                          
                Dispatcher.BeginInvoke(new Action(() => 
                {
                    AddLineMain($"The thread has ended at {DateTime.Now.ToLongTimeString()}");
                    if (nightWinInstance.logTime)
                    {
                        finishLabel.Content = nightWin.endTime - startTime;
                    }                    
                }));                
            }


                   // this boolean doIExist still works even when I created a new thread so the nightWin is not made on the new thread but is ran on the new thread
            if (numOfRunsTB.Text != "" && !nightWin.doIExist)   // its like as if the whole application was created and initialized on one thread or the main thread maybe?
            {
                Thread nightThread = new Thread(new ThreadStart(ThreadProc));

                nightThread.SetApartmentState(ApartmentState.STA);
                nightThread.Name = "nightThread";
                nightThread.IsBackground = true;

                nightThread.Start();

                startTime = DateTime.Now;
                startLabel.Content = startTime.ToLongTimeString();

                nightWin.doIExist = true;
                AddLineMain($"The thread '{nightThread.Name}' has succesfully initialized at {DateTime.Now.ToLongTimeString()}");
            }
            else if (nightWin.doIExist)
            {
                AddLineMain("Bruh, only one instance of the Night Run is allowed");
            }
            else
            {
                AddLineMain("Oi, how many runs do you want?");
            }
        }         

        //---------------------------------------------------------------(end of night run methods)------------------------------------------------------------------------------------------------


        private async void Color_Click(object sender, RoutedEventArgs e)
        {
            getInput(allXandYint, allTBInput);
            Utilities.SetCursorPos(allXandYint[0, 0], allXandYint[0, 1]);
            AddLineMain(utilities.GetColorAt(allXandYint[0, 0], allXandYint[0, 1]).ToString());
            await Task.Delay(500);
            AddLineMain("I do nothing rn but check color");
        }
        private async void Preview_Click(object sender, RoutedEventArgs e)   
        {
            getInput(allXandYint, allTBInput);

            await Task.Delay(500);
            Utilities.SetCursorPos(allXandYint[1, 0], allXandYint[1, 1]);
            
            AddLineMain("--------Finished Preview");
        }


        private async void  CLICK(object sender, RoutedEventArgs e) 
        {
            AddLineMain("ight m8, u have 10 seconds to position the mouse starting now");
            await Task.Delay(10000);
            int gotX = (int)Utilities.GetCursorPosition().X;
            int gotY = (int)Utilities.GetCursorPosition().Y;
            int numOfClicks = 2;                    //defaults
            int ogDelay = 300;

            if (!string.IsNullOrWhiteSpace(clickingDelayTB.Text))
            {
                try { ogDelay = Int32.Parse(clickingDelayTB.Text); }                
                catch
                {
                    AddLineMain("bruh, did you say something? Ima just delay the c l i c k i n g by 300ms");
                    ogDelay = 300;
                }
            }
            AddLineMain($"I got delay: {ogDelay} ms");



            if ((!string.IsNullOrWhiteSpace(numOfClicksTB.Text)) && infinityCB.IsChecked == false)
            {
                try { numOfClicks = Int32.Parse(numOfClicksTB.Text); }
                catch
                {
                    AddLineMain("bruh, did you say something? Ima just loop 10,000,000");
                    numOfClicks = 10000000;
                }
            }
            AddLineMain($"I got the order to click: {numOfClicks} times");



            if (infinityCB.IsChecked == true)
            {
                AddLineMain("infinnnnnnnniiiiiiiiiiiiiiiittttttttttttty");
                for (int i = 0; i > -1; i++)
                {
                    if (Keyboard.IsKeyDown(Key.RightShift)) { break; }
                    await Task.Delay(ogDelay);
                    Utilities.leftMouseClick(gotX, gotY);
                    AddLineMain($"Click number {i+1}");
                }
            }
            else
            {
                for (int i = 0; i < numOfClicks; i++)
                {
                    if (Keyboard.IsKeyDown(Key.RightShift)) { break; }
                    await Task.Delay(ogDelay);
                    Utilities.leftMouseClick(gotX, gotY);
                }
            }
           
            AddLineMain("------Finished c l i c k i n g");
        }



        //-----------------------------------------------------------------------------(Utilities)------------------------------------------------------------------------------------
        private void getInput(int[,] intArray, TextBox[,] TBarray)             //why is this an array and involves populating it? (too busy to fix this)
        {   //clear all data points first
            for (int i = 0; i < (intArray.Length / 2); i++)
            {
                intArray[i, 0] = 0;
                intArray[i, 1] = 0;
            }

            for (int i = 0; i < (intArray.Length / 2); i++)
            {
                if ((TBarray[i, 0].Text != "") && (TBarray[i, 1].Text != ""))
                {
                    try
                    {
                        intArray[i, 0] += Int32.Parse(TBarray[i, 0].Text);
                        intArray[i, 1] += Int32.Parse(TBarray[i, 1].Text);

                        AddLineMain($"Received from {TBarray[i, 0].Name}: {intArray[i, 0]}");
                        AddLineMain($"Received from {TBarray[i, 1].Name}: {intArray[i, 1]}");
                    }
                    catch
                    {
                        AddLineMain("I need only an integer in the boxes");
                    }
                }
                else
                {
                    AddLineMain($"Row {TBarray[i, 0].Name} or/and {TBarray[i, 1].Name} is blank");
                }
            }
        }

 

        private void Window_Closed(object sender, EventArgs e)
        {
            //nightWin.Close();       //set owner properties on nightWin to be owned by MainWindow so that it closes automattcailly when Main window closes
                                      // cannot because it is on a different thread
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            shutDown();            
        }


        private async void shutDown()
        {
            if (shutDownCB.IsChecked == true)
            {
                //first need to close nox which will be a couple of clicks
                //pressing the pg_up or pg_dwn opens the apps opened on the phone screen so maybe use an automated keyboard press instead of click? nah im hacking at that point
                await Task.Delay(2000); //just so that I have time to jump on the cancel
                Utilities.leftMouseClick(1900, 930);
                
                
                await Task.Delay(2000);
                Utilities.leftMouseClick(960, 540);
                
                await Task.Delay(2000);
                Utilities.leftMouseClick(1900,10);    //change this to leftMouseClick when ready


                /*
                await Task.Delay(1000);
                Utilities.SetCursorPos(960, 540);    //change this to leftMouseClick when ready as well
                await Task.Delay(2000);
                */



                /*
                //hard code all of this because it will always be the same                   
                Utilities.leftMouseClick(20, 1058);
                await Task.Delay(1000);
                Utilities.leftMouseClick(20, 1015);
                await Task.Delay(1000);
                Utilities.SetCursorPos(20, 929);    //change this to leftMouseClick when ready
                AddLineMain("CY@");
                */
            }
        }
    }

   
}