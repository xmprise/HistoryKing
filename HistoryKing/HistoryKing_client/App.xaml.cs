using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
namespace HistoryKing_client
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private static DispatcherOperationCallback exitFrameCallback = new DispatcherOperationCallback(ExitFrame);
        
        public static void DoEvents()
        {
            // Create new nested message pump.
            DispatcherFrame nestedFrame = new DispatcherFrame();

            // Dispatch a callback to the current message queue, when getting called,
            // this callback will end the nested message loop.
            // note that the priority of this callback should be lower than the that of UI event messages.
            DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(
                                                  DispatcherPriority.Background, exitFrameCallback, nestedFrame);

            // pump the nested message loop, the nested message loop will
            // immediately process the messages left inside the message queue.
            Dispatcher.PushFrame(nestedFrame);

            // If the "exitFrame" callback doesn't get finished, Abort it.
            if (exitOperation.Status != DispatcherOperationStatus.Completed)
            {
                exitOperation.Abort();
            }
        }

        private static Object ExitFrame(Object state)
        {
            DispatcherFrame frame = state as DispatcherFrame;

            // Exit the nested message loop.
            frame.Continue = false;
            return null;
        }
    }
}
