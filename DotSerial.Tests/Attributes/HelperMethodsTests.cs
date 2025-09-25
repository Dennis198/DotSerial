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

namespace DotSerial.Tests.Attributes
{
    public class HelperMethodsTests
    {
        [Fact]
        public void GetPropertyID_True()
        {
            // Arrange
            NoAttributeClass tmp = new ();            
            PropertyInfo[] props = tmp.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == nameof(NoAttributeClass.Boolean))
                {
                    // Act
                    int id = DotSerial.Attributes.HelperMethods.GetPropertyID(prop);

                    // Assert
                    Assert.Equal(1, id);
                    return;
                }
            }
        }

        [Fact]
        public void GetPropertyID_False()
        {
            // Arrange
            NoAttributeClass tmp = new ();
            PropertyInfo[] props = tmp.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == nameof(NoAttributeClass.BooleanNoAttribute))
                {
                    //Act
                    int id = DotSerial.Attributes.HelperMethods.GetPropertyID(prop);

                    // Assert
                    Assert.Equal(-1, id);
                    return;
                }
            }
        }
    }
}
