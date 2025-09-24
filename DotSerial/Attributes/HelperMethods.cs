#region License
//Copyright (c) 2025 Dennis Sölch

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
#endregion

using System.Reflection;
using DotSerial.Core.Exceptions;
using DotSerial.Core.XML;

namespace DotSerial.Attributes
{
    internal static class HelperMethods
    {
        /// <summary> Get the parameter ID
        /// </summary>
        /// <param name="prop">PropertyInfo</param>
        /// <returns>ID</returns>
        internal static int GetPropertyID(PropertyInfo prop)
        {
            object[] attrs = prop.GetCustomAttributes(true);
            foreach (object att in attrs)
            {
                if (att is DSPropertyIDAttribute saveAtt)
                {
                    int result = (int)saveAtt.PropertyID;

                    if (result == Constants.NoAttributeID)
                    {
                        throw new DSInvalidIDException(result);
                    }

                    return result;
                }
            }
            return Constants.NoAttributeID;
        }

    }
}
