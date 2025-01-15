using UnityEngine;
using UnityEngine.UI;
public class Boss : EnemyBase
{
    [SerializeField]
    Slider hpBar = null;
    [SerializeField]
    Text enemyText = null;
    [SerializeField]
    string enemyName = "FireSkull";
    [SerializeField]
    GameObject bossBattle = null;

    public override void Initialize()
    {
        base.Initialize();
        hpBar.maxValue = health.maxHp;
        hpBar.value = health.currentHp;
        health.onTakeDamage = OnTakeDamage;
        enemyText.text = enemyName;
    }

    //funzione per aggiornare gli hp
    void OnTakeDamage()
    {
        hpBar.maxValue = health.maxHp;
        hpBar.value = health.currentHp;
        if (health.isDeath())
        {
            Destroy(bossBattle);
        }
    }

}
