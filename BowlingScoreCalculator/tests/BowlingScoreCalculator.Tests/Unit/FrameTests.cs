using BowlingScoreCalculator.Models;

namespace BowlingScoreCalculator.Tests.Unit;

[TestFixture]
public class FrameTests
{
    [Test]
    public void Frame_NormalFrame_TwoRolls_IsComplete()
    {
        // Arrange
        var frame = new Frame(1);

        // Act
        frame.AddRoll(new Roll(3));
        frame.AddRoll(new Roll(4));

        // Assert
        Assert.That(frame.IsComplete, Is.True);
        Assert.That(frame.Rolls, Has.Count.EqualTo(2));
    }

    [Test]
    public void Frame_Strike_OneRoll_IsComplete()
    {
        // Arrange
        var frame = new Frame(1);

        // Act
        frame.AddRoll(new Roll(10));

        // Assert
        Assert.That(frame.IsComplete, Is.True);
        Assert.That(frame.IsStrike, Is.True);
        Assert.That(frame.Rolls, Has.Count.EqualTo(1));
    }

    [Test]
    public void Frame_Spare_TwoRolls_IsComplete()
    {
        // Arrange
        var frame = new Frame(1);

        // Act
        frame.AddRoll(new Roll(7));
        frame.AddRoll(new Roll(3));

        // Assert
        Assert.That(frame.IsComplete, Is.True);
        Assert.That(frame.IsSpare, Is.True);
        Assert.That(frame.Rolls, Has.Count.EqualTo(2));
    }

    [Test]
    public void Frame_TwoFouls_IsComplete()
    {
        // Arrange
        var frame = new Frame(1);

        // Act
        frame.AddRoll(new Roll(0, true)); // Foul
        frame.AddRoll(new Roll(0, true)); // Foul

        // Assert
        Assert.That(frame.IsComplete, Is.True);
        Assert.That(frame.Rolls, Has.Count.EqualTo(2));
        Assert.That(frame.TotalPins, Is.EqualTo(0));
    }

    [Test]
    public void Frame_TwoZeros_IsComplete()
    {
        // Arrange
        var frame = new Frame(1);

        // Act
        frame.AddRoll(new Roll(0)); // Not a foul, just 0
        frame.AddRoll(new Roll(0));

        // Assert
        Assert.That(frame.IsComplete, Is.True);
        Assert.That(frame.Rolls, Has.Count.EqualTo(2));
    }

    [Test]
    public void Frame10_NoStrikeOrSpare_TwoRolls_IsComplete()
    {
        // Arrange
        var frame = new Frame(10);

        // Act
        frame.AddRoll(new Roll(3));
        frame.AddRoll(new Roll(4));

        // Assert
        Assert.That(frame.IsComplete, Is.True);
        Assert.That(frame.Rolls, Has.Count.EqualTo(2));
    }

    [Test]
    public void Frame10_Strike_ThreeRolls_IsComplete()
    {
        // Arrange
        var frame = new Frame(10);

        // Act
        frame.AddRoll(new Roll(10)); // Strike
        frame.AddRoll(new Roll(7));
        frame.AddRoll(new Roll(2));

        // Assert
        Assert.That(frame.IsComplete, Is.True);
        Assert.That(frame.Rolls, Has.Count.EqualTo(3));
    }

    [Test]
    public void Frame10_Spare_ThreeRolls_IsComplete()
    {
        // Arrange
        var frame = new Frame(10);

        // Act
        frame.AddRoll(new Roll(7));
        frame.AddRoll(new Roll(3)); // Spare
        frame.AddRoll(new Roll(5));

        // Assert
        Assert.That(frame.IsComplete, Is.True);
        Assert.That(frame.Rolls, Has.Count.EqualTo(3));
    }

    [Test]
    public void Frame10_ThreeStrikes_IsComplete()
    {
        // Arrange
        var frame = new Frame(10);

        // Act
        frame.AddRoll(new Roll(10));
        frame.AddRoll(new Roll(10));
        frame.AddRoll(new Roll(10));

        // Assert
        Assert.That(frame.IsComplete, Is.True);
        Assert.That(frame.Rolls, Has.Count.EqualTo(3));
    }

    [Test]
    public void Frame10_TwoFouls_IsComplete()
    {
        // Arrange
        var frame = new Frame(10);

        // Act
        frame.AddRoll(new Roll(0, true)); // Foul
        frame.AddRoll(new Roll(0, true)); // Foul

        // Assert
        Assert.That(frame.IsComplete, Is.True, "Frame 10 with 2 fouls (0+0, no spare) should be complete");
        Assert.That(frame.Rolls, Has.Count.EqualTo(2));
    }

    [Test]
    public void Frame10_Strike_TwoRolls_IsNotComplete()
    {
        // Arrange
        var frame = new Frame(10);

        // Act
        frame.AddRoll(new Roll(10)); // Strike
        frame.AddRoll(new Roll(7));

        // Assert
        Assert.That(frame.IsComplete, Is.False, "Frame 10 with strike needs 3 rolls");
        Assert.That(frame.Rolls, Has.Count.EqualTo(2));
    }

    [Test]
    public void Frame10_Spare_TwoRolls_IsNotComplete()
    {
        // Arrange
        var frame = new Frame(10);

        // Act
        frame.AddRoll(new Roll(7));
        frame.AddRoll(new Roll(3)); // Spare

        // Assert
        Assert.That(frame.IsComplete, Is.False, "Frame 10 with spare needs 3 rolls");
        Assert.That(frame.Rolls, Has.Count.EqualTo(2));
    }

    [Test]
    public void Frame_AddRollAfterComplete_ThrowsException()
    {
        // Arrange
        var frame = new Frame(1);
        frame.AddRoll(new Roll(10)); // Strike - frame complete

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => frame.AddRoll(new Roll(5)));
    }

    [Test]
    public void Frame10_AddFourthRoll_ThrowsException()
    {
        // Arrange
        var frame = new Frame(10);
        frame.AddRoll(new Roll(10));
        frame.AddRoll(new Roll(10));
        frame.AddRoll(new Roll(10));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => frame.AddRoll(new Roll(5)));
    }

    [Test]
    public void Frame_ExceedTenPins_ThrowsException()
    {
        // Arrange
        var frame = new Frame(1);
        frame.AddRoll(new Roll(7));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => frame.AddRoll(new Roll(5)));
    }
}