using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class ShiftEvent : EventArgs    //thanks to 'https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/events/how-to-publish-events-that-conform-to-net-framework-guidelines'
    {
        public event EventHandler<EventArgs> RaiseCustomEvent;
        public static bool doIExist = false;

        public ShiftEvent()
        {
            //switchTime();

            // Write some code that does something useful here
            // then raise the event. You can also raise an event
            // before you execute a block of code.

        }
        public void switchTime()
        {
            if (doIExist)
            {
                doIExist = false;
                Console.WriteLine("\n\n\n\n\n\n\n SWITCH OFF \n\n\n\n\n\n\n");
            }
            else
            {
                doIExist = true;
                Console.WriteLine("\n\n\n\n\n\n\n SWITCH ON\n\n\n\n\n\n\n");
            }

            // Event will be null if there are no subscribers
            if (RaiseCustomEvent != null)
            {
                Console.WriteLine("\n\n\n\n\n\n\nHERE\n\n\n\n\n\n\n");
                RaiseCustomEvent(this, new EventArgs());
            }
        }
        // Wrap event invocations inside a protected virtual method
        // to allow derived classes to override the event invocation behavior
    }

}
