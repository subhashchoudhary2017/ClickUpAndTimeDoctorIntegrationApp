using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ClickUpIntegration.Models.TimeDoctor
{
    public class TimeDoctorEnums
    {
        public enum GroupByTypeEnum
        {
            [Description("Group By Task")]
            GroupByTask,
            [Description("Group By User")]
            GroupByUser,
            [Description("Group By Task and User")]
            GroupByTaskAndUser
        }

        public static string GetEnumDescription(Enum value)
        {
            // Get the Description attribute value for the enum value
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
