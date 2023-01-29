using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotationSequence : MonoBehaviour
{
    #region Fields
    [Header("The Y angle by which the object is rotated.")]
    [SerializeField] private float rotationAngle;
    [Header("The time it takes for an object to turn.")]
    [SerializeField] private float rotateDuration;
    [Header("The amount of time an object waits before turning.")]
    [SerializeField] private float waitingTimer;
    #endregion

    #region Methods
    /// <summary>
    /// At the start, run the object rotation method.
    /// </summary>
    private void Start()
    {
        ObjectRotation();
    }

    /// <summary>
    /// A sequence of Tween's in which there are two turns: 
    /// to the specified angle and then to the starting position.
    /// </summary>
    private void ObjectRotation()
    {
        DOTween.Sequence()
        .Append(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y + rotationAngle, transform.rotation.z), rotateDuration, RotateMode.LocalAxisAdd))
            .SetEase(Ease.Linear)
            .SetLoops(-1)
            .SetDelay(waitingTimer)
        .Append(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y - rotationAngle, transform.rotation.z), rotateDuration, RotateMode.LocalAxisAdd))
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }
    #endregion
}
