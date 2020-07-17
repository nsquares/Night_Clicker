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

        private delegate void NightRunDelegate();


        public MainWindow()
        {
            InitializeComponent();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();            

            AddLine("(Origin (0,0) is top-left of the monitor)");
            //Task.Factory.StartNew(() =>
            //{
            //  AddLine();
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

            AddLine($"Current Mouse Position: {Utilities.GetCursorPosition().ToString()}");

  
        }





        public void AddLine(string text)
        {

            //outputBox.Dispatcher.BeginInvoke(new (() =>


            outputBox.AppendText(text);
            outputBox.AppendText("\u2028"); // Linebreak, not paragraph break
            outputBox.ScrollToEnd();

            /*this.outputBox.Dispatcher.Invoke(DispatcherPriority.Render,
                new Action(() => {
                    outputBox.AppendText(text);
                    outputBox.AppendText("\u2028"); // Linebreak, not paragraph break
                    outputBox.ScrollToEnd();
                }));*/

        }


        





        //int counter = 0;
        private void Timer_Tick(object sender, EventArgs e)     //will change wpf starttime to be starttime of night_click method because finishtime is finishtime of night_click method, why even track application uptime and indirectly figure out the night run time-length?
        {

            //await Task.Delay(2000);
            timeLabel.Content = DateTime.Now.ToLongTimeString();

            utcLabel.Content = DateTime.UtcNow.ToLongTimeString(); //I should probably do some math on this boi to get it to reflect what in-app uses to determine what is a new day
            //can I get a utc-7 in the chat?


            //DateTime.Now.Subtract(d);

            //counter++;

            //DateTime.UtcNow
            //d.TimeOfDay
            //timeLabel.Content = d.Kind; //this outputs local so thats good
            //timeLabel.Content = d.Hour + ": " + d.Minute + ": " + d.Second;   =   DateTime.Now.ToLongTimeString();
        }
        



        private void Help_Click(object sender, RoutedEventArgs e)
        {
            //AddLine("lol");
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

            AddLine("im commented out");
        }       

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
                catch { AddLine($"I caught something wrong for {ID} delay"); }
                AddLine($"{ID} Delay: {colorDelay} milliseconds");
            }
            return colorDelay;
        }





        

        private void Night_Click(object sender, RoutedEventArgs e)
        {
            //pleaseStop = false;

            /*         //okay, this cannot be used right now  because AddLine() / updating the UI is obviously on a different thread than this new one I just created so it bricks
             *         //could look into this and modify more
            Thread nightRunThread = new Thread(new ThreadStart(nightRun));
            nightRunThread.SetApartmentState(ApartmentState.STA);
            nightRunThread.IsBackground = true;
            AddLine("alright im ready to start");
            nightRunThread.Start();
            // ---------------------------------------------------------------------------------------------- please do the /* but the ending one here because yea */

            //Thread pleaseKill = new Thread(new ThreadStart());

            //nightButton.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NightRunDelegate(nightRun));
            //nightRun();


            


            if (numOfRunsTB.Text != "" && !nightWin.doIExist)
            {
                nightWin nightWinInstance = new nightWin();

                nightWinInstance.Owner = this;
                nightWinInstance.Show();
                nightWinInstance.numberOfRuns = numOfRunsTB.Text;


                DateTime startTime = DateTime.Now;
                startLabel.Content = startTime.ToLongTimeString();

            }
            else
            {
                //AddLine("I should stop putting logic code inside code behind for the UI window and in turn, run it on the UI thread and take processing power");
                AddLine("Oi, how many runs do you want?");
            }



        } 
        









        //---------------------------------------------------------------(end of night run methods)------------------------------------------------------------------------------------------------




        private async void Color_Click(object sender, RoutedEventArgs e)
        {
            getInput(allXandYint, allTBInput);
            Utilities.SetCursorPos(allXandYint[0, 0], allXandYint[0, 1]);
            AddLine(utilities.GetColorAt(allXandYint[0, 0], allXandYint[0, 1]).ToString());
            await Task.Delay(500);
            AddLine("I do nothing rn but check color");
        }
        private async void Preview_Click(object sender, RoutedEventArgs e)   
        {
            getInput(allXandYint, allTBInput);

            await Task.Delay(500);
            Utilities.SetCursorPos(allXandYint[1, 0], allXandYint[1, 1]);
            
            AddLine("--------Finished Preview");

            /*new Thread(() =>
            {
                //Task.Delay(250);
                SetCursorPos(xInput1, yInput1);

            }).Start();*/
        }


        private async void  CLICK(object sender, RoutedEventArgs e) 
        {

            //clickingDelayTB
            AddLine("ight m8, u have 10 seconds to position the mouse starting now");
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
                    AddLine("bruh, did you say something? Ima just delay the c l i c k i n g by 300ms");
                    ogDelay = 300;
                }
            }
            AddLine($"I got delay: {ogDelay} ms");



            if ((!string.IsNullOrWhiteSpace(numOfClicksTB.Text)) && infinityCB.IsChecked == false)
            {
                try { numOfClicks = Int32.Parse(numOfClicksTB.Text); }
                catch
                {
                    AddLine("bruh, did you say something? Ima just loop 10,000,000");
                    numOfClicks = 10000000;
                }
            }
            AddLine($"I got the order to click: {numOfClicks} times");



            if (infinityCB.IsChecked == true)
            {
                AddLine("infinnnnnnnniiiiiiiiiiiiiiiittttttttttttty");
                for (int i = 0; i > -1; i++)
                {
                    if (Keyboard.IsKeyDown(Key.RightShift)) { break; }
                    await Task.Delay(ogDelay);
                    Utilities.leftMouseClick(gotX, gotY);
                    AddLine($"Click number {i+1}");
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
           
            AddLine("------Finished c l i c k i n g");
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

                        AddLine($"Received from {TBarray[i, 0].Name}: {intArray[i, 0]}");
                        AddLine($"Received from {TBarray[i, 1].Name}: {intArray[i, 1]}");
                    }
                    catch
                    {
                        AddLine("I need only an integer in the boxes");
                    }
                }
                else
                {
                    AddLine($"Row {TBarray[i, 0].Name} or/and {TBarray[i, 1].Name} is blank");
                }
            }
        }

 

        private void Window_Closed(object sender, EventArgs e)
        {
            //nightWin.Close();       //set owner properties on nightWin to be owned by MainWindow so that it closes automattcailly when Main window closes


            //this.Close();
            //need to abort or shut down all threads or like deal with this child parent window relationship that im about to make


            //Console.WriteLine(Thread.CurrentThread.Name);



            //GCCollectionMode.Forced

            //Utilities.thebigMUTEXboi.Close();

            //Utilities.thebigMUTEXboi.Dispose();

            System.Windows.Application.Current.Shutdown();
        }




        /*
        private void Window_KeyDown(object sender, KeyEventArgs e) //does not work unless wpf has keyboard focus but this will never happen because I will always be clicking and focusing on other apps
        {                                                                     //is there a way that I can have keyboard focus lock or global lock, I do not thinking a sperate thread will do it but it might since the thread is always running in the background and then figure out how to make a thread kill another thread or just how to end a specific thread, maybe put shift key on UI thread (dispatcher)?
            
            //order of events, I get the hook to globally monitor the keyboard, then create a new thread that will constantly run/check if the key is pressed globally, and then exit the thread that runs nightRun. Dont put anything on the UI thread and figure that out later when I know what logically should/can be on the UI thread
            //                                                                                                               (while loop?)

            if (e.Key == Key.RightShift) //Keyboard.IsKeyDown(Key.RightShift) 
            {
                AddLine("SHIFT DOWN");
            }
        }
        */
    }
}