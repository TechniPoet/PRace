using GameLogic;

namespace Services
{
    public class InputService
    {
        public static InputService Instance => _instance;
        private static InputService _instance;
        private GameRunner _gameRunner;
        
        public InputService(GameRunner runner)
        {
            _instance = this;
            _gameRunner = runner;
        }
        
        ~InputService()
        {
            _instance = null;
            _gameRunner = null;
        }

        public void AdjustRowerSpeed(GameRunner.RowerId id, bool up)
        {
            _gameRunner.AdjustRowerSpeed(id, up);
        }
    }
}