/* REMOVE ME
using Microsoft.Phone.Controls;
using System.Threading.Tasks;
using System.Windows;
REMOVE ME */ 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Moga.Windows.Phone.Unity
{
    enum ControllerMode { Poll, Listen }

    class ControllerUnity
    {
        // Private Listener Class
		
/* REMOVE ME
        private class ControllerUnityListener : IControllerListener
        {

            public ControllerUnityListener()
            {
            }

            public void OnKeyEvent(KeyEvent e)
            {
                Moga_Controller.ReceiveSingleButtonState((int)e.KeyCode, (int)e.Action);
            }

            public void OnMotionEvent(MotionEvent e)
            {
                Moga_Controller.ReceiveSingleAxisState((int)e.Axis, e.AxisValue);
            }

            public void OnStateEvent(StateEvent e)
            {
                Moga_Controller.ReceiveSingleStateState((int)e.StateKey, (int)e.StateValue);
                if (e.StateKey == ControllerState.Connection && e.StateValue == ControllerResult.Disconnected)
                {
                    ClearState();
                }
            }

            private static void ClearState()
            {
                Moga_Controller.ReceiveAxisState(new Dictionary<int, float>());
                Moga_Controller.ReceiveButtonState(new Dictionary<int, int>());
                Moga_Controller.ReceiveStateState(new Dictionary<int, int>());
            }
        }
		 
        // Members

        private static ControllerManager controller;

        private static Thread controllerThread;

        // Constructor

        public static void Connect(ControllerMode mode)
        {

            // 1 (WP Ready)
            controller = new ControllerManager();
            controllerThread = new Thread(ControllerThreadRun);


            // 2 (Unity Ready)
            controller.Connect();
            if (mode == ControllerMode.Poll)
            {
                controllerThread.Start();
            }
            else if (mode == ControllerMode.Listen)
            {
                ControllerUnityListener listener = new ControllerUnityListener();
                controller.SetListener(listener);
            }
        }

        public static void Activated()
        {
            controller.Resuming();
        }

        public static void Deactivated()
        {
            controller.Suspending();
        }

        private static void ControllerThreadRun()
        {
            // State store
            Dictionary<int, int> buttonState = new Dictionary<int, int>();
            Dictionary<int, float> axisState = new Dictionary<int, float>();
            Dictionary<int, int> stateState = new Dictionary<int, int>();

            int failCount = 0;
            while (true)
            {

                for (KeyCode k = KeyCode.A; k <= KeyCode.DirectionRight; k++)
                {
                    buttonState[(int)k] = (int)controller.GetKeyCode(k);
                }

                for (Axis a = Axis.X; a <= Axis.RightTrigger; a++)
                {
                    axisState[(int)a] = controller.GetAxisValue(a);
                }

                for (ControllerState s = ControllerState.Connection; s <= ControllerState.SelectedVersion; s++)
                {
                    stateState[(int)s] = (int)controller.GetState(s);
                }

                try
                {
                    Moga_Controller.ReceiveButtonState(buttonState);
                    Moga_Controller.ReceiveAxisState(axisState);
                    Moga_Controller.ReceiveStateState(stateState);
                    failCount = 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ERROR: Failed to send state to Unity.");
                    failCount++;
                    if (failCount > 5) break;
                    Thread.Sleep(2000); // Wait incase Unity init is still running.
                    // NOTE: This will retry 5 times over 10 seconds.
                }

                Thread.Sleep(10);
            }

            Debug.WriteLine("FATAL ERROR: Failed to send state to Unity on five consecutive attempts");

        }
REMOVE ME */ 
    }
}
