﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AppKit;
using CoreFoundation;
using Foundation;

namespace HelloAsync
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Button.Activated += async (s, e) => await Button_Activated(s, e);
        }

        private async Task Button_Activated(object sender, EventArgs e)
        {
            Label.StringValue = "開始";
            await AsyncVoidExceptions_CannotBeCaughtByCatch();
            await Task.Run(() => HeavyMethod());
            Label.StringValue = "完了";
        }

        /// <summary>
        ///     時間のかかる処理
        /// </summary>
        public void HeavyMethod()
        {
            for (var i = 0; i <= 100; i++)
            {
                Thread.Sleep(100);
                // UI処理をUIスレッドで行うための記述
                InvokeOnMainThread(() =>
                {
                    Progress.IncrementBy(1);
                });
            }
        }

        private async Task ThrowExceptionAsync()
        {
            throw new InvalidOperationException();
        }
        public async Task AsyncVoidExceptions_CannotBeCaughtByCatch()
        {
            try
            {
                await ThrowExceptionAsync();
            }
            catch (Exception)
            {
                // The exception is never caught here!
                throw;
            }
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
