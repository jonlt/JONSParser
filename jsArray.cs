using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace JONSParser
{
    public class jsArray : jsObject
    {
        private int length = 0;
        public override string ToString()
        {
            if (Fields.Count > 0)
            {
                var sb = new StringBuilder();
                sb.Append("[");
                for (int i = 0; i < length + 1; i++)
                {
                    var key = i.ToString();
                    if (Fields.Keys.Contains(key))
                        sb.Append(Fields[key].ToString() + ",");
                    else
                        sb.Append("undefined,");
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");

                return sb.ToString();
            }
            else
            {
                if (value != null)
                    return value.ToString();
                else
                    return "[]";
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name == "length")
            {
                result = length;
                return true;
            }
            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (binder.Name == "length")
            {
                length = (int)value;
                return true;
            }
            return base.TrySetMember(binder, value);
        }


        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var index = indexes[0];
            try
            {
                int i = Int32.Parse(((string)index));
                var str_index = i.ToString();
                if (i > length)
                {
                    result = undefined;
                    return true;
                }
                result = Fields[str_index];
                return true;
            }
            catch { }

            try
            {
                int i = (int)index;
                if (i > length)
                {
                    result = undefined;
                    return true;
                }
                result = Fields[i.ToString()];
                return true;

            }
            catch
            {
                result = undefined;
                return true;
            }

        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            var index = indexes[0];
            try
            {
                // check if the sting is a int string ( like "12" or "2342")

                int i = Int32.Parse(((string)index));
                if (i > length)
                    length = i;

                Fields[i.ToString()] = new jsObject(value);
                return true;
            }
            catch { }

            try
            {
                int i = (int)index;
                if (i > length)
                    length = i;

                Fields[i.ToString()] = new jsObject(value);
                return true;

            }
            catch
            {
                return true;
            }

        }
    }
}
