using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace QR
{
    /// <summary>
    /// Nativeな構造体のヘルパークラス
    /// </summary>
    static public class NativeStructHelper
    {
        public static T ReadStruct<T>( Stream s)
            where T : struct
        {
            //構造体のサイズを取得
            int size = Marshal.SizeOf(typeof(T));

            //ストリームからbyteを読み取る
            byte[] nativeStruct = new byte[size];
            if (s.Read(nativeStruct, 0, size) != size)
                throw new Exception("streamから必要なbyte数を読み取れませんでした");

            return ReadStruct<T>(nativeStruct);
        }

        public static T ReadStruct<T>(byte[] nativeStruct)
            where T : struct
        {
            //byteのgcハンドル取得
            GCHandle gchBytes = GCHandle.Alloc(nativeStruct, GCHandleType.Pinned);
            try
            {
                //Structに変換
                return (T)Marshal.PtrToStructure(gchBytes.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                gchBytes.Free();
            }
        }


        /// <summary>
        /// structをストリームに書き込みます
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="struct"></param>
        public static void WriteStruct<T>(Stream s,T @struct)
            where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                byte[] bytes = new byte[size];
                Marshal.StructureToPtr(@struct, ptr, false);
                Marshal.Copy(ptr, bytes, 0, size);

                s.Write(bytes, 0, size); //ストリームに書き込み
            }
            finally
            {
                Marshal.FreeHGlobal(ptr); //確保したポインタを解放する
            }
        }
    }
}
