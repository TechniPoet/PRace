namespace Services
{
    public class ScoreService
    {
        public static InputService Instance => _instance;
        private static InputService _instance;
        private GameRunner _gameRunner;
        
        public InputService(GameRunner runner)
        {
            _instance = this;
            _gameRunner = runner;
        }

        public void AdjustRowerSpeed(GameRunner.RowerId id, bool up)
        {
            _gameRunner.AdjustRowerSpeed(id, up);
        }
    }
}