using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class OutlineInteractable : MonoBehaviour
{
    [SerializeField] private Transform _mHighlight;
    private RaycastHit _mHit;
    private int _mTargetLayer = 6;

    void Update()
    {
        Outlining();
    }

    void Outlining()
    {
        if (_mHighlight != null) 
        {
            _mHighlight.gameObject.GetComponent<Outline>().enabled = false;
            _mHighlight = null;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out _mHit))
        {
            _mHighlight = _mHit.transform;
            if (_mHighlight != null &&
                _mHighlight.gameObject.layer == _mTargetLayer && 
                (_mHighlight.gameObject.GetComponent<XRGrabInteractable>().interactionLayers == InteractionLayerMask.GetMask("ray interaction") ||
                (_mHighlight.gameObject.GetComponent<XRGrabInteractable>().interactionLayers == InteractionLayerMask.GetMask("ray interaction", "direct interaction"))))
            {
                if (_mHighlight.gameObject.GetComponent<Outline>() != null) 
                {
                    _mHighlight.gameObject.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    Outline outline = _mHighlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                }
            }
            else
            {
                _mHighlight = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_mHighlight != null && other.gameObject.layer == _mTargetLayer && _mHighlight.gameObject.GetComponent<XRGrabInteractable>().interactionLayers == InteractionLayerMask.GetMask("direct interaction"))
        {
            if (other.gameObject.GetComponent<Outline>() != null)
            {
                other.gameObject.GetComponent<Outline>().enabled = true;
            }
            else
            {
                Outline outline = other.gameObject.AddComponent<Outline>();
                outline.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_mHighlight != null && other.gameObject.layer == _mTargetLayer && _mHighlight.gameObject.GetComponent<XRGrabInteractable>().interactionLayers == InteractionLayerMask.GetMask("direct interaction"))
        {
            if (other.gameObject.GetComponent<Outline>() != null)
            {
                other.gameObject.GetComponent<Outline>().enabled = false;
            }
            else
            {
                Outline outline = other.gameObject.AddComponent<Outline>();
                outline.enabled = false;
            }
        }
    }
}
