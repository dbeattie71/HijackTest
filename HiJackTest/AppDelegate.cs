using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace HiJackTest
{
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        UIWindow window;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // create a new window instance based on the screen size
            window = new UIWindow(UIScreen.MainScreen.Bounds);
			
            var s = new StartViewController();
            window.RootViewController = s;
            // If you have defined a root view controller, set it here:
            // window.RootViewController = myViewController;
			
            // make the window visible
            window.MakeKeyAndVisible();
			
            return true;
        }
    }
}

