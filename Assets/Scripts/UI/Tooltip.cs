using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _text;
    [Tooltip("The text padding")]
    [SerializeField] private Vector2 _padding;
    private Rect _screenBounds;
    [SerializeField] private bool _active;
    private bool _offScreenUp;
    private bool _offScreenRight;
    public Camera cam;

    private void Start()
    {
        _screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);
    }

    public void Active(string text)
    {
        _active = true;
        _background.enabled = true;
        _text.enabled = true;
        _text.text = text;
        Canvas.ForceUpdateCanvases();
        _background.GetComponent<RectTransform>().sizeDelta = new Vector2(_text.preferredWidth + _padding.x, _text.preferredHeight + _padding.y);
        _background.GetComponent<RectTransform>().ForceUpdateRectTransforms();
    }

    public void Desactive()
    {
        _active = false;
        _background.enabled = false;
        _text.enabled = false;
        if (_offScreenUp)
        {
            _offScreenUp = false;
            GetComponent<RectTransform>().pivot = new Vector2(GetComponent<RectTransform>().pivot.x, GetComponent<RectTransform>().pivot.y - 1);
        }
        if (_offScreenRight)
        {
            _offScreenRight = false;
            GetComponent<RectTransform>().pivot = new Vector2(GetComponent<RectTransform>().pivot.x - 1.2f, GetComponent<RectTransform>().pivot.y);
        }
    }

    private void Update()
    {
        if (_active)
        {
            Vector3[] tmp = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(tmp);
            for(int i = 0; i < tmp.Length; i++)
            {
                tmp[i] = cam.WorldToScreenPoint(tmp[i]);
            }
            if (!_screenBounds.Contains(tmp[1]) && !_offScreenUp) //Up offscreen test
            {
                _offScreenUp = true;
                GetComponent<RectTransform>().pivot = new Vector2(GetComponent<RectTransform>().pivot.x, GetComponent<RectTransform>().pivot.y + 1);
            }
            if (!_screenBounds.Contains(tmp[3]) && !_offScreenRight) //Right offscreen test
            {
                _offScreenRight = true;
                GetComponent<RectTransform>().pivot = new Vector2(GetComponent<RectTransform>().pivot.x + 1.2f, GetComponent<RectTransform>().pivot.y);
            }
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = 100.0f; //distance of the plane from the camera

            transform.position = cam.ScreenToWorldPoint(screenPoint);
        }
    }

}