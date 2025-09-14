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
