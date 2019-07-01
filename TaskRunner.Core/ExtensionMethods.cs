using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

namespace TaskRunner.Core
{
    public static class ExtensionMethods
    {

        // Converts a list of objects to a DataTable
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            for (int i = 0; i <= properties.Count - 1; i++)
            {
                PropertyDescriptor property = properties[i];
                dt.Columns.Add(property.Name, System.Nullable.GetUnderlyingType(property.PropertyType) is null ? property.PropertyType : System.Nullable.GetUnderlyingType(property.PropertyType)
    );
            }
            object[] values = new object[properties.Count - 1 + 1];
            foreach (T item in data)
            {
                for (int i = 0; i <= values.Length - 1; i++)
                    values[i] = properties[i].GetValue(item);
                dt.Rows.Add(values);
            }
            return dt;
        }

        public static DataTable ToDataTableOne<T>(this T Item) where T : class
        {
            DataTable dt = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i <= propertyDescriptorCollection.Count - 1; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);
                dt.Columns.Add(propertyDescriptor.Name, type);
            }

            object[] values = new object[propertyDescriptorCollection.Count - 1 + 1];
            for (int i = 0; i <= values.Length - 1; i++)
                values[i] = propertyDescriptorCollection[i].GetValue(Item);

            dt.Rows.Add(values);
            return dt;
        }

        public static TU Map<TU, T>(this TU target, T source)
        {
            PropertyDescriptorCollection sourceproperties = TypeDescriptor.GetProperties(source);
            PropertyDescriptorCollection targetproperties = TypeDescriptor.GetProperties(target);

            foreach (PropertyDescriptor pd in targetproperties)
            {
                foreach (PropertyDescriptor _pd in sourceproperties)
                {
                    if (pd.Name == _pd.Name)
                        pd.SetValue(target, _pd.GetValue(source));
                }
            }

            return target;
        }

        public static bool IsNumeric(this string text)
        {
            return double.TryParse(text, out double test);
        }

        public static bool ConvertToBoolean(string value)
        {
            // First checks if value is an integer (i.e. 1 = True, anything else = False)
            if ((IsNumeric(value)))
            {
                int i = int.Parse(value);
                if (i == 1)
                    return true;
                else
                    return false;
            }
            // Else try to parse as boolean
            bool.TryParse(value, out bool output);
            return output;
        }

        //public static string FormatJsonToNewLineString(string val)
        //{
        //    var str = "";
        //    var json = JsonConvert.DeserializeObject(val);
        //    if (json != null)
        //    {
        //        if (json.GetType() == typeof(JArray))
        //        {
        //            str = "";
        //            foreach (JObject item in json)
        //            {
        //                var obj = "";
        //                foreach (var prop in item)
        //                    obj += prop.Key.ToString() + ": " + prop.Value.ToString() + ", ";
        //                if (obj != "")
        //                {
        //                    // remove last comma
        //                    obj = obj.Substring(0, obj.Length - 2);

        //                    obj += Environment.NewLine;
        //                    str += obj;
        //                }
        //            }
        //        }
        //        else if (json.GetType == typeof(JObject))
        //        {
        //            str = "";
        //            foreach (JProperty item in json)
        //                str += item.Name.ToString() + ": " + item.Value.ToString() + " " + Environment.NewLine;
        //            if (str.EndsWith(Environment.NewLine))
        //                str = str.Remove(str.LastIndexOf(Environment.NewLine));
        //        }
        //    }

        //    return str;
        //}

     
        public static string ConvertBooleanToString(bool? value, string trueValue, string falseValue, string nullValue = "")
        {
            if (value.HasValue)
            {
                if (value.Value == true)
                    return trueValue;
                else
                    return falseValue;
            }
            else
                return nullValue;
        }

        public static IEnumerable<DataRow> AsEnumerable(this DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                yield return table.Rows[i];
            }
        }
    }
}
