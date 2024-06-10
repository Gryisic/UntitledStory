namespace Infrastructure.Utils
{
    public static class InputEnums
    {
        public enum Keyboard
        {
            Q, W, E, R, T, Y, U, I, O, P,
            A, S, D, F, G, H, J, K, L,
            Z, X, C, V, B, N, M,
            UpArrow, LeftArrow, RightArrow, DownArrow,
            Space, Enter, Backspace,
            Shift, Control, Alt,
        }
        
        public enum SonyGamepad
        {
            Cross, Circle, Triangle, Square,
            Share, Options,
            Left, Right, Up, Down,
            L1, L2, L3,
            R1, R2, R3
        }
        
        public enum MicrosoftGamepad
        {
            A, B, Y, X,
            View, Menu,
            Left, Right, Up, Down,
            LeftBumper, LeftTrigger, LeftStickPress,
            RightBumper, RightTrigger, RightStickPress,
        }
    }
}