using DG.Tweening;
using UnityEngine;
public class SpreadOutLine : MonoBehaviour
{
    [Range(0, 1)]
    public float Max = 0.4f;
    [Range(0, 1)]
    public float Out = 0.2f;
    [Range(0, 1)]
    public float In = 0.1f;
    [Range(0, 1)]
    public float Freq = 0.8f;
    public Color MyColor;
    Sequence spread;
    private void Start()
    {
        SpriteRenderer myRenderer;
        Sprite mySprite;
        gameObject.TryGetComponent<SpriteRenderer>(out myRenderer);
        mySprite = myRenderer.sprite;

        SpriteRenderer spreadImage = new GameObject().AddComponent<SpriteRenderer>();
        SpriteMask followImage = new GameObject().AddComponent<SpriteMask>();
        Destroy(followImage.GetComponent<RectTransform>()); 
        SetSpreadImage( mySprite, Out, spreadImage);
        SetSpreadImage( mySprite, In, null, followImage);
        spreadImage.color = MyColor;
        spreadImage.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        spreadImage.sortingOrder = myRenderer.sortingOrder-1;


        spread.SetAutoKill();
        spread = DOTween.Sequence();
        Vector3 max = Vector3.one * Max;

        //순차 스프레드
        spread.Append(spreadImage.transform.DOScale(spreadImage.transform.localScale + max, Freq)).
            Append(followImage.transform.DOScale(followImage.transform.localScale + max, Freq));

        //동시 스프레드
        /*spread.Append(spreadImage.transform.DOScale(spreadImage.transform.localScale + max, _freq)).
            Join(followImage.transform.DOScale(followImage.transform.localScale + max, _freq));*/


        spread.SetLoops(-1);
    }
    private void SetSpreadImage( Sprite sprite, float num, SpriteRenderer spriteRenderer = null, SpriteMask spriteMask=null)
    {
        if(spriteMask==null)
        {
            spriteRenderer.transform.SetParent(gameObject.transform);
            spriteRenderer.transform.localPosition = Vector3.zero;
            spriteRenderer.transform.localScale = Vector3.one + Vector3.one * num;
            spriteRenderer.sprite = sprite;
        }
        else
        {
            spriteMask.transform.SetParent(gameObject.transform);
            spriteMask.transform.localPosition = Vector3.zero;
            spriteMask.transform.localScale = Vector3.one + Vector3.one * num;
            spriteMask.sprite = sprite;
        }
    }
}
