public class Monster : CharacterBase
{
    float monsterSpeed;
    protected override void Start()
    {
        base.Start();
    }

    public override void Move()
    {
    }

    public override void Attack()
    {
    }

    protected override void Die()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}