using UnityEngine;

namespace ConjurerDice
{
    public class TargetingReticle : MonoBehaviour
    {
        public void AttachTo(Transform t) { transform.SetParent(t, false); transform.localPosition = Vector3.zero; gameObject.SetActive(true); }
        public void Hide() { gameObject.SetActive(false); transform.SetParent(null); }
    }
}