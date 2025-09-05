using TidalWrapper.Engines;

namespace TidalWrapper.Tests;

public class RegexTests
{
    [Theory]
    [InlineData("https://tidal.com/browse/track/12345678", true)]
    [InlineData("https://tidal.com/browse/track/12345678/u", true)]
    [InlineData("https://tidal.com/browse/track/12345678?u=test", true)]
    [InlineData("https://tidal.com/track/12345678", true)]
    [InlineData("https://tidal.com/track/12345678/u", true)]
    [InlineData("https://tidal.com/track/12345678?u=test", true)]
    [InlineData("https://tidal.com/browse/track/12345678X", false)]
    [InlineData("https://tidal.com/browse/12345678", false)]
    [InlineData("https://tidal.com/12345678", false)]
    public void URLRegex_WorksAsExpected(string input, bool expectedMatch)
    {

        bool isMatch = TrackUtil.IsValidUrl(input);
        Assert.Equal(expectedMatch, isMatch);
    }
}
