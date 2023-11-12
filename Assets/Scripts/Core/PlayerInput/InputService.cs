using System;
using Core.Interfaces;

namespace Core.PlayerInput
{
    public class InputService : IInputService
    {
        public Input Input { get; }

        public InputService(Input input)
        {
            Input = input;
        }

        public void Dispose()
        {
            Input?.Dispose();
        }
    }
}