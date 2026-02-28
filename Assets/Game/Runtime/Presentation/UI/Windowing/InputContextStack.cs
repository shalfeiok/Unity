using System.Collections.Generic;

namespace Game.Presentation.UI.Windowing
{
    public enum InputContext
    {
        Gameplay = 0,
        UI = 1,
        ChatTyping = 2,
        Modal = 3
    }

    public sealed class InputContextStack
    {
        private readonly Stack<InputContext> _stack = new();

        public InputContextStack()
        {
            _stack.Push(InputContext.Gameplay);
        }

        public InputContext Current => _stack.Peek();

        public void Push(InputContext context) => _stack.Push(context);

        public void Pop()
        {
            if (_stack.Count > 1)
                _stack.Pop();
        }
    }
}
