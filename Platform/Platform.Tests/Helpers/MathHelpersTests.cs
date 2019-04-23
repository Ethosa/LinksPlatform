﻿using System;
using Xunit;
using Platform.Helpers;

namespace Platform.Tests.Helpers
{
    public class MathHelpersTests
    {
        [Theory]
        [InlineData(00, -1)] // 0000 0000 (none, -1)
        [InlineData(01, 00)] // 0000 0001 (first, 0)
        [InlineData(08, 03)] // 0000 1000 (forth, 3)
        [InlineData(88, 03)] // 0101 1000 (forth, 3)
        public void GetLowestBitPositionTest(ulong value, int expectedPosition)
        {
            Assert.True(MathHelpers.GetLowestBitPosition(value) == expectedPosition);
        }

        [Fact]
        public void CompiledOperationsTest()
        {
            Assert.True(MathHelpers<short>.Add(1, 2) == 3);
            Assert.True(MathHelpers<byte>.Increment(1) == 2);
            Assert.True(MathHelpers<ulong>.Decrement(2) == 1);
            Assert.True(Math.Abs(MathHelpers<double>.Subtract(3D, 2D) - 1D) < 0.01);
            Assert.True(MathHelpers<sbyte>.Equals(2, 2));
            Assert.True(MathHelpers<sbyte>.GreaterThan(3, 2));
            Assert.True(MathHelpers<sbyte>.GreaterOrEqualThan(5, 5));
            Assert.True(MathHelpers<sbyte>.GreaterOrEqualThan(5, 1));
            Assert.True(MathHelpers<sbyte>.LessThan(4, 5));
            Assert.True(MathHelpers<sbyte>.LessOrEqualThan(5, 5));
            Assert.True(MathHelpers<sbyte>.LessOrEqualThan(1, 5));
            Assert.Throws<NotSupportedException>(() => MathHelpers<string>.Subtract("1", "2"));
        }

        [Fact]
        public void PartialReadWriteTest()
        {
            {
                uint firstValue = 1;
                uint secondValue = 1543;

                // Pack (join) two values at the same time
                uint value = (secondValue << 1) | firstValue;

                uint unpackagedFirstValue = value & 1;
                uint unpackagedSecondValue = (value & 0xFFFFFFFE) >> 1;

                Assert.True(firstValue == unpackagedFirstValue);
                Assert.True(secondValue == unpackagedSecondValue);

                // Using universal functions:

                Assert.True(PartialRead(value, 0, 1) == firstValue);
                Assert.True(PartialRead(value, 1, -1) == secondValue);

                firstValue = 0;
                secondValue = 6892;

                value = PartialWrite(value, firstValue, 0, 1);
                value = PartialWrite(value, secondValue, 1, -1);

                Assert.True(PartialRead(value, 0, 1) == firstValue);
                Assert.True(PartialRead(value, 1, -1) == secondValue);
            }

            {
                uint firstValue = 1;
                uint secondValue = 1543;

                // Pack (join) two values at the same time
                uint value = (secondValue << 1) | firstValue;

                uint unpackagedFirstValue = value & 1;
                uint unpackagedSecondValue = (value & 0xFFFFFFFE) >> 1;

                Assert.True(firstValue == unpackagedFirstValue);
                Assert.True(secondValue == unpackagedSecondValue);

                // Using universal functions:

                Assert.True(MathHelpers.PartialRead(value, 0, 1) == firstValue);
                Assert.True(MathHelpers.PartialRead(value, 1, -1) == secondValue);

                firstValue = 0;
                secondValue = 6892;

                value = MathHelpers.PartialWrite(value, firstValue, 0, 1);
                value = MathHelpers.PartialWrite(value, secondValue, 1, -1);

                Assert.True(MathHelpers.PartialRead(value, 0, 1) == firstValue);
                Assert.True(MathHelpers.PartialRead(value, 1, -1) == secondValue);
            }

            {
                uint firstValue = 1;
                uint secondValue = 1543;

                // Pack (join) two values at the same time
                uint value = (secondValue << 1) | firstValue;

                uint unpackagedFirstValue = value & 1;
                uint unpackagedSecondValue = (value & 0xFFFFFFFE) >> 1;

                Assert.True(firstValue == unpackagedFirstValue);
                Assert.True(secondValue == unpackagedSecondValue);

                // Using universal functions:

                var readMasksAndShiftFor0And1 = GetReadMaskAndShift(0, 1);
                var readMasksAndShiftFor1AndMinus1 = GetReadMaskAndShift(1, -1);
                var writeMasksAndShiftFor0And1 = GetWriteMasksAndShift(0, 1);
                var writeMasksAndShiftFor1AndMinus1 = GetWriteMasksAndShift(1, -1);

                Assert.True(PartialRead(value, readMasksAndShiftFor0And1) == firstValue);
                Assert.True(PartialRead(value, readMasksAndShiftFor1AndMinus1) == secondValue);

                firstValue = 0;
                secondValue = 6892;

                value = PartialWrite(value, firstValue, writeMasksAndShiftFor0And1);
                value = PartialWrite(value, secondValue, writeMasksAndShiftFor1AndMinus1);

                Assert.True(PartialRead(value, readMasksAndShiftFor0And1) == firstValue);
                Assert.True(PartialRead(value, readMasksAndShiftFor1AndMinus1) == secondValue);
            }
        }

        // TODO: Can be optimized using precalculation of TargetMask and SourceMask
        private uint PartialWrite(uint target, uint source, int shift, int limit)
        {
            if (shift < 0) shift = 32 + shift;
            if (limit < 0) limit = 32 + limit;
            var sourceMask = ~(uint.MaxValue << limit) & uint.MaxValue;
            var targetMask = ~(sourceMask << shift);
            return (target & targetMask) | ((source & sourceMask) << shift);
        }

        private uint PartialRead(uint target, int shift, int limit)
        {
            if (shift < 0) shift = 32 + shift;
            if (limit < 0) limit = 32 + limit;
            var sourceMask = ~(uint.MaxValue << limit) & uint.MaxValue;
            var targetMask = sourceMask << shift;
            return (target & targetMask) >> shift;
        }

        private Tuple<uint, uint, int> GetWriteMasksAndShift(int shift, int limit)
        {
            if (shift < 0) shift = 32 + shift;
            if (limit < 0) limit = 32 + limit;
            var sourceMask = ~(uint.MaxValue << limit) & uint.MaxValue;
            var targetMask = ~(sourceMask << shift);
            return new Tuple<uint, uint, int>(targetMask, sourceMask, shift);
        }

        private Tuple<uint, int> GetReadMaskAndShift(int shift, int limit)
        {
            if (shift < 0) shift = 32 + shift;
            if (limit < 0) limit = 32 + limit;
            var sourceMask = ~(uint.MaxValue << limit) & uint.MaxValue;
            var targetMask = sourceMask << shift;
            return new Tuple<uint, int>(targetMask, shift);
        }

        private uint PartialWrite(uint target, uint targetMask, uint source, uint sourceMask, int shift)
        {
            return (target & targetMask) | ((source & sourceMask) << shift);
        }

        private uint PartialWrite(uint target, uint source, Tuple<uint, uint, int> masksAndShift)
        {
            return PartialWrite(target, masksAndShift.Item1, source, masksAndShift.Item2, masksAndShift.Item3);
        }

        private uint PartialRead(uint target, uint targetMask, int shift)
        {
            return (target & targetMask) >> shift;
        }

        private uint PartialRead(uint target, Tuple<uint, int> masksAndShift)
        {
            return PartialRead(target, masksAndShift.Item1, masksAndShift.Item2);
        }
    }
}
