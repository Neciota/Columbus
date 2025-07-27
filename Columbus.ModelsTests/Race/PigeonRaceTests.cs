using Columbus.Models.Pigeon;
using Columbus.Models.Owner;

namespace Columbus.Models.Race.Tests;

[TestClass()]
public class PigeonRaceTests
{
    [TestMethod()]
    public void GetCorrectedArrivalTimeTest()
    {
        DateTime submissionTime = new(2014, 07, 24, 19, 47, 28, DateTimeKind.Local);
        DateTime rawArrivalTime = new(2014, 07, 27, 23, 56, 47, DateTimeKind.Local);
        DateTime stopTime = new(2014, 07, 28, 16, 46, 27, DateTimeKind.Local);
        TimeSpan deviation = TimeSpan.FromSeconds(-1);

        DateTime expected = new(2014, 07, 27, 23, 56, 46, DateTimeKind.Local);

        Pigeon.Pigeon pigeon = new(CountryCode.Create("NL"), 14, RingNumber.Create(1234567), 0, Sex.Male);
        PigeonRace pigeonRace = new(pigeon, OwnerId.Create(12345678), rawArrivalTime, 1, 1);

        DateTime? actual = pigeonRace.GetCorrectedArrivalTime(submissionTime, stopTime, deviation);

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected.Year, actual.Value.Year);
        Assert.AreEqual(expected.Month, actual.Value.Month);
        Assert.AreEqual(expected.Day, actual.Value.Day);
        Assert.AreEqual(expected.Hour, actual.Value.Hour);
        Assert.AreEqual(expected.Minute, actual.Value.Minute);
        Assert.AreEqual(expected.Second, actual.Value.Second);
    }

    [TestMethod()]
    public void GetCorrectedArrivalTimeWhenNoCorrectionTest()
    {
        DateTime rawArrivalTime = new(2014, 07, 27, 23, 56, 47, DateTimeKind.Local);

        DateTime expected = new(2014, 07, 27, 23, 56, 47, DateTimeKind.Local);

        Pigeon.Pigeon pigeon = new(CountryCode.Create("NL"), 14, RingNumber.Create(1234567), 0, Sex.Male);
        PigeonRace pigeonRace = new(pigeon, OwnerId.Create(12345678), rawArrivalTime, 1, 1);

        DateTime? actual = pigeonRace.GetCorrectedArrivalTime(null, null, TimeSpan.Zero);

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected.Year, actual.Value.Year);
        Assert.AreEqual(expected.Month, actual.Value.Month);
        Assert.AreEqual(expected.Day, actual.Value.Day);
        Assert.AreEqual(expected.Hour, actual.Value.Hour);
        Assert.AreEqual(expected.Minute, actual.Value.Minute);
        Assert.AreEqual(expected.Second, actual.Value.Second);
    }
}