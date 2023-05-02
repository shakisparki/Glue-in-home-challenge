// Copyright (c) WAAM3D Limited.
//  All rights reserved.

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace TT.Deliveries.Core.Extensions
{
    public static class EnumExtensionMethods
    {
        public static string GetDescription(this Enum GenericEnum)
        {
            var genericEnumType = GenericEnum.GetType();
            var memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if (memberInfo.Length > 0)
            {
                var attrib = memberInfo.First().GetCustomAttribute<DescriptionAttribute>();
                if (!(attrib is null))
                {
                    return attrib.Description;
                }
            }
            return GenericEnum.ToString();
        }

        public static string GetName(this Enum genericEnum)
        {
            return Enum.GetName(genericEnum.GetType(), genericEnum);
        }
    }
}