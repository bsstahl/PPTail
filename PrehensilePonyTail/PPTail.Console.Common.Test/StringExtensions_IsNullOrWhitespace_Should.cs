using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using PPTail;
using TestHelperExtensions;
using PPTail.Console.Common.Extensions;

namespace PPTail.Console.Common.Test;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class StringExtensions_IsNullOrWhitespace_Should
{
    [Fact]
    public void ReturnFalseIfArgsIsNull()
    {
        string[]? target = null;
        bool actual = target.IsNullOrWhiteSpace();
        Assert.False(actual);
    }

    [Fact]
    public void ReturnFalseIfArgsIsAnEmptyArray()
    {
        string[] target = new string[] { };
        bool actual = target.IsNullOrWhiteSpace();
        Assert.False(actual);
    }

    [Fact]
    public void ReturnFalseIfTheOnlyValueIsNotNull()
    {
        String value1 = string.Empty.GetRandom();
        var target = new string[] { value1 };
        bool actual  = target.IsNullOrWhiteSpace();
        Assert.False(actual);
    }

    [Fact]
    public void ReturnTrueIfTheOnlyValueIsNull()
    {
        String value1 = null;
        var target = new string[] { value1 };
        bool actual = target.IsNullOrWhiteSpace();
        Assert.True(actual);
    }

    [Fact]
    public void ReturnTrueIfTheOnlyValueIsWhitespace()
    {
        String value1 = "   ";
        var target = new string[] { value1  };
        bool actual = target.IsNullOrWhiteSpace();
        Assert.True(actual);
    }

    [Fact]
    public void ReturnFalseIfAllValuesAreNotNull()
    {
        String value1 = string.Empty.GetRandom();
        String value2 = string.Empty.GetRandom();
        String value3 = string.Empty.GetRandom();
        var target = new string[] { value1, value2, value3 };
        bool actual = target.IsNullOrWhiteSpace();
        Assert.False(actual);
    }

    [Fact]
    public void ReturnTrueIfTheFirstValueIsNull()
    {
        String value1 = null;
        String value2 = string.Empty.GetRandom();
        String value3 = string.Empty.GetRandom();
        var target = new string[] { value1, value2, value3 };
        bool actual = target.IsNullOrWhiteSpace();
        Assert.True(actual);
    }

    [Fact]
    public void ReturnTrueIfTheFirstValueIsEmpty()
    {
        String value1 = string.Empty;
        String value2 = string.Empty.GetRandom();
        String value3 = string.Empty.GetRandom();
        var target = new string[] { value1, value2, value3 };
        bool actual = target.IsNullOrWhiteSpace();
        Assert.True(actual);
    }

    [Fact]
    public void ReturnTrueIfAMiddleValueIsNull()
    {
        String value1 = string.Empty.GetRandom();
        String? value2 = null;
        String value3 = string.Empty.GetRandom();
        var target = new string?[] { value1, value2, value3 };
        bool actual = target.IsNullOrWhiteSpace();
        Assert.True(actual);
    }

    [Fact]
    public void ReturnTrueIfAMiddleValueIsEmpty()
    {
        String value1 = string.Empty.GetRandom();
        String value2 = string.Empty;
        String value3 = string.Empty.GetRandom();
        var target = new string[] { value1, value2, value3 };
        bool actual = target.IsNullOrWhiteSpace();
        Assert.True(actual);
    }

    [Fact]
    public void ReturnTrueIfTheEndValueIsNull()
    {
        String value1 = string.Empty.GetRandom();
        String value2 = string.Empty.GetRandom();
        String value3 = null;
        var target = new string[] { value1, value2, value3 };
        bool actual = target.IsNullOrWhiteSpace();
        Assert.True(actual);
    }

    [Fact]
    public void ReturnTrueIfTheEndValueIsEmpty()
    {
        String value1 = string.Empty.GetRandom();
        String value2 = string.Empty.GetRandom();
        String value3 = string.Empty;
        var target = new string[] { value1, value2, value3 };
        bool actual = target.IsNullOrWhiteSpace();
        Assert.True(actual);
    }
}
