using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.IO;

namespace K2H
{
    class Program
    {
        [ComImport]
        [Guid("019F7152-E6DB-11D0-83C3-00C04FDDB82E")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IFELanguage
        {
            void Open();
            
            void Close();

            void Dummy5(); /* DO NOT CALL */ // GetJMorphResult
            //[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            //int GetJMorphResult(uint dwRequest, uint dwCMode, int cwchInput, [MarshalAs(UnmanagedType.LPWStr)] string pwchInput, IntPtr pfCInfo, out IntPtr ppResult);
            
            void Dummy6(); /* DO NOT CALL */
            
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetPhonetic([MarshalAs(UnmanagedType.BStr)] string str, int start, int length);
            
            void Dummy8(); /* DO NOT CALL */
        }

        [STAThread] // デフォルトのMTAThreadだと動かない
        static void Main(string[] args)
        {
            TextReader input;

            // 読み込み元はファイル
            try
            {
                Console.OutputEncoding = Encoding.UTF8;

                if (args.Length == 0)
                {
                    // 読み込み元は標準入力
                    input = Console.In;
                    Console.InputEncoding = Encoding.UTF8;
                }
                else
                {
                    input = new StreamReader(args[0], Encoding.UTF8);
                }

                var fel = Activator.CreateInstance(Type.GetTypeFromProgID("MSIME.Japan")) as IFELanguage;
                fel.Open();

                string line;
                while ((line = input.ReadLine()) != null)
                {
                    Console.WriteLine(fel.GetPhonetic(line, 1, -1));
                }

                fel.Close();
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
