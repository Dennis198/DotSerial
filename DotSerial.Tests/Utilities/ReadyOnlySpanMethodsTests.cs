using DotSerial.Utilities;

namespace DotSerial.Tests.Utilities
{
    public class ReadyOnlySpanMethodsTests
    {
        [Fact]
        public void EqualFirstNoWhiteSpaceChar_ShouldReturnTrue_WhenFirstNonWhitespaceCharMatches()
        {
            ReadOnlySpan<char> span = "   a".AsSpan();
            Assert.True(ReadOnlySpanMethods.EqualFirstNoWhiteSpaceChar(span, 'a'));
        }

        [Fact]
        public void EqualLastNoWhiteSpaceChar_ShouldReturnTrue_WhenLastNonWhitespaceCharMatches()
        {
            ReadOnlySpan<char> span = "a   ".AsSpan();
            Assert.True(ReadOnlySpanMethods.EqualLastNoWhiteSpaceChar(span, 'a'));
        }

        [Fact]
        public void EqualsNullString_ShouldReturnTrue_WhenSpanEqualsNullString()
        {
            ReadOnlySpan<char> span = "null".AsSpan();
            Assert.True(ReadOnlySpanMethods.EqualsNullString(span));
        }

        [Fact]
        public void GetTrimParameter_ShouldReturnTrue_WhenSpanCanBeTrimmed()
        {
            ReadOnlySpan<char> span = "   test   ".AsSpan();
            Assert.True(ReadOnlySpanMethods.GetTrimParameter(span, out int startTrim, out int endTrim));
            Assert.Equal(3, startTrim);
            Assert.Equal(3, endTrim);
        }

        [Fact]
        public void HasStartAndEndQuotes_ShouldReturnTrue_WhenSpanHasQuotes()
        {
            ReadOnlySpan<char> span = "\"test\"".AsSpan();
            Assert.True(ReadOnlySpanMethods.HasStartAndEndQuotes(span));
        }

        [Fact]
        public void IsNullOrWhiteSpace_ShouldReturnTrue_WhenSpanIsEmptyOrWhitespace()
        {
            ReadOnlySpan<char> span = "   ".AsSpan();
            Assert.True(ReadOnlySpanMethods.IsNullOrWhiteSpace(span));
        }

        [Fact]
        public void SkipEnclosingValue_ShouldReturnCorrectIndex_WhenValidInputProvided()
        {
            ReadOnlySpan<char> span = "(test)".AsSpan();
            int result = ReadOnlySpanMethods.SkipEnclosingValue(span, 0, '(', ')');
            Assert.Equal(5, result);
        }

        [Fact]
        public void SkipQuotedValue_ShouldReturnCorrectIndex_WhenQuoteIsClosed()
        {
            ReadOnlySpan<char> span = "\"test\"".AsSpan();
            int result = ReadOnlySpanMethods.SkipQuotedValue(span, 0);
            Assert.Equal(5, result);
        }

        [Fact]
        public void SkipTillStopChar_ShouldReturnCorrectIndex_WhenStopCharIsFound()
        {
            ReadOnlySpan<char> span = "test;".AsSpan();
            int result = ReadOnlySpanMethods.SkipTillStopChar(span, 0, ';');
            Assert.Equal(3, result);
        }

        [Fact]
        public void SkipTillStopChars_ShouldReturnCorrectIndex_WhenStopCharsAreFound()
        {
            ReadOnlySpan<char> span = "test;".AsSpan();
            ReadOnlySpan<char> stopChars = ";".AsSpan();
            int result = ReadOnlySpanMethods.SkipTillStopChars(span, 0, stopChars);
            Assert.Equal(3, result);
        }

        [Fact]
        public void SliceFromTo_ShouldReturnCorrectSlice_WhenValidRangeProvided()
        {
            ReadOnlySpan<char> span = "test".AsSpan();
            ReadOnlySpan<char> result = ReadOnlySpanMethods.SliceFromTo(span, 1, 2);
            Assert.Equal("es", result.ToString());
        }
    }
}
