using Engine.ViewModels;

namespace TestEngine.ViewModels;

public class TestGameSessions {
    [Fact]
    public void TestCreateGameSession() {
        GameSession gameSession = new GameSession();
        Assert.NotNull(gameSession.CurrentPlayer);
        Assert.Equal("TownSquare", gameSession.CurrentLocation.Name);
    }

    [Fact]
    public void TestPlayerMovesHomeAndIsCompletelyHealedOnKilled() {
        GameSession gameSession = new GameSession();
        gameSession.CurrentPlayer.TakeDamage(999);
        Assert.Equal("Home", gameSession.CurrentLocation.Name);
        Assert.Equal(gameSession.CurrentPlayer.MaximumHitPoints, gameSession.CurrentPlayer.CurrentHitPoints);
    }
}