using System;
using NUnit.Framework;

namespace TT.Core.Tests;

/// <summary>
/// Юнит тесты структуры <see cref="Result{T, E}"/>
/// </summary>

[TestFixture(typeof(int), typeof(string))]
[TestFixture(typeof(float), typeof(uint))]
[TestFixture(typeof(long), typeof(char))]
[TestFixture(typeof(double), typeof(double))]
public class ResultGenericTests<V, E>
{

    [Test]
    public void SimpleTypeInstanceCreateTest()
    {
        try
        {
            var result = new Result<V, E>();
        }
        catch
        {
            Assert.Fail($"Не удалось создать экземпляр с параметрами: {typeof(V).FullName}, {typeof(E).FullName}");
        }
    }
}

[TestFixture]
public class ResultTests
{
    [Test]
    public void CustomTypeInstanceCreateTest()
    {
        Type valueType = null;
        Type errorType = null;
        try
        {
            valueType = typeof(ClassGenericParamType);
            errorType = typeof(EnumGenericParamType);
            var classEnum = new Result<ClassGenericParamType, EnumGenericParamType>();

            valueType = typeof(EnumGenericParamType);
            errorType = typeof(ClassGenericParamType);
            var enumClass = new Result<EnumGenericParamType, ClassGenericParamType>();

            valueType = typeof(ClassGenericParamType);
            errorType = typeof(StructGenericParamType);
            var classStruct = new Result<ClassGenericParamType, StructGenericParamType>();

            valueType = typeof(StructGenericParamType);
            errorType = typeof(ClassGenericParamType);
            var structClass = new Result<StructGenericParamType, ClassGenericParamType>();
        }
        catch
        {
            Assert.Fail($"Не удалось создать экземпляр с параметрами: {valueType.FullName}, {errorType.FullName}");
        }
    }

    [Test]
    public void SuccessTest()
    {
        Func<int, Result<int, string>> pretendMethod = (s) =>
        {
            if (s % 2 == 0)
                return 0;

            return "Ошибка";
        };

        var result = pretendMethod.Invoke(2);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(0, result.Value);
    }

    [Test]
    public void FailedTest()
    {
        Func<string, Result<string, int>> pretendMethod = (s) =>
        {
            if (string.IsNullOrEmpty(s))
            {
                /* типо код ошибки */
                return 2319;
            }

            return "No error";
        };

        var result = pretendMethod.Invoke(string.Empty);
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(2319, result.Error);
    }

    [Test]
    public void SuccessCallbackTest()
    {
        Func<string, Result<ClassGenericParamType, string>> pretendMethod = (s) =>
        {
            if (!string.IsNullOrEmpty(s))
            {
                return new ClassGenericParamType() { IntProperty = 20, Name = "testing" };
            }

            return "input con't be empty";
        };

        var result = pretendMethod.Invoke("erer");

        var onSuccess = (ClassGenericParamType value) =>
        {
            Assert.True(value.IntProperty == 20);
            Assert.True(value.Name == "testing");
            return value;
        };

        var onFailed = (string value) =>
        {
            Assert.Fail();
            return new ClassGenericParamType();
        };

        var withCallback = result.Match(onSuccess, onFailed);
    }

    [Test]
    public void FailedCallbackTest()

    {
        Func<string, Result<EnumGenericParamType, string>> pretendMethod = (s) =>
        {
            if (!string.IsNullOrEmpty(s))
            {
                return EnumGenericParamType.First;
            }

            return "input con't be empty";
        };

        var result = pretendMethod.Invoke(string.Empty);

        var onSuccess = (EnumGenericParamType value) =>
        {
            Assert.Fail();
            return value;
        };

        var onFailed = (string value) =>
        {
            Assert.IsNotNull(value);
            Assert.AreEqual("input con't be empty", value);
            return EnumGenericParamType.First;
        };

        var withCallback = result.Match(onSuccess, onFailed);
    }

    #region nested types
    private class ClassGenericParamType
    {
        public int IntProperty { get; set; }
        public string Name { get; set; }
    }

    private enum EnumGenericParamType
    {
        First = 1,
        Second = 2,
        Third = 3,
    }

    private struct StructGenericParamType
    {
        public float x;
        public float y;
        public double d;

    }
    #endregion
}
