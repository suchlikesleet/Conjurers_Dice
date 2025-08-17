using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName="ConjurerDice/Input/Input Bus (Targeting)")]
    public class InputBusTargetingSO : ScriptableObject, IInputTargeting
    {
        public UnityEvent<Vector2> OnPointerMoved;
        public UnityEvent OnConfirm;
        public UnityEvent OnCancel;
        public UnityEvent OnReset;
        public UnityEvent<int,int> OnNavigate; // (dx, dy)

        public void RaisePointerMoved(Vector2 p) => OnPointerMoved?.Invoke(p);
        public void RaiseConfirm() => OnConfirm?.Invoke();
        public void RaiseCancel()  => OnCancel?.Invoke();
        public void RaiseReset()   => OnReset?.Invoke();
        public void RaiseNavigate(int dx, int dy) => OnNavigate?.Invoke(dx, dy);
    }
}