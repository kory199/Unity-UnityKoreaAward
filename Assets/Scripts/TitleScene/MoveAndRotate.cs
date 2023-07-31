using Cysharp.Threading.Tasks;
using UnityEngine;

public class MoveAndRotate : MonoBehaviour
{
    [SerializeField] Vector3 moveDirection = new Vector3(1f, 0f, 0f);
    [SerializeField] Vector3 rotateAxis = new Vector3(0f, 1f, 0f);

    [SerializeField] float speed = 0.5f;

    // AnimationCurve objects that control the movement and rotation.
    private AnimationCurve _moveCurve;
    private AnimationCurve _rotateCurve;

    // This Array of keyframes that controls the movement
    // Each keyframe represents a pair of (time, value)
    Keyframe[] moveKeyframes = new Keyframe[]
    {
        new Keyframe(0, 0),
        new Keyframe(1, 0.8f),
        new Keyframe(2, 1f),
        new Keyframe(3, 0.8f),
        new Keyframe(4, 0)
    };

    // This Array of keyframes that controls the rotation
    Keyframe[] rotateKeyframes = new Keyframe[]
    {
        new Keyframe(0, 0),
        new Keyframe(1, 360 / 2f),
        new Keyframe(2, 360)
    };

    private async void Awake()
    {
        _moveCurve = new AnimationCurve(moveKeyframes);
        _rotateCurve = new AnimationCurve(rotateKeyframes);

        await MoveAndRotateCoroutine();
    }

    // This coroutine moves the object in the desired direction and rotates it around the desired axis.
    async UniTask MoveAndRotateCoroutine()
    {
        float timer = 0f;

        Vector3 initialPosition = transform.position;

        while (true)
        {
            // Stop the coroutine in this GameObject no longer exists.
            if (this == null) break;

            // Increment the timer value
            // This value is used as an input for the AnimationCurve.
            timer += Time.deltaTime * speed;

            // Calculate and apply the target position of the movement.
            Vector3 targetPosition = initialPosition + moveDirection * _moveCurve.Evaluate(timer % moveKeyframes[moveKeyframes.Length - 1].time);
            transform.position = targetPosition;

            // Calculate and apply the target angle of the rotation.
            Quaternion targetRotation = Quaternion.AngleAxis(_rotateCurve.Evaluate(timer % rotateKeyframes[rotateKeyframes.Length - 1].time), rotateAxis);
            transform.rotation = targetRotation;

            // Wait for the next frame.
            await UniTask.Yield();
        }
    }
}