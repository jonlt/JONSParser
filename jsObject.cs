using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace JONSParser
{
    public class jsObject : DynamicObject, IEnumerable<jsObject>
    {
        protected const object undefined = null;
        protected object value = undefined;
        protected Dictionary<string, jsObject> Fields { get; private set; }

        public jsObject(object val)
        {
            value = val;
            Fields = new Dictionary<string, jsObject>();
        }

        public jsObject()
            : this(undefined)
        {

        }

        public override string ToString()
        {
            if (Fields.Count > 0)
            {

                var sb = new StringBuilder();
                sb.Append("{");
                foreach (var field in Fields)
                {
                    sb.Append(field.Key + ":" + field.Value.ToString() + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("}");

                return sb.ToString();
            }
            else
            {
                if (value != null)
                    return value.ToString();
                else
                    return "{}";
            }
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(string))
            {
                result = this.ToString();
                return true;
            }
            result = undefined;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (binder.Name == "__defineProperty__")
            {
                if (binder.CallInfo.ArgumentCount == 2)
                {
                    Fields[(string)args[0]] = (jsObject)args[1];
                }
                result = undefined;
            }
            else if (binder.Name == "hasOwnProperty" || binder.Name == "__has")
            {
                if (binder.CallInfo.ArgumentCount == 1)
                {
                    var arg = "";
                    try
                    {
                        arg = ((int)args[0]).ToString();
                        result = Fields.Keys.Contains(arg);
                        return true;
                    }
                    catch { }
                    try
                    {
                        arg = (string)args[0];
                        result = Fields.Keys.Contains(arg);
                        return true;
                    }
                    catch
                    {
                        result = false;
                        return true;
                    }
                }
            }

            result = undefined;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            try
            {
                Fields[binder.Name] = new jsObject(value);
                return true;
            }
            catch
            {
                return true;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name == "__fields__")
            {
                result = Fields;
                return true;
            }

            try
            {
                result = Fields[binder.Name];
                return true;
            }
            catch
            {
                result = undefined;
                return true;
            }
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var index = indexes[0];
            try
            {
                result = Fields[(string)index];
                return true;
            }
            catch { }

            try
            {
                result = Fields[((int)index).ToString()];
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
                Fields[(string)index] = new jsObject(value);
                return true;
            }
            catch { }

            try
            {
                Fields[((int)index).ToString()] = new jsObject(value);
                return true;

            }
            catch
            {
                return true;
            }

        }

        #region IEnumerable<jobject> Members

        public IEnumerator<jsObject> GetEnumerator()
        {
            return Fields.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Fields.Values.GetEnumerator();
        }

        #endregion
    }
}
