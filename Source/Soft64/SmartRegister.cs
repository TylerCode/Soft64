/*
Soft64 - C# N64 Emulator
Copyright (C) Soft64 Project @ Codeplex
Copyright (C) 2013 - 2015 Bryan Perris
	
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Soft64
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class RegisterFieldAttribute : Attribute
    {
        private String m_FieldName;
        private Int32 m_FieldSize;
        private Int32 m_FieldOffset;
        private RuntimeTypeHandle m_FieldType;

        public RegisterFieldAttribute(String name, Int32 size, Int32 offset, Type type)
        {
            m_FieldName = name;
            m_FieldSize = size;
            m_FieldOffset = offset;
            m_FieldType = type.TypeHandle;
        }

        public RuntimeTypeHandle FieldType
        {
            get { return m_FieldType; }
        }

        public Int32 FieldOffset
        {
            get { return m_FieldOffset; }
        }

        public Int32 FieldSize
        {
            get { return m_FieldSize; }
        }

        public String FieldName
        {
            get { return m_FieldName; }
        }

    }

    public abstract class SmartRegister<T>
        where T : struct
    {
        private T m_Register;
        private IEnumerable<RegisterFieldAttribute> m_MapDefinitions;
        private ExpandoObject m_DynamicObject;

        protected SmartRegister()
        {
            m_MapDefinitions = GetType().GetCustomAttributes(typeof(RegisterFieldAttribute), true).OfType<RegisterFieldAttribute>();
            m_DynamicObject = new ExpandoObject();
            BuildProps();
        }

        private void BuildProps()
        {
           foreach (var def in m_MapDefinitions)
           {
               Func<dynamic> getter;
               Action<dynamic> setter;
               Type fieldType = Type.GetTypeFromHandle(def.FieldType);
               dynamic mask = ((UInt64)Math.Pow(2, def.FieldSize) - 1) << def.FieldOffset;

               getter = () =>
               {
                   dynamic value = Activator.CreateInstance(fieldType);
                   dynamic maskedRegValue = InternalRegister;
                   maskedRegValue &= (T)mask;
                   maskedRegValue >>= def.FieldOffset;
                   return Convert.ChangeType(maskedRegValue, fieldType);
               };

               setter = (v) =>
               {
                   dynamic newValue = Convert.ToUInt64(v);
                   newValue <<= def.FieldOffset;

                   dynamic maskedRegValue = InternalRegister;
                   maskedRegValue &= ~mask;
                   maskedRegValue |= newValue;
                   InternalRegister = (T)maskedRegValue;
               };


               ((IDictionary<String, Object>)m_DynamicObject).Add("Get" + def.FieldName, getter);
               ((IDictionary<String, Object>)m_DynamicObject).Add("Set" + def.FieldName, setter);
           }
        }

        public T RegisterValue
        {
            get { return InternalRegister; }
            set { InternalRegister = value; }
        }

        protected dynamic AutoRegisterProps
        {
            get { return m_DynamicObject; }
        }

        protected virtual T InternalRegister
        {
            get { return m_Register; }
            set { m_Register = value; }
        }
    }
}
