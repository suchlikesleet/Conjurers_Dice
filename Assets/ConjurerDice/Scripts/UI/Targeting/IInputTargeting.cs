namespace ConjurerDice
{
    public interface IInputTargeting
    {
        // Fired when pointing device moves (screen pixels)
        void RaisePointerMoved(UnityEngine.Vector2 screenPos);

        // Fired when user requests a pick/confirm at current pointer
        void RaiseConfirm();

        // Optional extras
        void RaiseCancel();
        void RaiseReset();
        void RaiseNavigate(int dx, int dy); // dpad/keyboard focus navigation if you add it
    }
}