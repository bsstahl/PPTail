using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using PPTail;
using TestHelperExtensions;

namespace PPTail.Test
{
    public class StringExtensions_IsNullOrWhitespace_Should
    {
        [Fact]
        public void ReturnFalseIfArgsIsNull()
        {
            string[] target = null;
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
            string value1 = string.Empty.GetRandom();
            var target = new string[] { value1 };
            bool actual  = target.IsNullOrWhiteSpace();
            Assert.False(actual);
        }

        [Fact]
        public void ReturnTrueIfTheOnlyValueIsNull()
        {
            string value1 = null;
            var target = new string[] { value1 };
            bool actual = target.IsNullOrWhiteSpace();
            Assert.True(actual);
        }

        [Fact]
        public void ReturnTrueIfTheOnlyValueIsWhitespace()
        {
            string value1 = "   ";
            var target = new string[] { value1  };
            bool actual = target.IsNullOrWhiteSpace();
            Assert.True(actual);
        }

        [Fact]
        public void ReturnFalseIfAllValuesAreNotNull()
        {
            string value1 = string.Empty.GetRandom();
            string value2 = string.Empty.GetRandom();
            string value3 = string.Empty.GetRandom();
            var target = new string[] { value1, value2, value3 };
            bool actual = target.IsNullOrWhiteSpace();
            Assert.False(actual);
        }

        [Fact]
        public void ReturnTrueIfTheFirstValueIsNull()
        {
            string value1 = null;
            string value2 = string.Empty.GetRandom();
            string value3 = string.Empty.GetRandom();
            var target = new string[] { value1, value2, value3 };
            bool actual = target.IsNullOrWhiteSpace();
            Assert.True(actual);
        }

        [Fact]
        public void ReturnTrueIfTheFirstValueIsEmpty()
        {
            string value1 = string.Empty;
            string value2 = string.Empty.GetRandom();
            string value3 = string.Empty.GetRandom();
            var target = new string[] { value1, value2, value3 };
            bool actual = target.IsNullOrWhiteSpace();
            Assert.True(actual);
        }

        [Fact]
        public void ReturnTrueIfAMiddleValueIsNull()
        {
            string value1 = string.Empty.GetRandom();
            string value2 = null;
            string value3 = string.Empty.GetRandom();
            var target = new string[] { value1, value2, value3 };
            bool actual = target.IsNullOrWhiteSpace();
            Assert.True(actual);
        }

        [Fact]
        public void ReturnTrueIfAMiddleValueIsEmpty()
        {
            string value1 = string.Empty.GetRandom();
            string value2 = string.Empty;
            string value3 = string.Empty.GetRandom();
            var target = new string[] { value1, value2, value3 };
            bool actual = target.IsNullOrWhiteSpace();
            Assert.True(actual);
        }

        [Fact]
        public void ReturnTrueIfTheEndValueIsNull()
        {
            string value1 = string.Empty.GetRandom();
            string value2 = string.Empty.GetRandom();
            string value3 = null;
            var target = new string[] { value1, value2, value3 };
            bool actual = target.IsNullOrWhiteSpace();
            Assert.True(actual);
        }

        [Fact]
        public void ReturnTrueIfTheEndValueIsEmpty()
        {
            string value1 = string.Empty.GetRandom();
            string value2 = string.Empty.GetRandom();
            string value3 = string.Empty;
            var target = new string[] { value1, value2, value3 };
            bool actual = target.IsNullOrWhiteSpace();
            Assert.True(actual);
        }
    }
}
