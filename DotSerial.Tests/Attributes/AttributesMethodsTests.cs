using System.Reflection;

namespace DotSerial.Tests.Attributes
{
    public class AttributesMethodsTests
    {
        [Fact]
        public void GetSerializeName_True()
        {
            // Arrange
            NoAttributeClass tmp = new ();            
            PropertyInfo[] props = tmp.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == nameof(NoAttributeClass.Boolean))
                {
                    // Act
                    string? id = DotSerial.Attributes.AttributesMethods.GetSerializeName(prop);

                    // Assert
                    Assert.Equal("1", id);
                    return;
                }
            }
        }

        [Fact]
        public void GetSerializeName_False()
        {
            // Arrange
            NoAttributeClass tmp = new ();
            PropertyInfo[] props = tmp.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == nameof(NoAttributeClass.BooleanNoAttribute))
                {
                    //Act
                    string? id = DotSerial.Attributes.AttributesMethods.GetSerializeName(prop);

                    // Assert
                    Assert.Null(id);
                    return;
                }
            }
        }

        [Fact]
        public void GetCustomPropertyName()
        {
            // Arrange
            NoAttributeClass tmp = new ();            
            PropertyInfo[] props = tmp.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == nameof(NoAttributeClass.Boolean))
                {
                    // Act
                    string? id = DotSerial.Attributes.AttributesMethods.GetSerializeName(prop);

                    // Assert
                    Assert.Equal("1", id);
                    return;
                }
            }
        }        
    }
}
