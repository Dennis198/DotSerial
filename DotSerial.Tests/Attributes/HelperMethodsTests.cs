using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotSerial.Tests.Attributes
{
    public class HelperMethodsTests
    {
        [Fact]
        public void GetPropertyID_True()
        {
            NoAttributeClass tmp = new NoAttributeClass();
            // Get all Properties and iterate threw
            PropertyInfo[] props = tmp.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == nameof(NoAttributeClass.Boolean))
                {
                    
                    int id = DotSerial.Attributes.HelperMethods.GetPropertyID(prop);
                    Assert.Equal(1, id);
                    return;
                }
            }
        }

        [Fact]
        public void GetPropertyID_False()
        {
            NoAttributeClass tmp = new NoAttributeClass();
            // Get all Properties and iterate threw
            PropertyInfo[] props = tmp.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == nameof(NoAttributeClass.BooleanNoAttribute))
                {

                    int id = DotSerial.Attributes.HelperMethods.GetPropertyID(prop);
                    Assert.Equal(-1, id);
                    return;
                }
            }
        }
    }
}
