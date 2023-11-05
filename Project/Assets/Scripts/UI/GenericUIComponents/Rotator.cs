using UnityEngine;

namespace UI.GenericUIComponents
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 _angularVelocity;

        void Update()
        {
            transform.Rotate(_angularVelocity * Time.deltaTime);
        }
    }
}