using System.Collections;
using System.Reflection;

namespace DotSerial.Tests
{
    internal static  class HelperMethods
    {
        public static void AssertClassEqual(object? isClass, object? expectedClass)
        {
            // Null check
            if (null == expectedClass)
            {
                Assert.Null(isClass);
                return;
            }

            ArgumentNullException.ThrowIfNull(isClass);

            if (DotSerial.Core.Misc.HelperMethods.IsClass(isClass.GetType()))
            {
                // Check if Class
                Assert.True(DotSerial.Core.Misc.HelperMethods.IsClass(isClass.GetType()));
                Assert.True(DotSerial.Core.Misc.HelperMethods.IsClass(expectedClass.GetType()));
            }
            else
            {
                // Check if Struct
                Assert.True(DotSerial.Core.Misc.HelperMethods.IsStruct(isClass.GetType()));
                Assert.True(DotSerial.Core.Misc.HelperMethods.IsStruct(expectedClass.GetType()));
            }


            // Type check
            Assert.Equal(isClass.GetType(), expectedClass.GetType());

            // Size check
            PropertyInfo[] propsIs = isClass.GetType().GetProperties();
            PropertyInfo[] propsExpected = expectedClass.GetType().GetProperties();
            Assert.Equal(propsIs.Length, propsExpected.Length);

            // value Check
            for (int i = 0; i < propsExpected.Length; i++)
            {
                PropertyInfo isPropInfo = propsIs[i];
                object? isValue = isPropInfo.GetValue(isClass);
                string isPropName = isPropInfo.Name;

                PropertyInfo expectedPropInfo = propsExpected[i];
                object? expectedValue = expectedPropInfo.GetValue(expectedClass);
                string expectedPropName = expectedPropInfo.Name;

                Assert.Equal(isPropName, expectedPropName);
                Assert.Equal(isPropInfo.PropertyType, expectedPropInfo.PropertyType);

                if (null == expectedValue)
                {
                    Assert.Null(isValue);
                }
                else if (isPropInfo.PropertyType == typeof(string))
                {
                    Assert.Equal(expectedValue, isValue);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsList(isValue))
                {
                    AssertListEqual(isValue, expectedValue);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsArray(isValue))
                {
                    AssertArrayEqual(isValue, expectedValue);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsDictionary(isValue))
                {
                    AssertDictionaryEqual(isValue, expectedValue);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsClass(isPropInfo.PropertyType))
                {
                    AssertClassEqual(isValue, expectedValue);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsStruct(isPropInfo.PropertyType))
                {
                    AssertClassEqual(isValue, expectedValue);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsPrimitive(isPropInfo.PropertyType))
                {
                    Assert.Equal(expectedValue, isValue);
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// Check if to arrays are equal
        /// </summary>
        /// <param name="isObj"> Is Array</param>
        /// <param name="targetObj">Expected Array</param>
        public static void AssertArrayEqual(object? isObj, object? targetObj)
        {
            // Null check
            if (null == targetObj)
            {
                Assert.Null(isObj);
                return;
            }

            IEnumerable? isArray = isObj as IEnumerable;
            IEnumerable? targetArray = targetObj as IEnumerable;

            if (null == isArray || null == targetArray)
            {
                Assert.Fail();
            }

            // Check if array
            Assert.True(DotSerial.Core.Misc.HelperMethods.IsArray(isArray));
            Assert.True(DotSerial.Core.Misc.HelperMethods.IsArray(targetArray));

            // Type Check
            Type targetItemType = DotSerial.Core.Misc.HelperMethods.GetItemTypeOfIEnumerable(targetArray);
            Type isItemType = DotSerial.Core.Misc.HelperMethods.GetItemTypeOfIEnumerable(isArray);
            Assert.Equal(targetItemType, isItemType);

            // Value Check
            IEnumerator targetEnum = targetArray.GetEnumerator();
            IEnumerator isEnum = isArray.GetEnumerator();

            while (targetEnum.MoveNext() && isEnum.MoveNext())
            {
                if (isItemType == typeof(string))
                {
                    Assert.Equal(targetEnum.Current, isEnum.Current);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsList(isItemType))
                {
                    AssertListEqual(targetEnum.Current, isEnum.Current);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsArray(isItemType))
                {
                    AssertArrayEqual(targetEnum.Current, isEnum.Current);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsDictionary(isItemType))
                {
                    AssertDictionaryEqual(targetEnum.Current, isEnum.Current);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsClass(isItemType))
                {
                    AssertClassEqual(targetEnum.Current, isEnum.Current);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsStruct(isItemType))
                {
                    AssertClassEqual(targetEnum.Current, isEnum.Current);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsPrimitive(isItemType))
                {
                    Assert.Equal(targetEnum.Current, isEnum.Current);
                }
                else
                {
                    Assert.Fail();
                }
            }
            
        }

        /// <summary>
        /// Check if two lists are equal.
        /// </summary>
        /// <param name="isObj">s List</param>
        /// <param name="targetObj">Expected List</param>
        public static void AssertListEqual(object? isObj, object? targetObj)
        {
            // Null check
            if (null == targetObj)
            {
                Assert.Null(isObj);
                return;
            }

            IList? isList = isObj as IList;
            IList? targetList = targetObj as IList;

            if (null == isList || null == targetList)
            {
                Assert.Fail();
            }

            // Check if lists
            Assert.True(DotSerial.Core.Misc.HelperMethods.IsList(isList));
            Assert.True(DotSerial.Core.Misc.HelperMethods.IsList(targetList));

            // Size check
            Assert.Equal(targetList.Count, isList?.Count);

            // Type Check
            Type targetItemType = DotSerial.Core.Misc.HelperMethods.GetItemTypeOfIEnumerable(targetList);
#pragma warning disable CS8604
            Type isItemType = DotSerial.Core.Misc.HelperMethods.GetItemTypeOfIEnumerable(isList);
#pragma warning restore CS8604
            Assert.Equal(targetItemType, isItemType);

            // Value Check
            for (int i = 0; i < targetList.Count; i++)
            {
                if (isItemType == typeof(string))
                {
                    Assert.Equal(targetList[i], isList[i]);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsList(isItemType))
                {
                    AssertListEqual(targetList[i], isList[i]);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsArray(isItemType))
                {
                    AssertArrayEqual(targetList[i], isList[i]);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsDictionary(isItemType))
                {
                    AssertDictionaryEqual(targetList[i], isList[i]);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsClass(isItemType))
                {
                    AssertClassEqual(targetList[i], isList[i]);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsStruct(isItemType))
                {
                    AssertClassEqual(targetList[i], isList[i]);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsPrimitive(isItemType))
                {
                    Assert.Equal(targetList[i], isList[i]);
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// Check if two Dictionarys are Equal.
        /// </summary>
        /// <param name="isObj">Is Dictionary</param>
        /// <param name="targetObj">Expected Dictionary</param>
        public static void AssertDictionaryEqual(object? isObj, object? targetObj)
        {
            // Null check
            if (null == targetObj)
            {
                Assert.Null(isObj);
                return;
            }

            IDictionary? isDic = isObj as IDictionary;
            IDictionary? targetDic = targetObj as IDictionary;

            if (null == isDic || null == targetDic)
            {
                Assert.Fail();
            }

            // Check if dictionary
            Assert.True(DotSerial.Core.Misc.HelperMethods.IsDictionary(isDic));
            Assert.True(DotSerial.Core.Misc.HelperMethods.IsDictionary(targetDic));

            // Size check
            Assert.Equal(targetDic.Count, isDic?.Count);

            // Type Check
            if (false == DotSerial.Core.Misc.HelperMethods.GetKeyValueTypeOfDictionary(targetDic, out Type targetDicKeyType, out Type targetDicValueType))
            {
                Assert.Fail();
            }
#pragma warning disable CS8604
            if (false == DotSerial.Core.Misc.HelperMethods.GetKeyValueTypeOfDictionary(isDic, out Type isDicKeyType, out Type isDicValueType))
            {
                Assert.Fail();
            }
#pragma warning disable CS8604
            Assert.Equal(targetDicKeyType, isDicKeyType);
            Assert.Equal(targetDicValueType, isDicValueType);

            // Value Check
            foreach (DictionaryEntry keyValue in targetDic)
            {
                Assert.True(isDic.Contains(keyValue.Key));

                if (isDicValueType == typeof(string))
                {
                    Assert.Equal(isDic[keyValue.Key], keyValue.Value);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsList(isDicValueType))
                {
                    AssertListEqual(isDic[keyValue.Key], keyValue.Value);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsArray(isDicValueType))
                {
                    AssertArrayEqual(isDic[keyValue.Key], keyValue.Value);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsDictionary(isDicValueType))
                {
                    AssertDictionaryEqual(isDic[keyValue.Key], keyValue.Value);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsClass(isDicValueType))
                {
                    AssertClassEqual(isDic[keyValue.Key], keyValue.Value);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsStruct(isDicValueType))
                {
                    AssertClassEqual(isDic[keyValue.Key], keyValue.Value);
                }
                else if (DotSerial.Core.Misc.HelperMethods.IsPrimitive(isDicValueType))
                {
                    Assert.Equal(isDic[keyValue.Key], keyValue.Value);
                }
                else
                {
                    Assert.Fail();
                }

            }

        }
    }
}
