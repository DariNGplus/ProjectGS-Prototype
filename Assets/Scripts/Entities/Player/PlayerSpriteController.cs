using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    [SerializeField] private PlayerSpriteReferences _references;

    public GameObject PlayerSpriteGameObject;
    [HideInInspector] public Animator PlayerSpriteAnimator;

    public GameObject CurrentArm
    {
        get 
        {
            if (FacingDown)
            {
                return _references.RightArm;
            }
            else 
            { 
                return _references.LeftArm;
            }
        }
    }

    public bool FacingDown = true;

    public bool FacingRight = true;


    public void UpdateSprites() 
    {
        _references.UpdateSprites(FacingDown, FacingRight);
    }

    void Awake()
    {
        FacingDown = true;
        FacingRight = true;
        PlayerSpriteAnimator = PlayerSpriteGameObject.GetComponent<Animator>();    
    }

}
[System.Serializable]
public class PlayerSpriteReferences
{
    public SpriteRenderer _bodyRenderer;
    public SpriteRenderer _headRenderer;

    public GameObject RightArm;
    public SpriteRenderer _rightArmRenderer;
    public SpriteRenderer _rightHandRenderer;
    public GameObject _rightGun;

    public GameObject LeftArm;
    public  SpriteRenderer _leftArmRenderer;
    public SpriteRenderer _leftHandRenderer;
    public GameObject _leftGun;

    public SpriteRenderer _leftLegRenderer;
    public SpriteRenderer _rightLegRenderer;

    private void ToggleGun(bool facingDown)
    {
        _rightGun.SetActive(facingDown);
        _leftGun.SetActive(!facingDown);

        if (_rightGun.activeSelf) LeftArm.transform.rotation = Quaternion.identity;
        else RightArm.transform.rotation = Quaternion.identity;

    }

    public void UpdateSprites(bool facingDown, bool facingRight)
    {
        ToggleGun(facingDown);
        if (facingDown)
        {
            _headRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/head_front");
            _bodyRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/body_front");

            if (facingRight)
            {
                _headRenderer.flipX = false;
                _leftLegRenderer.flipX = false;
                _rightLegRenderer.flipX = false;

                _rightArmRenderer.sortingOrder = 4;
                _rightHandRenderer.sortingOrder = 5;
                _leftArmRenderer.sortingOrder = 2;
                _leftHandRenderer.sortingOrder = 1;
                _leftLegRenderer.sortingOrder = 3;
                _rightLegRenderer.sortingOrder = 2;
            }
            else
            {
                _headRenderer.flipX = true;
                _leftLegRenderer.flipX = true;
                _rightLegRenderer.flipX = true;

                _rightArmRenderer.sortingOrder = 2;
                _rightHandRenderer.sortingOrder = 1;
                _leftArmRenderer.sortingOrder = 4;
                _leftHandRenderer.sortingOrder = 5;
                _leftLegRenderer.sortingOrder = 2;
                _rightLegRenderer.sortingOrder = 3;
            }
        }
        else
        {
            _headRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/head_back");
            _bodyRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/body_back");

            _rightArmRenderer.sortingOrder = 1;
            _rightHandRenderer.sortingOrder = 0;
            _leftArmRenderer.sortingOrder = 1;
            _leftHandRenderer.sortingOrder = 0;

            if (facingRight)
            {
                _headRenderer.flipX = false;
                _leftLegRenderer.flipX = false;
                _rightLegRenderer.flipX = false;

                _leftLegRenderer.sortingOrder = 3;
                _rightLegRenderer.sortingOrder = 2;
            }
            else
            {
                _headRenderer.flipX = true;
                _leftLegRenderer.flipX = true;
                _rightLegRenderer.flipX = true;

                _leftLegRenderer.sortingOrder = 2;
                _rightLegRenderer.sortingOrder = 3;
            }
        }
    }
}
