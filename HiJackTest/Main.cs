using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using System.Runtime.InteropServices;
using ObjCRuntime;

namespace HiJackTest
{
    public class Application
    {
        [DllImport("/usr/lib/libobjc.dylib")]
        extern static IntPtr class_getInstanceMethod(IntPtr classHandle, IntPtr Selector);

        [DllImport("/usr/lib/libobjc.dylib")]
        extern static Func<IntPtr,IntPtr,IntPtr> method_getImplementation(IntPtr method);

        [DllImport("/usr/lib/libobjc.dylib")]
        extern static IntPtr imp_implementationWithBlock(ref BlockLiteral block);

        [DllImport("/usr/lib/libobjc.dylib")]
        extern static void method_setImplementation(IntPtr method, IntPtr imp);

        static Func<IntPtr,IntPtr,IntPtr> original_impl;

        static void HijackWillMoveToSuperView()
        {
            var method = class_getInstanceMethod(new UIView().ClassHandle, new Selector("willMoveToSuperview:").Handle);
            original_impl = method_getImplementation(method);
            var block_value = new BlockLiteral();
            CaptureDelegate d = MyCapture;
            block_value.SetupBlock(d, null);
            var imp = imp_implementationWithBlock(ref block_value);
            method_setImplementation(method, imp);
        }

        delegate void CaptureDelegate(IntPtr block,IntPtr self,IntPtr uiView);

        [MonoPInvokeCallback(typeof(CaptureDelegate))]
        static void MyCapture(IntPtr block, IntPtr self, IntPtr uiView)
        {
            Console.WriteLine("Moving to: {0}", Runtime.GetNSObject(uiView));
            original_impl(self, uiView);
            Console.WriteLine("Added");
        }

        static void Main(string[] args)
        {
            HijackWillMoveToSuperView();

            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
