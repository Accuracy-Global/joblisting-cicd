using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobPortal.Library.Extensions
{
    static public class Extension
    {
        static public T? NullableCast<T>(object obj) where T : struct
        {
            return obj as T?;
        }

        static public T Cast<T>(object obj) where T : struct
        {

            return obj == null || obj == DBNull.Value ? default(T) : (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}
