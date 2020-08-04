//N^2
using System;
using System.Collections.Generic;
using System.Data;
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
        public static bool doIExist = false;
        private KeyboardInput globalKeyboard;

        public nightWin()    //okay, so I think this is the only method which runs on the main thread and all other methods run on my custom because if I take a custom method and call it in this Main method, exception will be thrown which says object (on the UI like richtextbox) is owned or created by a different thread than this one or the Main thread
        {                           //so it is like the main thread initializes the window and the custom thread runs or maintains it.
                                      //objects on the UI are now owned by the custom thread in the end
            InitializeComponent();

            globalKeyboard = new KeyboardInput();
            globalKeyboard.KeyBoardKeyPressed += globalKeyboard_KeyBoardKeyPressed;

            //pro tip: this should be treated as the main method of the window and this runs on the UI / Main thread, stuff below can run on a new thread that you create            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)  //this runs on custom thread xd
        {
            AddLine($"The thread '{Dispatcher.Thread.Name}' has succesfully initialized");
            nightRun();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            doIExist = false;

            globalKeyboard.Dispose();

            GC.Collect();

            GC.WaitForPendingFinalizers();

            //Environment.Exit(0); //ends the whole application so not this one

            Dispatcher.InvokeShutdown();
        }        

        private void globalKeyboard_KeyBoardKeyPressed(object sender, EventArgs e)  //goal is this, I have this call .close() on the second window that displays a conole-like feedback log while the night run executes
        {
            if (Keyboard.IsKeyDown(Key.RightShift)) //&& this.IsLoaded
            {              
                /*
                using (nightButton.Dispatcher.DisableProcessing())         //all this can do is pause the run and resume after the brackets execute fully
                {
                    AddLine("please stop but dont exit");
                }
                */             
                this.Close();
            }
        }        

        public float blueDelay = 2000;    //you can modify these while running the application
        public float redDelay = 2000;     //there is no point but will keep as variables
        public float whiteDelay = 2000; //8000

        public string numberOfRuns = "";
        public static DateTime endTime;
        public bool logTime = false;
        public bool shutDown = false;
        //public bool anniRuns = false;

        private async void nightRun()
        {
            int firstX = 1625;          //hard code all of these variables
            int firstY = 950;

            int secondX = 1600;
            int secondY = 850;

            int pausePixelX = 1696; //   101
            int pausePixelY = 100;  //   925

            string blueHex = "#FF005F89";  // FF005F89   (originial)
            string redHex = "#FF792201";  // FF792201   
            string whiteHex = "#FFFFFFFF";  // TODO: find the color of white being overlayed by the black end screen and test to see if it is always the same color on every level (is anni different than other levels because of that unique anni report rectangle in the middle of the screen)
                                            //nah nah nah, this has to be white and the boolean has to be "NOT EQUAL", the overlay randomly blurs the screen and color of blur is different for almost all stages so it does not matter


            AddLine("To end the Night Run, move the mouse or press the right shift key");
            bool mouseShok = false;

            async Task oneClick(int x, int y, int delay, string colorHex, string whatColor)
            {
                int j = 0;
                Utilities.SetCursorPos(x, y);                
                AddLine($"Checking {whatColor} button now");

                while (true)
                {                                        
                    if (Utilities.GetCursorPosition().ToString() == $"{x},{y}" && mouseShok == false)
                    {
                        await Task.Delay(delay);

                        if (j == 0) { AddLine($"How much time has passed right before next color check: {(((float)(j + 1) * (delay / 1000)) / 60)} minutes"); }
                        else { replaceLine($"How much time has passed right before next color check: {(((float)(j) * (delay / 1000)) / 60)} minutes", $"How much time has passed right before next color check: {(((float)(j + 1) * (delay / 1000)) / 60)} minutes"); }

                        if (colorHex == "#FFFFFFFF")  //this is for annihaltion runs (blur is inconsistent)
                        {
                            if (utilities.GetColorAt(x, y).ToString() != colorHex)
                            {
                                AddLine($"White has disappear and {utilities.GetColorAt(x, y).ToString()} was found");

                                while (utilities.GetColorAt(firstX, firstY).ToString() != blueHex)
                                {                                    
                                    AddLine($"Delay for {(delay*4) / 1000} seconds before clicking");
                                    await Task.Delay(delay*4);                                          
                                    Utilities.leftMouseClick(x, y);
                                }
                                return;
                            }
                        }
                        else
                        {
                            if (utilities.GetColorAt(x, y).ToString() == colorHex)  //nah, its going to be clicked when a specific hex color is found at this specific pixel
                            {
                                AddLine($"{utilities.GetColorAt(x, y).ToString()} found");          //feedback
                                AddLine($"Delay for {delay / 1000} seconds before clicking");
                                await Task.Delay(delay);
                                Utilities.leftMouseClick(x, y);
                                return;
                            }
                        }
                    }
                    else
                    {
                        AddLine($"Whoa, you moved the mouse before or during the {whatColor.ToUpper()} color check!  _m o u s e   w a s   s h a k e n_");
                        mouseShok = true;
                        break;
                    }
                    j++;
                }               
            }
            //-------------------------------------------------------------------------------(end of local method)----------------------------------------------------------------------------

            if (numberOfRuns != "")          //user must input the number of runs for the for loop to run through for any of this to work
            {
                AddLine("Starting in 3 seconds:");
                AddLine("3");
                await Task.Delay(1000);
                AddLine("2");
                await Task.Delay(1000);
                AddLine("1");
                await Task.Delay(1000);

                try
                {
                    for (int i = 0; i < Int32.Parse(numberOfRuns); i++) //will do however much based on input
                    {
                        AddLine($"\n ---Night_Run Counter: {i + 1}");

                        await oneClick(firstX, firstY, (int)blueDelay, blueHex, "blue");
                        await oneClick(secondX, secondY, (int)redDelay, redHex, "red");

                        if (mouseShok == false)
                        {
                            AddLine("Wait for 25 seconds before first color check. I will move to the pause button without clicking");
                            await Task.Delay(25000); //there will be a loading screen and the mission starting so this is why it is 25 seconds, no real rush here
                        }
                        await oneClick(pausePixelX, pausePixelY, (int)whiteDelay, whiteHex, "white");

                        if (mouseShok == true)
                        {
                            break;
                        }
                    }
                }
                catch (System.FormatException)
                {
                    AddLine("Uh, it seems the input was not an integer");
                }
                
                logTime = true;
                endTime = DateTime.Now;
                AddLine("---------All Knight Runs Finished");



                // shut down protocol used to be here

            }
        }

        public void replaceLine(string oldLine, string newLine)
        {
            TextRange text = new TextRange(outputBoxNight.Document.ContentStart, outputBoxNight.Document.ContentEnd);   //select all text
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
                        outputBoxNight.Selection.Select(selection.Start, selection.End);                      // this is used to update
                        //outputBoxNight.Focus();   not needed really i think
                    }
                }
                current = current.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        public void AddLine(string text)
        {

            outputBoxNight.AppendText(text);
            outputBoxNight.AppendText("\u2028"); // Linebreak, not paragraph break
            outputBoxNight.ScrollToEnd();

            /*   //okay, keep this because this does actually allow me to "cross threads", specifically between the UI / Main thread and my custom thread
            outputBoxNight.Dispatcher.BeginInvoke(new Action(() =>
            {
                outputBoxNight.AppendText(text);
                outputBoxNight.AppendText("\u2028"); // Linebreak, not paragraph break
                outputBoxNight.ScrollToEnd();
            }));
            */

            /*this.outputBoxNight.Dispatcher.Invoke(DispatcherPriority.Render,
                new Action(() => {
                    outputBoxNight.AppendText(text);
                    outputBoxNight.AppendText("\u2028"); // Linebreak, not paragraph break
                    outputBoxNight.ScrollToEnd();
                }));*/
        }
    }
}
