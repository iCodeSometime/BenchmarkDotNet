﻿using System;
using System.Linq;
using BenchmarkDotNet.Horology;
using BenchmarkDotNet.Reports;
using Xunit;
using Xunit.Abstractions;

namespace BenchmarkDotNet.Tests
{
    public class TimeUnitTests
    {
        private readonly ITestOutputHelper output;

        public TimeUnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ConvertTest()
        {
            CheckConvertTwoWay(1000, TimeUnit.Nanoseconds, 1, TimeUnit.Microseconds);
            CheckConvertTwoWay(1000, TimeUnit.Microseconds, 1, TimeUnit.Millisecond);
            CheckConvertTwoWay(1000, TimeUnit.Millisecond, 1, TimeUnit.Second);
            CheckConvertTwoWay(60, TimeUnit.Second, 1, TimeUnit.Minute);
            CheckConvertTwoWay(60, TimeUnit.Minute, 1, TimeUnit.Hour);
            CheckConvertTwoWay(24, TimeUnit.Hour, 1, TimeUnit.Day);
        }

        [Fact]
        public void GetBestTimeUnitTest()
        {
            CheckGetBestTimeUnit(TimeUnit.Nanoseconds, 1.0);
            CheckGetBestTimeUnit(TimeUnit.Nanoseconds, 100.0);
            CheckGetBestTimeUnit(TimeUnit.Microseconds, 1.0 * 1000);
            CheckGetBestTimeUnit(TimeUnit.Microseconds, 100.0 * 1000);
            CheckGetBestTimeUnit(TimeUnit.Millisecond, 1.0 * 1000 * 1000);
            CheckGetBestTimeUnit(TimeUnit.Millisecond, 100.0 * 1000 * 1000);
            CheckGetBestTimeUnit(TimeUnit.Second, 1.0 * 1000 * 1000 * 1000);
            CheckGetBestTimeUnit(TimeUnit.Second, 100.0 * 1000 * 1000 * 1000);
        }

        private void CheckGetBestTimeUnit(TimeUnit timeUnit, params double[] values)
        {
            output.WriteLine($"Best TimeUnit for ({string.Join(";", values)})ns is {timeUnit.Description}");
            Assert.Equal(timeUnit.Name, TimeUnit.GetBestTimeUnit(values).Name);
        }

        private void CheckConvertTwoWay(double value1, TimeUnit unit1, double value2, TimeUnit unit2)
        {
            CheckConvertOneWay(value1, unit1, value2, unit2);
            CheckConvertOneWay(value2, unit2, value1, unit1);
        }

        private void CheckConvertOneWay(double value1, TimeUnit unit1, double value2, TimeUnit unit2)
        {
            var convertedValue2 = TimeUnit.Convert(value1, unit1, unit2);
            output.WriteLine($"Expected: {value1} {unit1.Name} = {value2} {unit2.Name}");
            output.WriteLine($"Actual: {value1} {unit1.Name} = {convertedValue2} {unit2.Name}");
            output.WriteLine("");
            Assert.Equal(value2, convertedValue2, 4);
        }
    }
}