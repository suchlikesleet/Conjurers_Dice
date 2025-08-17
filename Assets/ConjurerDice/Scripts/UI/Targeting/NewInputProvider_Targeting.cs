using UnityEngine;
using UnityEngine.InputSystem;

namespace ConjurerDice
{
    public class NewInputProvider_Targeting : MonoBehaviour
    {
        [SerializeField] private InputBusTargetingSO bus;

        

        private void OnPoint(InputAction.CallbackContext ctx)
        {
            bus?.RaisePointerMoved(ctx.ReadValue<Vector2>());
        }

        private void OnClick(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
            {
                bus?.RaiseConfirm();
            }
            
        }

        private void OnCancel(InputAction.CallbackContext ctx)
        {
            bus?.RaiseCancel();
        }

        private void OnReset(InputAction.CallbackContext ctx)
        {
            bus?.RaiseReset();
        }

        private void OnNav(InputAction.CallbackContext ctx)
        {
            var v = ctx.ReadValue<Vector2>();
            bus?.RaiseNavigate(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
        }
    }
}
