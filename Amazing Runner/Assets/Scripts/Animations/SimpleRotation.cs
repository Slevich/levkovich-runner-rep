using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleRotation : MonoBehaviour
{
    #region Fields
    [Header("The Y angle by which the object is rotated.")]
    [SerializeField] private float rotationAngle;
    [Header("The time it takes for an object to turn.")]
    [SerializeField] private float rotateDuration;
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
    /// The method rotates the object by the specified angle.
    /// </summary>
    private void ObjectRotation()
    {
        transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y + rotationAngle, transform.rotation.z), rotateDuration, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }
    #endregion
}
