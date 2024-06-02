using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D _crosshairTexture;
    [SerializeField] private Texture2D _reloadingTexture;
    private Texture2D _currentTexture;

    const float ReloadRotationSpeed = 300f;
    const float CrosshairRotationSpeed = 60f;
    
    private float RotationSpeed = 50f;
    private float _rotationAngle = 0.0f;
    private Vector2 _mPos;

    [SerializeField] private float _cursorSize = 60;

    void Awake()
    {
        Cursor.visible = false;
        _currentTexture = _crosshairTexture;
    }

    void Update()
    {   
        _rotationAngle += Time.deltaTime * RotationSpeed;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus) 
        {
            Cursor.visible = false;
        }
    }

    public void IsReloading(bool state) 
    {
        if (state)
        {
            _currentTexture = _reloadingTexture;
            RotationSpeed = ReloadRotationSpeed;
        }
        else {
            _currentTexture = _crosshairTexture;
            RotationSpeed = CrosshairRotationSpeed;
        }
    }

    private void OnGUI()
    {
        Matrix4x4 matrixBackup = GUI.matrix;
        _mPos = Event.current.mousePosition;
        GUIUtility.RotateAroundPivot(_rotationAngle, _mPos);
        GUI.DrawTexture(new Rect(_mPos.x - (_cursorSize/2), _mPos.y - (_cursorSize/2), _cursorSize, _cursorSize), _currentTexture);
        GUI.matrix = matrixBackup;
    }
}
