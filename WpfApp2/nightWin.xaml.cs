using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for nightWin.xaml
    /// </summary>
    public partial class nightWin : Window
    {
        //this leads to a stack overflow guy
        //MainWindow windowMain = new MainWindow();  //so you do this only for non-static methods and create a new instance for this class to use
        // for static methods, 

        Utilities utilities = new Utilities();
        private KeyboardInput globalKeyboard;

        public static bool doIExist = false;

        public nightWin()
        {
            InitializeComponent();

            


            globalKeyboard = new KeyboardInput();
            globalKeyboard.KeyBoardKeyPressed += globalKeyboard_KeyBoardKeyPressed;

            imRUNNING();
            

        }

        private async void imRUNNING()
        {
            for (int i = 0; i<1000; i++)
            {
                await Task.Delay(100);
                AddLine($"Hello {i}");
            }
        }


        private void globalKeyboard_KeyBoardKeyPressed(object sender, EventArgs e)  //goal is this, I have this call .close() on the second window that displays a conole-like feedback log while the night run executes
        {
            if (Keyboard.IsKeyDown(Key.RightShift) && this.IsLoaded)
            {
                //pleaseStop = true;
                //AddLine("Right_shift has been pressed, thats all I do right now");

                /*
                using (nightButton.Dispatcher.DisableProcessing())                      //all this can do is pause the run and resume after the brackets execute fully
                {
                    AddLine("please stop but dont exit");
                }
                */

                //nightButton.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);  // I might as well just go this.Close()
                //AddLine("nope, I actually just ended the dispatcher");


                this.Close();  //okay, this will throw an error when releasing the mutex so im thinking of this: what if I just lock the window in place and then have this keyboard hook do a leftmouseclick() on the fricking close button on the window itself

                //Utilities.leftMouseClick(658, 430);
                //Utilities.SetCursorPos(658, 430);

            }
            //throw new NotImplementedException();
            //AddLine("this is it");            
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


        public float blueDelay = 1500;    //you can modify these while running the application
        public float redDelay = 1500;     //there is no point but will keep as variables
        public float whiteDelay = 5000;

        public string numberOfRuns = "";

        private async void nightRun()
        {
            AddLine("To end the Night Run, move the mouse or hold the right shift key");
            bool mouseShok = false;


            async Task oneClick(int x, int y, int delay, string colorHex, string whatColor)
            {
                AddLine($"Delay for {delay / 1000} seconds then click _{whatColor.ToUpper()}_ button");    //delete this 
                Utilities.SetCursorPos(x, y);
                await Task.Delay(delay);                                                            // and delete this once we get the global key logger working to save time

                if (Utilities.GetCursorPosition().ToString() == $"{x},{y}" && mouseShok == false)      //also why do the mouseShok here when it should really be in the for loop in checkColorBeforeClicking()?
                {
                    AddLine($"Checking {whatColor} button now");

                    for (int j = 0; j < 250; j++) //last for max 25 minutes even  //actually I do not know because the delay can change
                    {
                        if (Keyboard.IsKeyDown(Key.RightShift))
                        {
                            AddLine("Right Shift is pressed \n KEEP HOLDING");
                            break;
                        }
                        await Task.Delay(delay);

                        if (j == 0) { AddLine($"How much time has passed right before next color check: {(((float)(j + 1) * (delay / 1000)) / 60)} minutes"); }
                        else { replaceLine($"How much time has passed right before next color check: {(((float)(j) * (delay / 1000)) / 60)} minutes", $"How much time has passed right before next color check: {(((float)(j + 1) * (delay / 1000)) / 60)} minutes"); }

                        if (utilities.GetColorAt(x, y).ToString() == colorHex)  //nah, its going to be clicked when a specific hex color is found at this specific pixel
                        {
                            AddLine(utilities.GetColorAt(x, y).ToString());          //feedback
                            AddLine($"Delay for {delay / 1000} seconds before clicking");
                            await Task.Delay(delay);
                            Utilities.leftMouseClick(x, y);
                            return;
                        }
                    }
                }
                else
                {
                    AddLine($"Whoa, you moved the mouse before the {whatColor.ToUpper()} color check!  _m o u s e   w a s   s h a k e n_");
                    mouseShok = true;
                }
            }



            int firstX = 1625;          //hard code all of these variables
            int firstY = 950;

            int secondX = 1600;
            int secondY = 850;

            int pausePixelX = 101;      // TODO: hey man change these two variables to be the pause button again and... (yea, no matter what)
            int pausePixelY = 925;

            string blueHex = "#FFFFFFFF";  // FF005F89   (originial)
            string redHex = "#FFFFFFFF";  // FF792201   

            string whiteHex = "#FFFFFFFF";  // TODO: find the color of white being overlayed by the black end screen and test to see if it is always the same color on every level (is anni different than other levels because of that unique anni report rectangle in the middle of the screen)
                                            //nah nah nah, this has to be white and the boolean has to be "NOT EQUAL", the overlay randomly blurs the screen and color of blur is different for almost all stages so it does not matter

            
            if (numberOfRuns != "")          //user must input the number of runs for the for loop to run through for any of this to work
            {

                // wait what happens if it cannot be parsed?
                for (int i = 0; i < Int32.Parse(numberOfRuns); i++) //will do however much based on input
                {


                    AddLine($"\n ---Night_Run Counter: {i + 1}");
                    //if (Keyboard.IsKeyDown(Key.RightShift)) { AddLine("Right Shift is pressed, ok"); break; }


                    await oneClick(firstX, firstY, (int)blueDelay, blueHex, "blue");

                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------

                    await oneClick(secondX, secondY, (int)redDelay, redHex, "red");

                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------


                    //here for visual indication
                    AddLine("Wait for 25 seconds before first color check. I will move to 'Results' text without clicking");
                    await Task.Delay(25000); //there will be a loading screen and the mission starting so this is why it is 20 seconds, no real rush here

                    AddLine("Initializing color check for loop phase 3 (WHITE)");
                    await oneClick(pausePixelX, pausePixelY, (int)whiteDelay, whiteHex, "white");



                }


                AddLine("---------All Knight Runs Finished");

                /*
                if (shutDownCB.IsChecked == true)
                {
                    //first need to close nox which will be a couple of clicks
                    //pressing the pg_up or pg_dwn opens the apps opened on the phone screen so maybe use an automated keyboard press instead of click?


                    //hard code all of this because it will always be the same                   
                    leftMouseClick(20, 1175);
                    await Task.Delay(1000);
                    leftMouseClick(20, 1140);
                    await Task.Delay(1000);
                    SetCursorPos(20, 1050);    //change this to leftMouseClick when ready
                    AddLine("CY@");

                }

                finishLabel.Content = DateTime.Now.Subtract(startTime);




                */
            }
        }





        public void replaceLine(string oldLine, string newLine)
        {
            TextRange text = new TextRange(outputBox.Document.ContentStart, outputBox.Document.ContentEnd);   //select all text
            TextPointer current = text.Start.GetInsertionPosition(LogicalDirection.Forward);
            while (current != null)                                                                    //cursor through like sql
            {
                string textInRun = current.GetTextInRun(LogicalDirection.Forward);
                if (!string.IsNullOrWhiteSpace(textInRun))
                {
                    int index = textInRun.IndexOf(oldLine);
                    if (index != -1)
                    {
                        TextPointer selectionStart = current.GetPositionAtOffset(index, LogicalDirection.Forward);
                        TextPointer selectionEnd = selectionStart.GetPositionAtOffset(oldLine.Length, LogicalDirection.Forward);
                        TextRange selection = new TextRange(selectionStart, selectionEnd);
                        selection.Text = newLine;
                        selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                        outputBox.Selection.Select(selection.Start, selection.End);                      // this is used to update
                        //outputBox.Focus();   not needed really i think
                    }
                }
                current = current.GetNextContextPosition(LogicalDirection.Forward);
            }
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("ima take out the trash (i.e. dispose key-log and GC)");
            globalKeyboard.Dispose();

            GC.Collect();

            GC.WaitForPendingFinalizers();



            //MainWindow blah = new MainWindow();
            //blah.nightThread.Abort();


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            doIExist = true;
        }
    }
}
